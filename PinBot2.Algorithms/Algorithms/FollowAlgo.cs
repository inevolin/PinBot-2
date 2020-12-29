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
    public class FollowAlgo : Algo
    {
        public FollowAlgo(IAccountRepository repository)
            : base(repository)
        {
            Config = new FollowConfiguration();
        }

        public FollowAlgo(IAccount account, IConfiguration config, IAccountRepository repository)
            : base(repository)
        {
            this.account = account; //cookiecontainer and misc.
            Config = (FollowConfiguration)config;
            Running = Config.Enabled; // if false, don't Run
            // CurrentCount defines a random 'goal'-value for the session.
            Random r = new Random();
            CurrentCount = new Range<int>(0, r.Next(Config.CurrentCount.Min, Config.CurrentCount.Max + 1));

            SessionManager = new ScrapeSessionManager(this, Config, request, this.account);
        }
        public override void Run()
        {
            try
            {
                if (
               SessionManager.Sessions.Any(y => y.resource.Equals(PinterestObjectResources.IndividualUser))
               &&
               !SessionManager.Sessions.Any(y => !y.resource.Equals(PinterestObjectResources.IndividualUser))
               )
                {
                    int cnt =
                        SessionManager.Sessions.Select(y =>
                            y.PinterestObjects.Select(z =>
                                z.Resource.GetType().Equals(PinterestObjectResources.IndividualUser)
                            )
                        ).Count();
                    CurrentCount = new Range<int>(0, cnt);
                }
            }
            catch (Exception ex)
            {
                string msg = "Error FOALGO120." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log(account.Username, this.GetType().ToString(), msg);
            }

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
                            if (!Inspect((Board)pin) || ((Board)pin).FollowedByMe == true)
                                continue;
                            FollowBoard(pin);
                        }
                        else
                        {
                            if (
                                !pin.Resource.Equals(PinterestObjectResources.IndividualUser)
                                &&
                                (!Inspect((Pinner)pin) || ((Pinner)pin).FollowedByMe == true)
                            )
                            { continue; }

                            if (((Pinner)pin).FollowedByMe)
                            {
                                ++CurrentCount.Min;
                                if (SessionManager != null)
                                    SessionManager.UsedPinterestObjects.Add(pin);
                            }
                            else
                            {
                                FollowPinner(pin);
                            }
                        }

                        if (!ContinueRunning()) { Running = false; return; }

                        TimeOut();
                    }
                }
                catch (Exception ex)
                {
                    string msg = "Error FOALGO80." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                    Logging.Log(account.Username, this.GetType().ToString(), msg);

                }
            }
        }

        private void FollowBoard(PinterestObject p)
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
                    "Origin: https://www.pinterest.com"*/
                };

            string rs = null;

            try
            {
                rs =
                    request.POST(
                        "https://www.pinterest.com/resource/BoardFollowResource/create/",
                        data.referer,
                        "application/x-www-form-urlencoded; charset=UTF-8",
                        http_data,
                        headers,
                        account.CookieContainer,
                        account.WebProxy,
                        100000,
                        "application/json, text/javascript, */*; q=0.01");

                Logging.Log(account.Username, this.GetType().ToString(), "follow POST" + Environment.NewLine + Environment.NewLine + strBuilder.ToString());
            }
            catch (Exception ex)
            {
                string msg = "Error FOALGO124." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log(account.Username, this.GetType().ToString(), msg);

            }

            ProcessResponse(rs, p);
        }
        private void FollowPinner(PinterestObject p)
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
                    "Origin: https://www.pinterest.com" */
                };

            string rs = null;

            try
            {
                rs =
                    request.POST(
                        "https://www.pinterest.com/resource/UserFollowResource/create/",
                        data.referer,
                        "application/x-www-form-urlencoded; charset=UTF-8",
                        http_data,
                        headers,
                        account.CookieContainer,
                        account.WebProxy,
                        100000,
                        "application/json, text/javascript, */*; q=0.01");

                Logging.Log(account.Username, this.GetType().ToString(), "follow POST" + Environment.NewLine + Environment.NewLine + strBuilder.ToString());
            }
            catch (Exception ex)
            {
                string msg = "Error FOALGO169." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
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
                case PinterestObjectResources.SearchResource:
                    data.content =
                            "source_url="
                            + HttpUtility.UrlEncode("/search/boards/?q=" + p.SearchQuery)
                            + "&data="
                            + HttpUtility.UrlEncode("{\"options\":{\"board_id\":\"" + p.Id + "\"},\"context\":{}}")
                            + "&module_path="
                            + HttpUtility.UrlEncode("App()>SearchPage(resource=BaseSearchResource(constraint_string=null, show_scope_selector=true, restrict=null, scope=boards, query=" + p.SearchQuery + "))"
                            + ">SearchPageContent(resource=SearchResource(layout=null, places=null, constraint_string=null, show_scope_selector=true, query=" + p.SearchQuery + ", scope=boards))"
                            + ">Grid(resource=SearchResource(layout=null, places=null, constraint_string=null, show_scope_selector=true, query=" + p.SearchQuery + ", scope=boards))"
                            + ">GridItems(resource=SearchResource(layout=null, places=null, constraint_string=null, show_scope_selector=true, query=" + p.SearchQuery + ", scope=boards))"
                            + ">Board(resource=BoardResource(board_id=" + p.Id + "))"
                            + ">BoardFollowButton(board_id=" + p.Id + ", followed=false, class_name=boardFollowUnfollowButton, unfollow_text=Unfollow, unfollow_ga_category=board_unfollow, disabled=false, color=null, text=Follow, log_element_type=37, follow_ga_category=board_follow, user_id=" + p.UserId + ", follow_text=Follow, follow_class=null, is_my_board=null)");
                    data.referer = "https://www.pinterest.com/search/boards/?q=" + HttpUtility.UrlEncode(p.SearchQuery);
                    break;

                case PinterestObjectResources.BoardFollowingResource:
                    data.content =
                        "source_url="
                        + HttpUtility.UrlEncode("/" + p.Username + "/following/")
                        + "&data="
                        + HttpUtility.UrlEncode("{\"options\":{\"board_id\":\"" + p.Id + "\"},\"context\":{}}")
                        + "&module_path="
                        + HttpUtility.UrlEncode("App()>UserProfilePage(resource=UserResource(username=" + p.Username + "))"
                        + ">UserProfileContent(resource=UserResource(username=" + p.Username + "))"
                        + ">Grid(resource=BoardFollowingResource(username=" + p.Username + "))"
                        + ">GridItems(resource=BoardFollowingResource(username=" + p.Username + "))"
                        + ">Board(resource=BoardResource(board_id=" + p.Id + "))"
                        + ">BoardFollowButton(followed=false, board_id=" + p.Id + ", class_name=boardFollowUnfollowButton, user_id=" + p.UserId + ", follow_class=default, log_element_type=37, text=Follow, color=default, disabled=false, follow_text=Follow, unfollow_text=Unfollow, is_my_board=undefined, follow_ga_category=board_follow, unfollow_ga_category=board_unfollow)");
                    data.referer = "https://www.pinterest.com/" + p.Username + "/following/";
                    break;

                case PinterestObjectResources.ProfileBoardsResource:
                    data.content =
                        "source_url="
                        + HttpUtility.UrlEncode("/" + p.Username + "/")
                        + "&data="
                        + HttpUtility.UrlEncode("{\"options\":{\"board_id\":\"" + p.Id + "\"},\"context\":{}}")
                        + "&module_path="
                        + HttpUtility.UrlEncode("App()>UserProfilePage(resource=UserResource(username=" + p.Username + "))"
                        + ">UserProfileContent(resource=UserResource(username=" + p.Username + "))"
                        + ">UserBoards()>Grid(resource=ProfileBoardsResource(username=" + p.Username + "))"
                        + ">GridItems(resource=ProfileBoardsResource(username=" + p.Username + "))"
                        + ">Board(resource=BoardResource(board_id=" + p.Id + "))"
                        + ">BoardFollowButton(board_id=" + p.Id + ", followed=false, class_name=boardFollowUnfollowButton, unfollow_text=Unfollow, unfollow_ga_category=board_unfollow, disabled=false, color=null, text=Follow, log_element_type=37, follow_ga_category=board_follow, user_id=" + p.UserId + ", follow_text=Follow, follow_class=null, is_my_board=null)");
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
                case PinterestObjectResources.SearchResource:
                    data.content =
                        "source_url=" +
                        HttpUtility.UrlEncode("/search/people/?q=" + HttpUtility.UrlEncode(p.SearchQuery))
                        + "&data="
                        + HttpUtility.UrlEncode("{\"options\":{\"user_id\":\"" + p.Id + "\"},\"context\":{}}")
                        + "&module_path="
                        + HttpUtility.UrlEncode("App()>SearchPage(resource=BaseSearchResource(constraint_string=null, show_scope_selector=true, restrict=null, scope=people, query=" + p.SearchQuery + "))"
                        + ">SearchPageContent(resource=SearchResource(layout=null, places=false, constraint_string=null, show_scope_selector=true, query=" + p.SearchQuery + ", scope=people))"
                        + ">Grid(resource=SearchResource(layout=null, places=false, constraint_string=null, show_scope_selector=true, query=" + p.SearchQuery + ", scope=people))"
                        + ">GridItems(resource=SearchResource(layout=null, places=false, constraint_string=null, show_scope_selector=true, query=" + p.SearchQuery + ", scope=people))"
                        + ">User(resource=UserResource(username=" + p.Username + "))"
                        + ">UserFollowButton(followed=false, class_name=gridItem, unfollow_text=Unfollow, unfollow_ga_category=user_unfollow, disabled=false, is_me=false, text=Follow, follow_class=default, log_element_type=62, follow_ga_category=user_follow, user_id=" + p.Id + ", follow_text=Follow, color=default)");
                    data.referer = "https://www.pinterest.com/search/people/?q=" + HttpUtility.UrlEncode(p.SearchQuery);
                    break;

                case PinterestObjectResources.UserFollowersResource:
                    data.content =
                        "source_url=" +
                        HttpUtility.UrlEncode("/" + p.Username + "/followers/")
                        + "&data="
                        + HttpUtility.UrlEncode("{\"options\":{\"user_id\":\"" + p.Id + "\"},\"context\":{}}")
                        + "&module_path="
                        + HttpUtility.UrlEncode("App()>UserProfilePage(resource=UserResource(username=" + p.BaseUsername + "))"
                        + ">UserProfileContent(resource=UserResource(username=" + p.BaseUsername + "))"
                        + ">Grid(resource=UserFollowersResource(username=" + p.BaseUsername + "))"
                        + ">GridItems(resource=UserFollowersResource(username=" + p.BaseUsername + "))"
                        + ">User(resource=UserResource(username=" + p.Username + "))"
                        + ">UserFollowButton(user_id=" + p.Id + ", follow_class=default, followed=false, class_name=gridItem, log_element_type=62, text=Follow, color=default, disabled=false, follow_text=Follow, unfollow_text=Unfollow, is_me=false, follow_ga_category=user_follow, unfollow_ga_category=user_unfollow)");
                    data.referer = "https://www.pinterest.com/" + p.BaseUsername + "/followers/";
                    break;

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
                        + ">UserFollowButton(user_id=" + p.Id + ", follow_class=default, followed=false, class_name=gridItem, log_element_type=62, text=Follow, color=default, disabled=false, follow_text=Follow, unfollow_text=Unfollow, is_me=false, follow_ga_category=user_follow, unfollow_ga_category=user_unfollow)");
                    data.referer = "https://www.pinterest.com/" + p.BaseUsername + "/following/";
                    break;

                case PinterestObjectResources.BoardFollowersResource:
                    data.content =
                            "source_url="
                            + HttpUtility.UrlEncode("/" + p.BaseUsername + "/" + p.SearchQuery + "/")
                            + "&data="
                            + HttpUtility.UrlEncode("{\"options\":{\"user_id\":\"" + p.Id + "\"},\"context\":{}}")
                            + "&module_path="
                            + HttpUtility.UrlEncode("App>ModalManager>Modal>BoardFollowers>PagedGrid>Grid>GridItems>User>UserFollowButton(user_id=" + p.Id + ", follow_class=primary, followed=false, log_element_type=62, text=Follow, color=primary, disabled=false, follow_text=Follow, unfollow_text=Unfollow, is_me=false, follow_ga_category=user_follow, unfollow_ga_category=user_unfollow, state_disabled=true)");
                    //data.content = Regex.Replace(data.content, "%\\d(\\w)", m => m.ToString().ToUpper());
                    data.referer = "https://www.pinterest.com" + "/" + p.BaseUsername + "/" + p.SearchQuery + "/";
                    break;

                case PinterestObjectResources.IndividualUser:
                    data.content = "source_url=" +
                        HttpUtility.UrlEncode("/" + p.Username + "/")
                        + "&data="
                        + HttpUtility.UrlEncode("{\"options\":{\"user_id\":\"" + p.Id + "\"},\"context\":{}}")
                        + "&module_path="
                        + HttpUtility.UrlEncode("App>UserProfilePage>UserProfileHeader>UserFollowButton(followed=false, is_me=false, text=Follow, memo=[object Object], disabled=false, suggested_users_menu=[object Object], follow_ga_category=user_follow, follow_text=Follow, follow_class=primary, user_id=" + p.Id + ", unfollow_text=Unfollow, unfollow_ga_category=user_unfollow, color=primary, state_disabled=true)");
                    data.referer = "https://www.pinterest.com/";
                    break;
            }

            return data;
        }

        private bool Inspect(Pinner pin)
        {
            FollowConfiguration c = (FollowConfiguration)Config;
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

            }
            catch (Exception ex)
            {
                string msg = "Error FOALGO336." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log(account.Username, this.GetType().ToString(), msg);

                return false;
            }
            return true;
        }
        private bool Inspect(Board board)
        {
            FollowConfiguration c = (FollowConfiguration)Config;
            if (!c.BoardsCriteria)
                return true;

            string url = "/" + board.Url + "/";
            string _ref = "/" + board.Username + "/";
            int pincount = board.PinsCount;

            if (!(pincount >= c.BoardPins.Min && pincount <= c.BoardPins.Max)) return false;

            try
            {
                string _rs = MakeRequest(baseUrl + url, baseUrl + _ref, http.ACCEPT_HTML);

                int followers = int.Parse(Regex.Match(_rs, "name=\"followers\" content=\"(\\d+)\"").Groups[1].Value);
                //int pins = int.Parse(Regex.Match(_rs, "name=\"pinterestapp:pins\" content=\"(\\d+)\"").Groups[1].Value);

                if (!(followers >= c.BoardFollowers.Min && followers <= c.BoardFollowers.Max)) return false;
                //if (!(pins >= c.BoardPins.Min && pins <= c.BoardPins.Max)) return false;

            }
            catch (Exception ex)
            {
                string msg = "Error FOALGO364." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log(account.Username, this.GetType().ToString(), msg);

                return false;
            }
            return true;
        }
    }
}
