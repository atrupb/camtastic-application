using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace camtastic_application
{
    /// <summary>
    /// Interaction logic for ChartInfo.xaml
    /// </summary>
    
    
    public partial class ChartInfo : Window
    {
        public ChartInfo()
        {
            InitializeComponent();
            LoadBarChartData();
        }
        /// <summary>
        /// used to load the data for the chart
        /// </summary>
        public void LoadBarChartData()
        {
            MainWindowViewModel methodExtender = new MainWindowViewModel(); //we create a method extender since we need it
            Dictionary<string, double> ratingsPerBrand = new Dictionary<string, double>(); //this dictionary is used for final additions to the chart
            Dictionary<string, List<Photo>> mainDictionary = methodExtender.SortData(); //new var for photosPerBrand dictionary for easier hnadling
            mainDictionary.OrderBy(x => x.Value.Select(x=>x.Rating).Average()); //linq to grab top 10
            try
            {
                for (var i = 0; i < 10; i++)
                {
                    try
                    {
                        ratingsPerBrand.Add(mainDictionary.Keys.ToList()[i].ToUpper(), mainDictionary.Values.ToList()[i].Select(x => x.Rating).Average());
                    }
                    catch(ArgumentException)
                    {
                        continue;
                    }
                    //linq used, we add each brand and then the average rating of the list of pictures as a value
                }
            }
            catch(InvalidOperationException)
            {
                return; //if there is no info present, we open the chart empty
            }
            ((BarSeries)mcChart.Series[0]).ItemsSource = ratingsPerBrand.OrderBy(x => x.Value); //we add the stuff to the chart ordered by value

            //these are customizations, im not supposed to do these in c# but i barely know xaml.
            LinearGradientBrush myLinearGradientBrush =
            new LinearGradientBrush();
            myLinearGradientBrush.StartPoint = new Point(0.5, 0);
            myLinearGradientBrush.EndPoint = new Point(0.5, 1);
            myLinearGradientBrush.GradientStops.Add(
                new GradientStop(Colors.Black, 0.0));
            myLinearGradientBrush.GradientStops.Add(
                new GradientStop(Colors.DeepPink, 1));
            mcChart.Background = myLinearGradientBrush;
            mcChart.Foreground = new SolidColorBrush(Colors.DarkSlateGray);
            mcChart.FontSize = 15;
            mcChart.FontWeight = FontWeights.Bold;
        }
    }
}

