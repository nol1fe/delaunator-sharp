using DelaunatorSharp.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace DelaunatorSharp.Models
{
    public struct Edge : IEdge
    {
        public IPoint P { get; set; }
        public IPoint Q { get; set; }
        public int Index { get; set; }

        public Edge(int e, IPoint p, IPoint q)
        {
            Index = e;
            P = p;
            Q = q;
        }
    }
}
