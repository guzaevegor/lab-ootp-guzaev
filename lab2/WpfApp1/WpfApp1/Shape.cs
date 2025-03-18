using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    public abstract class ShapeBase
    {
        public Color StrokeColor { get; set; } = Colors.Black;
        public double StrokeThickness { get; set; } = 1;
        public Color FillColor { get; set; } = Colors.White;

        public abstract Shape Draw();

        public Point StartPoint { get; set; }
        public Point EndPoint { get; set; }
        public double Width { get; protected set; }
        public double Height { get; protected set; }
        public Point TopLeft { get; set; }

        public virtual void UpdateDrawing(Point newEndPoint)
        {
            EndPoint = newEndPoint;
            Width = Math.Abs(EndPoint.X - StartPoint.X);
            Height = Math.Abs(EndPoint.Y - StartPoint.Y);
            TopLeft = new Point(
                Math.Min(StartPoint.X, EndPoint.X),
                Math.Min(StartPoint.Y, EndPoint.Y)
            );
        }
    }

    // Абстрактный базовый класс для эллиптических фигур
    public abstract class EllipseShapeBase : ShapeBase
    {
    }

    // Класс для эллипса
    public class EllipseShape : EllipseShapeBase
    {
        public EllipseShape(double width, double height)
        {
            Width = width;
            Height = height;
            TopLeft = new Point(0, 0);
            StartPoint = TopLeft;
            EndPoint = new Point(width, height);
        }

        public override Shape Draw()
        {
            var ellipse = new Ellipse
            {
                Width = Width,
                Height = Height,
                Stroke = new SolidColorBrush(StrokeColor),
                StrokeThickness = StrokeThickness,
                Fill = new SolidColorBrush(FillColor)
            };
            Canvas.SetLeft(ellipse, TopLeft.X);
            Canvas.SetTop(ellipse, TopLeft.Y);
            return ellipse;
        }

        public override void UpdateDrawing(Point newEndPoint)
        {
            EndPoint = newEndPoint;
            Width = Math.Abs(EndPoint.X - StartPoint.X);
            Height = Math.Abs(EndPoint.Y - StartPoint.Y);
            TopLeft = new Point(
                Math.Min(StartPoint.X, EndPoint.X),
                Math.Min(StartPoint.Y, EndPoint.Y)
            );
        }
    }

    // Класс для круга
    public class CircleShape : EllipseShapeBase
    {
        public CircleShape(double radius)
        {
            Width = radius;
            Height = radius;
            TopLeft = new Point(0, 0);
            StartPoint = TopLeft;
            EndPoint = new Point(radius, radius);
        }

        public override void UpdateDrawing(Point newEndPoint)
        {
            EndPoint = newEndPoint;
            double radius = Math.Max(
                Math.Abs(EndPoint.X - StartPoint.X),
                Math.Abs(EndPoint.Y - StartPoint.Y)
            );
            Width = radius;
            Height = radius;
            TopLeft = new Point(
                Math.Min(StartPoint.X, EndPoint.X),
                Math.Min(StartPoint.Y, EndPoint.Y)
            );
        }

        public override Shape Draw()
        {
            var circle = new Ellipse
            {
                Width = Width,
                Height = Height,
                Stroke = new SolidColorBrush(StrokeColor),
                StrokeThickness = StrokeThickness,
                Fill = new SolidColorBrush(FillColor)
            };
            Canvas.SetLeft(circle, TopLeft.X);
            Canvas.SetTop(circle, TopLeft.Y);
            return circle;
        }
    }

    // Абстрактный базовый класс для замкнутых многоугольников
    public abstract class PolygonShapeBase : ShapeBase
    {
        public PointCollection Points { get; protected set; }
    }

    // Класс для прямоугольника
    public class RectangleShape : ShapeBase
    {
        public RectangleShape(Point startPoint, Point endPoint)
        {
            StartPoint = startPoint;
            EndPoint = endPoint;
            Width = Math.Abs(endPoint.X - startPoint.X);
            Height = Math.Abs(endPoint.Y - startPoint.Y);
            TopLeft = new Point(
                Math.Min(startPoint.X, endPoint.X),
                Math.Min(startPoint.Y, endPoint.Y)
            );
        }

        public override Shape Draw()
        {
            var rectangle = new Rectangle
            {
                Width = Width,
                Height = Height,
                Stroke = new SolidColorBrush(StrokeColor),
                StrokeThickness = StrokeThickness,
                Fill = new SolidColorBrush(FillColor)
            };
            Canvas.SetLeft(rectangle, TopLeft.X);
            Canvas.SetTop(rectangle, TopLeft.Y);
            return rectangle;
        }

        public override void UpdateDrawing(Point newEndPoint)
        {
            EndPoint = newEndPoint;
            Width = Math.Abs(EndPoint.X - StartPoint.X);
            Height = Math.Abs(EndPoint.Y - StartPoint.Y);
            TopLeft = new Point(
                Math.Min(StartPoint.X, EndPoint.X),
                Math.Min(StartPoint.Y, EndPoint.Y)
            );
        }
    }

    // Класс для произвольного многоугольника
    public class PolygonShape : PolygonShapeBase
    {
        public PolygonShape(PointCollection points)
        {
            Points = points;
            if (points.Count > 0)
            {
                StartPoint = points[0];
                EndPoint = points.Count > 1 ? points[1] : points[0];
                UpdateFromPoints();
            }
        }

        protected void UpdateFromPoints()
        {
            if (Points.Count == 0) return;

            double minX = Points.Min(p => p.X);
            double minY = Points.Min(p => p.Y);
            double maxX = Points.Max(p => p.X);
            double maxY = Points.Max(p => p.Y);

            TopLeft = new Point(minX, minY);
            Width = maxX - minX;
            Height = maxY - minY;
        }

        public override void UpdateDrawing(Point newEndPoint)
        {
            EndPoint = newEndPoint;
            base.UpdateDrawing(newEndPoint);

            Points = new PointCollection {
                StartPoint,
                new Point(EndPoint.X, StartPoint.Y),
                EndPoint,
                new Point(StartPoint.X, EndPoint.Y)
            };
            UpdateFromPoints();
        }

        public override Shape Draw()
        {
            return new Polygon
            {
                Points = Points,
                Stroke = new SolidColorBrush(StrokeColor),
                StrokeThickness = StrokeThickness,
                Fill = new SolidColorBrush(FillColor)
            };
        }
    }

    // Класс для треугольника
    public class TriangleShape : PolygonShapeBase
    {
        public TriangleShape(PointCollection points)
        {
            Points = points;
            if (points.Count > 0)
            {
                StartPoint = points[0];
                EndPoint = points.Count > 1 ? points[1] : points[0];
                UpdateFromPoints();
            }
        }

        public override void UpdateDrawing(Point newEndPoint)
        {
            EndPoint = newEndPoint;

            Points = new PointCollection {
                StartPoint,                        // Левая нижняя точка
                new Point((StartPoint.X + EndPoint.X) / 2, StartPoint.Y - Math.Abs(EndPoint.Y - StartPoint.Y)), // Верхняя точка
                EndPoint                           // Правая нижняя точка
            };

            UpdateFromPoints();
        }

        protected void UpdateFromPoints()
        {
            if (Points.Count == 0) return;

            double minX = Points.Min(p => p.X);
            double minY = Points.Min(p => p.Y);
            double maxX = Points.Max(p => p.X);
            double maxY = Points.Max(p => p.Y);

            TopLeft = new Point(minX, minY);
            Width = maxX - minX;
            Height = maxY - minY;
        }

        public override Shape Draw()
        {
            var triangle = new Polygon
            {
                Points = Points,
                Stroke = new SolidColorBrush(StrokeColor),
                StrokeThickness = StrokeThickness,
                Fill = new SolidColorBrush(FillColor)
            };
            return triangle;
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
            if (points.Count > 0)
            {
                StartPoint = points[0];
                EndPoint = points.Count > 1 ? points[1] : points[0];
                UpdateFromPoints();
            }
        }

        protected void UpdateFromPoints()
        {
            if (Points.Count == 0) return;

            double minX = Points.Min(p => p.X);
            double minY = Points.Min(p => p.Y);
            double maxX = Points.Max(p => p.X);
            double maxY = Points.Max(p => p.Y);

            TopLeft = new Point(minX, minY);
            Width = maxX - minX;
            Height = maxY - minY;
        }

        public override void UpdateDrawing(Point newEndPoint)
        {
            EndPoint = newEndPoint;
            if (Points.Count > 1)
            {
                Points[1] = newEndPoint;
            }
            UpdateFromPoints();
        }

        public override Shape Draw()
        {
            return new Polyline
            {
                Points = Points,
                Stroke = new SolidColorBrush(StrokeColor),
                StrokeThickness = StrokeThickness
            };
        }
    }

    // Класс для отдельной линии (с двумя точками)
    public class LineShape : PolylineShapeBase
    {
        public LineShape(Point start, Point end)
        {
            StartPoint = start;
            EndPoint = end;
            Points = new PointCollection { start, end };
            UpdateDrawing(end);
        }

        public override void UpdateDrawing(Point newEndPoint)
        {
            EndPoint = newEndPoint;
            if (Points.Count > 1)
            {
                Points[1] = newEndPoint;
            }

            TopLeft = new Point(
                Math.Min(StartPoint.X, EndPoint.X),
                Math.Min(StartPoint.Y, EndPoint.Y)
            );
            Width = Math.Abs(EndPoint.X - StartPoint.X);
            Height = Math.Abs(EndPoint.Y - StartPoint.Y);
        }

        public override Shape Draw()
        {
            return new Line
            {
                X1 = StartPoint.X,
                Y1 = StartPoint.Y,
                X2 = EndPoint.X,
                Y2 = EndPoint.Y,
                Stroke = new SolidColorBrush(StrokeColor),
                StrokeThickness = StrokeThickness
            };
        }
    }
}
