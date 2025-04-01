using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Win32;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        private DrawingController controller;
        private DrawingView view;

        public MainWindow()
        {
            InitializeComponent();
            view = new DrawingView(myCanvas);
            controller = new DrawingController(view);
            view.BindController(controller);
            InitializeThicknessSlider();
        }
        private void InitializeThicknessSlider()
        {
            // Настройка min/max значений
            thicknessSlider.Minimum = 1;
            thicknessSlider.Maximum = 10;
            thicknessSlider.Value = 1; // Начальное значение

            // Добавляем обработчик события только после инициализации controller
            thicknessSlider.ValueChanged += Slider_ValueChanged;
        }



        private void OpenMenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Файлы рисунков (*.drw)|*.drw|Все файлы (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                // Здесь нужно добавить метод десериализации в контроллер
                // controller.DeserializeShapes(openFileDialog.FileName);
            }
        }

        private void SaveMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Файлы рисунков (*.drw)|*.drw|Все файлы (*.*)|*.*";

            if (saveFileDialog.ShowDialog() == true)
            {
                controller.SerializeShapes(saveFileDialog.FileName);
            }
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void AboutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Графический редактор\nВерсия 1.0", "О программе",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ShapeButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null && button.CommandParameter is string shapeType)
            {
                controller.SetShapeType(shapeType);
            }
        }


        private bool selectingStrokeColor = true;

        private void PaletteColor_Click(object sender, RoutedEventArgs e)
        {
            Button colorButton = sender as Button;
            if (colorButton != null)
            {
                SolidColorBrush brush = colorButton.Background as SolidColorBrush;
                if (brush != null)
                {
                    if (selectingStrokeColor)
                    {
                        controller.SetStrokeColor(brush.Color);
                        colorButton.Background = new SolidColorBrush(brush.Color);
                    }
                    else
                    {
                        controller.SetFillColor(brush.Color);
                        colorButton.Background = new SolidColorBrush(brush.Color);
                    }
                }
            }
        }


        private void FillButton_Click(object sender, RoutedEventArgs e)
        {
            // Переключаем режим заливки
            bool newFillMode = !controller.IsFillMode();
            controller.SetFillMode(newFillMode);

            // Меняем внешний вид кнопки, чтобы показать, что режим активен
            Button fillBtn = sender as Button;
            if (fillBtn != null)
            {
                if (newFillMode)
                {
                    // Визуальное выделение кнопки при активном режиме
                    fillBtn.BorderBrush = new SolidColorBrush(Colors.DarkBlue);
                    fillBtn.BorderThickness = new Thickness(2);
                }
                else
                {
                    // Возврат к обычному виду при отключении режима
                    fillBtn.BorderBrush = new SolidColorBrush(Colors.Gray);
                    fillBtn.BorderThickness = new Thickness(1);
                }
            }

        }



        private void UndoButton_Click(object sender, RoutedEventArgs e)
        {
            controller.Undo();
        }


        private void RedoButton_Click(object sender, RoutedEventArgs e)
        {
            controller.Redo();
        }

        private void LoadPluginMenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Плагины (*.dll)|*.dll|Все файлы (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                controller.LoadPlugin(openFileDialog.FileName);
            }
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (controller != null)
            {
                controller.SetStrokeThickness(e.NewValue);
            }
        }

        private void StrokeColorMode_Checked(object sender, RoutedEventArgs e)
        {
            selectingStrokeColor = true;
        }

        private void FillColorMode_Checked(object sender, RoutedEventArgs e)
        {
            selectingStrokeColor = false;
        }

        private void ColorButton_Click(object sender, RoutedEventArgs e)
        {
            Button colorButton = sender as Button;
            if (colorButton != null)
            {
                SolidColorBrush brush = colorButton.Background as SolidColorBrush;
                if (brush != null)
                {
                    if (selectingStrokeColor)
                    {
                        controller.SetStrokeColor(brush.Color);
                    }
                    else
                    {
                        controller.SetFillColor(brush.Color);
                    }
                }
            }
        }

    }
}
