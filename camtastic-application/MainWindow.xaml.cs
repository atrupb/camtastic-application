using HtmlAgilityPack;

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
        DatabaseHandler database = new DatabaseHandler();
        Dictionary<string, List<Photo>> photosPerBrand = new Dictionary<string, List<Photo>>();
        bool addedKey;
        public MainWindow()
        {
            InitializeComponent();
            GetInfo();
        }
        public void GetInfo()
        {
            database.Connect();
            List<Photo> photosInOneBrand = new List<Photo>();   //assigning a new temporary camera and photo, which we will play around with now. also adding a list for photos in a single brand to add to the dictionary
            Photo tempPhoto = new Photo();
            Camera tempCamera = new Camera();
            for (var i = 0; i < 50; i++)  //beginning loop right here, this will cycle between the websites and get the information
            {
                if(addedKey == true)  //if a key was added to the dictionary on the last cycle, we reset the list that contains all photos in one brand
                {
                    photosInOneBrand = new List<Photo>();
                    addedKey = false;
                }
                tempPhoto = new Photo();     //this resets the photo and camera back to a null state
                tempCamera = new Camera();    
                string url = "https://photo-forum.net/i/" + i;
                var web = new HtmlAgilityPack.HtmlWeb();    //htmlAgilityPack doing its magic
                HtmlDocument doc = web.Load(url);
                try   //a try construct, if it doesnt find a rating or cameramodel or camerabrand, it should skip to catch
                {
                    int rating = int.Parse(doc.DocumentNode.SelectNodes("/html/body/div[4]/div[5]/div[2]/div/div/div[2]/div/ul[1]/li[1]/ul/li[1]/span[2]/span")[0].InnerText);
                    string cameraModel = doc.DocumentNode.SelectNodes("/html/body/div[4]/div[5]/div[1]/div[1]/div/div[2]/div/div[2]/div[2]/div[2]/span")[0].InnerText;
                    string cameraBrand = doc.DocumentNode.SelectNodes("/html/body/div[4]/div[5]/div[1]/div[1]/div/div[2]/div/div[2]/div[1]/div[2]/span")[0].InnerText;   //we grab values using xPath (a thing you copy off google idk much either lol)
                    tempCamera.CameraBrand = cameraBrand;
                    tempCamera.CameraModel = cameraModel;
                    tempPhoto.Url = url;
                    tempPhoto.Rating = rating;
                    tempPhoto.Camera = tempCamera;
                    if (photosPerBrand.ContainsKey(cameraBrand))  //checking if camerabrand already exists in dictionary
                    {
                        photosInOneBrand.Add(tempPhoto);
                    }
                    else // if it doesnt, we add a new key with the cameraBrand and the list of pictures we were saving (this might pose a few issues, but we will test and get them fixed, of course)
                    {
                        photosPerBrand.Add(cameraBrand, photosInOneBrand);
                        addedKey = true;

                    }
                }
                catch   //catch construct just skips to next iteration, since we skip pictures without metadata (untested, hopefully it sends an error)
                {
                    continue;
                }

                /* Brand.Text = cameraBrand.ToString();
                 * Model.Text = cameraModel.ToString();
                 * Rating.Text = rating.ToString():
                 */
            }
        }
    }
}
