
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;

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
using System.Threading;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace camtastic_application
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static bool isSearching = false;
        readonly MainWindowViewModel methodExtender = new MainWindowViewModel();
        DatabaseHandler database = new DatabaseHandler();
        static Random random;
        public MainWindow()
        {
            InitializeComponent();
            database.Connect(); // this connects us to our database
            random = new Random();
            Brand.Text = "Sony";
            Model.Text = "Sony";
        }
        /// <summary>
        /// event handler for getInfo button click
        /// </summary>
        private void GetInfoButton_Click(object sender, RoutedEventArgs e)
        {
            if (isSearching == false)
            {
                getInfoButton.Content = "Getting info, please wait..."; //if no process is being worked on, we change the text
                isSearching = true;
                methodExtender.GetInfo();
            }
            else
            {
                getInfoButton.Content = "Cancelling...";
                isSearching = false;
                methodExtender.CancelInfoCollection();
            }
        }
        /// <summary>
        /// event handler for chartButton button click
        /// </summary>
        private void ChartButton_Click(object sender, RoutedEventArgs e)
        {
            ChartInfo window = new ChartInfo();
            window.Show();
        }
    }
}
