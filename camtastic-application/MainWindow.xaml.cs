
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
        ChromeDriverService service;
        ChromeOptions options;

        static Random random;
        public MainWindow()
        {
            InitializeComponent();
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
                DataContext = new object();

                SeriesCollection = new SeriesCollection
                {
                    new PieSeries
                    {
                        Title = "1. Instagram",
                        Values = new ChartValues<ObservableValue> { new ObservableValue(random.Next(50000, 650000))},
                        DataLabels = true
                    },new PieSeries
                    {
                        Title = "2. Twitter",
                        Values = new ChartValues<ObservableValue> { new ObservableValue(random.Next(45000, 400000))},
                        DataLabels = true
                    },new PieSeries
                    {
                        Title = "3. YouTube",
                        Values = new ChartValues<ObservableValue> { new ObservableValue(random.Next(30000, 800000))},
                        DataLabels = true
                    }
                };
                DataContext = this;
            }
            catch (Exception exp)
            {
                MessageBox.Show("Pasta Grafiği Oluşturulamadı \n" + exp.Message);
            }

            /// <summary>
            /// event handler for getInfo button click
            /// </summary>

        }
        private void GetInfoButton_Click(object sender, RoutedEventArgs e)
        {
            if (ThreadPool.PendingWorkItemCount == 0)
            {
                getInfoButton.Content = "Getting info, please wait."; //if no process is being worked on, we change the text
                methodExtender.GetInfo();



            }
        }

        private void DefaultLegend_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
    


        
        
    
    

