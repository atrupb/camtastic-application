
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
    class MainWindowViewModel
    {
        DatabaseHandler database = new DatabaseHandler();

        public static List<ChromeDriver> chromeDrivers = new List<ChromeDriver>();

      
        /// <summary>
        /// this is the code behind all the threads that collect the information
        /// </summary>
        public void Thread1(int start, int end)
        {
            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;
            ChromeOptions options = new ChromeOptions();
            options.AddArguments(new List<string>() //assigning a bunch of chromedrive options for optimization
            {
                "--silent-launch",
                "--no-startup-window",
                "--no-sandbox",
                "--headless",
                "--disable-gpu"
            });
            options.PageLoadStrategy = PageLoadStrategy.Eager;
            var web = new ChromeDriver(service, options);  //selenium doing its magic
            chromeDrivers.Add(web); //adding chromeDriver to the static list incase user decides to cancel operation
            for (var i = start; i < end; i++)  //beginning loop right here, this will cycle between the websites and get the information
            {
                if(MainWindow.isSearching == false) //if button has been pressed against searching, we break out of this loop pt.1
                {
                    break;
                }
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
                catch (OpenQA.Selenium.NoSuchElementException)   //catch construct just skips to next iteration, since we skip pictures without metadata (not sure if this does anything, however id rather keep it here as a secondary safety net)
                {
                    continue;
                }
                catch (OpenQA.Selenium.WebDriverException) //web driver exception might get thrown if cancellation is mid loop, so we break through this catch as well pt.2
                {
                    break;
                }
            }
            web.Close();
            web.Quit(); //after loop, we quit out of the current chromedriver and close the chrome tab
        }
        /// <summary>
        /// used to start collecting information process
        /// </summary>
        public async void GetInfo()
        {
            for (var i = 0; i < 6000; i++)
            {
                ThreadPool.QueueUserWorkItem(o => Thread1(i * 500 + 1, i * 500 + 500));
                await Task.Delay(5000);
            }
        }
        /// <summary>
        /// used to grab the data and sort it into a dictionary for convenient graphing.
        /// </summary>
        public Dictionary<string, List<Photo>> SortData()
        {
            List<Photo> photosInOneBrand = new List<Photo>();
            Dictionary<string, List<Photo>> photosPerBrand = new Dictionary<string, List<Photo>>();
            List<Photo> allPhotos = database.GrabData();
            foreach(Photo photo in allPhotos)
            {
                photosInOneBrand.Add(photo);
                if (!photosPerBrand.ContainsKey(photo.Camera.CameraBrand))
                {
                    photosPerBrand.Add(photo.Camera.CameraBrand, new List<Photo>(photosInOneBrand));
                    photosInOneBrand.Clear();
                }
            }
            return photosPerBrand;
        }
        /// <summary>
        /// used to cancel current search for info (this was fully copied off of google, i dont really know how it works.)
        /// </summary>
        public void CancelInfoCollection()
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            for (int i = 0; i < 6000; i++)
            {
                ThreadPool.QueueUserWorkItem(s =>
                {
                    CancellationToken token = (CancellationToken)s;
                    if (token.IsCancellationRequested)
                        return;
                    Console.WriteLine("Output2");
                    token.WaitHandle.WaitOne(1000);
                }, cts.Token);
            }
            cts.Cancel();
        }
    }
}
