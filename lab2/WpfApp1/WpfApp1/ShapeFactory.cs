using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace WpfApp1
{
    // Делегат для создания фигур
    public delegate ShapeBase ShapeCreator(Point startPoint, Point endPoint);

    class ShapeFactory
    {
        private Dictionary<string, ShapeCreator> shapeCreators;

        public ShapeFactory()
        {
            InitializeShapeCreators();
        }

        private void InitializeShapeCreators()
        {
            shapeCreators = new Dictionary<string, ShapeCreator>
        {
            { "Line", (start, end) => new LineShape(start, end) },
            { "Rectangle", (start, end) => new RectangleShape(start, end) },
            { "Ellipse", (start, end) => {
                var ellipse = new EllipseShape(Math.Abs(end.X - start.X), Math.Abs(end.Y - start.Y));
                ellipse.StartPoint = start;
                ellipse.TopLeft = new Point(
                    Math.Min(start.X, end.X),
                    Math.Min(start.Y, end.Y)
                );
                return ellipse;
            }},
            { "Circle", (start, end) => {
                double radius = Math.Max(Math.Abs(end.X - start.X), Math.Abs(end.Y - start.Y));
                var circle = new CircleShape(radius);
                circle.StartPoint = start;
                circle.TopLeft = new Point(
                    Math.Min(start.X, end.X),
                    Math.Min(start.Y, end.Y)
                );
                return circle;
            }},
            { "Polyline", CreatePolyline },
            { "Polygon", CreatePolygon },
            { "Triangle", CreateTriangle }
        };
        }

        // Методы создания фигур остаются прежними
        private ShapeBase CreatePolyline(Point start, Point end)
        {
            var points = new PointCollection { start, end };
            return new PolylineShape(points);
        }

        private ShapeBase CreatePolygon(Point start, Point end)
        {
            var points = new PointCollection { start, end, new Point(start.X, end.Y) };
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

        // Добавляем метод для регистрации новых фигур
        public void RegisterShapeCreator(string shapeName, ShapeCreator creator)
        {
            shapeCreators[shapeName] = creator;
        }

        // Метод для получения списка доступных типов фигур
        public IEnumerable<string> GetAvailableShapeTypes() => shapeCreators.Keys;

        // Метод для создания фигуры
        public ShapeBase CreateShape(string shapeType, Point startPoint, Point endPoint)
        {
            if (shapeCreators.TryGetValue(shapeType, out var creator))
            {
                var shape = creator(startPoint, endPoint);
                shape.StartPoint = startPoint;
                shape.EndPoint = endPoint;
                return shape;
            }
            throw new ArgumentException($"Unknown shape type: {shapeType}");
        }
    }

}
