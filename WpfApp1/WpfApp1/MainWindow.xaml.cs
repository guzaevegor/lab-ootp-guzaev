using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
//https://pastebin.com/Kz4nMujU
namespace WpfApp1
{
    // Общий базовый класс для всех фигур
    public abstract class ShapeBase
    {
        public abstract Shape Draw();
    }


    // Абстрактный базовый класс для эллиптических фигур
    public abstract class EllipseShapeBase : ShapeBase
    {
        public double Width { get; protected set; }
        public double Height { get; protected set; }
    }

    // Класс для эллипса
    public class EllipseShape : EllipseShapeBase
    {
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

    // Класс для круга (используем те же параметры, что и для эллипса)
    public class CircleShape : EllipseShapeBase
    {
        public CircleShape(double radius)
        {
            Width = radius;
            Height = radius;
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


    // Абстрактный базовый класс для замкнутых многоугольников
    public abstract class PolygonShapeBase : ShapeBase
    {
        public PointCollection Points { get; protected set; }
    }

    // Класс для произвольного многоугольника
    public class PolygonShape : PolygonShapeBase
    {
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

    // Класс для треугольника
    public class TriangleShape : PolygonShapeBase
    {
        public TriangleShape()
        {
            Points = new PointCollection
            {
                new Point(50, 150),
                new Point(150, 50),
                new Point(250, 150)
            };
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



    // Абстрактный базовый класс для ломаных линий
    public abstract class PolylineShapeBase : ShapeBase
    {
        public PointCollection Points { get; protected set; }
    }

    // Класс для ломаной линии (многоугольной линии)
    public class PolylineShape : PolylineShapeBase
    {
        public PolylineShape(PointCollection points)
        {
            Points = points;
        }

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

    // Класс для отдельной линии (с двумя точками)
    public class LineShape : PolylineShapeBase
    {
        public LineShape(Point start, Point end)
        {
            Points = new PointCollection { start, end };
        }

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


    // === ГЛАВНОЕ ОКНО ===
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Эллипсы
            var ellipse = new EllipseShape(120, 80).Draw();
            var circle = new CircleShape(80).Draw();

            // Многоугольники
            var triangle = new TriangleShape().Draw();
            var polygon = new PolygonShape(new PointCollection
            {
                new Point(300, 200),
                new Point(350, 150),
                new Point(400, 170),
                new Point(390, 220),
                new Point(320, 230)
            }).Draw();

            // Ломаные
            var line = new LineShape(new Point(50, 50), new Point(200, 50)).Draw();
            var polyline = new PolylineShape(new PointCollection
            {
                new Point(100, 300),
                new Point(150, 250),
                new Point(200, 270),
                new Point(250, 230),
                new Point(300, 280)
            }).Draw();

            // Добавляем фигуры на Canvas
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
