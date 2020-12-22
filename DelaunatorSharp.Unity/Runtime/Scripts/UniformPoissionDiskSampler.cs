using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DelaunatorSharp.Unity
{
    // http://theinstructionlimit.com/fast-uniform-poisson-disk-sampling-in-c <- copied from Renaud Bédard

    // Adapated from java source by Herman Tulleken
    // http://www.luma.co.za/labs/2008/02/27/poisson-disk-sampling/

    // The algorithm is from the "Fast Poisson Disk Sampling in Arbitrary Dimensions" paper by Robert Bridson
    // http://www.cs.ubc.ca/~rbridson/docs/bridson-siggraph07-poissondisk.pdf

    public static class UniformPoissonDiskSampler
    {
        public const int DefaultPointsPerIteration = 30;

        static readonly float SquareRootTwo = (float)Mathf.Sqrt(2);

        struct Settings
        {
            public Vector2 TopLeft, LowerRight, Center;
            public Vector2 Dimensions;
            public float? RejectionSqDistance;
            public float MinimumDistance;
            public float CellSize;
            public int GridWidth, GridHeight;
        }

        struct State
        {
            public Vector2?[,] Grid;
            public List<Vector2> ActivePoints, Points;
        }

        public static List<Vector2> SampleCircle(Vector2 center, float radius, float minimumDistance)
        {
            return SampleCircle(center, radius, minimumDistance, DefaultPointsPerIteration);
        }
        public static List<Vector2> SampleCircle(Vector2 center, float radius, float minimumDistance, int pointsPerIteration)
        {
            return Sample(center - new Vector2(radius, radius), center + new Vector2(radius, radius), radius, minimumDistance, pointsPerIteration);
        }

        public static List<Vector2> SampleRectangle(Vector2 topLeft, Vector2 lowerRight, float minimumDistance)
        {
            return SampleRectangle(topLeft, lowerRight, minimumDistance, DefaultPointsPerIteration);
        }
        public static List<Vector2> SampleRectangle(Vector2 topLeft, Vector2 lowerRight, float minimumDistance, int pointsPerIteration)
        {
            return Sample(topLeft, lowerRight, null, minimumDistance, pointsPerIteration);
        }

        static List<Vector2> Sample(Vector2 topLeft, Vector2 lowerRight, float? rejectionDistance, float minimumDistance, int pointsPerIteration)
        {
            var settings = new Settings
            {
                TopLeft = topLeft,
                LowerRight = lowerRight,
                Dimensions = lowerRight - topLeft,
                Center = (topLeft + lowerRight) / 2,
                CellSize = minimumDistance / SquareRootTwo,
                MinimumDistance = minimumDistance,
                RejectionSqDistance = rejectionDistance == null ? null : rejectionDistance * rejectionDistance
            };
            settings.GridWidth = (int)(settings.Dimensions.x / settings.CellSize) + 1;
            settings.GridHeight = (int)(settings.Dimensions.y / settings.CellSize) + 1;

            var state = new State
            {
                Grid = new Vector2?[settings.GridWidth, settings.GridHeight],
                ActivePoints = new List<Vector2>(),
                Points = new List<Vector2>()
            };

            AddFirstPoint(ref settings, ref state);

            while (state.ActivePoints.Count != 0)
            {
                var listIndex = Random.Range(0, state.ActivePoints.Count);

                var point = state.ActivePoints[listIndex];
                var found = false;

                for (var k = 0; k < pointsPerIteration; k++)
                    found |= AddNextPoint(point, ref settings, ref state);

                if (!found)
                    state.ActivePoints.RemoveAt(listIndex);
            }

            return state.Points;
        }

        static void AddFirstPoint(ref Settings settings, ref State state)
        {
            var added = false;
            while (!added)
            {
                var d = Random.value;
                var xr = settings.TopLeft.x + settings.Dimensions.x * d;

                d = Random.value;
                var yr = settings.TopLeft.y + settings.Dimensions.y * d;

                var p = new Vector2((float)xr, (float)yr);
                if (settings.RejectionSqDistance != null && DistanceSquared(settings.Center, p) > settings.RejectionSqDistance)
                    continue;
                added = true;

                var index = Denormalize(p, settings.TopLeft, settings.CellSize);

                state.Grid[(int)index.x, (int)index.y] = p;

                state.ActivePoints.Add(p);
                state.Points.Add(p);
            }
        }

        static bool AddNextPoint(Vector2 point, ref Settings settings, ref State state)
        {
            var found = false;
            var q = GenerateRandomAround(point, settings.MinimumDistance);

            if (q.x >= settings.TopLeft.x && q.x < settings.LowerRight.x &&
                q.y > settings.TopLeft.y && q.y < settings.LowerRight.y &&
                (settings.RejectionSqDistance == null || DistanceSquared(settings.Center, q) <= settings.RejectionSqDistance))
            {
                var qIndex = Denormalize(q, settings.TopLeft, settings.CellSize);
                var tooClose = false;

                for (var i = (int)Mathf.Max(0, qIndex.x - 2); i < Mathf.Min(settings.GridWidth, qIndex.x + 3) && !tooClose; i++)
                    for (var j = (int)Mathf.Max(0, qIndex.y - 2); j < Mathf.Min(settings.GridHeight, qIndex.y + 3) && !tooClose; j++)
                        if (state.Grid[i, j].HasValue && Vector2.Distance(state.Grid[i, j].Value, q) < settings.MinimumDistance)
                            tooClose = true;

                if (!tooClose)
                {
                    found = true;
                    state.ActivePoints.Add(q);
                    state.Points.Add(q);
                    state.Grid[(int)qIndex.x, (int)qIndex.y] = q;
                }
            }
            return found;
        }

        static Vector2 GenerateRandomAround(Vector2 center, float minimumDistance)
        {
            var d = Random.value;
            var radius = minimumDistance + minimumDistance * d;

            d = Random.value;
            var angle = MathHelper.TwoPi * d;

            var newX = radius * Mathf.Sin(angle);
            var newY = radius * Mathf.Cos(angle);

            return new Vector2((float)(center.x + newX), (float)(center.y + newY));
        }

        static Vector2 Denormalize(Vector2 point, Vector2 origin, double cellSize)
        {
            return new Vector2((int)((point.x - origin.x) / cellSize), (int)((point.y - origin.y) / cellSize));
        }
        static float DistanceSquared(Vector2 value1, Vector2 value2)
        {
            float x = value1.x - value2.x;
            float y = value1.y - value2.y;

            return (x * x) + (y * y);
        }
    }

    public static class MathHelper
    {
        public const float Pi = (float)Mathf.PI;
        public const float HalfPi = (float)(Mathf.PI / 2);
        public const float TwoPi = (float)(Mathf.PI * 2);
    }
}