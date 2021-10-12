# Delaunator C# 
[![openupm](https://img.shields.io/npm/v/com.nol1fe.delaunator?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.nol1fe.delaunator/)

Fast [Delaunay triangulation](https://en.wikipedia.org/wiki/Delaunay_triangulation) of 2D points implemented in C#.

This code was ported from [Mapbox's Delaunator project](https://github.com/mapbox/delaunator) (JavaScript).
<p float="left" align="middle">
<img src="https://raw.githubusercontent.com/nol1fe/delaunator-sharp/master/Images/Delaunator_Unity_Example.gif" height="400" width="410">
<img src="https://raw.githubusercontent.com/nol1fe/delaunator-sharp/master/Images/Delaunator_Unity_Example_Mesh.gif" height="400" width="410">
</p>


## Documentation

See https://mapbox.github.io/delaunator/ for more information about the `Triangles` and `Halfedges` data structures.


## Unity Installation

Simply edit manifest.json file in your Unity Packages directory 
```
{
  "dependencies": {
    "com.nol1fe.delaunator": "https://github.com/nol1fe/delaunator-sharp.git?path=DelaunatorSharp.Unity",
}
```

## Unity Example

From UnityPackage Menager select Delaunator and press "Import into Project".
<p float="left" align="middle">
<img src="https://raw.githubusercontent.com/nol1fe/delaunator-sharp/master/Images/Delaunator_Package%20Manager.png" height="300" width="450">
</p>


## WPF Example

There is available playground in DelaunatorSharp.WPF.Example project that shows examples of drawing [Voronoi Diagram](https://en.wikipedia.org/wiki/Voronoi_diagram), [Delaunay triangulation](https://en.wikipedia.org/wiki/Delaunay_triangulation) and [Convex Hull](https://en.wikipedia.org/wiki/Convex_hull)

<p float="left" align="middle">
<img src="https://raw.githubusercontent.com/nol1fe/delaunator-sharp/master/Images/Delaunator_Rectangle.png" height="200" width="200">
<img src="https://raw.githubusercontent.com/nol1fe/delaunator-sharp/master/Images/Delaunator_Circle.PNG" height="200" width="200">
</p>

Points were generated with [Poisson Disc Sampling](https://www.jasondavies.com/poisson-disc)
implemented by [UniformPoissonDiskSampler](http://theinstructionlimit.com/fast-uniform-poisson-disk-sampling-in-c)


## Performance
```
BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19043
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores .NET Core SDK=5.0.401
```

|     Method |   Count |     Type |      Mean |    Error |   StdDev |
|----------- |-------- |--------- |----------:|---------:|---------:|
| Delaunator |  100000 |  Uniform |  63.66 ms | 21.68 ms | 14.34 ms |
| Delaunator |  100000 | Gaussian |  62.07 ms | 21.60 ms | 14.29 ms |
| Delaunator |  100000 |     Grid |  46.88 ms | 18.53 ms | 12.25 ms |
| Delaunator | 1000000 |  Uniform | 658.91 ms | 41.11 ms | 27.19 ms |
| Delaunator | 1000000 | Gaussian | 680.02 ms | 96.03 ms | 63.52 ms |
| Delaunator | 1000000 |     Grid | 516.89 ms | 60.64 ms | 40.11 ms |

## NuGet

https://www.nuget.org/packages/Delaunator/

