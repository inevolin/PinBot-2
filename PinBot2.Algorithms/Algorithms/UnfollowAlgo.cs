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
namespace PinBot2.Algorithms
{
    public class UnfollowAlgo : Algo
    {
        public UnfollowAlgo(IAccountRepository repository)
            : base(repository)
        {
            Config = new UnfollowConfiguration();
        }

        public UnfollowAlgo(IAccount account, IConfiguration config, IAccountRepository repository)
            : base(repository)
        {
            this.account = account; //cookiecontainer and misc.
            Config = (UnfollowConfiguration)config;
            Running = Config.Enabled; // if false, don't Run
            // CurrentCount defines a random 'goal'-value for the session.
            Random r = new Random();
            CurrentCount = new Range<int>(0, r.Next(Config.CurrentCount.Min, Config.CurrentCount.Max + 1));

            SessionManager = new ScrapeSessionManager(this, Config, request, this.account);
        }
        public override void Run()
        {
            Running = true;
            while (Running)
            {
                Console.WriteLine(this.ToString());
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
                        if (SessionManager.UsedPinterestObjects.Contains(pin))
                            continue;

                        if (!ContinueRunning()) { Running = false; return; }

                        if (pin.GetType() == typeof(Board))
                        {
                            if (!Inspect((Board)pin) || ((Board)pin).FollowedByMe == false)
                                continue;
                            UnfollowBoard(pin);
                        }
                        else
                        {
                            if (!Inspect((Pinner)pin) || ((Pinner)pin).FollowedByMe == false)
                                continue;
                            UnfollowPinner(pin);
                        }
                        
                        if (!ContinueRunning()) { Running = false; return; }

                        TimeOut();
                    }
                }
                catch (Exception ex)
                {
                    string msg = "Error UFALGO80." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                    Logging.Log(account.Username, this.GetType().ToString(), msg);
                    
                }
            }
        }

        private void UnfollowBoard(PinterestObject p)
        {
            Data data = GetData(p);

            StringBuilder strBuilder = new StringBuilder();
            strBuilder.AppendLine(data.content);
            byte[] hdrbytes = System.Text.Encoding.UTF8.GetBytes(strBuilder.ToString());

            List<byte[]> http_data = new List<byte[]>();
            http_data.Add(hdrbytes);
            List<string> headers = new List<string>
                {   "X-NEW-APP: 1",
                    "X-APP-VERSION: " + account.AppVersion,
                    "X-Requested-With: XMLHttpRequest",
                    "X-CSRFToken: " + account.CsrfToken,
                    "Accept-Encoding: gzip,deflate,sdch",
                    "Accept-Language: en-US,en;q=0.8"/*,
                    "Origin: https://www.pinterest.com"*/ };

            string rs = null;

            try
            {
                rs =
                    request.POST(
                        "https://www.pinterest.com/resource/BoardFollowResource/delete/",
                        data.referer,
                        "application/x-www-form-urlencoded; charset=UTF-8",
                        http_data,
                        headers,
                        account.CookieContainer,
                        account.WebProxy,
                        100000,
                        "application/json, text/javascript, */*; q=0.01");

                Logging.Log(account.Username, this.GetType().ToString(), "unfollow POST" + Environment.NewLine + Environment.NewLine + strBuilder.ToString());
            }
            catch (Exception ex)
            {
                string msg = "Error UFALGO123." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log(account.Username, this.GetType().ToString(), msg);
                
            }

            ProcessResponse(rs, p);
        }
        private void UnfollowPinner(PinterestObject p)
        {
            Data data = GetData(p);

            StringBuilder strBuilder = new StringBuilder();
            strBuilder.AppendLine(data.content);
            byte[] hdrbytes = System.Text.Encoding.UTF8.GetBytes(strBuilder.ToString());

            List<byte[]> http_data = new List<byte[]>();
            http_data.Add(hdrbytes);
            List<string> headers = new List<string>
                {   "X-NEW-APP: 1",
                    "X-APP-VERSION: " + account.AppVersion,
                    "X-Requested-With: XMLHttpRequest",
                    "X-CSRFToken: " + account.CsrfToken,
                    "Accept-Encoding: gzip,deflate,sdch",
                    "Accept-Language: en-US,en;q=0.8"/*,
                    "Origin: https://www.pinterest.com"*/ };

            string rs = null;

            try
            {
                rs =
                    request.POST(
                        "https://www.pinterest.com/resource/UserFollowResource/delete/",
                        data.referer,
                        "application/x-www-form-urlencoded; charset=UTF-8",
                        http_data,
                        headers,
                        account.CookieContainer,
                        account.WebProxy,
                        100000,
                        "application/json, text/javascript, */*; q=0.01");

                Logging.Log(account.Username, this.GetType().ToString(), "unfollow POST" + Environment.NewLine + Environment.NewLine + strBuilder.ToString());
            }
            catch (Exception ex)
            {
                string msg = "Error UFALGO167." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log(account.Username, this.GetType().ToString(), msg);
            }

            ProcessResponse(rs, p);
        }
        protected override Data GetData(PinterestObject p)
        {
            if (p.GetType() == typeof(Board))
                return GetData_Board(p);
            else
                return GetData_Pinner(p);
        }


        private Data GetData_Board(PinterestObject P)
        {
            Board p = (Board)P;
            Data data = new Data();
            switch (P.Resource)
            {
                case PinterestObjectResources.BoardFollowingResource:
                    data.content =
                        "source_url=/" + p.Username + "/following/"
                        + "&data="
                        + HttpUtility.UrlEncode("{\"options\":{\"board_id\":\"" + p.Id + "\"},\"context\":{}}")
                        + "&module_path="
                        + HttpUtility.UrlEncode("App()>UserProfilePage(resource=UserResource(username=" + p.Username + "))"
                        + ">UserProfileContent(resource=UserResource(username=" + p.Username + "))"
                        + ">Grid(resource=BoardFollowingResource(username=" + p.Username + "))"
                        + ">GridItems(resource=BoardFollowingResource(username=" + p.Username + "))"
                        + ">Board(resource=BoardResource(board_id=" + p.Id + "))"
                        + ">BoardFollowButton(followed=true, board_id=" + p.Id + ", class_name=boardFollowUnfollowButton, user_id=" + p.UserId + ", follow_class=default, log_element_type=37, text=Unfollow, color=dim, disabled=false, follow_text=Follow, unfollow_text=Unfollow, is_my_board=undefined, follow_ga_category=board_follow, unfollow_ga_category=board_unfollow)");
                    data.referer = "https://www.pinterest.com/" + p.Username + "/following/";
                    break;
            }

            return data;
        }
        private Data GetData_Pinner(PinterestObject P)
        {
            Pinner p = (Pinner)P;
            Data data = new Data();
            switch (P.Resource)
            {               
                case PinterestObjectResources.UserFollowingResource:
                    data.content =
                        "source_url=" +
                        HttpUtility.UrlEncode("/" + p.Username + "/following/")
                        + "&data="
                        + HttpUtility.UrlEncode("{\"options\":{\"user_id\":\"" + p.Id + "\"},\"context\":{}}")
                        + "&module_path="
                        + HttpUtility.UrlEncode("App()>UserProfilePage(resource=UserResource(username=" + p.BaseUsername + "))"
                        + ">UserProfileContent(resource=UserResource(username=" + p.BaseUsername + "))"
                        + ">Grid(resource=UserFollowingResource(username=" + p.BaseUsername + "))"
                        + ">GridItems(resource=UserFollowingResource(username=" + p.BaseUsername + "))"
                        + ">User(resource=UserResource(username=" + p.Username + "))"
                        + ">UserFollowButton(user_id=" + p.Id + ", follow_class=default, followed=true, class_name=gridItem, log_element_type=62, text=Unfollow, color=dim, disabled=false, follow_text=Follow, unfollow_text=Unfollow, is_me=false, follow_ga_category=user_follow, unfollow_ga_category=user_unfollow)");
                    data.referer = "https://www.pinterest.com/" + p.BaseUsername + "/following/";
                    break;
            }

            return data;
        }

        private bool Inspect(Pinner pin)
        {
            UnfollowConfiguration c = (UnfollowConfiguration)Config;
            if (!c.UsersCriteria)
                return true;

            string url = "/" + ((Pinner)pin).Username + "/";
            string _ref = ((Pinner)pin).BaseUsername;
            int pincount = ((Pinner)pin).PinsCount;
            int followerscount = ((Pinner)pin).FollowersCount;

            if (!(pincount >= c.UserPins.Min && pincount <= c.UserPins.Max)) return true;
            if (!(followerscount >= c.UserFollowers.Min && followerscount <= c.UserFollowers.Max)) return true;

            try
            {
                if (!http.ValidUrl(baseUrl + url))
                    return false;

                string _rs = MakeRequest(baseUrl + url, baseUrl + _ref, http.ACCEPT_HTML);

                int boardcount = int.Parse(Regex.Match(_rs, "name=\"pinterestapp:boards\" content=\"(\\d+)\"").Groups[1].Value);
                int followingcount = int.Parse(Regex.Match(_rs, "name=\"pinterestapp:following\" content=\"(\\d+)\"").Groups[1].Value);

                if (!(followingcount >= c.UserFollowing.Min && followingcount <= c.UserFollowing.Max)) return true;
                if (!(boardcount >= c.UserBoards.Min && boardcount <= c.UserBoards.Max)) return true;

            }
            catch (Exception ex)
            {
                string msg = "Error UFALGO259." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log(account.Username, this.GetType().ToString(), msg);
                return false;
            }
            return true;
        }
        private bool Inspect(Board pin)
        {
            UnfollowConfiguration c = (UnfollowConfiguration)Config;
            if (!c.BoardsCriteria)
                return true;

            string url = "/" + ((Board)pin).Username + "/";
            string _ref = ((Board)pin).BaseUsername;
            int pincount = ((Board)pin).PinsCount;

            if (!(pincount >= c.BoardPins.Min && pincount <= c.BoardPins.Max)) return true;

            try
            {
                if (!http.ValidUrl(baseUrl + url))
                    return false;


                string _rs = MakeRequest(baseUrl + url, baseUrl + _ref, http.ACCEPT_HTML);

                int followers = int.Parse(Regex.Match(_rs, "name=\"pinterestapp:followers\" content=\"(\\d+)\"").Groups[1].Value);
                //int pins = int.Parse(Regex.Match(_rs, "name=\"pinterestapp:pins\" content=\"(\\d+)\"").Groups[1].Value);

                if (!(followers >= c.BoardFollowers.Min && followers <= c.BoardFollowers.Max)) return true;
                //if (!(pins >= c.BoardPins.Min && pins <= c.BoardPins.Max)) return false;

            }
            catch (Exception ex)
            {
                string msg = "Error UFALGO290." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log(account.Username, this.GetType().ToString(), msg);
                return false;
            }
            return true;
        }

    }
}
