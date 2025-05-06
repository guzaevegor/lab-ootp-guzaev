using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using WpfApp1.Factories;
using WpfApp1.Models.Shapes.Base;
using WpfApp1.Models.Shapes.Implementations;
using System.Text.Json;
using System.IO;
using System.Reflection;

namespace WpfApp1
{
    class DrawingController
    {
        private string currentShapeType = "Line";
        private List<ShapeBase> shapes = new List<ShapeBase>();
        private Stack<List<ShapeBase>> undoStack = new Stack<List<ShapeBase>>();
        private Stack<List<ShapeBase>> redoStack = new Stack<List<ShapeBase>>();
        private DrawingView view;
        private ShapeBase previewShape;
        private ShapeFactory shapeFactory;
        private bool isDrawing = false;
        private Color strokeColor = Colors.Black;
        private double strokeThickness = 1;
        private Color fillColor = Colors.White;

        public DrawingController(DrawingView view)
        {
            this.view = view;
            this.shapeFactory = new ShapeFactory();
        }

        public void SetShapeType(string type)
        {
            currentShapeType = type;
        }
        public void RegisterShapeCreator(string shapeName, ShapeCreator creator, bool supportsCorners = false)
        {
            shapeFactory.RegisterShapeCreator(shapeName, creator, supportsCorners);
            UpdateView();
        }
        public IEnumerable<string> GetAvailableShapeTypes()
        {
            return shapeFactory.GetAvailableShapeTypes();
        }
        public void SetStrokeColor(Color color)
        {
            strokeColor = color;
        }

        public void SetStrokeThickness(double thickness)
        {
            strokeThickness = thickness;
        }
        public bool IsFillMode()
        {
            return isFillMode;
        }

        private bool isFillMode = false;
        private bool IsPointInShape(ShapeBase shape, Point point)
        {
            return point.X >= shape.TopLeft.X &&
                   point.X <= shape.TopLeft.X + shape.Width &&
                   point.Y >= shape.TopLeft.Y &&
                   point.Y <= shape.TopLeft.Y + shape.Height;
        }

        public void SetFillMode(bool fillMode)
        {
            isFillMode = fillMode;

            if (fillMode)
            {
                isDrawing = false;
            }
        }

        public void SetFillColor(Color color)
        {
            fillColor = color;
        }
        public bool IsDrawing()
        {
            return isDrawing;
        }

        public void CancelDrawing()
        {
            isDrawing = false;
            isCornerMode = false;
            cornerPoint = null;
            previewShape = null;
            view.Render(shapes);
        }

        public void HandleMouseDown(Point position)
        {
            if (isFillMode)
            {
                for (int i = shapes.Count - 1; i >= 0; i--)
                {
                    var shape = shapes[i];
                    if (IsPointInShape(shape, position))
                    {
                        shape.FillColor = fillColor;
                        view.Render(shapes);
                        return;
                    }
                }
            }
            else if (isCornerMode && cornerPoint.HasValue)
            {
                isDrawing = true;
                isCornerMode = false;

                var points = new PointCollection { cornerPoint.Value, position };
                previewShape = new PolylineShape(points);
                ApplyShapeProperties(previewShape);
                previewShape.StartPoint = cornerPoint.Value;
                previewShape.EndPoint = position;

                view.Render(shapes, previewShape);
                view.CaptureMouseForCanvas();

                cornerPoint = null;
            }
            else
            {
                isDrawing = true;
                previewShape = shapeFactory.CreateShape(currentShapeType, position, position);
                ApplyShapeProperties(previewShape);
                view.Render(shapes, previewShape);
                view.CaptureMouseForCanvas();
            }
        }

        public void HandleMouseMove(Point position)
        {
            if (!isDrawing || previewShape == null) return;

            previewShape.UpdateDrawing(position);
            view.Render(shapes, previewShape);
        }

        public void HandleMouseUp(Point position)
        {
            if (!isDrawing || previewShape == null) return;

            SaveStateForUndo();
            previewShape.UpdateDrawing(position);

            ShapeBase finalShape = shapeFactory.CreateShape(
                currentShapeType,
                previewShape.StartPoint,
                position);

            ApplyShapeProperties(finalShape);
            shapes.Add(finalShape);
            previewShape = null;
            isDrawing = false;
            view.Render(shapes);
            redoStack.Clear();

        }

