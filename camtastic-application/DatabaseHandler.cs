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
        public void Connect()
        {
            string connStr = "server=localhost;user=root;database=world;port=3306;password=12345";
            MySqlConnection conn = new MySqlConnection(connStr);
            conn.Open();
        }
        void AddData(int rating, string cameraBrand, string cameraModel, string url)
        {
           
        }
    }
}
