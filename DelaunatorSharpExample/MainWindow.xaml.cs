using DelaunatorSharp;
using DelaunatorSharp.Interfaces;
using DelaunatorSharp.Models;
using SlimDX;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DelaunatorSharpExample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        private Delaunator delaunator;

        private Brush TriangleBrush = Brushes.Black;
        private Brush VoronoiBrush = Brushes.White;
        private Brush VoronoiCircleBrush = Brushes.Blue;
        private Brush TriangleCircleBrush = Brushes.Red;

        private const string TimeFormat = @"hh\:mm\:ss";
        #region Observables
        private IObservable<Point> MouseMoveStream => Observable
            .FromEventPattern<MouseEventArgs>(this, nameof(MouseMove))
            .Select(x => x.EventArgs.GetPosition(this))
            .Select(point => new Point(point.X, point.Y));
        private IObservable<Point> MouseDownStream => Observable
            .FromEventPattern<MouseEventArgs>(this, nameof(MouseLeftButtonDown))
            .Select(evt => evt.EventArgs.GetPosition(this))
            .Select(point => new Point(point.X, point.Y));
        private IObservable<TimeSpan> Interval(double time = 1) => Observable
            .Interval(TimeSpan.FromSeconds(time))
            .TimeInterval()
            .Scan(TimeSpan.Zero, (result, item) => result += item.Interval)
            .ObserveOn(Dispatcher);
        #endregion Observables
        private bool IsLengthOfPointsValid => Points.Count > 2;
        private readonly ObservableCollection<IPoint> Points = new ObservableCollection<IPoint>();

        public MainWindow()
        {
            InitializeComponent();
            Interval(1).Subscribe(x => ApplicationTime.Content = x.ToString(TimeFormat));

            InitializeMouseStreams();

            GenerateSamples();
            DrawDiagram();
        }
        private void InitializeMouseStreams()
        {
            MouseMoveStream.Subscribe(point => MousePosition.Content = point.ToString());

            MouseDownStream.Subscribe(x =>
            {
                Points.Add(x);
                if (IsLengthOfPointsValid)
                {
                    DrawDiagram();
                }
            });
        }

        private void GenerateSamples()
        {
            var width = (float)(ActualWidth != 0 ? ActualWidth : Width);
            var height = (float)(ActualHeight != 0 ? ActualHeight : Height);
            var samples = UniformPoissonDiskSampler.SampleCircle(new Vector2(width/2, height/3), 220 , 40).Select(x => new Point(x.X, x.Y));

            foreach (var sample in samples)
            {
                Points.Add(sample);
                DrawCircle(sample);
            }
        }
        private void DrawDiagram()
        {
            if (!IsLengthOfPointsValid) return;
            ClearDiagram();

            delaunator = new Delaunator(Points);
            DrawCircles(Points);
            DrawDelaunay();
            DrawVoronoi();
            DrawHull();
        }
        private void DrawVoronoi()
        {
            RefreshDelaunator();
            delaunator.ForEachVoronoiCell((cell) =>
            {
                var polygon = new Polygon
                {
                    Stroke = VoronoiBrush,
                    StrokeThickness = .2,
                    Points = new PointCollection(cell.Points.Select(point => new System.Windows.Point(point.X, point.Y)))
                };

                DrawCircles(cell.Points, VoronoiCircleBrush);
                PlayGround.Children.Add(polygon);
            });
        }
        private void DrawDelaunay()
        {
            RefreshDelaunator();
            delaunator.ForEachTriangleEdge(edge =>
            {
                DrawLine(edge.P, edge.Q, TriangleBrush);
            });
        }
        private void DrawHull()
        {
            RefreshDelaunator();
            foreach (var edge in delaunator.GetHullEdges())
            {
                DrawLine(edge.P, edge.Q, Brushes.BlueViolet, .5);
            }
        }
        private void ClearDiagram()
        {
            PlayGround.Children.Clear();
        }
        private void RefreshDelaunator()
        {
            if (!IsLengthOfPointsValid || Points.Count() == delaunator?.Points.Count()) return;
            delaunator = new Delaunator(Points);
        }

        #region Canvas
        private void DrawCircles(IEnumerable<IPoint> points, Brush brush = null)
        {
            foreach (var point in points)
            {
                DrawCircle(point, brush);
            }
        }
        private void DrawCircle(IPoint point, Brush brush = null)
        {
            var ellipse = new Ellipse
            {
                Width = 4,
                Height = 4,
                Fill = brush ?? TriangleCircleBrush,
                Stroke = brush ?? TriangleCircleBrush,
            };

            PlayGround.Children.Add(ellipse);
            Canvas.SetLeft(ellipse, point.X - ellipse.Width / 2);
            Canvas.SetTop(ellipse, point.Y - ellipse.Height / 2);
        }
        private void DrawLine(IPoint startPoint, IPoint endPoint, Brush stroke, double thickness = .3)
        {
            var line = new Line
            {
                X1 = startPoint.X,
                Y1 = startPoint.Y,

                X2 = endPoint.X,
                Y2 = endPoint.Y,
                Stroke = stroke,
                StrokeThickness = thickness
            };

            PlayGround.Children.Add(line);
        }
        #endregion Canvas

        #region ClickHandlers
        private void OnClearClick(object sender, System.Windows.RoutedEventArgs e)
        {
            Points.Clear();
            ClearDiagram();
        }
        private void OnGenerateSamplesClick(object sender, System.Windows.RoutedEventArgs e) => GenerateSamples();
        private void OnDrawVoronoiClick(object sender, System.Windows.RoutedEventArgs e) => DrawVoronoi();
        private void OnDrawDelaunayClick(object sender, System.Windows.RoutedEventArgs e) => DrawDelaunay();
        private void OnDrawDiagramClick(object sender, System.Windows.RoutedEventArgs e) => DrawDiagram();
        private void OnDrawHullClick(object sender, System.Windows.RoutedEventArgs e) => DrawHull();

        #endregion ClickHandlers
    }
}
