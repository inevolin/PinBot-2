using PinBot2.Common;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinBot2.Dal
{
    public static class DatabaseManager
    {

        public static bool Create()
        {
            try
            {
                if (!File.Exists(Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory + "/db")))
                    SQLiteConnection.CreateFile(Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory + "/db"));

                SQLiteConnection m_dbConnection = new SQLiteConnection(@"Data Source=db;Version=3;New=False;Compress=True;");
                m_dbConnection.Open();
                string sql = "CREATE TABLE IF NOT EXISTS accounts (id INTEGER PRIMARY KEY, data text)";
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                command.ExecuteNonQuery();

                sql = "CREATE TABLE IF NOT EXISTS configurations (id INTEGER PRIMARY KEY, accountId INTEGER, campaignName text, data text)";
                command = new SQLiteCommand(sql, m_dbConnection);
                command.ExecuteNonQuery();

                m_dbConnection.Close();
                return true;

            }
            catch (Exception ex)
            {
                string msg = "Error creating Database." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log("user", "account action: ", msg);
                
                return false;
            }
        }

        public static bool Test()
        {
            try
            {
                SQLiteConnection m_dbConnection = new SQLiteConnection(@"Data Source=db;Version=3;New=False;Compress=True;");
                m_dbConnection.Open();
                string sql = "select count(*) as c from accounts";
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                SQLiteDataReader sqlreader = command.ExecuteReader();
                string i = "";
                while (sqlreader.Read())
                {
                    i = sqlreader["c"].ToString();
                }
                m_dbConnection.Close();
                return true;
            }
            catch (Exception ex)
            {
                string msg = "Error creating Database." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log("user", "account action: ", msg);
                
                return false;
            }
        }

    }
}
