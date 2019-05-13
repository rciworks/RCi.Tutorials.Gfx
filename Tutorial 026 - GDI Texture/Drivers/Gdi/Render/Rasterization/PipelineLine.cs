using System;
using System.Collections.Generic;
using System.Linq;
using RCi.Tutorials.Gfx.Mathematics;
using RCi.Tutorials.Gfx.Mathematics.Extensions;

namespace RCi.Tutorials.Gfx.Drivers.Gdi.Render.Rasterization
{
    public partial class Pipeline<TVsIn, TPsIn>
    {
        /// <summary>
        /// Line primitive.
        /// </summary>
        internal struct PrimitiveLine
        {
            public TPsIn PsIn0;
            public TPsIn PsIn1;
            public Vector4F PositionScreen0;
            public Vector4F PositionScreen1;
        }

        #region // vertex post-processing

        /// <summary>
        /// <see cref="VertexPostProcessing"/> for line.
        /// </summary>
        private void VertexPostProcessingLine(ref TPsIn psin0, ref TPsIn psin1)
        {
            // clipping: go through each plane
            for (var i = 0; i < 6; i++)
            {
                if (!Clipping<TPsIn>.ClipByPlane((ClippingPlane)(1 << i), ref psin0, ref psin1))
                {
                    // both outside, clipped out
                    return;
                }
            }

            // vertex post processing + primitive assembly
            PrimitiveLine primitive;
            primitive.PsIn0 = psin0;
            primitive.PsIn1 = psin1;
            VertexPostProcessing(ref primitive.PsIn0, out primitive.PositionScreen0);
            VertexPostProcessing(ref primitive.PsIn1, out primitive.PositionScreen1);

            // rasterization stage
            RasterizeLine(primitive);
        }

        #endregion

        #region // rasterization

        /// <summary>
        /// Rasterize line.
        /// </summary>
        private void RasterizeLine(in PrimitiveLine primitive)
        {
            var pixels = LineRasterization.DiamondExit.GetPixels(
                    primitive.PositionScreen0.ToVector2F(),
                    primitive.PositionScreen1.ToVector2F())
                .ToArray();

            // get delta per pixel going from vertex0 to vertex1
            var deltaAlpha = 1f / pixels.Length;
            // get alpha (with prestep)
            var alpha = deltaAlpha * 0.5f;

            foreach (var (x, y) in pixels)
            {
                // interpolate screen
                var interpolantScreen = primitive.PositionScreen0.InterpolateLinear(primitive.PositionScreen1, alpha);

                // interpolate attributes
                var interpolant = primitive.PsIn0.InterpolateLinear(primitive.PsIn1, alpha);

                // pass to pixel shader
                StagePixelShader(x, y, interpolantScreen.Z, interpolant);

                // increment (interpolate going towards vertex1)
                alpha += deltaAlpha;
            }
        }

        #endregion
    }

    /// <summary>
    /// Line rasterization algorithms.
    /// </summary>
    public static class LineRasterization
    {
        #region // bresenham

        /// <summary>
        /// Bresenham's line algorithm line rasterization algorithm.
        /// https://en.wikipedia.org/wiki/Bresenham%27s_line_algorithm
        /// </summary>
        public static class Bresenham
        {
            public static IEnumerable<(int X, int Y)> GetPixels(Vector2F point0, Vector2F point1)
            {
                var x0 = (int)Math.Round(point0.X);
                var y0 = (int)Math.Round(point0.Y);
                var x1 = (int)Math.Round(point1.X);
                var y1 = (int)Math.Round(point1.Y);
                var w = x1 - x0;
                var h = y1 - y0;
                int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;
                if (w < 0) dx1 = -1; else if (w > 0) dx1 = 1;
                if (h < 0) dy1 = -1; else if (h > 0) dy1 = 1;
                if (w < 0) dx2 = -1; else if (w > 0) dx2 = 1;
                var longest = Math.Abs(w);
                var shortest = Math.Abs(h);
                if (longest <= shortest)
                {
                    longest = Math.Abs(h);
                    shortest = Math.Abs(w);
                    if (h < 0) dy2 = -1; else if (h > 0) dy2 = 1;
                    dx2 = 0;
                }
                var numerator = longest >> 1;
                for (var i = 0; i <= longest; i++)
                {
                    yield return (x0, y0);
                    numerator += shortest;
                    if (numerator < longest)
                    {
                        x0 += dx2;
                        y0 += dy2;
                    }
                    else
                    {
                        numerator -= longest;
                        x0 += dx1;
                        y0 += dy1;
                    }
                }
            }
        }

        #endregion

        #region // diamond exit

        /// <summary>
        /// https://docs.microsoft.com/en-us/windows/desktop/direct3d11/d3d10-graphics-programming-guide-rasterizer-stage-rules
        /// </summary>
        public static class DiamondExit
        {
            #region // structs

