using System.Collections.Generic;
using System.Linq;
using PinBot2.Dal.Interface;
using PinBot2.Model;
using System.Data.SQLite;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using PinBot2.Model.Configurations;
using PinBot2.Common;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Threading;

//DATA ACCESS LAYER
namespace PinBot2.Dal
{
    public class InMemoryAccountRepository : IAccountRepository
    {

        private static IList<IAccount> Accounts;
        public InMemoryAccountRepository()
        {
            InMemoryAccountRepository.Accounts = new List<IAccount>();
        }

        public IAccount GetAccount(int id)
        {
            return Accounts.FirstOrDefault(a => a.Id == id);
        }

        private SQLiteConnection CreateConnection()
        {
            return new SQLiteConnection(@"Data Source=db;Version=3;New=False;Compress=True;");
        }

        public IList<IAccount> GetAccounts(bool FromDatabase = false)
        {
            
            if (File.Exists(Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory + "ajsn_admin.txt")))
            {
                var jsn = File.ReadAllText(Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory + "ajsn_admin.txt"));
                List<Account> tmp = JsonConvert.DeserializeObject<List<Account>>(jsn);
                IList<IAccount> tmp2 = new List<IAccount>();
                foreach (var f in tmp)
                {
                    tmp2.Add((IAccount)f);
                }

                return tmp2;
            }
            

            if (!FromDatabase)
                return new List<IAccount>(Accounts);

            Accounts.Clear();
            using (SQLiteConnection conn = CreateConnection())
            {
                conn.Open();
                string sql = "select id,data from accounts";
                using (SQLiteCommand command = new SQLiteCommand(sql, conn))
                {
                    SQLiteDataReader sqlreader = command.ExecuteReader();
                    while (sqlreader.Read())
                    {
                        try
                        {
                            string obj = sqlreader["data"].ToString();
                            IAccount acc = (Account)Serializer.DeSerializeObject<Account>(obj);
                            acc.Id = int.Parse(sqlreader["id"].ToString());
                            Accounts.Add(acc);
                        }
                        catch (Exception ex)
                        {
                            string msg = "Error IMAR57." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                            Logging.Log("user or auto", "IMAR", msg);
                            
                        }
                    }
                }
            }
            foreach (var a in Accounts)
                SaveAccount(a);

