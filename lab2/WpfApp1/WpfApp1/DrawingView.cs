using System.Collections.Generic;
using System.Windows.Controls;

namespace WpfApp1
{
    class DrawingView
    {
        private Canvas canvas;
        private DrawingController controller;

        public DrawingView(Canvas canvas)
        {
            this.canvas = canvas;
        }

        public void Render(IEnumerable<ShapeBase> shapes, ShapeBase previewShape = null)

        {
            Clear();

            // Рисуем все сохраненные фигуры
            foreach (var shape in shapes)
            {
                var uiShape = shape.Draw();
                canvas.Children.Add(uiShape);
            }

            // Рисуем предпросмотр
            if (previewShape != null)
            {
                var previewUiShape = previewShape.Draw();
                canvas.Children.Add(previewUiShape);
            }
        }

        public void Clear()
        {
            canvas.Children.Clear();
        }

        public void BindController(DrawingController controller)
        {
            this.controller = controller;

            canvas.MouseDown += (sender, e) =>
                controller.HandleMouseDown(e.GetPosition(canvas));

            canvas.MouseMove += (sender, e) =>
                controller.HandleMouseMove(e.GetPosition(canvas));

            canvas.MouseUp += (sender, e) =>
                controller.HandleMouseUp(e.GetPosition(canvas));
        }
    }
}
