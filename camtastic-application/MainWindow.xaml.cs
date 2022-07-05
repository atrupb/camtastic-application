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

namespace camtastic_application
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<string> cameraBrands = new List<string>();
        public List<string> cameraModels = new List<string>();
        public List<int> ratings = new List<int>();
        public MainWindow()
        {
            InitializeComponent();
            
        }
        public void GetInfo()
        {

        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Brand_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
