using System.Collections.Generic;

namespace DelaunatorSharp
{
    public struct Triangle : ITriangle
    {
        public int Index { get; set; }

        public IEnumerable<IPoint> Points { get; set; }

        public Triangle(int t, IEnumerable<IPoint> points)
        {
            Points = points;
            Index = t;
        }
    }
}
