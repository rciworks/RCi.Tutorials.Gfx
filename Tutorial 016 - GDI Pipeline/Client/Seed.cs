using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MathNet.Spatial.Euclidean;
using RCi.Tutorials.Gfx.Materials;
using RCi.Tutorials.Gfx.Mathematics;
using RCi.Tutorials.Gfx.Mathematics.Extensions;

namespace RCi.Tutorials.Gfx.Client
{
    /// <summary>
    /// Class for seeding some defaults.
    /// </summary>
    public static class Seed
    {
        /// <summary>
        /// Collection of polylines representing edges of cube 1x1x1 at (0, 0, 0).
        /// </summary>
        private static readonly Vector3F[][] CubePolylines = new[]
        {
            // bottom
            new[]
            {
                new Vector3F(0, 0, 0),
                new Vector3F(1, 0, 0),
                new Vector3F(1, 1, 0),
                new Vector3F(0, 1, 0),
                new Vector3F(0, 0, 0),
            },
            // top
            new[]
            {
                new Vector3F(0, 0, 1),
                new Vector3F(1, 0, 1),
                new Vector3F(1, 1, 1),
                new Vector3F(0, 1, 1),
                new Vector3F(0, 0, 1),
            },
            // sides
            new[] { new Vector3F(0, 0, 0), new Vector3F(0, 0, 1), },
            new[] { new Vector3F(1, 0, 0), new Vector3F(1, 0, 1), },
            new[] { new Vector3F(1, 1, 0), new Vector3F(1, 1, 1), },
            new[] { new Vector3F(0, 1, 0), new Vector3F(0, 1, 1), },
        }.Select(polyline => MatrixEx.Translate(-0.5, -0.5, -0.5).Transform(polyline).ToArray()).ToArray();

        /// <summary>
        /// Get period leftover ratio in given timespan.
        /// </summary>
        private static double GetTimeSpanPeriodRatio(TimeSpan duration, TimeSpan periodDuration)
        {
            return duration.TotalMilliseconds % periodDuration.TotalMilliseconds / periodDuration.TotalMilliseconds;
        }

        /// <summary>
        /// Get graphical primitives.
        /// </summary>
        public static IEnumerable<IPrimitive> GetPrimitives()
        {
            return GetPrimitivesScreenViewLines()
                .Concat(GetPrimitivesWorldAxis())
                .Concat(GetPrimitivesCubes())
                ;
        }

        /// <summary>
        /// Get primitives showing difference between <see cref="Space.Screen"/> and <see cref="Space.View"/>.
        /// </summary>
        private static IEnumerable<IPrimitive> GetPrimitivesScreenViewLines()
        {
            // screen space
            yield return new Materials.Position.Primitive
            (
                new PrimitiveBehaviour(Space.Screen),
                PrimitiveTopology.LineStrip,
                new Materials.Position.IVertex[]
                {
                    new Materials.Position.Vertex(new Vector3F(3, 20, 0)),
                    new Materials.Position.Vertex(new Vector3F(140, 20, 0)),
                },
                Color.Gray
            );

            // view space
            yield return new Materials.Position.Primitive
            (
                new PrimitiveBehaviour(Space.View),
                PrimitiveTopology.LineStrip,
                new Materials.Position.IVertex[]
                {
                    new Materials.Position.Vertex(new Vector3F(-0.9f, -0.9f, 0)),
                    new Materials.Position.Vertex(new Vector3F(0.9f, -0.9f, 0)),
                },
                Color.Gray
            );
        }

        /// <summary>
        /// Get primitives representing world axis at world origin (each length of 1).
        /// </summary>
        private static IEnumerable<IPrimitive> GetPrimitivesWorldAxis()
        {
            // x axis
            yield return new Materials.Position.Primitive
            (
                new PrimitiveBehaviour(Space.World),
                PrimitiveTopology.LineStrip,
                new Materials.Position.IVertex[]
                {
                    new Materials.Position.Vertex(new Vector3F(0, 0, 0)),
                    new Materials.Position.Vertex(new Vector3F(1, 0, 0)),
                },
                Color.Red
            );

            // y axis
            yield return new Materials.Position.Primitive
            (
                new PrimitiveBehaviour(Space.World),
                PrimitiveTopology.LineStrip,
                new Materials.Position.IVertex[]
                {
                    new Materials.Position.Vertex(new Vector3F(0, 0, 0)),
                    new Materials.Position.Vertex(new Vector3F(0, 1, 0)),
                },
                Color.LawnGreen
            );

            // z axis
            yield return new Materials.Position.Primitive
            (
                new PrimitiveBehaviour(Space.World),
                PrimitiveTopology.LineStrip,
                new Materials.Position.IVertex[]
                {
                    new Materials.Position.Vertex(new Vector3F(0, 0, 0)),
                    new Materials.Position.Vertex(new Vector3F(0, 0, 1)),
                },
                Color.Blue
            );
        }

        /// <summary>
        /// Get some primitives to demonstrate hierarchical matrix multiplication.
        /// </summary>
        private static IEnumerable<IPrimitive> GetPrimitivesCubes()
        {
            var duration = new TimeSpan(DateTime.UtcNow.Ticks);

            // world space bigger cube
            var angle = GetTimeSpanPeriodRatio(duration, new TimeSpan(0, 0, 0, 5)) * Math.PI * 2;
            var matrixModel =
                MatrixEx.Scale(0.5) *
                MatrixEx.Rotate(new UnitVector3D(1, 0, 0), angle) *
                MatrixEx.Translate(1, 0, 0);

            foreach (var cubePolyline in CubePolylines)
            {
                yield return new Materials.Position.Primitive
                (
                    new PrimitiveBehaviour(Space.World),
                    PrimitiveTopology.LineStrip,
                    matrixModel.Transform(cubePolyline).Select(position => new Materials.Position.Vertex(position)).Cast<Materials.Position.IVertex>(),
                    Color.White
                );
            }

            // world space smaller cube
            angle = GetTimeSpanPeriodRatio(duration, new TimeSpan(0, 0, 0, 1)) * Math.PI * 2;
            matrixModel =
                MatrixEx.Scale(0.5) *
                MatrixEx.Rotate(new UnitVector3D(0, 1, 0), angle) *
                MatrixEx.Translate(0, 1, 0) *
                matrixModel;

            foreach (var cubePolyline in CubePolylines)
            {
                yield return new Materials.Position.Primitive
                (
                    new PrimitiveBehaviour(Space.World),
                    PrimitiveTopology.LineStrip,
                    matrixModel.Transform(cubePolyline).Select(position => new Materials.Position.Vertex(position)).Cast<Materials.Position.IVertex>(),
                    Color.Yellow
                );
            }
        }
    }
}
