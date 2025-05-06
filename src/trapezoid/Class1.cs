using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using WpfApp1;
using WpfApp1.Factories; 
using WpfApp1.Models.Shapes.Base;

namespace TrapezoidPlugin
{
    public class TrapezoidShape : ShapeBase
    {
        public TrapezoidShape(Point startPoint, Point endPoint)
        {
            StartPoint = startPoint;
            EndPoint = endPoint;
            TopLeft = new Point(
                Math.Min(startPoint.X, endPoint.X),
                Math.Min(startPoint.Y, endPoint.Y)
            );
            Width = Math.Abs(endPoint.X - startPoint.X);
            Height = Math.Abs(endPoint.Y - startPoint.Y);
        }

        public override Shape Draw()
        {
            Polygon polygon = new Polygon();

            // Расчет точек трапеции (верхняя сторона меньше на 20% с каждой стороны)
            double inset = Width * 0.2;
            Point[] points = new Point[4];
            points[0] = new Point(TopLeft.X + inset, TopLeft.Y);
            points[1] = new Point(TopLeft.X + Width - inset, TopLeft.Y);
            points[2] = new Point(TopLeft.X + Width, TopLeft.Y + Height);
            points[3] = new Point(TopLeft.X, TopLeft.Y + Height);

            polygon.Points = new PointCollection(points);
            polygon.Stroke = new SolidColorBrush(StrokeColor);
            polygon.StrokeThickness = StrokeThickness;
            polygon.Fill = new SolidColorBrush(FillColor);

            return polygon;
        }
    }

    public class TrapezoidPlugin : IShapePlugin
    {
        public string ShapeName => "Trapezoid";

        public ShapeCreator GetShapeCreator()
        {
            return (startPoint, endPoint) => new TrapezoidShape(startPoint, endPoint);
        }

        public bool SupportsCorners => false;
    }
}
