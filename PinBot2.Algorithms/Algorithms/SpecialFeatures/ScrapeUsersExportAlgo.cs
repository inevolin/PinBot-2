using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PinBot2;
using PinBot2.Dal.Interface;
using PinBot2.Model.Configurations;
using PinBot2.Model.PinterestObjects;
using PinBot2.Model;
using PinBot2.Common;
using System.Threading;
using System.Web;
using PinBot2.Algorithms.Scraping;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using PinBot2.Model.Configurations.SpecialFeatures;
using System.IO;
namespace PinBot2.Algorithms
{
    public class ScrapeUsersExportAlgo : Algo
    {
        private bool DoLimitScrape, AppendToFile;
        private string Filename;

        public ScrapeUsersExportAlgo(IAccountRepository repository)
            : base(repository)
        {
            Config = new ScrapeUsersExportConfiguration();
        }

        public ScrapeUsersExportAlgo(IAccount account, IConfiguration config, IAccountRepository repository, string Filename, bool AppendToFile)
            : base(repository)
        {
            this.AppendToFile = AppendToFile;
            this.Filename = Filename;
            this.account = account; //cookiecontainer and misc.
            Config = (ScrapeUsersExportConfiguration)config;
            DoLimitScrape = ((ScrapeUsersExportConfiguration)config).DoLimitScrape;
            Running = Config.Enabled; // if false, don't Run
            // CurrentCount defines a random 'goal'-value for the session.
            Random r = new Random();
            CurrentCount = new Range<int>(0, r.Next(Config.CurrentCount.Min, Config.CurrentCount.Max + 1));

            SessionManager = new ScrapeSessionManager(this, Config, request, this.account);
        }
        private IList<string> usernames = new List<string>();
        private void LoadFromFile() //prevent dups
        {
            try
            {
                if (!AppendToFile) //clear all
                    using (StreamWriter sw = new StreamWriter(Filename, AppendToFile))
                    { sw.Write(""); }

                var str = System.IO.File.ReadAllLines(Filename);
                foreach (var s in str)
                    usernames.Add(s);
            }
            catch (Exception ex)
            {
                string msg = "Error SUEALGO61." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log(account.Username, this.GetType().ToString(), msg);

            }
        }
        public override void Run()
        {
            LoadFromFile();
            Running = true;
            while (Running)
            {
                try
                {
                    if (!ContinueRunning()) { Running = false; return; }
                    IList<PinterestObject> Pins = SessionManager.Scrape(this.GetType().ToString());
                    if (!ContinueRunning()) { Running = false; return; }

                    if (Pins == null) { Status = STATUS.LIMIT_REACHED; Running = false; return; }
                    if (Pins.Count <= 0) continue;

                    for (int i = 0; i < Pins.Count; i++)
                    {
                        PinterestObject pin = Pins[i];
                        if (SessionManager.UsedPinterestObjects.Contains(pin)
                            ||
                            usernames.Contains(((Pinner)pin).Username))
                            continue;

                        if (!ContinueRunning()) { Running = false; return; }

                        if (pin.GetType() == typeof(Pinner))
                        {
                            if (!Inspect((Pinner)pin))// || ((Pinner)pin).FollowedByMe == true)
                                continue;
                            Console.WriteLine(((Pinner)pin).Username);
                            //save

                            using (StreamWriter sw = new StreamWriter(Filename, true))
                            {
                                sw.WriteLine(((Pinner)pin).Username);
                            }
                        }

                        if (!ContinueRunning()) { Running = false; return; }
                    }
                    if (Pins.Count > 0)
                        TimeOut();
                }
                catch (Exception ex)
                {
                    string msg = "Error SUEALGO76." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                    Logging.Log(account.Username, this.GetType().ToString(), msg);

                }
            }
        }

        protected override bool ContinueRunning()
        {
            if (Aborted)
            {
                Logging.Log(account.Username, this.GetType().ToString(), "Headshot");
                return false;
            }
            else if (DoLimitScrape && CheckCurrentCount())
                return false;

            return true;
        }

        //not used
        protected override Data GetData(PinterestObject p)
        {
            return new Data();
        }

        private bool Inspect(Pinner pin)
        {
            ScrapeUsersExportConfiguration c = (ScrapeUsersExportConfiguration)Config;
            if (!c.UsersCriteria)
                return true;

            string url = "/" + pin.Username + "/";
            string _ref = pin.BaseUsername;
            int pincount = pin.PinsCount;
            int followerscount = pin.FollowersCount;

            if (!(pincount >= c.UserPins.Min && pincount <= c.UserPins.Max)) return false;
            if (!(followerscount >= c.UserFollowers.Min && followerscount <= c.UserFollowers.Max)) return false;

            try
            {
                if (!http.ValidUrl(baseUrl + url))
                    return false;

                string _rs = MakeRequest(baseUrl + url, baseUrl + _ref, http.ACCEPT_HTML);

                int boardcount = int.Parse(Regex.Match(_rs, "name=\"pinterestapp:boards\" content=\"(\\d+)\"").Groups[1].Value);
                int followingcount = int.Parse(Regex.Match(_rs, "name=\"pinterestapp:following\" content=\"(\\d+)\"").Groups[1].Value);

                if (!(followingcount >= c.UserFollowing.Min && followingcount <= c.UserFollowing.Max)) return false;
                if (!(boardcount >= c.UserBoards.Min && boardcount <= c.UserBoards.Max)) return false;

                bool hasCustomPic = !_rs.Contains("https://s-passets-cache-ak0.pinimg.com/images/user/default_140.png");
                if (c.HasCustomPic.HasValue && c.HasCustomPic != hasCustomPic) return false;

                bool hasWebsite = _rs.Contains("\"website_url\":\"http");
                if (c.HasWebsite.HasValue && hasWebsite != c.HasWebsite) return false;

                bool hasFb = _rs.Contains("\"facebook_url\":\"http");
                if (c.HasFb.HasValue && hasFb != c.HasFb) return false;

                bool hasTw = _rs.Contains("\"twitter_url\":\"http");
                if (c.HasTw.HasValue && hasTw != c.HasTw) return false;

                bool hasLocation = !_rs.Contains("\"location\":\"\"");
                if (c.HasLocation.HasValue && hasLocation != c.HasLocation) return false;

                bool hasAbout = !_rs.Contains("\"about\":\"\"");
                if (c.HasAboutText.HasValue && hasAbout != c.HasAboutText) return false;



            }
            catch (Exception ex)
            {
                string msg = "Error SUEALGO119." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log(account.Username, this.GetType().ToString(), msg);

                return false;
            }
            return true;
        }

    }
}
