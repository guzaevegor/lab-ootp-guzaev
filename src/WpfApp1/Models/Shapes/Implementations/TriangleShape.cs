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
}
