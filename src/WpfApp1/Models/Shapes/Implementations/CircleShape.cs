using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using WpfApp1.Models.Shapes.Base;

namespace WpfApp1.Models.Shapes.Implementations
{
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
}
