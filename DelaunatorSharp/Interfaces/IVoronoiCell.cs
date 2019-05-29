using System.Collections.Generic;
using System.Windows;

namespace DelaunatorSharp.Interfaces
{
    public interface IVoronoiCell
    {
        IEnumerable<IPoint> Points { get; }
        int Index { get; }
    }
}