            private readonly struct LineEdge
            {
                public Vector2F Start { get; }
                public Vector2F End { get; }

                public LineEdge(Vector2F start, Vector2F end) =>
                    (Start, End) = (start, end);
            }

            private readonly struct LineStandardForm
            {
                public float A { get; }
                public float B { get; }
                public float C { get; }

                public LineStandardForm(float a, float b, float c) =>
                    (A, B, C) = (a, b, c);
            }

            private readonly struct LineIntersection
            {
                public float LineOffset { get; }
                public float EdgeOffset { get; }
                public float Denominator { get; }

                public LineIntersection(float lineOffset, float edgeOffset, float denominator) =>
                    (LineOffset, EdgeOffset, Denominator) = (lineOffset, edgeOffset, denominator);
            }

            #endregion

            #region // routines

            private static bool LineIsOnLeftSide(LineEdge lineEdge, Vector2F point, bool strict)
            {
                var value = (lineEdge.End.X - lineEdge.Start.X) * (point.Y - lineEdge.Start.Y) -
                            (lineEdge.End.Y - lineEdge.Start.Y) * (point.X - lineEdge.Start.X);

                var numerator = point.X - lineEdge.Start.X;
                var denominator = lineEdge.End.X - lineEdge.Start.X;
                var inBounds = denominator < 0
                    ? numerator < 0 && numerator > denominator
                    : numerator > 0 && numerator < denominator;

                return value < 0 || !strict && inBounds && value.Equals(0f);
            }

            private static bool LineIsRejected(Vector2F point, LineStandardForm lineStandardForm)
            {
                if (lineStandardForm.A.Equals(0f))
                {
                    return (point.Y - lineStandardForm.C) * (point.Y - lineStandardForm.C) > 0.25f;
                }
                if (lineStandardForm.B.Equals(0f))
                {
                    return (point.X - lineStandardForm.C) * (point.X - lineStandardForm.C) > 0.5f;
                }

                var numerator = lineStandardForm.A * point.X + lineStandardForm.B * point.Y + lineStandardForm.C;
                var denominator = lineStandardForm.A * lineStandardForm.A + lineStandardForm.B * lineStandardForm.B;

                return numerator * numerator > denominator * 0.5f;
            }

            private static bool LineGetEdgeIntersection(Vector2F p0, Vector2F p1, LineEdge lineEdge, out LineIntersection info)
            {
                // check for lines being parallel
                var denominator = (p1.X - p0.X) * (lineEdge.End.Y - lineEdge.Start.Y) - (p1.Y - p0.Y) * (lineEdge.End.X - lineEdge.Start.X);
                if (denominator.Equals(0f))
                {
                    info = default;
                    return false;
                }

                // get intersections
                info = new LineIntersection
                (
                    (p0.Y - lineEdge.Start.Y) * (lineEdge.End.X - lineEdge.Start.X) - (p0.X - lineEdge.Start.X) * (lineEdge.End.Y - lineEdge.Start.Y),
                    (p0.Y - lineEdge.Start.Y) * (p1.X - p0.X) - (p0.X - lineEdge.Start.X) * (p1.Y - p0.Y),
                    denominator
                );

                return true;
            }

            private static void LineGetBounds(Vector2F p0, Vector2F p1,
                out int xOffset, out int yOffset,
                out int xStart, out int xStop, out int xDelta,
                out int yStart, out int yStop, out int yDelta)
            {
                var minX = Math.Min(p0.X, p1.X);
                var minY = Math.Min(p0.Y, p1.Y);
                var maxX = Math.Max(p0.X, p1.X);
                var maxY = Math.Max(p0.Y, p1.Y);

                xOffset = (int)minX - 1;
                yOffset = (int)minY - 1;
                var width = (int)Math.Ceiling(maxX) - (int)minX + 1;
                var height = (int)Math.Ceiling(maxY) - (int)minY + 1;

                if (p0.X > p1.X)
                {
                    xStart = width - 1;
                    xStop = -1;
                    xDelta = -1;
                }
                else
                {
                    xStart = 0;
                    xStop = width;
                    xDelta = 1;
                }

                if (p0.Y > p1.Y)
                {
                    yStart = height - 1;
                    yStop = -1;
                    yDelta = -1;
                }
                else
                {
                    yStart = 0;
                    yStop = height;
                    yDelta = 1;
                }
            }

            private static LineStandardForm LineGetStandardForm(Vector2F p0, Vector2F p1)
            {
                float a, b, c;

                if (!p0.X.Equals(p1.X))
                {
                    a = (p1.Y - p0.Y) / (p1.X - p0.X);
                    b = -1f;
                    c = p0.Y - a * p0.X;
                }
                else
                {
                    a = 1f;
                    b = 0f;
                    c = p0.X;
                }

                return new LineStandardForm(a, b, c);
            }

