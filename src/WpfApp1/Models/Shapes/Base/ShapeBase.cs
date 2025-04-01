using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfApp1.Models.Shapes.Base
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
}
