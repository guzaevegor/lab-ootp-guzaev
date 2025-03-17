using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace WpfApp1
{
    public enum ShapeType
    {
        Line,
        Rectangle,
        Ellipse,
        Polyline,
        Polygon,
        Circle
    }
    class ShapeFactory
    {
        private Dictionary<ShapeType, Func<Point, Point, ShapeBase>> shapeCreators;

        public ShapeFactory()
        {
            InitializeShapeCreators();
        }

        private void InitializeShapeCreators()
        {
            shapeCreators = new Dictionary<ShapeType, Func<Point, Point, ShapeBase>>
            {
                { ShapeType.Line, (start, end) => new ShapeBase.LineShape(start, end) },
                { ShapeType.Rectangle, (start, end) => new ShapeBase.RectangleShape(start, end) },
                { ShapeType.Ellipse, (start, end) =>
                    new ShapeBase.EllipseShape(Math.Abs(end.X - start.X), Math.Abs(end.Y - start.Y)) },
                { ShapeType.Circle, (start, end) => {
                    double radius = Math.Max(Math.Abs(end.X - start.X), Math.Abs(end.Y - start.Y));
                    return new ShapeBase.CircleShape(radius);
                }},
                { ShapeType.Polyline, CreatePolyline },
                { ShapeType.Polygon, CreatePolygon }
            };
        }

        private ShapeBase CreatePolyline(Point start, Point end)
        {
            var points = new System.Windows.Media.PointCollection { start, end };
            return new ShapeBase.PolylineShape(points);
        }

        private ShapeBase CreatePolygon(Point start, Point end)
        {
            var points = new System.Windows.Media.PointCollection { start, end, new Point(start.X, end.Y) };
            return new ShapeBase.PolygonShape(points);
        }

        public ShapeBase CreateShape(ShapeType type, Point startPoint, Point endPoint)
        {
            if (shapeCreators.TryGetValue(type, out var creator))
            {
                var shape = creator(startPoint, endPoint);
                shape.StartPoint = startPoint;
                shape.EndPoint = endPoint;
                return shape;
            }

            throw new ArgumentException($"Unknown shape type: {type}");
        }
    }
}
