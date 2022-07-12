
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
        public static Button getInfoButtonAccess;
        public static Label percentage;
        public static bool isSearching = false;
        readonly MainWindowViewModel methodExtender = new MainWindowViewModel();
        DatabaseHandler database = new DatabaseHandler();
        public MainWindow()
        {
            InitializeComponent();
            database.Connect(); // this connects us to our database
            getInfoButtonAccess = getInfoButton;
            percentage = percentDone;
        }
        /// <summary>
        /// event handler for getInfo button click
        /// </summary>
        private void GetInfoButton_Click(object sender, RoutedEventArgs e)
        {
            if (isSearching == false)
            {
                SpeedSelect window = new SpeedSelect();
                window.Show();
            }
            else
            {
                getInfoButton.Content = "Gathering has been canceled. Click to try again.";
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
