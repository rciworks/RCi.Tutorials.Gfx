using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using MathNet.Spatial.Euclidean;
using RCi.Tutorials.Gfx.Engine.Render;
using RCi.Tutorials.Gfx.Inputs;
using RCi.Tutorials.Gfx.Materials;
using RCi.Tutorials.Gfx.Mathematics;
using RCi.Tutorials.Gfx.Mathematics.Extensions;
using RCi.Tutorials.Gfx.Utils;

namespace RCi.Tutorials.Gfx.Client
{
    /// <summary>
    /// Class for seeding some defaults.
    /// </summary>
    public static class Seed
    {
        #region // storage

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
        }.Select(polyline => Matrix4DEx.Translate(-0.5, -0.5, -0.5).Transform(polyline)).ToArray();

        /// <summary>
        /// Point cloud of a bunny.
        /// </summary>
        private static readonly IVisual[] PointCloudBunny = new Func<IVisual[]>(() =>
        {
            // adjust for different coordinate system
            var matrix = Matrix4DEx.Scale(10) * Matrix4DEx.Rotate(QuaternionEx.AroundAxis(UnitVector3D.XAxis, Math.PI * 0.5)) * Matrix4DEx.Translate(-1, -1, -0.5);

            // point cloud source: http://graphics.stanford.edu/data/3Dscanrep/
            var vertices = StreamPointCloud_XYZ(@"..\..\..\resources\bunny.xyz")
                .Select(vertex => matrix.Transform(vertex))
                .ToArray();

            return new IVisual[]
            {
                // construct point list (point cloud) model
                new Visual(new Model
                {
                    PrimitiveTopology = PrimitiveTopology.PointList,
                    Positions = vertices,
                },
                new Materials.Position.Material(Space.World, Color.White.ToRgba()))
            };
        })();

        #endregion

        /// <summary>
        /// Get period leftover ratio in given timespan.
        /// </summary>
        private static double GetTimeSpanPeriodRatio(TimeSpan duration, TimeSpan periodDuration)
        {
            return duration.TotalMilliseconds % periodDuration.TotalMilliseconds / periodDuration.TotalMilliseconds;
        }

        /// <summary>
        /// Get graphical models.
        /// </summary>
        public static IEnumerable<IVisual> GetVisuals()
        {
            return new IVisual[0]
                .Concat(GetWorldAxis())
                .Concat(GetScreenViewLines())
                .Concat(GetTriangles())
                .Concat(GetCubes())
                .Concat(GetPointCloud())
                .Concat(GetPositionColorSamples())
                .Concat(GetPositionTextureSamples())
                ;
        }

        /// <summary>
        /// Get models showing difference between <see cref="Space.Screen"/> and <see cref="Space.View"/>.
        /// </summary>
        private static IEnumerable<IVisual> GetScreenViewLines()
        {
            // screen space
            yield return new Visual(new Model
            {
                PrimitiveTopology = PrimitiveTopology.LineList,
                Positions = new[]
                {
                    new Vector3F(3, 20, 0),
                    new Vector3F(140, 20, 0),
                },
            },
            new Materials.Position.Material(Space.Screen, Color.Gray.ToRgba()));

            // view space
            yield return new Visual(new Model
            {
                PrimitiveTopology = PrimitiveTopology.LineList,
                Positions = new[]
                {
                    new Vector3F(-0.9f, -0.9f, 0),
                    new Vector3F(0.9f, -0.9f, 0),
                },
            },
            new Materials.Position.Material(Space.View, Color.Gray.ToRgba()));
        }

        /// <summary>
        /// Get models representing world axis at world origin (each length of 1).
        /// </summary>
        private static IEnumerable<IVisual> GetWorldAxis()
        {
            // x axis
            yield return new Visual(new Model
            {
                PrimitiveTopology = PrimitiveTopology.LineList,
                Positions = new[]
                {
                    new Vector3F(0, 0, 0),
                    new Vector3F(1, 0, 0),
                },
            },
            new Materials.Position.Material(Space.World, Color.Red.ToRgba()));

            // y axis
            yield return new Visual(new Model
            {
                PrimitiveTopology = PrimitiveTopology.LineList,
                Positions = new[]
                {
                    new Vector3F(0, 0, 0),
                    new Vector3F(0, 1, 0),
                },
            },
            new Materials.Position.Material(Space.World, Color.FromArgb(255, 0, 255, 0).ToRgba()));

            // z axis
            yield return new Visual(new Model
            {
                PrimitiveTopology = PrimitiveTopology.LineList,
                Positions = new[]
                {
                    new Vector3F(0, 0, 0),
                    new Vector3F(0, 0, 1),
                },
            },
            new Materials.Position.Material(Space.World, Color.Blue.ToRgba()));
        }

