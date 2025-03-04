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

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
           // DrawingController controller = new DrawingController();
           // DrawingView view = new DrawingView(myCanvas); // например, передаем элемент Canvas
           /*
            var circle = new CircleShape(80).Draw();
            myCanvas.Children.Add(circle);
            Canvas.SetLeft(circle, 50);
            Canvas.SetTop(circle, 50);*/
        }

        private void OpenMenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SaveMenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AboutMenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Rectangle_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Ellipse_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Straight_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Circle_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Fractured_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ColorButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void FillButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void UndoButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ThicknessComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void RedoButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
