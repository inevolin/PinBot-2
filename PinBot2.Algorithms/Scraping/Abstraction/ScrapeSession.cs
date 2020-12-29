using PinBot2.Common;
using PinBot2.Model;
using PinBot2.Model.Configurations;
using PinBot2.Model.PinterestObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PinBot2.Algorithms.Scraping
{
    internal abstract class ScrapeSession
    {
        protected string Url { get; set; }
        protected string Bookmark { get; set; }
        protected string Referer { get; set; }
        protected http request;
        protected string rs;
        protected readonly IAccount account;
        protected static readonly string baseUrl = "https://www.pinterest.com";
        protected readonly IConfiguration config;

        protected string baseUsername;
        protected string query;
        public PinterestObjectResources resource { get; set; }

        internal IList<PinterestObject> PinterestObjects { get; set; } //pass Objects to Algorithm
        protected IList<PinterestObject> UsedObjects { get; set; } //prevent already seen objects
        public string Query { get { return query; } }

        internal bool FirstRequest { get; set; }
        internal int CountRestarted { get; set; }
        internal bool EndReached { get; set; }

        internal abstract void Scrape();

        protected abstract string GetData();
        protected abstract void SetUrlAndRef();
        protected abstract void Parse();

        internal ScrapeSession(string query, http request, IAccount account, IConfiguration config)
        {
            EndReached = false;
            this.query = query;
            this.account = account;
            this.request = request;
            this.config = config;
            FirstRequest = true;
            Bookmark = "";
            PinterestObjects = new List<PinterestObject>();
            UsedObjects = new List<PinterestObject>();
        }
        protected virtual void MakeRequest()
        {
            Console.WriteLine(this.ToString());
            rs = MakeRequest(Url, Referer, http.ACCEPT_JSON);
            Logging.Log(account.Username, this.GetType().ToString(), "scrape rs qry( " + query + " )"+Environment.NewLine+Environment.NewLine + rs);
        }
        protected virtual string MakeRequest(string _url, string _ref, string ctype)
        {
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
                                    ctype )
                        );
                task.Wait();
                _rs = task.Result;
                //if (ctype == http.ACCEPT_JSON)
                    //_rs = _rs.Replace("\\", "");
            }
            catch (Exception ex)
            {
                string msg = "Error SCRS94." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log(account.Username, this.GetType().ToString(), msg);
            }
            return _rs;
        }
        protected virtual void SetBookmark()
        {
            if (rs == null) return;
            try
            {
                Match match = Regex.Match(rs, "\"bookmarks\":\\[\"(?=[^\\-])(.+?)\"\\]", RegexOptions.Multiline);
                Bookmark = match.Groups[1].ToString();
                if (Bookmark.Length == 0)
                    EndReached = true;
            }
            catch (Exception ex)
            {
                string msg = "Error SCRS110." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log(account.Username, this.GetType().ToString(), msg);
                EndReached = true;
            }
        }
        protected static Int64 GetTime()
        {
            Int64 retval = 0;
            var st = new DateTime(1970, 1, 1);
            TimeSpan t = (DateTime.Now.ToUniversalTime() - st);
            retval = (Int64)(t.TotalMilliseconds + 0.5);
            return retval;
        }
        protected string SetBoardId()
        {
            if (rs == null) return null;
            return Regex.Match(rs, "\"board_id\":\"(\\d+)\"").Groups[1].Value;
        }
        protected string SetUserId()
        {
            if (rs == null) return null;
            return Regex.Match(rs, "\"user_id\":\"(\\d+)\",").Groups[1].Value;
        }
    }
}
