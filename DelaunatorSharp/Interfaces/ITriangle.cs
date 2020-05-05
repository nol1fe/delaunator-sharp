using System.Collections.Generic;

namespace DelaunatorSharp
{
    public interface ITriangle
    {
        IEnumerable<IPoint> Points { get; }
        int Index { get; }
    }
}
