using System;
using System.Collections.Generic;
using System.Linq;

namespace Delaunator.Benchmark
{
    public class Distribution
    {
        private Random random = new Random();
        public enum Type { Uniform, Gaussian, Grid };

        public IEnumerable<IPoint> GetPoints(Type type, int count)
        {
            switch (type)
            {
                case Type.Uniform: return Uniform(count);
                case Type.Gaussian: return Gaussian(count);
                case Type.Grid: return Grid(count);
            }

            return Enumerable.Empty<IPoint>();
        }

        public IEnumerable<IPoint> Uniform(int count)
        {
            for (var i = 0; i < count; i++)
                yield return new Point(random.NextDouble() * Math.Pow(10, 3), random.NextDouble() * Math.Pow(10, 3));
        }

        public IEnumerable<IPoint> Grid(int count)
        {
            var size = Math.Sqrt(count);
            for (var i = 0; i < size; i++)
                for (var j = 0; j < size; j++)
                    yield return new Point(i, j);
        }
        public IEnumerable<IPoint> Gaussian(int count)
        {
            for (var i = 0; i < count; i++)
                yield return new Point(PseudoNormal() * Math.Pow(10, 3), PseudoNormal() * Math.Pow(10, 3));
        }

        private double PseudoNormal()
        {
            var v = random.NextDouble() + random.NextDouble() + random.NextDouble() + random.NextDouble() + random.NextDouble() + random.NextDouble();
            return Math.Min(0.5 * (v - 3) / 3, 1);
        }
    }

}
