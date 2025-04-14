using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using WpfApp1.Models.Shapes.Base;
using WpfApp1.Models.Shapes.Implementations;

namespace WpfApp1.Factories
{
    public delegate ShapeBase ShapeCreator(Point startPoint, Point endPoint);

    public class ShapeInfo
    {
        public ShapeCreator Creator { get; }
        public bool SupportsCorners { get; }

        public ShapeInfo(ShapeCreator creator, bool supportsCorners = false)
        {
            Creator = creator;
            SupportsCorners = supportsCorners;
        }
    }

    class ShapeFactory
    {
        private Dictionary<string, ShapeInfo> shapeInfos;

        public ShapeFactory()
        {
            InitializeShapeCreators();
        }

        private void InitializeShapeCreators()
        {
            shapeInfos = new Dictionary<string, ShapeInfo>
            {
                { "Line", new ShapeInfo((start, end) => new LineShape(start, end)) },
                { "Rectangle", new ShapeInfo((start, end) => new RectangleShape(start, end)) },
                { "Ellipse", new ShapeInfo((start, end) => {
                    var ellipse = new EllipseShape(Math.Abs(end.X - start.X), Math.Abs(end.Y - start.Y));
                    ellipse.StartPoint = start;
                    ellipse.TopLeft = new Point(
                        Math.Min(start.X, end.X),
                        Math.Min(start.Y, end.Y)
                    );
                    return ellipse;
                }) },
                { "Circle", new ShapeInfo((start, end) => {
                    double radius = Math.Max(Math.Abs(end.X - start.X), Math.Abs(end.Y - start.Y));
                    var circle = new CircleShape(radius);
                    circle.StartPoint = start;
                    circle.TopLeft = new Point(
                        Math.Min(start.X, end.X),
                        Math.Min(start.Y, end.Y)
                    );
                    return circle;
                }) },
                { "Polyline", new ShapeInfo(CreatePolyline, true) },
                { "Polygon", new ShapeInfo(CreatePolygon) },
                { "Triangle", new ShapeInfo(CreateTriangle) }
            };
        }

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

        public bool SupportsCorners(string shapeType)
        {
            return shapeInfos.TryGetValue(shapeType, out var info) && info.SupportsCorners;
        }

        public void RegisterShapeCreator(string shapeName, ShapeCreator creator, bool supportsCorners = false)
        {
            shapeInfos[shapeName] = new ShapeInfo(creator, supportsCorners);
        }

        public IEnumerable<string> GetAvailableShapeTypes() => shapeInfos.Keys;

        public ShapeBase CreateShape(string shapeType, Point startPoint, Point endPoint)
        {
            if (shapeInfos.TryGetValue(shapeType, out var info))
            {
                var shape = info.Creator(startPoint, endPoint);
                shape.StartPoint = startPoint;
                shape.EndPoint = endPoint;
                return shape;
            }
            throw new ArgumentException($"Unknown shape type: {shapeType}");
        }
    }
}
