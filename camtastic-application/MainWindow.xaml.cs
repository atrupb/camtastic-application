
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

        bool buttonClicked = false;

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

            service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;
            options = new ChromeOptions();
            options.AddArguments(new List<string>() //assigning a bunch of chromedrive options for optimization
            {
                "--silent-launch",
                "--no-startup-window",
                "--no-sandbox",
                "--headless",
                "--disable-gpu"
            });
            options.PageLoadStrategy = PageLoadStrategy.Eager;
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
        /// <summary>
        /// event handler for single photo button
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!buttonClicked)
            {
                string url = photoUrlBox.Text;
                var web = new ChromeDriver(service, options);  //selenium doing its magic
                try
                {
                    web.Navigate().GoToUrl(url);
                    int rating = int.Parse(web.FindElement(By.XPath("/html/body/div[4]/div[5]/div[2]/div/div/div[2]/div/ul[1]/li[1]/ul/li[1]/span[2]/span")).Text);
                    string cameraBrand = web.FindElement(By.XPath("/html/body/div[4]/div[5]/div[1]/div[1]/div/div[2]/div/div[2]/div[1]/div[2]/span")).GetAttribute("textContent");
                    string cameraModel = web.FindElement(By.XPath("/html/body/div[4]/div[5]/div[1]/div[1]/div/div[2]/div/div[2]/div[2]/div[2]/span")).GetAttribute("textContent");
                    Brand.Text = cameraBrand;
                    Rating.Text = rating.ToString();
                    Model.Text = cameraModel;
                }
                catch
                {
                    LabelTimer();
                }
            }
        }
        private async void LabelTimer()
        {
            buttonClicked = true;
            badLinkLabel.Content = "Link is invalid or has no metadata attached.";
            await Task.Delay(4000);
            badLinkLabel.Content = "";
            buttonClicked = false;
        }
    }
}
