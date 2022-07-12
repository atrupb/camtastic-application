
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
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Defaults;


namespace camtastic_application
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly MainWindowViewModel methodExtender = new MainWindowViewModel();

        
        public MainWindow()
        {

            InitializeComponent();
            

            Brand.Text = "Sony";
            Model.Text = "Sony";
        }

        public SeriesCollection SeriesCollection { get; set; }

        private void ButtonChangePieChart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataContext = new object();

                SeriesCollection = new SeriesCollection
                {
                    new PieSeries
                    {
                        Title = "1. Instagram",
                        Values = new ChartValues<ObservableValue> { new ObservableValue()},
                        DataLabels = true
                    },new PieSeries
                    {
                        Title = "2. Twitter",
                        Values = new ChartValues<ObservableValue> { new ObservableValue()},
                        DataLabels = true
                    },new PieSeries
                    {
                        Title = "3. YouTube",
                        Values = new ChartValues<ObservableValue> { new ObservableValue()},
                        DataLabels = true
                    }
                };
                DataContext = this;
            }
            catch (Exception exp)
            {
                MessageBox.Show("Please enter valid data \n" + exp.Message);
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

       
    }
}
    


        
        
    
    

