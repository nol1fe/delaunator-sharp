using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DelaunatorSharp;
using System.Linq;
using DelaunatorSharp.Unity.Extensions;
using System;


namespace DelaunatorSharp.Unity.Extensions
{
    public partial class DelaunatorPreview : MonoBehaviour
    {
        [SerializeField] GameObject trianglePointPrefab;
        [SerializeField] GameObject voronoiPointPrefab;

        private List<IPoint> points = new List<IPoint>();
        private GameObject meshObject;

        private Delaunator delaunator;

        private Transform PointsContainer;
        private Transform HullContainer;
        private Transform VoronoiContainer;
        private Transform TrianglesContainer;

        [SerializeField] float voronoiEdgeWidth = .01f;
        [SerializeField] float triangleEdgeWidth = .01f;
        [SerializeField] float hullEdgeWith = .01f;

        [SerializeField] Color triangleEdgeColor = Color.black;
        [SerializeField] Color hullColor = Color.magenta;
        [SerializeField] Color voronoiColor = Color.white;

        [SerializeField] Material meshMaterial;
        [SerializeField] Material lineMaterial;


        [SerializeField] float generationSize = 3;
        [SerializeField] float generationMinDistance = .2f;

        [SerializeField] bool drawTrianglePoints = true;
        [SerializeField] bool drawTriangleEdges = true;
        [SerializeField] bool drawVoronoiPoints = true;
        [SerializeField] bool drawVoronoiEdges = true;
        [SerializeField] bool drawHull = true;
        [SerializeField] bool createMesh = true;


        private void Start()
        {
            Clear();
        }
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                points.Clear();
                Clear();
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                Clear();

                var sampler = UniformPoissonDiskSampler.SampleCircle(Vector2.zero, generationSize, generationMinDistance);
                points = sampler.Select(point => new Vector2(point.x, point.y)).ToPoints().ToList();
                Debug.Log($"Generated Points Count {points.Count}");
                Create();
                return;
            }

            if (!Input.GetMouseButtonDown(0)) return;

            var target = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            points.Add(new Point(target.x, target.y));
            Create();
        }

        private void Create()
        {
            if (points.Count < 3) return;

            Clear();

            delaunator = new Delaunator(points.ToArray());

            CreateMesh();
            CreateTriangle();
            CreateHull();
            CreateVoronoi();
        }

        private void Clear()
        {
            CreateNewContainers();

            if (meshObject != null)
            {
                Destroy(meshObject);
            }

            delaunator = null;
        }

        private void CreateTriangle()
        {
            if (delaunator == null) return;

            delaunator.ForEachTriangleEdge(edge =>
            {
                if (drawTriangleEdges)
                {
                    CreateLine(TrianglesContainer, $"TriangleEdge - {edge.Index}", new Vector3[] { edge.P.ToVector3(), edge.Q.ToVector3() }, triangleEdgeColor, triangleEdgeWidth, 0);
                }

                if (drawTrianglePoints)
                {
                    var pointGameObject = Instantiate(trianglePointPrefab, PointsContainer);
                    pointGameObject.transform.SetPositionAndRotation(edge.P.ToVector3(), Quaternion.identity);
                }
            });
        }

        private void CreateHull()
        {
            if (!drawHull) return;
            if (delaunator == null) return;

            CreateNewHullContainer();

            foreach (var edge in delaunator.GetHullEdges())
            {
                CreateLine(HullContainer, $"Hull Edge", new Vector3[] { edge.P.ToVector3(), edge.Q.ToVector3() }, hullColor, hullEdgeWith, 3);
            }
        }

        private void CreateVoronoi()
        {
            if (delaunator == null) return;

            delaunator.ForEachVoronoiEdge(edge =>
            {
                if (drawVoronoiEdges)
                {
                    CreateLine(VoronoiContainer, $"Voronoi Edge", new Vector3[] { edge.P.ToVector3(), edge.Q.ToVector3() }, voronoiColor, voronoiEdgeWidth, 2);
                }
                if (drawVoronoiPoints)
                {
                    var pointGameObject = Instantiate(voronoiPointPrefab, PointsContainer);
                    pointGameObject.transform.SetPositionAndRotation(edge.P.ToVector3(), Quaternion.identity);
                }
            });
        }

        private void CreateLine(Transform container, string name, Vector3[] points, Color color, float width, int order = 1)
        {
            var lineGameObject = new GameObject(name);
            lineGameObject.transform.parent = container;
            var lineRenderer = lineGameObject.AddComponent<LineRenderer>();

            lineRenderer.SetPositions(points);

            lineRenderer.material = lineMaterial ?? new Material(Shader.Find("Standard"));
            lineRenderer.startColor = color;
            lineRenderer.endColor = color;
            lineRenderer.startWidth = width;
            lineRenderer.endWidth = width;
            lineRenderer.sortingOrder = order;
        }

        private void CreateMesh()
        {
            if (!createMesh) return;

            if (meshObject != null)
            {
                Destroy(meshObject);
            }

            var mesh = new Mesh
            {
                vertices = delaunator.Points.ToVectors3(),
                triangles = delaunator.Triangles
            };

            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            meshObject = new GameObject("DelaunatorMesh");
            var meshRenderer = meshObject.AddComponent<MeshRenderer>();
            meshRenderer.sharedMaterial = meshMaterial ?? new Material(Shader.Find("Standard"));
            var meshFilter = meshObject.AddComponent<MeshFilter>();
            meshFilter.mesh = mesh;
        }

        private void CreateNewContainers()
        {
            CreateNewPointsContainer();
            CreateNewTrianglesContainer();
            CreateNewVoronoiContainer();
            CreateNewHullContainer();
        }

        private void CreateNewPointsContainer()
        {
            if (PointsContainer != null)
            {
                Destroy(PointsContainer.gameObject);
            }

            PointsContainer = new GameObject(nameof(PointsContainer)).transform;
        }

        private void CreateNewTrianglesContainer()
        {
            if (TrianglesContainer != null)
            {
                Destroy(TrianglesContainer.gameObject);
            }

            TrianglesContainer = new GameObject(nameof(TrianglesContainer)).transform;
        }

        private void CreateNewHullContainer()
        {
            if (HullContainer != null)
            {
                Destroy(HullContainer.gameObject);
            }

            HullContainer = new GameObject(nameof(HullContainer)).transform;
        }

        private void CreateNewVoronoiContainer()
        {
            if (VoronoiContainer != null)
            {
                Destroy(VoronoiContainer.gameObject);
            }

            VoronoiContainer = new GameObject(nameof(VoronoiContainer)).transform;
        }
    }
}