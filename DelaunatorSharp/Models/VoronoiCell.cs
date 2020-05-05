namespace DelaunatorSharp
{
    public struct VoronoiCell : IVoronoiCell
    {
        public IPoint[] Points { get; set; }
        public int Index { get; set; }
        public VoronoiCell(int triangleIndex, IPoint[] points)
        {
            Points = points;
            Index = triangleIndex;
        }
    }
}
