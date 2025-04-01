using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using WpfApp1.Models.Shapes.Base;

namespace WpfApp1.Models.Shapes.Implementations
{
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