        private void ApplyShapeProperties(ShapeBase shape)
        {
            shape.StrokeColor = strokeColor;
            shape.StrokeThickness = strokeThickness;
            shape.FillColor = fillColor;
        }

        private void SaveStateForUndo()
        {
            undoStack.Push(new List<ShapeBase>(shapes));
        }

        public void AddShape(ShapeBase shape)
        {
            SaveStateForUndo();
            shapes.Add(shape);
            view.Render(shapes);
        }

        public void RemoveShape(ShapeBase shape)
        {
            SaveStateForUndo();
            shapes.Remove(shape);
            view.Render(shapes);
        }

        public void Undo()
        {
            if (undoStack.Count > 0)
            {
                redoStack.Push(new List<ShapeBase>(shapes));
                shapes = undoStack.Pop();

                if (shapes.LastOrDefault() is PolylineShape polyline)
                {
                    polyline.Points.Clear();
                }
                view.Render(shapes);
            }
        }


        public void Redo()
        {
            if (redoStack.Count > 0)
            {
                undoStack.Push(new List<ShapeBase>(shapes));
                shapes = redoStack.Pop();
                view.Render(shapes);
            }
        }
        private Point? cornerPoint;
        private bool isCornerMode = false;

        public void CreateCornerForPolyline(Point position)
        {
            if (shapeFactory.SupportsCorners(currentShapeType))
            {
                cornerPoint = position;
                isCornerMode = true;

                var marker = shapeFactory.CreateShape("Circle",
                    new Point(position.X - 3, position.Y - 3),
                    new Point(position.X + 3, position.Y + 3));
                marker.StrokeColor = Colors.Red;
                marker.FillColor = Colors.Red;

                view.Render(shapes, marker);
            }
        }


