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
