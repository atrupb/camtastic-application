using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using MySql.Data;
using MySql.Data.MySqlClient;


namespace camtastic_application
{
    class DatabaseHandler
    {
        public MySqlConnection conn;
        /// <summary>
        /// establishes a connection to the database
        /// </summary>
        public void Connect() //method to connect to SQL database
        {
            string connStr = "server=localhost;user=root;database=project;port=3306;password=12345"; //IMPORTANT!!!! YOU NEED TO CHANGE SOME OF THE INFO SO IT WORKS ON YOUR OWN DATABASE!
            conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();
            }
            catch
            {
                MySqlConnection.ClearAllPools();
                conn.Open();
            }
        }
        /// <summary>
        /// used to add a new row of data
        /// </summary>
        public void AddData(int rating, string cameraBrand, string cameraModel, string url)
        {
            Connect();
            string sqlQuery = $"INSERT INTO `photo` VALUES ('{url}', '{cameraBrand}', '{cameraModel}', '{rating}')";
            MySqlCommand cmd = new MySqlCommand(sqlQuery, conn);
            cmd.ExecuteNonQueryAsync();
        }
        /// <summary>
        /// used to grab every element from a row
        /// </summary>
        public List<Photo> GrabData()
        {
            Connect();
            string sqlQuery = $"SELECT * FROM `photo`";
            List<Photo> allPhotos = new List<Photo>();
            MySqlCommand cmd = new MySqlCommand(sqlQuery, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Photo tempPhoto = new Photo();
                Camera tempCamera = new Camera(); // creating temp objects to play around with
                string url = reader.GetString(0);
                string cameraBrand = reader.GetString(1);
                string cameraModel = reader.GetString(2);
                int rating = reader.GetInt32(3);
                tempCamera.CameraBrand = cameraBrand;
                tempCamera.CameraModel = cameraModel;
                tempPhoto.Url = url;
                tempPhoto.Rating = rating;
                tempPhoto.Camera = tempCamera; //we grab info needed and shove it into the temporary camera and photo
                allPhotos.Add(tempPhoto);
            }
            conn.Close();
            return allPhotos;
        }
    }
}
