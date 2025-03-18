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
        Triangle,
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
                    { ShapeType.Triangle, CreateTriangle },
                    { ShapeType.Line, (start, end) => new LineShape(start, end) },
                    { ShapeType.Rectangle, (start, end) => new RectangleShape(start, end) },
                    { ShapeType.Ellipse, (start, end) => {
                        var ellipse = new EllipseShape(Math.Abs(end.X - start.X), Math.Abs(end.Y - start.Y));
                        ellipse.StartPoint = start;
                        ellipse.TopLeft = new Point(
                            Math.Min(start.X, end.X),
                            Math.Min(start.Y, end.Y)
                        );
                        return ellipse;
                    }},
                    { ShapeType.Circle, (start, end) => {
                        double radius = Math.Max(Math.Abs(end.X - start.X), Math.Abs(end.Y - start.Y));
                        var circle = new CircleShape(radius);
                        circle.StartPoint = start;
                        circle.TopLeft = new Point(
                            Math.Min(start.X, end.X),
                            Math.Min(start.Y, end.Y)
                        );
                        return circle;
                    }},
                    { ShapeType.Polyline, CreatePolyline },
                    { ShapeType.Polygon, CreatePolygon }
                };
        }



        private ShapeBase CreatePolyline(Point start, Point end)
        {
            var points = new System.Windows.Media.PointCollection { start, end };
            return new PolylineShape(points);
        }

        private ShapeBase CreatePolygon(Point start, Point end)
        {
            var points = new System.Windows.Media.PointCollection { start, end, new Point(start.X, end.Y) };
            return new PolygonShape(points);
        }
        private ShapeBase CreateTriangle(Point start, Point end)
        {
            var points = new PointCollection {
        start,
        new Point((start.X + end.X)/2, start.Y - Math.Abs(end.Y - start.Y)),
        end
    };
            return new TriangleShape(points);
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
