using PinBot2.Common;
using PinBot2.Dal.Interface;
using PinBot2.Model;
using PinBot2.Model.Configurations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PinBot2.Algorithms
{
    public class Mapper
    {
        private static Mapper _instance;
        public static BackgroundWorker bgWorker;
        public static IDictionary<IAccount, IList<Algo>> mapping = new Dictionary<IAccount, IList<Algo>>();
        public static IDictionary<IAccount, bool> running = new Dictionary<IAccount, bool>();
        public static IAccountRepository repository { get; set; }
        public bool Aborted { get; set; }
        public event EventHandler UpdateDGV;

        protected Mapper()
        {
            //request = new http();
        }
        public static Mapper Instance()
        {
            if (_instance == null)
            {
                _instance = new Mapper();
            }
            return _instance;
        }

        public void RegisterUser(IAccount account)
        {
            if (!mapping.ContainsKey(account))
            {
                IList<Algo> list = new List<Algo>();

                ////////
                list.Add(new LikeAlgo(Mapper.repository));
                list.Add(new FollowAlgo(Mapper.repository));
                list.Add(new UnfollowAlgo(Mapper.repository));
                list.Add(new RepinAlgo(Mapper.repository));
                list.Add(new InviteAlgo(Mapper.repository));
                list.Add(new PinAlgo(Mapper.repository));
                list.Add(new CommentAlgo(Mapper.repository));
                //...

                mapping.Add(account, list);
            }
            if (!running.ContainsKey(account))
            {
                running.Add(account, false);
            }
        }

        public void StartAllAlgos(IAccount account)
        {
            running[account] = true;
            account.LoginSync(false, account.Request);
            int attempts = 0;
            while (!account.IsLoggedIn && attempts <= 3) {
                account.LoginSync(false, account.Request);
                attempts++;
            }
            if (!account.IsLoggedIn) {
                running[account] = false;
                account.Request.Abort();
                return;
            }

            ICampaign campaign = repository.GetCampaign(account.SelectedCampaignId);
            campaign.ConfigurationContainer.ClearConfigurations();
            foreach (IConfiguration c in campaign.ConfigurationContainer.EnabledConfigurations())
            {
                StartAlgo(account, c);
            }
        }       
        public void StartAlgo(IAccount account, IConfiguration config)
        {
            try
            {
                Aborted = false;
                Algo algo = null;
               // Thread t;

                for (int i = 0; i < mapping[account].Count; i++)
                {
                    if (mapping[account][i].ConfigType != config.GetType())
                        continue;
                    else if (mapping[account][i].Running)
                        continue;
                    else
                        mapping[account][i].Running = true;

                    ///////
                    if (mapping[account][i].ConfigType == typeof(LikeConfiguration))
                        algo = mapping[account][i] = new LikeAlgo(account, config, Mapper.repository);
                    if (mapping[account][i].ConfigType == typeof(FollowConfiguration))
                        algo = mapping[account][i] = new FollowAlgo(account, config, Mapper.repository);
                    if (mapping[account][i].ConfigType == typeof(UnfollowConfiguration))
                        algo = mapping[account][i] = new UnfollowAlgo(account, config, Mapper.repository);
                    if (mapping[account][i].ConfigType == typeof(RepinConfiguration))
                        algo = mapping[account][i] = new RepinAlgo(account, config, Mapper.repository);
                    if (mapping[account][i].ConfigType == typeof(InviteConfiguration))
                        algo = mapping[account][i] = new InviteAlgo(account, config, Mapper.repository);
                    if (mapping[account][i].ConfigType == typeof(PinConfiguration))
                        algo = mapping[account][i] = new PinAlgo(account, config, Mapper.repository);
                    if (mapping[account][i].ConfigType == typeof(CommentConfiguration))
                        algo = mapping[account][i] = new CommentAlgo(account, config, Mapper.repository);
                    //...

                    if (!Aborted && algo != null)
                    {
                        algo.UpdateDGV += UpdateDGV;
                        MapperDataSource.UpdateDataSource();
                       // t = new Thread(algo.Run);
                       // t.Start();
                        Task.Factory.StartNew(() => {
                            algo.Run();
                        },CancellationToken.None,TaskCreationOptions.LongRunning,TaskScheduler.Default);
                    }
                    else if (Aborted && algo != null)
                    {
                        algo.Abort();
                    }
                }

            }
            catch (Exception ex)
            {
                string msg = "Error MPPR129." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log(account.Username, this.GetType().ToString(), msg);
            }
        }

        public void AbortAll()
        {
            Aborted = true;
            foreach (IAccount account in mapping.Keys)
            {
                Abort(account);
            }

        }
        public void AbortSelected(IAccount account)
        {
            Aborted = true;
            if (mapping.ContainsKey(account))
            {
                Abort(account);
            }
        }
        private void Abort(IAccount account)
        {
            try
            {
                account.Request.Abort();
                running[account] = false;
                Task.Factory.StartNew(() =>
                {
                    IList<Algo> algos = mapping[account];
                    foreach (Algo algo in algos)
                        if (algo != null)//algo.Running)
                            algo.Abort();
                    foreach (Algo algo in algos)
                    {
                        while (algo.Status == Algo.STATUS.ACTIVE)
                            Thread.Sleep(100);
                        algo.Status = Algo.STATUS.IDLE;
                        algo.CurrentCount = null;
                    }
                    account.IsLoggedIn = false;
                    account.Status = Account.STATUS.READY;
                },CancellationToken.None, TaskCreationOptions.LongRunning,TaskScheduler.Default);
            }
            catch (Exception ex)
            {
                string msg = "Error MPPR178." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log(account.Username, this.GetType().ToString(), msg);
            }
        }
    }
}
