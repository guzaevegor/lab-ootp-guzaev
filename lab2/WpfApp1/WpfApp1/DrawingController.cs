using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace WpfApp1
{
    class DrawingController
    {
        // Заменяем тип на строку
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
        public void RegisterShapeCreator(string shapeName, ShapeCreator creator)
        {
            shapeFactory.RegisterShapeCreator(shapeName, creator);
            // После регистрации обновляем представление
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
                // Код для режима заливки остается без изменений
                // Проходим по всем фигурам от верхней к нижней
                for (int i = shapes.Count - 1; i >= 0; i--)
                {
                    var shape = shapes[i];
                    // Проверяем, попадает ли точка в фигуру
                    if (IsPointInShape(shape, position))
                    {
                        // Применяем цвет заливки
                        shape.FillColor = fillColor;
                        // Перерисовываем
                        view.Render(shapes);
                        return;
                    }
                }
            }
            else if (isCornerMode && cornerPoint.HasValue)
            {
                // Если в режиме угла и есть точка угла, начинаем ломаную
                isDrawing = true;
                isCornerMode = false;

                // Создаем полилинию с начальной точкой в углу
                var points = new PointCollection { cornerPoint.Value, position };
                previewShape = new PolylineShape(points);
                ApplyShapeProperties(previewShape);
                previewShape.StartPoint = cornerPoint.Value;
                previewShape.EndPoint = position;

                view.Render(shapes, previewShape);
                view.CaptureMouseForCanvas();

                // Сбрасываем точку угла после начала рисования
                cornerPoint = null;
            }
            else
            {
                // Стандартное поведение для рисования
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

        // Новый метод для создания угла
        public void CreateCornerForPolyline(Point position)
        {
            if (currentShapeType == "Polyline") // Теперь сравниваем строки
            {
                cornerPoint = position;
                isCornerMode = true;

                // Можно добавить визуальный маркер точки угла
                var marker = shapeFactory.CreateShape("Circle",
                    new Point(position.X - 3, position.Y - 3),
                    new Point(position.X + 3, position.Y + 3));
                marker.StrokeColor = Colors.Red;
                marker.FillColor = Colors.Red;

                view.Render(shapes, marker);
            }
        }

        public void SerializeShapes(string filename) { /* Реализация сериализации */ }

        public void LoadPlugin(string path)
        {
            try
            {
                // Здесь будет код загрузки плагинов
                // Для примера просто показываем сообщение
                MessageBox.Show($"Загрузка плагина: {path}");

                // Тут должен быть код для загрузки сборки и поиска реализаций плагинов
                // Assembly assembly = Assembly.LoadFrom(path);
                // ...
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки плагина: {ex.Message}", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void UpdateView()
        {
            view.Render(shapes);
        }
    }
}