            private static unsafe bool LineDiamondTest(int x, int y, Vector2F p0, Vector2F p1, LineStandardForm lineStandardForm, bool xMajor)
            {
                var center = new Vector2F(x + 0.5f, y + 0.5f);

                if (LineIsRejected(center, lineStandardForm))
                {
                    return false;
                }

                // create diamond
                var points = stackalloc Vector2F[4]
                {
                    new Vector2F(center.X - 0.5f, center.Y), // left
                    new Vector2F(center.X + 0.5f, center.Y), // right
                    new Vector2F(center.X, center.Y - 0.5f), // bottom
                    new Vector2F(center.X, center.Y + 0.5f), // top
                };
                var edges = stackalloc LineEdge[4]
                {
                    new LineEdge(points[0], points[3]), // left <=> top
                    new LineEdge(points[3], points[1]), // top <=> right
                    new LineEdge(points[1], points[2]), // right <=> bottom
                    new LineEdge(points[2], points[0]), // bottom <=> left
                };

                // clear cases
                if (p1 == points[3] || !xMajor && p1 == points[1] ||
                    LineIsOnLeftSide(edges[0], p1, false) &&
                    LineIsOnLeftSide(edges[1], p1, false) &&
                    LineIsOnLeftSide(edges[2], p1, true) &&
                    LineIsOnLeftSide(edges[3], p1, true))
                {
                    return false;
                }
                if (p0 == points[3] || !xMajor && p0 == points[1] ||
                    LineIsOnLeftSide(edges[0], p0, false) &&
                    LineIsOnLeftSide(edges[1], p0, false) &&
                    LineIsOnLeftSide(edges[2], p0, true) &&
                    LineIsOnLeftSide(edges[3], p0, true))
                {
                    return true;
                }

                // count intersections
                var intersectionCount = 0;

                for (var i = 0; i < 4 && intersectionCount < 2; i++)
                {
                    if (!LineGetEdgeIntersection(p0, p1, edges[i], out var intersection)) continue;

                    if (intersection.EdgeOffset.Equals(0f))
                    {
                        if (edges[i].Start == points[3] || !xMajor && edges[i].Start == points[1])
                        {
                            return true;
                        }
                    }
                    if (intersection.EdgeOffset.Equals(intersection.Denominator))
                    {
                        if (edges[i].End == points[3] || !xMajor && edges[i].End == points[1])
                        {
                            return true;
                        }
                    }

                    var edgeIntersection = intersection.Denominator < 0
                        ? intersection.EdgeOffset <= 0 && intersection.EdgeOffset > intersection.Denominator
                        : intersection.EdgeOffset >= 0 && intersection.EdgeOffset < intersection.Denominator;
                    var lineIntersection = intersection.Denominator < 0
                        ? intersection.LineOffset <= 0 && intersection.LineOffset >= intersection.Denominator
                        : intersection.LineOffset >= 0 && intersection.LineOffset <= intersection.Denominator;

                    if (edgeIntersection && lineIntersection)
                    {
                        intersectionCount++;
                    }
                }

                return intersectionCount == 2;
            }

            #endregion

            public static IEnumerable<(int X, int Y)> GetPixels(Vector2F point0, Vector2F point1)
            {
                // get rasterization area
                LineGetBounds(point0, point1,
                    out var xOffset, out var yOffset,
                    out var xStart, out var xStop, out var xDelta,
                    out var yStart, out var yStop, out var yDelta);

                // get line standard form
                var standardForm = LineGetStandardForm(point0, point1);

                // rasterize
                if (Math.Abs(point1.X - point0.X) >= Math.Abs(point1.Y - point0.Y))
                {
                    #region // x-major

                    var xHit = -1;
                    for (var y = yStart; y != yStop; y += yDelta)
                    {
                        var yHit = false;
                        for (var x = xHit == -1 ? xStart : xHit + 1; x != xStop; x += xDelta)
                        {
                            int xPixel = x + xOffset, yPixel = y + yOffset;
                            if (LineDiamondTest(xPixel, yPixel, point0, point1, standardForm, true))
                            {
                                yield return (xPixel, yPixel);
                                xHit = x;
                                yHit = true;
                            }
                            else if (yHit)
                            {
                                break;
                            }
                        }
                    }

                    #endregion
                }
                else
                {
                    #region // y-major

                    var yHit = -1;
                    for (var x = xStart; x != xStop; x += xDelta)
                    {
                        var xHit = false;
                        for (var y = yHit == -1 ? yStart : yHit + 1; y != yStop; y += yDelta)
                        {
                            int xPixel = x + xOffset, yPixel = y + yOffset;
                            if (LineDiamondTest(xPixel, yPixel, point0, point1, standardForm, false))
                            {
                                yield return (xPixel, yPixel);
                                yHit = y;
                                xHit = true;
                            }
                            else if (xHit)
                            {
                                break;
                            }
                        }
                    }

                    #endregion
                }
            }
        }

        #endregion
    }
}
