using System.Collections.Generic;

namespace Delaunator
{
    public interface IVoronoiCell
    {
        IEnumerable<IPoint> Points { get; }
        int Index { get; }
    }
}
