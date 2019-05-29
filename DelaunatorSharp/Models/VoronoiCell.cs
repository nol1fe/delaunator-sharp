using DelaunatorSharp.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace DelaunatorSharp.Models
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
