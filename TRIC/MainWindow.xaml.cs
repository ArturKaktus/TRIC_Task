using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using TRIC.ModelView;
using tricLib;

namespace TRIC
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowMV mainWindow;
        public MainWindow()
        {
            InitializeComponent();
            mainWindow = new MainWindowMV();
            DataContext = mainWindow;
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Button_Click(object sender, RoutedEventArgs e) => mainWindow.AddList();
        public void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            mainWindow.SelectedService = ((ListView)sender).SelectedItem as Class1.Service;
        }

        private void Button_ToJson(object sender, RoutedEventArgs e)
        {
            mainWindow.SaveToJson();
        }
    }
}
