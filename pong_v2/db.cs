using System.Data.SQLite;
using System.IO;

namespace SpaceShooter
{
    /// <summary>
    ///använder denna för att skriva och läsa från databas filen
    /// </summary>
    internal class Database
    {
        public SQLiteConnection dbConn;
        private string dbFileName = "./player.db";

        public Database()
        {
            dbConn = new SQLiteConnection("Data Source = " + dbFileName);

            if (!File.Exists(dbFileName))
            {
                SQLiteConnection.CreateFile(dbFileName);
            }
        }

        public void OpenConn()
        {
            if (dbConn.State == System.Data.ConnectionState.Closed)
            {
                dbConn.Open();
            }
        }

        public void CloseConn()
        {
            if (dbConn.State == System.Data.ConnectionState.Open)
            {
                dbConn.Close();
            }
        }
    }
}