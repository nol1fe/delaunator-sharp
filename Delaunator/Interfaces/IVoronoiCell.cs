using System.Collections.Generic;

namespace Delaunator
{
    public interface IVoronoiCell
    {
        IPoint[] Points { get; }
        int Index { get; }
    }
}
