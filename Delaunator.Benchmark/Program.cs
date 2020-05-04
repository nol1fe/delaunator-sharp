using BenchmarkDotNet.Running;
using System.Linq;

namespace Delaunator.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<DelaunatorBenchmark>();
        }
    }
}
