
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

        DatabaseHandler database = new DatabaseHandler();

        Dictionary<string, List<Photo>> photosPerBrand = new Dictionary<string, List<Photo>>();
        
        public MainWindow()
        {
            database.Connect(); // this connects us to our database
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
            InitializeComponent();
        }
        /// <summary>
        /// used to start collecting information process
        /// </summary>
        public void GetInfo()
        {
            for (var i = 0; i < 6000; i++)
            {
                ThreadPool.QueueUserWorkItem(o => Thread1(i * 500 + 1, i * 500 + 500)); //per how many pics should we have a thread? ive changed it to 5000, but i think its still too little. maybe we can increase thread amount?
                Thread.Sleep(5000);
            }
        }
        /// <summary>
        /// this is the code behind all the threads that collect the information
        /// </summary>
        public void Thread1(int start, int end)
        {
            var web = new ChromeDriver(service, options);  //selenium doing its magic
            
            for (var i = start; i < end; i++)  //beginning loop right here, this will cycle between the websites and get the information
            {
                string url = "https://photo-forum.net/i/" + i;
                Debug.WriteLine(url);
                web.Navigate().GoToUrl(url);
                try   //a try construct, if it doesnt find a rating or cameramodel or camerabrand, it should skip to catch
                {
                    int rating = int.Parse(web.FindElement(By.XPath("/html/body/div[4]/div[5]/div[2]/div/div/div[2]/div/ul[1]/li[1]/ul/li[1]/span[2]/span")).Text);
                    string cameraBrand = web.FindElement(By.XPath("/html/body/div[4]/div[5]/div[1]/div[1]/div/div[2]/div/div[2]/div[1]/div[2]/span")).GetAttribute("textContent");
                    string cameraModel = web.FindElement(By.XPath("/html/body/div[4]/div[5]/div[1]/div[1]/div/div[2]/div/div[2]/div[2]/div[2]/span")).GetAttribute("textContent"); //we grab values using xPath (a thing you copy off google idk much either lol)
                    Regex timeFormatCheck = new Regex(@"(?:0[1-9]|[12][0-9]|3[01])[-/.](?:0[1-9]|1[012])[-/.](?:19\d{2}|20[01][0-9]|2020)\b"); //ive used a regex to check dates, its a safety net so no wrong info gets sent into the database
                    if (timeFormatCheck.IsMatch(cameraModel) || timeFormatCheck.IsMatch(cameraBrand) || cameraModel.Length == 0 || cameraBrand.Length == 0)
                    {
                        continue; // if regex matches (if its a date) or if one of them is empty, skip this one
                    }
                    else
                    {
                        database.AddData(rating, cameraBrand, cameraModel, url); //else, we send it to the database (i noticed not many entries have the full metadata)
                    }
                }
                catch(OpenQA.Selenium.NoSuchElementException)   //catch construct just skips to next iteration, since we skip pictures without metadata (not sure if this does anything, however id rather keep it here as a secondary safety net)
                {
                    continue;
                }
                /* Brand.Text = cameraBrand.ToString();
                 * Model.Text = cameraModel.ToString();
                 * Rating.Text = rating.ToString():
                 */
            }
            web.Close();
        }
        /// <summary>
        /// event handler for getInfo button click
        /// </summary>
        private void GetInfoButton_Click(object sender, RoutedEventArgs e)
        {
            if(ThreadPool.PendingWorkItemCount == 0) 
            {
                getInfoButton.Content = "Getting info, please wait."; //if no process is being worked on, we change the text
                GetInfo();
            }
        }
        /// <summary>
        /// this method is used to grab the information and sort it into lists for convenient graphing.
        /// </summary>
        public void SortInfo()
        {
            int entryAmount = database.FindDataAmount(); //checks how much rows there are
            List<Photo> photosInOneBrand = new List<Photo>();
            for (var i = 0; i < entryAmount; i++)
            {
                Photo tempPhoto = new Photo(); 
                Camera tempCamera = new Camera(); // creating temp objects to play around with
                string url = database.GrabData(i).Item1;
                string cameraBrand = database.GrabData(i).Item2;
                string cameraModel = database.GrabData(i).Item3;
                int rating = database.GrabData(i).Item4;
                tempCamera.CameraBrand = cameraBrand;
                tempCamera.CameraModel = cameraModel;
                tempPhoto.Url = url;
                tempPhoto.Rating = rating;
                tempPhoto.Camera = tempCamera; //we grab info needed and shove it into the temporary camera and photo
                if(!photosPerBrand.ContainsKey(cameraBrand))
                {
                    photosPerBrand.Add(cameraBrand, photosInOneBrand); 
                    photosInOneBrand.Clear(); //if camerabrand key doesnt exist, we create a new one, shove the list in and clear the list aftewards
                }
                else
                {
                    photosInOneBrand.Add(tempPhoto); //else, we continue adding photos onto our list
                }
            }
        }
    }
}
