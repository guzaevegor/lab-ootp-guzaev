using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfApp1.Models.Shapes.Base;

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
        public void CaptureMouseForCanvas()
        {
            canvas.CaptureMouse();
        }

        public void BindController(DrawingController controller)
        {
            this.controller = controller;

            canvas.MouseDown += (sender, e) => {
                var position = e.GetPosition(canvas);

                // Проверяем, какая кнопка мыши была нажата
                if (e.ChangedButton == MouseButton.Right)
                {
                    controller.CreateCornerForPolyline(position);
                }
                else
                {
                    // Стандартная обработка для левой кнопки
                    controller.HandleMouseDown(position);
                }
            };

            // Остальные обработчики без изменений
            canvas.MouseMove += (sender, e) =>
                controller.HandleMouseMove(e.GetPosition(canvas));

            canvas.MouseUp += (sender, e) => {
                controller.HandleMouseUp(e.GetPosition(canvas));
                ((UIElement)sender).ReleaseMouseCapture();
            };

            canvas.MouseLeave += (sender, e) => {
                if (controller.IsDrawing())
                {
                    controller.CancelDrawing();
                    ((UIElement)sender).ReleaseMouseCapture();
                }
            };
        }




    }
}
