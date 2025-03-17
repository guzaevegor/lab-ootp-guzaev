using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace WpfApp1
{
    class DrawingController
    {
        private List<ShapeBase> shapes = new List<ShapeBase>();
        private Stack<List<ShapeBase>> undoStack = new Stack<List<ShapeBase>>();
        private Stack<List<ShapeBase>> redoStack = new Stack<List<ShapeBase>>();
        private DrawingView view;
        private ShapeBase previewShape;
        private ShapeFactory shapeFactory;
        private ShapeType currentShapeType = ShapeType.Line;
        private bool isDrawing = false;
        private Color strokeColor = Colors.Black;
        private double strokeThickness = 1;
        private Color fillColor = Colors.White;

        public DrawingController(DrawingView view)
        {
            this.view = view;
            this.shapeFactory = new ShapeFactory();
        }

        public void SetShapeType(ShapeType type)
        {
            currentShapeType = type;
        }

        public void SetStrokeColor(Color color)
        {
            strokeColor = color;
        }

        public void SetStrokeThickness(double thickness)
        {
            strokeThickness = thickness;
        }

        public void SetFillColor(Color color)
        {
            fillColor = color;
        }

        public void HandleMouseDown(Point position)
        {
            isDrawing = true;
            previewShape = shapeFactory.CreateShape(currentShapeType, position, position);
            ApplyShapeProperties(previewShape);
            view.Render(shapes, previewShape);
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

        public void SerializeShapes(string filename) { /* Реализация сериализации */ }

        public void LoadPlugin(string path) { /* Реализация загрузки плагинов */ }

        public void UpdateView()
        {
            view.Render(shapes);
        }
    }
}
