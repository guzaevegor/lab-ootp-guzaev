using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfApp1
{
    public abstract class ShapeBase
    {
        public abstract Shape Draw();
    }

    // === ЭЛЛИПСЫ ===
    public class EllipseShape : ShapeBase
    {
        protected double Width;
        protected double Height;

        public EllipseShape(double width, double height)
        {
            Width = width;
            Height = height;
        }

        public override Shape Draw()
        {
            return new Ellipse
            {
                Width = Width,
                Height = Height,
                Stroke = Brushes.Blue,
                StrokeThickness = 2,
                Fill = Brushes.LightBlue
            };
        }
    }

    public class CircleShape : EllipseShape
    {
        public CircleShape(double radius) : base(radius, radius) { }
    }

    // === МНОГОУГОЛЬНИКИ ===
    // можно выделить отдельный класс под линии, так как это будет чуть логичнее для 
    public class PolygonShape : ShapeBase
    {
        protected PointCollection Points;

        public PolygonShape(PointCollection points)
        {
            Points = points;
        }

        public override Shape Draw()
        {
            return new Polygon
            {
                Points = Points,
                Stroke = Brushes.Green,
                StrokeThickness = 2,
                Fill = Brushes.LightGreen
            };
        }
    }

    public class TriangleShape : PolygonShape
    {
        public TriangleShape() : base(new PointCollection
        {
            new Point(50, 150),
            new Point(150, 50),
            new Point(250, 150)
        })
        { }
    }

    public class LineShape : PolygonShape
    {
        public LineShape(Point start, Point end) : base(new PointCollection { start, end }) { }

        public override Shape Draw()
        {
            return new Line
            {
                X1 = Points[0].X,
                Y1 = Points[0].Y,
                X2 = Points[1].X,
                Y2 = Points[1].Y,
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };
        }
    }

    public class PolylineShape : PolygonShape
    {
        public PolylineShape(PointCollection points) : base(points) { }

        public override Shape Draw()
        {
            return new Polyline
            {
                Points = Points,
                Stroke = Brushes.Red,
                StrokeThickness = 2
            };
        }
    }

    // === ГЛАВНОЕ ОКНО ===
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var ellipse = new EllipseShape(120, 80).Draw();
            var circle = new CircleShape(80).Draw();
            var triangle = new TriangleShape().Draw();
            var polygon = new PolygonShape(new PointCollection
            {
                new Point(300, 200),
                new Point(350, 150),
                new Point(400, 170),
                new Point(390, 220),
                new Point(320, 230)
            }).Draw();
            var line = new LineShape(new Point(50, 50), new Point(200, 50)).Draw();
            var polyline = new PolylineShape(new PointCollection
            {
                new Point(100, 300),
                new Point(150, 250),
                new Point(200, 270),
                new Point(250, 230),
                new Point(300, 280)
            }).Draw();

            myCanvas.Children.Add(ellipse);
            Canvas.SetLeft(ellipse, 50);
            Canvas.SetTop(ellipse, 50);

            myCanvas.Children.Add(circle);
            Canvas.SetLeft(circle, 200);
            Canvas.SetTop(circle, 50);

            myCanvas.Children.Add(triangle);
            Canvas.SetLeft(triangle, 100);
            Canvas.SetTop(triangle, 200);

            myCanvas.Children.Add(polygon);
            Canvas.SetLeft(polygon, 250);
            Canvas.SetTop(polygon, 250);

            myCanvas.Children.Add(line);

            myCanvas.Children.Add(polyline);
        }
    }
}
