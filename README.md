# Delaunator C#

Fast [Delaunay triangulation](https://en.wikipedia.org/wiki/Delaunay_triangulation) of 2D points implemented in C#.

This code was ported from [Mapbox's Delaunator project](https://github.com/mapbox/delaunator) (JavaScript).

## Documentation

See https://mapbox.github.io/delaunator/ for more information about the `Triangles` and `Halfedges` data structures.


##  Example

There is available playground in DelaunatorSharpExample project that shows examples of drawing [Voronoi Diagram](https://en.wikipedia.org/wiki/Voronoi_diagram), [Delaunay triangulation](https://en.wikipedia.org/wiki/Delaunay_triangulation) and [Convex Hull](https://en.wikipedia.org/wiki/Convex_hull)

<p float="left" align="middle">
<img src="https://github.com/nol1fe/delaunator-sharp/blob/master/Images/Delaunator_Rectangle.png" height="200" width="200">
<img src="https://github.com/nol1fe/delaunator-sharp/blob/master/Images/Delaunator_Circle.PNG" height="200" width="200">
</p>

Points were generated with [Poisson Disc Sampling](https://www.jasondavies.com/poisson-disc)
implemented by [UniformPoissonDiskSampler](http://theinstructionlimit.com/fast-uniform-poisson-disk-sampling-in-c)

## Performance

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.18362.778 (1903/May2019Update/19H1)
Intel Core i5-4460 CPU 3.20GHz (Haswell), 1 CPU, 4 logical and 4 physical cores .NET Core SDK=3.1.201
Job-XCGETU : .NET Core 3.1.3 (CoreCLR 4.700.20.11803, CoreFX 4.700.20.12001), X64 RyuJIT

|   **Count** |     **Type** |        **Mean** |
|-------- |--------- |------------:|
|  100000 |  Uniform |    **85.53 ms** |
|  100000 | Gaussian |    **84.49 ms** |
|  100000 |     Grid |    **71.95 ms** |
|  1000000 |  Uniform | **1,127.57 ms** |
|  1000000 | Gaussian | **1,128.37 ms** |
|  1000000 |     Grid |   **874.02 ms** |
