using PinBot2.Dal.Interface;
using PinBot2.Model.Configurations;
using PinBot2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PinBot2.Common;
using PinBot2.Model.PinterestObjects;
using PinBot2.Algorithms.Scraping;
using System.Threading;
using System.Threading.Tasks;

namespace PinBot2.Algorithms
{
    public abstract class Algo
    {
        protected static readonly string baseUrl = "https://www.pinterest.com";

        public event EventHandler UpdateDGV;
        protected http request = new http();
        protected IAccount account { get; set; }
        protected struct Data { public string content, referer;}
        protected ScrapeSessionManager SessionManager;
        public IConfiguration Config;
        protected IAccountRepository repository;

        protected bool Aborted { get; set; }
        public bool Running { get; set; }
        public Range<int> CurrentCount { get; set; }
        public enum STATUS { IDLE, ACTIVE, SLEEPING, DONE, ERROR, LIMIT_REACHED }

        public abstract void Run();
        public Type ConfigType { get { return Config.GetType(); } }
        public STATUS Status { get; set; }
        public string getStatus
        {
            get
            {

                string val = "";
                switch (Status)
                {
                    case STATUS.IDLE:
                        val = "idle";
                        break;
                    case STATUS.ACTIVE:
                        val = "active";
                        break;
                    case STATUS.SLEEPING:
                        val = "sleeping";
                        break;
                    case STATUS.DONE:
                        val = "finished";
                        break;
                    case STATUS.ERROR:
                        val = "Error";
                        break;
                    case STATUS.LIMIT_REACHED:
                        val = "Scrape limit reached";
                        break;
                }
                return val;
            }
        }
        public void Abort()
        {
            if (CurrentCount != null)
                CurrentCount = null;
            Status = STATUS.IDLE;
            Aborted = true;
            Running = false;
            if (request != null)
                request.Abort();
            if (SessionManager != null)
                SessionManager.Abort();
        }

        public Algo(IAccountRepository repository)
        {
            this.repository = repository;
        }

