using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using System.Collections.Generic;
using System.Linq;

namespace DelaunatorSharp.Benchmark
{
    [SimpleJob(RunStrategy.ColdStart, warmupCount:10, targetCount: 10)]
    [HtmlExporter]
    public class GetVoronoiCellsBenchmark
    {
        private Distribution distribution = new Distribution();
        private IPoint[] points;

        [Params(5000)]
        public int Count;

        [ParamsAllValues]
        public Distribution.Type Type { get; set; }

        private Delaunator delaunator;

        [GlobalSetup]
        public void GlobalSetup()
        {
            points = distribution.GetPoints(Type, Count).ToArray();
            delaunator = new Delaunator(points);
        }

        [Benchmark]
        public void GetVoronoiCells() 
        {
            using var enumerator = delaunator.GetVoronoiCells().GetEnumerator();
            while (enumerator.MoveNext()) { }
        }
    }
}
