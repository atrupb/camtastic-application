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
        public void Connect() //method to connect to SQL database
        {
            string connStr = "server=localhost;user=root;database=project;port=3306;password=12345"; //IMPORTANT!!!! YOU NEED TO CHANGE SOME OF THE INFO SO IT WORKS ON YOUR OWN DATABASE!
            conn = new MySqlConnection(connStr);
            conn.Open(); //opening sql connection
        }
        public void AddData(int rating, string cameraBrand, string cameraModel, string url) //method to add a new photo to the database
        {
            string sqlQuery = $"INSERT INTO `photo` VALUES ('{url}', '{cameraBrand}', '{cameraModel}', '{rating}')"; //ive adjusted the table slightly, ive sent you the code to create one.
            MySqlCommand cmd = new MySqlCommand(sqlQuery, conn);
            cmd.ExecuteNonQuery(); //executes command
        }    
    }
}
