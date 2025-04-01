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
}
