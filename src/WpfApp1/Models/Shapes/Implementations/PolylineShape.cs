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
}
