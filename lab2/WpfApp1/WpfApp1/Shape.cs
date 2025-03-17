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
    abstract class ShapeBase
    {

            public Color StrokeColor { get; set; } = Colors.Black;
            public double StrokeThickness { get; set; } = 1;
            public Color FillColor { get; set; } = Colors.White;

            public abstract Shape Draw();
        
            public Point StartPoint { get; set; }
            public Point EndPoint { get; set; }

            // Метод для обновления размеров фигуры при рисовании
            public virtual void UpdateDrawing(Point newEndPoint)
            {
                EndPoint = newEndPoint;
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
        // Класс для прямоугольника
        public class RectangleShape : ShapeBase
        {
            public double Width { get; protected set; }
            public double Height { get; protected set; }
            public Point TopLeft { get; set; }

            public RectangleShape(Point startPoint, Point endPoint)
            {
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


    }
}
