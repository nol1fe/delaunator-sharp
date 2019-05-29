using System.Collections.Generic;
using System.Windows;

namespace DelaunatorSharp.Interfaces
{
    public interface ITriangle
    {
        IEnumerable<IPoint> Points { get; }
        int Index { get; }
    }
}
