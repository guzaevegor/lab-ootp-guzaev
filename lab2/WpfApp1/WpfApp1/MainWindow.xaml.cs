using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static WpfApp1.ShapeBase;
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

            // Инициализируем представление и контроллер
            view = new DrawingView(myCanvas);
            controller = new DrawingController(view);
            view.BindController(controller);

            // Инициализация UI-элементов
            InitializeThicknessComboBox();
        }

        private void InitializeThicknessComboBox()
        {
           
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

        private void Rectangle_Click(object sender, RoutedEventArgs e)
        {
            controller.SetShapeType(ShapeType.Rectangle);
        }

        private void Ellipse_Click(object sender, RoutedEventArgs e)
        {
            controller.SetShapeType(ShapeType.Ellipse);
        }

        private void Straight_Click(object sender, RoutedEventArgs e)
        {
            controller.SetShapeType(ShapeType.Line);
        }

        private void Circle_Click(object sender, RoutedEventArgs e)
        {
            controller.SetShapeType(ShapeType.Circle);
        }

        private void Fractured_Click(object sender, RoutedEventArgs e)
        {
            controller.SetShapeType(ShapeType.Polyline);
        }

        private void ColorButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void FillButton_Click(object sender, RoutedEventArgs e)
        {

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
    }
}