        protected void TimeOut()
        {
            Random r = new Random();
            int i = r.Next(Config.Timeout.Min * 1000, Config.Timeout.Max * 1000);
            Console.WriteLine(this + " timeout: + " + i);
            while (i > 0)
            {
                if (Aborted)
                    return;
                Thread.Sleep(500);
                i -= 500;
            }
        }
        protected bool ProcessResponse(string response, PinterestObject p)
        {
            if (!response.Contains("[[[ERROR]]]") && response.Contains(PinterestErrors.NO_ERROR))
            {
                Logging.Log(account.Username, this.GetType().ToString(), "Success Action");
                ++CurrentCount.Min;
                if (SessionManager != null)
                    SessionManager.UsedPinterestObjects.Add(p);
                UpdateDGV(null, null);
                return true;
            }
            else if (response.Contains(PinterestErrors.STRANGE_ACTIVITY))
            {
                Logging.Log(account.Username, this.GetType().ToString(), " (!) Strange Activity ");
                account.Status = Account.STATUS.LIMITED;
                Mapper m = Mapper.Instance();
                m.AbortSelected(account);
            }
            else if (response.Contains(PinterestErrors.AUTHORIZATION_ERROR))
            {
                Logging.Log(account.Username, this.GetType().ToString(), " (!) Auth Error, attempts login ... ");
                account.Status = Account.STATUS.READY;
                account.LastLogin = DateTime.Now.AddYears(-1);
                account.LoginSync(false);
            }
            return false;
        }
        public bool MustBeActive(bool maxReached, DateTime Now, DateTime timeSleep)
        {
            //SLEEP while time(now) not in[min,max] and time(stop) in [min,max]
            TimeSpan now = Now.TimeOfDay;
            TimeSpan start = Config.AutoStart.Min.TimeOfDay;
            TimeSpan end = Config.AutoStart.Max.TimeOfDay;

            bool ret;
            if (start < end)
                ret = (start <= now && now <= end);
            else
                ret = !(end < now && now < start);

            if (ret && maxReached)
            {
                var dt = timeSleep.Add(new TimeSpan(1, 0, 0, 0)); //add a day
                var dx = timeSleep.TimeOfDay - start;
                dt = dt.Subtract(dx); //add a day
                if (Now < dt)
                    ret = false;
            }

            return ret;

        }
        protected void AutoPilot(bool maxReached)
        {
            DateTime timeSleep = DateTime.Now;
            while (!MustBeActive(maxReached, DateTime.Now, timeSleep))
            {
                if (Status != STATUS.SLEEPING)
                {
                    Status = STATUS.SLEEPING;
                    Logging.Log(account.Username, this.GetType().ToString(), "Sleeping ZzZzZzZz...");
                }
                if (Aborted)
                    return;
                Thread.Sleep(5000);                
            }
            if (Status == STATUS.SLEEPING)
            {
                Logging.Log(account.Username, this.GetType().ToString(), "Good morning!");
            }
            TimeSpan diff = DateTime.Now.Subtract(timeSleep);
            if (diff.TotalMinutes > 60)
            {
                //login again
                account.Status = Account.STATUS.READY;
                account.LastLogin = DateTime.Now.AddYears(-1);
                account.LoginSync(true);
            }
            if (SessionManager != null)
            SessionManager.Reset(); //after autopilot, let it start scraping from start
        }
        protected bool CheckCurrentCount()
        {
            if (!Config.Autopilot && CurrentCount.Min >= CurrentCount.Max)
            {
                Logging.Log(account.Username, this.GetType().ToString(), "I'm done!");
                Status = STATUS.DONE;
                Aborted = true;
            }
            else if (Config.Autopilot && CurrentCount.Min >= CurrentCount.Max)
            {
                Logging.Log(account.Username, this.GetType().ToString(), "I'm done for today!");
                Random r = new Random();
                AutoPilot(true);
                CurrentCount = new Range<int>(0, r.Next(Config.CurrentCount.Min, Config.CurrentCount.Max + 1));
            }
            else if (Config.Autopilot)
            {
                AutoPilot(false);
            }
            return Aborted;
        }
        protected virtual bool ContinueRunning()
        {
            if (Aborted)
            {
                Logging.Log(account.Username, this.GetType().ToString(), "Headshot");
                return false;
            }
            else if (CheckCurrentCount())
                return false;
            else
                Status = STATUS.ACTIVE;

            return true;
        }

        protected abstract Data GetData(PinterestObject P);
        protected string MakeRequest(string _url, string _ref, string ctype)
        {
            if (!http.ValidUrl(_url))
                return null;


            string _rs = "";
            List<string> headers = null;
            if (ctype == http.ACCEPT_JSON)
                headers = new List<string>
                    {   "X-NEW-APP: 1",
                        "X-APP-VERSION: " + account.AppVersion,
                        "X-Requested-With: XMLHttpRequest",
                        //"X-CSRFToken: " + account.CsrfToken,
                        "X-Pinterest-AppState: active",
                        "Accept-Encoding: gzip, deflate",
                        "Accept-Language: en-US,en;q=0.8"/*,
                        "Origin: https://www.pinterest.com"*/ };

            try
            {
                Task<string> task =
                    Task.Run<string>(() =>
                                request.GET(
                                    _url,
                                    _ref,
                                    account.CookieContainer,
                                    headers,
                                    account.WebProxy,
                                    60000,
                                    ctype)
                        );
                task.Wait();
                _rs = task.Result;
               /* if (ctype == http.ACCEPT_JSON)
                    _rs = _rs.Replace("\\", "");*/
            }
            catch (Exception ex)
            {
                string msg = "Error ALGO205." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log(account.Username, this.GetType().ToString(), msg);
                
            }
            return _rs;
        }
    }
}
