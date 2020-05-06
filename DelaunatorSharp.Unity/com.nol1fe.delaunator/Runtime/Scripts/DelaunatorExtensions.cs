using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DelaunatorSharp;
using System.Linq;
using System;


namespace DelaunatorSharp.Unity.Extensions
{
    public static class DelaunatorExtensions
    {
        public static IPoint[] ToPoints(this IEnumerable<Vector2> vertices) => vertices.Select(vertex => new Point(vertex.x, vertex.y)).OfType<IPoint>().ToArray();
        public static IPoint[] ToPoints(this Transform[] vertices) => vertices.Select(x => x.transform.position).OfType<Vector2>().ToPoints();

        public static Vector2[] ToVectors2(this IEnumerable<IPoint> points) => points.Select(point => point.ToVector2()).ToArray();
        public static Vector3[] ToVectors3(this IEnumerable<IPoint> points) => points.Select(point => point.ToVector3()).ToArray();

        public static Vector2 ToVector2(this IPoint point) => new Vector2((float)point.X, (float)point.Y);
        public static Vector3 ToVector3(this IPoint point) => new Vector3((float)point.X, (float)point.Y);
    }
}