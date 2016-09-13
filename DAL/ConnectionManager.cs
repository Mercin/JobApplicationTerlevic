using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

namespace DAL
{
    public class ConnectionManager
    {
        private static volatile ConnectionManager instance;
        private static object syncRoot = new Object();
        public SQLiteConnection dbConnection;

        public static ConnectionManager getInstance()
        {
            if(instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        instance = new ConnectionManager();
                        instance.Connect();
                    }
                }
            }

            return instance;

        }

        public void Connect()
        {
            if (!File.Exists("MyDatabase.sqlite"))
            {
                SQLiteConnection.CreateFile("MyDatabase.sqlite");
            }

            dbConnection = new SQLiteConnection("Data Source=MyDatabase.sqlite;Version=3;");
            dbConnection.Open();
        }

        public void CloseConnection()
        {
            dbConnection.Close();
        }
    }
}