            List<Account> tmp3 = new List<Account>();
            List<Campaign> tmp4 = new List<Campaign>();
            foreach (var f in Accounts)
            { tmp3.Add((Account)f);

                var camps = GetCampaigns(f.Id);                
                foreach (var c in camps)
                    tmp4.Add((Campaign)c);        
                        
            }

            
            var json = JsonConvert.SerializeObject(tmp3);
            System.IO.File.WriteAllText(Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory + "ajsn.txt"), json);

            var json0 = JsonConvert.SerializeObject(tmp4);
            System.IO.File.WriteAllText(Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory + "cjsn.txt"), json0);
            



            return new List<IAccount>(Accounts); //must return 'new' list, otherwise refresh() on checkbox list not working.
        }

        public void SaveAccount(IAccount account)
        {
            if (Accounts.Any(a => a.Id == account.Id))
            {
                IAccount acc = Accounts.First(a => a.Id == account.Id);

                using (SQLiteConnection conn = CreateConnection())
                {
                    conn.Open();
                    string sql = "update accounts set data = @account where id = @id";
                    using (SQLiteCommand command = new SQLiteCommand(sql, conn))
                    {
                        try
                        {
                            SQLiteParameter param = command.CreateParameter();
                            param.DbType = System.Data.DbType.String;
                            param.ParameterName = "account";
                            param.Value = Serializer.SerializeObject<Account>((Account)account);
                            command.Parameters.Add(param);

                            param = command.CreateParameter();
                            param.DbType = System.Data.DbType.Int16;
                            param.ParameterName = "id";
                            param.Value = account.Id;
                            command.Parameters.Add(param);

                            command.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            string msg = "Error IMAR97" + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                            Logging.Log("user or auto", "IMAR", msg);
                        }
                    }
                }
            }
        }

        public void AddAccount(IAccount account)
        {
            using (SQLiteConnection conn = CreateConnection())
            {
                conn.Open();
                string sql = "insert into  accounts  (data) values (@account)";
                using (SQLiteCommand command = new SQLiteCommand(sql, conn))
                {
                    SQLiteParameter param = command.CreateParameter();
                    param.DbType = System.Data.DbType.String;
                    param.ParameterName = "account";
                    param.Value = Serializer.SerializeObject<Account>((Account)account);
                    command.Parameters.Add(param);
                    command.ExecuteNonQuery();

                    command.CommandText = "select last_insert_rowid() as id from accounts";
                    command.Parameters.Clear();
                    account.Id = int.Parse(command.ExecuteScalar().ToString());

                    Accounts.Add(account);
                }
            }
        }

        public void DeleteAccount(int id)
        {
            if (Accounts.Any(a => a.Id == id))
            {
                IAccount acc = Accounts.First(a => a.Id == id);
                Accounts.Remove(acc);

                using (SQLiteConnection conn = CreateConnection())
                {
                    conn.Open();

                    string sql = "delete from accounts where id = @id";
                    using (SQLiteCommand command = new SQLiteCommand(sql, conn))
                    {
                        SQLiteParameter param = command.CreateParameter();
                        param.DbType = System.Data.DbType.Int16;
                        param.ParameterName = "id";
                        param.Value = id;
                        command.Parameters.Add(param);
                        command.ExecuteNonQuery();
                    }

                    sql = "delete from configurations where accountId = @id";
                    using (SQLiteCommand command = new SQLiteCommand(sql, conn))
                    {
                        SQLiteParameter param = command.CreateParameter();
                        param.DbType = System.Data.DbType.Int16;
                        param.ParameterName = "id";
                        param.Value = id;
                        command.Parameters.Add(param);
                        command.ExecuteNonQuery();
                    }
                }
            }

        }

       

        public void DeleteCampaign(ICampaign campaign)
        {
            using (SQLiteConnection conn = CreateConnection())
            {
                conn.Open();
                string sql = "delete from configurations where id = @id";
                using (SQLiteCommand command = new SQLiteCommand(sql, conn))
                {
                    SQLiteParameter param = command.CreateParameter();
                    param.DbType = System.Data.DbType.Int32;
                    param.ParameterName = "id";
                    param.Value = campaign.ID;
                    command.Parameters.Add(param);

                    command.ExecuteNonQuery();
                }
            }
        }

        private void updateCampaign(ICampaign campaign)
        {
            using (SQLiteConnection conn = CreateConnection())
            {
                conn.Open();
                string sql = "update configurations set data = @data, campaignName = @name where id = @id";
                using (SQLiteCommand command = new SQLiteCommand(sql, conn))
                {
                    SQLiteParameter param = command.CreateParameter();
                    param.DbType = System.Data.DbType.String;
                    param.ParameterName = "data";
                    param.Value = Serializer.SerializeObject(campaign);
                    command.Parameters.Add(param);

                    param = command.CreateParameter();
                    param.DbType = System.Data.DbType.String;
                    param.ParameterName = "name";
                    param.Value = campaign.CampaignName;
                    command.Parameters.Add(param);

                    param = command.CreateParameter();
                    param.DbType = System.Data.DbType.Int16;
                    param.ParameterName = "id";
                    param.Value = campaign.ID;
                    command.Parameters.Add(param);

                    command.ExecuteNonQuery();
                }
            }
        }

        public IList<ICampaign> GetCampaigns(int accountId)
        {
            try {
            if (File.Exists(Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory + "cjsn_admin.txt")))
            {
                var jsn0 = File.ReadAllText(Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory + "cjsn_admin.txt"));
                List<Campaign> tmp0 = JsonConvert.DeserializeObject<List<Campaign>>(jsn0);
                IList<ICampaign> tmp00 = new List<ICampaign>();
                foreach (var f0 in tmp0)
                {
                    if (f0.AccountId == accountId)
                        tmp00.Add(f0);
                }
                return tmp00;
            }
            }
            catch (Exception ex) {
                Logging.Log("me", "json", ex.StackTrace);
                Console.WriteLine(ex.StackTrace);
            }

            IList<ICampaign> list = new List<ICampaign>();
            using (SQLiteConnection conn = CreateConnection())
            {
                conn.Open();
                string sql = "select id, data from configurations where accountId = @accountId";
                using (SQLiteCommand command = new SQLiteCommand(sql, conn))
                {
                    SQLiteParameter param = command.CreateParameter();
                    param.DbType = System.Data.DbType.Int16;
                    param.ParameterName = "accountId";
                    param.Value = accountId;
                    command.Parameters.Add(param);

                    SQLiteDataReader sqlreader = command.ExecuteReader();
                    while (sqlreader.Read())
                    {
                        try
                        {
                            string obj = sqlreader["data"].ToString();
                            ICampaign campaign = (ICampaign)Serializer.DeSerializeObject<Campaign>(obj);
                            campaign.ID = int.Parse(sqlreader["id"].ToString());
                            list.Add(campaign);
                        }
                        catch (Exception ex)
                        {
                            string msg = "Error IMAR274." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                            Logging.Log("user or auto", "IMAR", msg);
                        }
                    }
                }
            }
            return list;
        }

        private static readonly Object objLOCK = new Object();

        public ICampaign GetCampaign(int Id)
        {
            ICampaign campaign = null;
            try
            {
                if (File.Exists(Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory + "cjsn_admin.txt")))
                {
                    var jsn0 = File.ReadAllText(Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory + "cjsn_admin.txt"));
                    List<Campaign> tmp0 = JsonConvert.DeserializeObject<List<Campaign>>(jsn0);
                    foreach (var f0 in tmp0)
                    {
                        if (f0.ID == Id)
                            return f0;
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.Log("me", "json", ex.StackTrace);
                Console.WriteLine(ex.StackTrace);
            }

            lock (objLOCK)
            {
                
                using (SQLiteConnection conn = CreateConnection())
                {
                    conn.Open();
                    string sql = "select data from configurations where ID = @id";
                    using (SQLiteCommand command = new SQLiteCommand(sql, conn))
                    {
                        SQLiteParameter param = command.CreateParameter();
                        param.DbType = System.Data.DbType.Int16;
                        param.ParameterName = "id";
                        param.Value = Id;
                        command.Parameters.Add(param);

                        SQLiteDataReader sqlreader = command.ExecuteReader();
                        while (sqlreader.Read())
                        {
                            try
                            {
                                string obj = sqlreader["data"].ToString();
                                campaign = (ICampaign)Serializer.DeSerializeObject<Campaign>(obj);
                            }
                            catch (Exception ex)
                            {
                                string msg = "Error IMAR308." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                                Logging.Log("user or auto", "IMAR", msg);
                            }
                        }
                    }
                }
            }
            return campaign;
        }
        public int SaveCampaign(ICampaign campaign)
        {
            int campaignID = 0;
            lock (objLOCK)
            {

                IList<ICampaign> campaigns = GetCampaigns(campaign.AccountId);
                if (campaigns.Any(a => a.ID == campaign.ID))
                {
                    campaignID = campaign.ID;
                    updateCampaign(campaign);
                    return campaignID;
                }

                using (SQLiteConnection conn = CreateConnection())
                {
                    conn.Open();
                    string sql = "insert into configurations (data, campaignName, accountId) VALUES (@data,@name,@id);";
                    sql += "SELECT last_insert_rowid();";
                    using (SQLiteCommand command = new SQLiteCommand(sql, conn))
                    {
                        SQLiteParameter param = command.CreateParameter();
                        param.DbType = System.Data.DbType.String;
                        param.ParameterName = "data";
                        param.Value = Serializer.SerializeObject(campaign);
                        command.Parameters.Add(param);

                        param = command.CreateParameter();
                        param.DbType = System.Data.DbType.String;
                        param.ParameterName = "name";
                        param.Value = campaign.CampaignName;
                        command.Parameters.Add(param);

                        param = command.CreateParameter();
                        param.DbType = System.Data.DbType.Int16;
                        param.ParameterName = "id";
                        param.Value = campaign.AccountId;
                        command.Parameters.Add(param);

                        campaignID = Convert.ToInt32(command.ExecuteScalar());
                    }
                }
            }
            return campaignID;
        }
    }
}