        /// <summary>
        /// Get some models to demonstrate hierarchical matrix multiplication.
        /// </summary>
        private static IEnumerable<IVisual> GetCubes()
        {
            var duration = new TimeSpan(DateTime.UtcNow.Ticks);

            // world space bigger cube
            var angle = GetTimeSpanPeriodRatio(duration, new TimeSpan(0, 0, 0, 5)) * Math.PI * 2;
            var matrixModel =
                Matrix4DEx.Scale(0.5) *
                Matrix4DEx.Rotate(new UnitVector3D(1, 0, 0), angle) *
                Matrix4DEx.Translate(1, 0, 0);

            foreach (var cubePolyline in CubePolylines)
            {
                yield return new Visual(new Model
                {
                    PrimitiveTopology = PrimitiveTopology.LineStrip,
                    Positions = matrixModel.Transform(cubePolyline),
                },
                new Materials.Position.Material(Space.World, Color.White.ToRgba()));
            }

            // world space smaller cube
            angle = GetTimeSpanPeriodRatio(duration, new TimeSpan(0, 0, 0, 1)) * Math.PI * 2;
            matrixModel =
                Matrix4DEx.Scale(0.5) *
                Matrix4DEx.Rotate(new UnitVector3D(0, 1, 0), angle) *
                Matrix4DEx.Translate(0, 1, 0) *
                matrixModel;

            foreach (var cubePolyline in CubePolylines)
            {
                yield return new Visual(new Model
                {
                    PrimitiveTopology = PrimitiveTopology.LineStrip,
                    Positions = matrixModel.Transform(cubePolyline),
                },
                new Materials.Position.Material(Space.World, Color.Yellow.ToRgba()));
            }
        }

        /// <summary>
        /// Get some point cloud models.
        /// </summary>
        private static IEnumerable<IVisual> GetPointCloud()
        {
            return PointCloudBunny;
        }

        /// <summary>
        /// Get some triangle models.
        /// </summary>
        private static IEnumerable<IVisual> GetTriangles()
        {
            yield return new Visual(new Model
            {
                PrimitiveTopology = PrimitiveTopology.TriangleStrip,
                Positions = new[]
                {
                    new Vector3F(0, -1, 0),
                    new Vector3F(1, -1, 0),
                    new Vector3F(0, -2, 0),
                    new Vector3F(1, -2, 0),
                },
            },
            new Materials.Position.Material(Space.World, Color.Goldenrod.ToRgba()));

            yield return new Visual(new Model
            {
                PrimitiveTopology = PrimitiveTopology.TriangleList,
                Positions = new[]
                {
                    new Vector3F(-2, 0, 0),
                    new Vector3F(-2, 1, 0),
                    new Vector3F(-1, 0, 0),
                    new Vector3F(-4, 0, 0),
                    new Vector3F(-4, 1, 0),
                    new Vector3F(-3, 0, 0),
                },
            },
            new Materials.Position.Material(Space.World, Color.Cyan.ToRgba()));
        }

        /// <summary>
        /// Get some models to demonstrate vertex interpolation.
        /// </summary>
        private static IEnumerable<IVisual> GetPositionColorSamples()
        {
            yield return new Visual(new Model
            {
                PrimitiveTopology = PrimitiveTopology.LineList,
                Positions = new[]
                {
                    new Vector3F(1f, 0, 0),
                    new Vector3F(0, 1f, 0),
                },
                Colors = new[]
                {
                    Color.Cyan.ToRgba(),
                    Color.Yellow.ToRgba(),
                },
            },
            new Materials.PositionColor.Material(Space.World));

            yield return new Visual(new Model
            {
                PrimitiveTopology = PrimitiveTopology.TriangleList,
                Positions = new[]
                {
                    new Vector3F(1, 0, 0),
                    new Vector3F(0, 0, 1),
                    new Vector3F(0, 1, 0),
                },
                Colors = new[]
                {
                    Color.Red.ToRgba(),
                    Color.Blue.ToRgba(),
                    Color.FromArgb(255, 0, 255, 0).ToRgba(),
                },
            },
            new Materials.PositionColor.Material(Space.World));
        }

        /// <summary>
        /// Get some textured models.
        /// </summary>
        private static IEnumerable<IVisual> GetPositionTextureSamples()
        {
            yield return new Visual(new Model
            {
                PrimitiveTopology = PrimitiveTopology.TriangleStrip,
                Positions = new[]
                {
                    new Vector3F(0, 0, 0),
                    new Vector3F(0, 1, 0),
                    new Vector3F(1, 0, 0),
                    new Vector3F(1, 1, 0),
                },
                TextureCoordinates = new[]
                {
                    new Vector2F(0, 1),
                    new Vector2F(0, 0),
                    new Vector2F(1, 1),
                    new Vector2F(1, 0),
                },
                TextureResources = new[] { TextureResourceLibrary.GetOrCreateFromFile("../../../resources/checkers.png"), },
            },
            new Materials.PositionTexture.Material(Space.World));
        }

        /// <summary>
        /// Read *.xyz point cloud file.
        /// </summary>
        public static IEnumerable<Vector3F> StreamPointCloud_XYZ(string filePath)
        {
            using (var inputStream = new FileStream(filePath, FileMode.Open))
            {
                var pointCount = inputStream.Length / (4 * 3); // 4 bytes per float, 3 floats per vertex
                using (var reader = new BinaryReader(inputStream))
                {
                    for (var i = 0L; i < pointCount; i++)
                    {
                        yield return new Vector3F(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                    }
                }
            }
        }

        #region // input

        public static void HostInputOnKeyDown(IKeyEventArgs args, IRenderHost renderHost)
        {
            switch (args.Modifiers)
            {
                case Modifiers.Control when args.Key == Key.F12:
                    SeedProjectionTransition.Switch(renderHost);
                    break;

                case Modifiers.None when args.Key == Key.F12:
                    SeedProjectionTransition.Transit(renderHost);
                    break;
            }
        }

        #endregion
    }
}
