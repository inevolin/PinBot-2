using PinBot2.Common;
using PinBot2.Model.PinterestObjects;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Linq;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PinBot2.Model
{
    [Serializable]// [ProtoBuf.ProtoContract(SkipConstructor = true)]
    public struct AccountInformation
    {
 
        public int Followers;

 
        public int Following;

 
        public int Pins;

 
        public int Likes;

 
        public int Boards;
    }

    [Serializable]// [ProtoBuf.ProtoContract(SkipConstructor = true)]
    public class Account : IAccount
    {

        public enum STATUS { READY, INVALID_PROXY, LOGGED_IN, WRONG_EMAIL_OR_PASS, NOT_CONFIGURED, LIMITED };

        [NonSerialized]// [ProtoBuf.ProtoIgnore]
        private http req;
        public http Request
        {
            get
            {
                if (req == null)
                    req = new http();
                return req;
            }
        }

 
        public AccountInformation AccountInfo { get; set; }

 
        public int Id { get; set; }

 
        public string Email { get; set; }

 
        public string Username { get; set; }

 
        public string Password { get; set; }

 
        public PinBot2.Proxy WebProxy { get; set; }

 
        public DateTime LastLogin { get; set; }

        // [ProtoBuf.ProtoIgnore]
        public CookieContainer CookieContainer { get; set; }

 
        public string CsrfToken { get; set; }

 
        public string AppVersion { get; set; }

        // [ProtoBuf.ProtoMember(11)]
        public STATUS Status { get; set; }

        // [ProtoBuf.ProtoMember(12)]
        public int SelectedCampaignId { get; set; }

        // [ProtoBuf.ProtoMember(13)]
        public string UsernameSlug { get; set; }

        // [ProtoBuf.ProtoMember(14)]
        public bool IsConfigured { get; set; }

        // [ProtoBuf.ProtoMember(15)]
        public bool ValidProxy { get; set; }

        // [ProtoBuf.ProtoMember(16)]
        public bool ValidCredentials { get; set; }

        // [ProtoBuf.ProtoMember(17)]
        public bool IsLoggedIn { get; set; }

        // [ProtoBuf.ProtoMember(18)]
        public HashSet<Board> Boards { get; set; }

        public string getStatus
        {
            get
            {

                string val = "";
                switch (Status)
                {
                    case STATUS.READY:
                        val = "Ready";
                        break;
                    case STATUS.INVALID_PROXY:
                        val = "Invalid proxy";
                        break;
                    case STATUS.LOGGED_IN:
                        val = "Logged in";
                        break;
                    case STATUS.WRONG_EMAIL_OR_PASS:
                        val = "Wrong email or password";
                        break;
                    case STATUS.NOT_CONFIGURED:
                        val = "Not configured";
                        break;
                    case STATUS.LIMITED:
                        val = "Limited";
                        break;
                }
                return val;
            }
        }

        public Account()
        {
            WebProxy = null;
            CookieContainer = new CookieContainer();
        }

        public void LoginSync(bool FullLogin, http request = null)
        {
            IsLoggedIn = false;
            if (request == null)
                request = new http();

            try
            {
                Logging.Log("user", "account action", "account login process starts");
                TimeSpan diff = DateTime.Now.Subtract(LastLogin);
                if (!FullLogin && diff.TotalMinutes <= 60)
                    QuickLogin(request);
                else
                    LongLogin(request);

            }
            catch (Exception ex)
            {
                string msg = "Error ACC160." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");

                Logging.Log("user", "account action", msg);
                return;
            }

        }
        public void RefreshAccountInformation()
        {
            var temp = Status;
            if (IsLoggedIn)
                LongLogin_3(new http());
            else
                LoginSync(true);
            Status = temp;
        }

        private void QuickLogin(http request)
        {
            /*try
            {*/
            var headers = new List<string>();
            Task<string> task;
            task =
                        Task.Run<string>(() =>
                            request.GET("https://www.pinterest.com/", "https://www.pinterest.com", CookieContainer, headers, WebProxy)
                        );
            task.Wait();
            string rs = task.Result;

            GetCsrfToken();
            GetUsername(rs);
            SetAccountInformation(rs);
            LoadBoards_1(request);
            LoadBoards_2(request);
            IsLoggedIn = true;
            Status = STATUS.LOGGED_IN;

            /* }
             catch (Exception ex)
             {
                 string msg = "Error ACC140." + Environment.NewLine + ex.Message;
                 LogWriter.writeLog(msg);
             }*/
        }
        private void LongLogin(http request)
        {
            if (CookieContainer == null)
                CookieContainer = new CookieContainer();

            LongLogin_1(request);
            GetCsrfToken();
            string rs = LongLogin_2(request);
            GetCsrfToken();

            if (rs.Contains(PinterestErrors.NO_ERROR))
            {
                LastLogin = DateTime.Now;
                IsLoggedIn = true;
                Status = STATUS.LOGGED_IN;

                LongLogin_3(request);
            }
            else if (rs.Contains(PinterestErrors.WRONG_EMAIL_OR_PASS))
            {
                Status = STATUS.WRONG_EMAIL_OR_PASS;
            }
        }
        private void LongLogin_1(http request)
        {
            /*try
            {*/
            var headers = new List<string>();
            Task<string> task;

            task =
                    Task.Run<string>(() =>
                        request.GET("https://www.pinterest.com/login/", "https://www.pinterest.com", CookieContainer, headers, WebProxy)
                    );
            task.Wait();
            string rs = task.Result;
            rs = Regex.Unescape(rs);
            rs = HttpUtility.HtmlDecode(rs);
            rs = rs.Replace("\\", "");
            var m = Regex.Match(rs, "\"app_version\":\"(.+?)\"");
            AppVersion = m.Groups[1].ToString();

            /*}
            catch (Exception ex)
            {
                string msg = "Error ACC194." + Environment.NewLine + ex.Message;
                LogWriter.writeLog(msg);
            }*/

        }
        private string LongLogin_2(http request)
        {
            /*try
            {*/

            var headers = new List<string>();
            Task<string> task;

            headers = new List<string>
                {
                  "Accept-Language: en-gb,en;q=0.5",
                  "X-NEW-APP: 1",
                  "X-APP-VERSION: " + AppVersion,
                  "Accept-Encoding: gzip, deflate",
                  "Cache-Control: no-cache",
                  "Pragma: no-cache",
                  "X-Requested-With: XMLHttpRequest",
                  "X-CSRFToken: " + CsrfToken
                };

            string strLogin = "source_url="
                                + HttpUtility.UrlEncode("/login/")
                                + "&data="
                                + HttpUtility.UrlEncode("{\"options\":{\"username_or_email\":\"" + Email
                                                        + "\",\"password\":\""
                                                        + Password.Replace(@"\", @"\\")
                                                        + "\"},\"context\":{}}")
                                + "&module_path="
                                + HttpUtility.UrlEncode("App()>LoginPage()>Login()>Button(class_name=primary, text=Log in, type=submit, size=large)");

            List<byte[]> data = new List<byte[]> { Encoding.ASCII.GetBytes(strLogin) };


            task =
                Task.Run<string>(() =>
                    request.POST(
                        "https://www.pinterest.com/resource/UserSessionResource/create/",
                        "https://www.pinterest.com/login/",
                        "application/x-www-form-urlencoded; charset=UTF-8",
                        data,
                        headers,
                        CookieContainer,
                        WebProxy,
                        100000,
                        "application/json, text/javascript, */*; q=0.01")
                );
            task.Wait();
            string rs = task.Result;
            return rs;

            /*}
            catch (Exception ex)
            {
                string msg = "Error ACC251." + Environment.NewLine + ex.Message;
                LogWriter.writeLog(msg);
                return null;
            }*/
        }
        private void LongLogin_3(http request)
        {
            /*try
            {*/
            var headers = new List<string>();
            Task<string> task;

            task =
                Task.Run<string>(() =>
                    request.GET("https://www.pinterest.com/", "https://www.pinterest.com", CookieContainer, headers, WebProxy)
                );
            task.Wait();
            string rs = task.Result;

            GetUsername(rs);

            task =
                Task.Run<string>(() =>
                    request.GET("https://www.pinterest.com/" + this.Username + "/", "https://www.pinterest.com", CookieContainer, headers, WebProxy)
                );
            task.Wait();
            rs = task.Result;

            LoadBoards(request);
            SetAccountInformation(rs);

            /*}
            catch (Exception ex)
            {
                string msg = "Error ACC284." + Environment.NewLine + ex.Message;
                LogWriter.writeLog(msg);
            }*/

        }

        private void GetCsrfToken()
        {
            try
            {
                for (int z = 0; z < CookieContainer.GetCookies(new Uri("https://pinterest.com")).Count; z++)
                {
                    string cookie = CookieContainer.GetCookies(new Uri("https://pinterest.com"))[z].ToString();
                    if (cookie.Contains("csrftoken"))
                        CsrfToken = cookie.Replace("csrftoken=", "").Trim();
                }
            }
            catch (Exception ex)
            {
                string msg = "Error ACC302." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");

                Logging.Log("user", "account action", msg);
            }
        }
        private void GetUsername(string rs)
        {
            try
            {
                string pattern = "\"username\":\"([a-zA-Z0-9_\\-]+)\",\"field_set_key\":\"homefeed\"";
                rs = Regex.Unescape(rs);
                rs = HttpUtility.HtmlDecode(rs);
                rs = rs.Replace("\\", "");
                Username = Regex.Match(rs, pattern).Groups[1].Value;
            }
            catch (Exception ex)
            {
                string msg = "Error ACC314." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");

                Logging.Log("user", "account action", msg);
            }
        }
        private void SetAccountInformation(string rs)
        {

            try
            {
                string pat = "";
                pat += "\"pin_count\":(\\d+?),(?:.+?)";
                pat += "\"follower_count\":(\\d+?),(?:.+?)";
                pat += "\"following_count\":(\\d+?),(?:.+?)";
                pat += "\"like_count\":(\\d+?),(?:.+?)";
                pat += "\"board_count\":(\\d+?),";


                AccountInformation AccountInfo = this.AccountInfo;
                rs = Regex.Unescape(rs);
                rs = HttpUtility.HtmlDecode(rs);
                rs = rs.Replace("\\", "");
                Match m = Regex.Match(rs, pat);
                AccountInfo.Pins = int.Parse(m.Groups[1].Value.ToString());
                AccountInfo.Followers = int.Parse(m.Groups[2].Value.ToString());
                AccountInfo.Following = int.Parse(m.Groups[3].Value.ToString());
                AccountInfo.Likes = int.Parse(m.Groups[4].Value.ToString());
                //AccountInfo.Boards = int.Parse(m.Groups[5].Value.ToString());
                AccountInfo.Boards = Boards.Count;

                this.AccountInfo = AccountInfo;
            }
            catch //(Exception ex)
            {
                //string msg = "Error ACC285." + Environment.NewLine + ex.Message;

            }
        }

        #region Boards


        public void LoadBoards(http request)
        {
            if (Boards == null)
                Boards = new HashSet<Board>();
            Boards.Clear();
            LoadBoards_1(request);
            LoadBoards_2(request);
        }
        private void LoadBoards_1(http request)
        {
            try
            {

                bool End = false;
                string bookmark = "";
                string url = "";
                string referer = "";
                string rs = "";
                bool FirstRequest = true;

                while (!End)
                {
                    SetUrlAndRef_1(ref url, ref referer, FirstRequest, bookmark, request);
                    rs = MakeRequest(url, referer, request);
                    SetBookmark(ref bookmark, rs, ref End);
                    if (bookmark != "") FirstRequest = false;
                    ParseBoards(rs);
                }
            }
            catch (Exception ex)
            {
                string msg = "Error ACC377." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");

                Logging.Log("user", "account action", msg);
            }
        }
        private void LoadBoards_2(http request)
        {
            try
            {

                bool End = false;
                string bookmark = "";
                string url = "";
                string referer = "";
                string rs = "";
                bool FirstRequest = true;

                while (!End)
                {
                    SetUrlAndRef_2(ref url, ref referer, FirstRequest, bookmark, request);
                    rs = MakeRequest(url, referer, request);
                    SetBookmark(ref bookmark, rs, ref End);
                    if (bookmark != "") FirstRequest = false;
                    ParseBoards(rs);
                }
            }
            catch (Exception ex)
            {
                string msg = "Error ACC377." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");

                Logging.Log("user", "account action", msg);
            }
        }
        private void SetUrlAndRef_1(ref string Url, ref string Referer, bool FirstRequest, string Bookmark, http request)
        {
            try
            {
                string baseUrl = "https://www.pinterest.com";
                Referer = baseUrl;
                Url = baseUrl + GetData_1(FirstRequest, Bookmark);
                Referer = baseUrl + "/" + Username + "/";

            }
            catch (Exception ex)
            {
                string msg = "Error ACC398." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log("user", "account action: ", msg);
            }
        }
        private void SetUrlAndRef_2(ref string Url, ref string Referer, bool FirstRequest, string Bookmark, http request)
        {
            try
            {
                string baseUrl = "https://www.pinterest.com";
                Referer = baseUrl;
                Url = baseUrl + GetData_2(FirstRequest, Bookmark);
                Referer = baseUrl + "/" + Username + "/";

            }
            catch (Exception ex)
            {
                string msg = "Error ACC398." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log("user", "account action: ", msg);
            }
        }
        private string GetData_1(bool FirstRequest, string Bookmark)
        {
            try
            {
                string str = "/resource/ProfileBoardsResource/get/";

                if (FirstRequest)
                    str += "?source_url="
                       + HttpUtility.UrlEncode("/" + Username + "/")
                       + "&data="
                       + HttpUtility.UrlEncode("{\"options\":{\"field_set_key\":\"grid_item\","
                       + "\"username\":\"" + Username + "\"},"
                       + "\"context\":{}}")
                       + "&_=" + GetTime();
                else
                    str += "?source_url="
                       + HttpUtility.UrlEncode("/" + Username + "/")
                       + "&data="
                       + HttpUtility.UrlEncode("{\"options\":{\"field_set_key\":\"grid_item\","
                       + "\"username\":\"" + Username + "\","
                        + "\"bookmarks\":[\"" + Bookmark + "\"]},"
                        + "\"context\":{}}")
                        + "&_=" + GetTime();

                str = Regex.Replace(str, "%\\d(\\w)", m => m.ToString().ToUpper());
                return str;

            }
            catch (Exception ex)
            {
                string msg = "Error ACC437." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log("user", "account action: ", msg);

                return "";
            }
        }
        private string GetData_2(bool FirstRequest, string Bookmark)
        {
            try
            {
                string str = "/resource/UserResource/get/";

                if (FirstRequest)
                    str += "?source_url="
                       + HttpUtility.UrlEncode("/" + Username + "/")
                       + "&data="
                       + HttpUtility.UrlEncode("{\"options\":{\"username\":\"" + Username + "\","
                                + "\"invite_code\":null},"
                                + "\"context\":{},"
                                + "\"module\":{\"name\":\"UserProfileContent\","
                                + "\"options\":{\"tab\":\"boards\"}},"
                                + "\"render_type\":1,"
                                + "\"error_strategy\":0}")
                       + "&_=" + GetTime();
                else
                    str += "?source_url="
                       + HttpUtility.UrlEncode("/" + Username + "/")
                       + "&data="
                       + HttpUtility.UrlEncode("{\"options\":{\"field_set_key\":\"grid_item\","
                                + "\"username\":\"" + Username + "\","
                                + "\"bookmarks\":[\"" + Bookmark + "\"]},"
                                + "\"context\":{}}")
                        + "&_=" + GetTime();

                str = Regex.Replace(str, "%\\d(\\w)", m => m.ToString().ToUpper());
                return str;

            }
            catch (Exception ex)
            {
                string msg = "Error ACC437." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log("user", "account action: ", msg);

                return "";
            }
        }
        private string MakeRequest(string url, string referer, http request)
        {

            string _rs = "";
            List<string> headers = null;
            headers = new List<string>
                    {   "X-NEW-APP: 1",
                        "X-APP-VERSION: " + AppVersion,
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
                                    url,
                                    referer,
                                    CookieContainer,
                                    headers,
                                    WebProxy,
                                    60000,
                                    http.ACCEPT_JSON)
                        );
                task.Wait();
                _rs = task.Result;
            }
            catch (Exception ex)
            {
                string msg = "Error ACC393." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log("user", "account action: ", msg);

            }
            return _rs;
        }



        private void ParseBoards(string rs)
        {
            string[] keys = { "resource_data_cache", "resource_response" };
            foreach (string key in keys) {
                ParseBoards_bykey(rs, key);
            }            
        }

        private void ParseBoards_bykey(string rs, string key)
        {
            var lst = new List<Board>();
            dynamic d = JsonConvert.DeserializeObject(rs);
            object a1 = d[key];
            JToken a2 = null;
            if (a1.GetType().Equals(typeof(JArray)))
                a2 = ((JArray)a1).FirstOrDefault(o => o["data"] != null && o["data"].GetType().Equals(typeof(JArray)));
            else if (a1.GetType().Equals(typeof(JObject)) && ((JObject)a1)["data"] != null && ((JObject)a1)["data"].GetType().Equals(typeof(JArray)))
                a2 = ((JObject)a1);
            else return;

            JArray a3 = null;
            if (a2 != null) {
                a3 = (JArray)a2["data"];
            } else {
                if (a1.GetType().Equals(typeof(JArray)))
                    a2 = ((JArray)a1).FirstOrDefault(o => o["data"]["results"] != null && o["data"]["results"].GetType().Equals(typeof(JArray)));
                else if (a1.GetType().Equals(typeof(JObject)) && ((JObject)a1)["data"]["results"] != null && ((JObject)a1)["data"]["results"].GetType().Equals(typeof(JArray)))
                    a2 = ((JObject)a1)["data"]["results"];
                else return;

                if (a2 != null)
                {
                    a3 = (JArray)a2["data"]["results"];
                }
            }

            if (a3 == null) return;
            int j = 0;
            foreach (var a4 in a3)
            {
                if (j++ == 0) continue; //skip first entry;
                try
                {
                    string boardId = (string)a4["id"];
                    dynamic user = a4["owner"];
                    string userId = (string)user["id"];
                    string username = (string)user["username"];
                    string url = (string)a4["url"];
                    int pincount = (int)a4["pin_count"];
                    bool FollowedByMe = (bool)a4["followed_by_me"];
                    string boardname = (string)a4["name"];

                    Board p = new Board(boardId, userId, username, "", PinterestObjectResources.ProfileBoardsResource);
                    p.PinsCount = pincount;
                    p.FollowedByMe = FollowedByMe;
                    p.Url = url;
                    p.Boardname = boardname;

                    if (Boards.Contains(p)) //prevent dubs
                        continue;

                    Boards.Add(p);

                }
                catch (Exception ex)
                {
                    string msg = "Error ACC586." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");

                    Logging.Log("user", "account action", msg);
                }
            }
        }
        /*
        private void ParseBoards(string rs)
        {
            try
            {                
                foreach (string pattern in ParsePatterns.Boards)
                {
                    List<Match> matches = Regex.Matches(rs, pattern, RegexOptions.Multiline).Cast<Match>().ToList();
                    if (matches.Count == 0)
                        continue;

                    foreach (Match m in matches)
                    {
                        string boardId = m.Groups["boardId"].ToString();
                        string userId = m.Groups["userId"].ToString();
                        string username = m.Groups["username"].ToString();
                        string url = m.Groups["url"].ToString();
                        string boardname = m.Groups["boardname"].ToString();
                        int pincount = int.Parse(m.Groups["pins"].Value);
                        bool FollowedByMe = bool.Parse(m.Groups["followed_by_me"].Value);

                        Board p = new Board(boardId, userId, username, "", PinterestObjectResources.ProfileBoardsResource);
                        p.PinsCount = pincount;
                        p.FollowedByMe = FollowedByMe;
                        p.Url = url;
                        p.Boardname = boardname;

                        if (Boards.Contains(p)) //prevent dubs
                            continue;

                        Boards.Add(p);

                    }
                }

            }
            catch (Exception ex)
            {
                string msg = "Error ACC518." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");

                Logging.Log("user", "account action", msg);
            }
        }*/
        protected static Int64 GetTime()
        {
            Int64 retval = 0;
            var st = new System.DateTime(1970, 1, 1);
            TimeSpan t = (System.DateTime.Now.ToUniversalTime() - st);
            retval = (Int64)(t.TotalMilliseconds + 0.5);
            return retval;
        }
        private void SetBookmark(ref string Bookmark, string rs, ref bool End)
        {
            try
            {
                Match match = Regex.Match(rs, "\"bookmarks\": \\[\"(?=[^\\-])(.+?)\"\\]", RegexOptions.Multiline);
                Bookmark = match.Groups[1].ToString();
                if (Bookmark.Length == 0)
                    End = true;
            }
            catch (Exception ex)
            {
                string msg = "Error ACC450." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");

                Logging.Log("user", "account action", msg);
                End = true;
            }
        }

        #endregion

        public override string ToString()
        {
            return string.Format("{0} |{1}|", Email, Status);
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            IAccount p = obj as Account;
            if ((System.Object)p == null)
            {
                return false;
            }

            return (Id == p.Id);
        }
        public bool Equals(IAccount p)
        {
            if ((object)p == null)
            {
                return false;
            }

            return (Id == p.Id);
        }
        public override int GetHashCode()
        {
            return Id;
        }
        public void CheckStatus()
        {
            if (ValidCredentials && IsConfigured && ValidProxy)
                Status = STATUS.READY;
            else if (!ValidCredentials)
                Status = STATUS.WRONG_EMAIL_OR_PASS;
            else if (!IsConfigured)
                Status = STATUS.NOT_CONFIGURED;
            else if (!ValidProxy)
                Status = STATUS.INVALID_PROXY;

        }

    }
}
