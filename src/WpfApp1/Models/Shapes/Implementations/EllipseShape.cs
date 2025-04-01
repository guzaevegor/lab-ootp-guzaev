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

    

}
