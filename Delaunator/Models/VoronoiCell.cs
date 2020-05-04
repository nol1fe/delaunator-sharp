using System.Collections.Generic;

namespace Delaunator
{
    public struct VoronoiCell : IVoronoiCell
    {
        public IEnumerable<IPoint> Points { get; set; }
        public int Index { get; set; }
        public VoronoiCell(int triangleIndex, IEnumerable<IPoint> points)
        {
            Points = points;
            Index = triangleIndex;
        }
    }
}