        public void LoadPlugin(string path)
        {
            try
            {
                // Загрузка сборки
                Assembly assembly = Assembly.LoadFrom(path);

                // Поиск типов, реализующих IShapePlugin
                bool pluginFound = false;
                foreach (Type type in assembly.GetTypes())
                {
                    if (typeof(IShapePlugin).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                    {
                        // Создание экземпляра плагина
                        IShapePlugin plugin = (IShapePlugin)Activator.CreateInstance(type);

                        // Регистрация создателя фигуры
                        RegisterShapeCreator(plugin.ShapeName, plugin.GetShapeCreator());

                        MessageBox.Show($"Плагин {plugin.ShapeName} успешно загружен", "Успех",
                            MessageBoxButton.OK, MessageBoxImage.Information);

                        pluginFound = true;

                        // Запускаем событие для обновления UI
                        PluginLoaded?.Invoke(this, new PluginEventArgs { ShapeName = plugin.ShapeName });
                    }
                }

                if (!pluginFound)
                {
                    MessageBox.Show("В сборке не найдены плагины, реализующие IShapePlugin", "Предупреждение",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (ReflectionTypeLoadException ex)
            {
                string errorDetails = "Детали ошибок:\n";
                foreach (var loaderEx in ex.LoaderExceptions)
                {
                    errorDetails += loaderEx.Message + "\n";
                }

                MessageBox.Show($"Ошибка загрузки плагина: {ex.Message}\n{errorDetails}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки плагина: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Событие для обновления UI
        public event EventHandler<PluginEventArgs> PluginLoaded;
        public class PluginEventArgs : EventArgs
        {
            public string ShapeName { get; set; }
        }

        public void UpdateView()
        {
            view.Render(shapes);
        }

        public void SerializeShapes(string filename)
        {
            try
            {
                var serializableShapes = new List<Dictionary<string, object>>();

                foreach (var shape in shapes)
                {
                    var shapeData = new Dictionary<string, object>
                    {
                        ["Type"] = shape.GetType().Name.Replace("Shape", ""),
                        ["StartPoint"] = new { X = shape.StartPoint.X, Y = shape.StartPoint.Y },
                        ["EndPoint"] = new { X = shape.EndPoint.X, Y = shape.EndPoint.Y },
                        ["TopLeft"] = new { X = shape.TopLeft.X, Y = shape.TopLeft.Y },
                        ["Width"] = shape.Width,
                        ["Height"] = shape.Height,
                        ["StrokeColor"] = new { R = shape.StrokeColor.R, G = shape.StrokeColor.G, B = shape.StrokeColor.B, A = shape.StrokeColor.A },
                        ["FillColor"] = new { R = shape.FillColor.R, G = shape.FillColor.G, B = shape.FillColor.B, A = shape.FillColor.A },
                        ["StrokeThickness"] = shape.StrokeThickness
                    };

                    if (shape is PolylineShape polyline)
                    {
                        var points = new List<object>();
                        foreach (var point in polyline.Points)
                        {
                            points.Add(new { X = point.X, Y = point.Y });
                        }
                        shapeData["Points"] = points;
                    }
                    else if (shape is PolygonShape polygon)
                    {
                        var points = new List<object>();
                        foreach (var point in polygon.Points)
                        {
                            points.Add(new { X = point.X, Y = point.Y });
                        }
                        shapeData["Points"] = points;
                    }
                    else if (shape is TriangleShape triangle)
                    {
                        var points = new List<object>();
                        foreach (var point in triangle.Points)
                        {
                            points.Add(new { X = point.X, Y = point.Y });
                        }
                        shapeData["Points"] = points;
                    }

                    serializableShapes.Add(shapeData);
                }

                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(serializableShapes, options);
                File.WriteAllText(filename, json);
                MessageBox.Show("Рисунок успешно сохранен", "Сохранение", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void DeserializeShapes(string filename)
        {
            try
            {
                if (!File.Exists(filename))
                {
                    MessageBox.Show("Файл не найден", "Ошибка",
                                   MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                string json = File.ReadAllText(filename);
                var shapeDatas = JsonSerializer.Deserialize<List<JsonElement>>(json);

                if (shapeDatas == null || !shapeDatas.Any())
                {
                    MessageBox.Show("В файле не найдены фигуры", "Предупреждение",
                                   MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                SaveStateForUndo();

                shapes.Clear();

                foreach (var shapeData in shapeDatas)
                {
                    string typeName = shapeData.GetProperty("Type").GetString();
                    Point startPoint = ExtractPoint(shapeData.GetProperty("StartPoint"));
                    Point endPoint = ExtractPoint(shapeData.GetProperty("EndPoint"));

                    ShapeBase shape = null;

                    if (typeName == "Polyline" || typeName == "Polygon" || typeName == "Triangle")
                    {
                        var pointCollection = new PointCollection();

                        if (shapeData.TryGetProperty("Points", out JsonElement pointsElement))
                        {
                            foreach (var point in pointsElement.EnumerateArray())
                            {
                                pointCollection.Add(ExtractPoint(point));
                            }
                        }

                        if (typeName == "Polyline")
                            shape = new PolylineShape(pointCollection);
                        else if (typeName == "Polygon")
                            shape = new PolygonShape(pointCollection);
                        else if (typeName == "Triangle")
                            shape = new TriangleShape(pointCollection);
                    }
                    else
                    {
                        shape = shapeFactory.CreateShape(typeName, startPoint, endPoint);
                    }

                    if (shape != null)
                    {
                        shape.StartPoint = startPoint;
                        shape.EndPoint = endPoint;
                        shape.TopLeft = ExtractPoint(shapeData.GetProperty("TopLeft"));
                        shape.Width = shapeData.GetProperty("Width").GetDouble();
                        shape.Height = shapeData.GetProperty("Height").GetDouble();
                        shape.StrokeColor = ExtractColor(shapeData.GetProperty("StrokeColor"));
                        shape.FillColor = ExtractColor(shapeData.GetProperty("FillColor"));
                        shape.StrokeThickness = shapeData.GetProperty("StrokeThickness").GetDouble();

                        shapes.Add(shape);
                    }
                }

                view.Render(shapes);
                redoStack.Clear();
                MessageBox.Show("Рисунок успешно загружен", "Загрузка", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке: {ex.Message}", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private Point ExtractPoint(JsonElement element)
        {
            double x = element.GetProperty("X").GetDouble();
            double y = element.GetProperty("Y").GetDouble();
            return new Point(x, y);
        }

        private Color ExtractColor(JsonElement element)
        {
            byte r = element.GetProperty("R").GetByte();
            byte g = element.GetProperty("G").GetByte();
            byte b = element.GetProperty("B").GetByte();
            byte a = element.GetProperty("A").GetByte();
            return Color.FromArgb(a, r, g, b);
        }

    }
}
