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
using System.Windows.Shapes;

namespace camtastic_application
{
    /// <summary>
    /// Interaction logic for SpeedSelect.xaml
    /// </summary>
    public partial class SpeedSelect : Window
    {
        public static bool slow;
        public static bool average;
        public static bool fast;
        public SpeedSelect()
        {
            InitializeComponent();
        }
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            warningLabel.Content = "WARNING! This setting should be used for high-end PCs only.";
            fast = true;
            slow = false;
            average = false;
        }
        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
            warningLabel.Content = "";
            fast = false;
            slow = false;
            average = true;
        }
        private void RadioButton_Checked_2(object sender, RoutedEventArgs e)
        {
            warningLabel.Content = "";
            fast = false;
            slow = true;
            average = false;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindowViewModel methodExtender = new MainWindowViewModel();
            MainWindow.isSearching = true;
            methodExtender.GetInfo();
            Close();
        }
    }
}
