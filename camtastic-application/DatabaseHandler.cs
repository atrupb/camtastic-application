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
        MySqlConnection conn;
        /// <summary>
        /// establishes a connection to the database
        /// </summary>
        public void Connect() //method to connect to SQL database
        {
            string connStr = "server=localhost;user=root;database=project;port=3306;password=12345"; //IMPORTANT!!!! YOU NEED TO CHANGE SOME OF THE INFO SO IT WORKS ON YOUR OWN DATABASE!
            conn = new MySqlConnection(connStr);
            conn.Open(); //opening sql connection
        }
        /// <summary>
        /// used to add a new row of data
        /// </summary>
        public void AddData(int rating, string cameraBrand, string cameraModel, string url) //method to add a new photo to the database
        {
            string sqlQuery = $"INSERT INTO `photo` VALUES ('{url}', '{cameraBrand}', '{cameraModel}', '{rating}')"; //ive adjusted the table slightly, ive sent you the code to create one.
            MySqlCommand cmd = new MySqlCommand(sqlQuery, conn);
            cmd.ExecuteNonQuery(); //executes command
        }
        /// <summary>
        /// used to grab every element from a row
        /// </summary>
        public (string, string, string, int) GrabData(int rowNum)
        {
            string sqlQuery = $"SELECT * FROM `photo` WHERE URL='https://photo-forum.net/i/'" + rowNum;
            MySqlCommand cmd = new MySqlCommand(sqlQuery, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            return (reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetInt32(3)); //returns url, camerbrand, cameramodel and rating after executing MySqlDataReader
        }
        /// <summary>
        /// used to get amount of data currently present in database
        /// </summary>
        public int FindDataAmount()
        {
            string sqlQuery2 = $"SELECT COUNT(*) FROM `photo`"; 
            MySqlCommand cmd2 = new MySqlCommand(sqlQuery2, conn);
            return (int)cmd2.ExecuteScalar(); //returns the amount of rows
        }
    }
}
