using PinBot2.Algorithms.Helpers;
using PinBot2.Algorithms.Scraping.Queue;
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
using System.Web;

namespace PinBot2.Algorithms.Scraping
{
    internal class ScrapeExternal_tumblr : ScrapeSession
    {
        private DuplicateChecker dupChecker;
        int page = 1;
        internal ScrapeExternal_tumblr(string query, http request, IAccount account, IConfiguration config)
            : base(query, request, account, config)
        {
            this.query = query;
            dupChecker = DuplicateChecker.init();
        }

        internal override void Scrape()
        {
            try
            {
                SetUrlAndRef();
                MakeRequest();
                SetBookmark();
                Parse();
            }
            catch (Exception ex)
            {
                string msg = "Error SCREtr36." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log(account.Username, this.GetType().ToString(), msg);
            }
        }

        private int num_posts_shown = 0;
        private int before = 0;
        protected override void SetUrlAndRef()
        {
            if (FirstRequest)
            {
                page = 1;
                Url = "https://www.tumblr.com/search/" + HttpUtility.UrlEncode(query);
                Referer = "https://www.tumblr.com/search/" + HttpUtility.UrlEncode(query);
            }
            else
            {
                Url = "https://www.tumblr.com/search/" + HttpUtility.UrlEncode(query) + "/post_page/" + page;
                Referer = "https://www.tumblr.com/search/" + HttpUtility.UrlEncode(query) + "/post_page/" + (page - 1);
                num_posts_shown += 30;
                before += 31;
            }
            ++page;
        }
        protected override void MakeRequest()
        {
            if (FirstRequest)
                rs = MakeFirstRequest(Url, Referer, http.ACCEPT_HTML);
            else
                rs = MakeRequest(Url, Referer, http.ACCEPT_JSON);
            Logging.Log(account.Username, this.GetType().ToString(), "scrape rs qry( " + query + " )" + Environment.NewLine + Environment.NewLine + rs);
        }
        protected string MakeFirstRequest(string _url, string _ref, string ctype)
        {
            string _rs = "";
            List<string> headers = null;
            if (ctype == http.ACCEPT_HTML)
                headers = new List<string>
                    {   "Accept-Encoding: gzip, deflate",
                        "Accept-Language: en-US,en;q=0.8"};
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
            }
            catch (Exception ex)
            {
                string msg = "Error SCRS94." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log(account.Username, this.GetType().ToString(), msg);
            }
            return _rs;
        }
        protected override string MakeRequest(string _url, string _ref, string ctype)
        {
            string _rs = "";
            List<string> headers = null;
            if (ctype == http.ACCEPT_JSON)
                headers = new List<string>
                    {   "Accept-Encoding: gzip, deflate",
                        "Accept-Language: en-US,en;q=0.8",
                        "X-tumblr-form-key: " + Bookmark,
                        "X-Requested-With: XMLHttpRequest",
                        "Origin: https://www.tumblr.com"
                    };

            string data = "q=" + HttpUtility.UrlEncode(query) + "&sort=top&post_view=masonry&blogs_before=8&num_blogs_shown=8&num_posts_shown=" + num_posts_shown + "&before=" + before + "&blog_page=" + page + "&post_page=" + page + "&filter_nsfw=true&filter_post_type=&next_ad_offset=0&ad_placement_id=0&more_posts=true";

            StringBuilder strBuilder = new StringBuilder();
            strBuilder.AppendLine(data);
            byte[] hdrbytes = System.Text.Encoding.UTF8.GetBytes(strBuilder.ToString());

            List<byte[]> http_data = new List<byte[]>();
            http_data.Add(hdrbytes);

            try
            {
                Task<string> task =
                    Task.Run<string>(() =>
                                request.POST(
                                    _url,
                                    _ref,
                                    "application/x-www-form-urlencoded; charset=UTF-8",
                                    http_data,
                                    headers,
                                    account.CookieContainer,
                                    account.WebProxy,
                                    60000,
                                    ctype)
                        );
                task.Wait();
                _rs = task.Result;
            }
            catch (Exception ex)
            {
                string msg = "Error SCRS94." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log(account.Username, this.GetType().ToString(), msg);
            }
            return _rs;
        }

        protected override string GetData()
        {
            return "";
        }
        protected override void SetBookmark()
        {
            try
            {
                if (Regex.IsMatch(rs, "<meta name=\"tumblr-form-key\" content=\"(.+?)\" id=\"tumblr_form_key\">"))
                {
                    Bookmark = Regex.Match(rs, "<meta name=\"tumblr-form-key\" content=\"(.+?)\" id=\"tumblr_form_key\">").Groups[1].Value.ToString();
                    FirstRequest = false;
                }
                else if (FirstRequest)
                    EndReached = true;
            }
            catch (Exception ex)
            {
                string msg = "Error SCREtr71." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log(account.Username, this.GetType().ToString(), msg);
            }
        }
        private string GenerateDescription(IList<string> lines)
        {
            try
            {
                string res = "";
                Random r = new Random();
                int g = r.Next(0, 3);
                if (g == 0)
                {
                    res += "#";
                    res += lines[r.Next(0, lines.Count)];
                }
                else if (g == 1)
                {
                    for (int i = 0; i < r.Next(1, lines.Count); i++)
                    {
                        res += lines[r.Next(0, lines.Count)] + " ";
                    }
                }
                else
                {
                    res += lines[r.Next(0, lines.Count)];
                }

                if (res == "")
                {
                    res = query;
                }

                return res.Trim();

            }
            catch (Exception ex)
            {
                string msg = "Error SCREtr84." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log(account.Username, this.GetType().ToString(), msg);
                return null;
            }
        }
        protected override void Parse()
        {
            try
            {
                PinterestObjects.Clear(); //clear previous ones

                if (rs == null) return;
                rs = Regex.Unescape(rs);
                rs = HttpUtility.HtmlDecode(rs);
                rs = rs.Replace("\\", "");

                foreach (string pattern in ParsePatterns.Tumblr)
                {
                    List<Match> matches = Regex.Matches(rs, pattern, RegexOptions.Multiline).Cast<Match>().ToList();
                    if (matches.Count == 0)
                    {
                        EndReached = true;
                        continue;
                    }
                    EndReached = false;
                    foreach (Match m in matches)
                    {
                        string img = m.Groups["img"].ToString();
                        if (dupChecker.IsDuplicate(account.Email, img))
                            continue;

                        string desc = m.Groups["desc"].ToString();
                        desc = Regex.Replace(desc, "<.*?>", String.Empty);

                        ExternalPin p = new ExternalPin(desc, img, null, PinterestObjectResources.External);
                        var url = QueueHelper.GetRandomSourceUrl((PinConfiguration)config);
                        if (url != null && url != "")
                            p.Link = url;

                        if ((UsedObjects.Any(x => x.Id == p.Id))) //prevent dubs
                            continue;

                        UsedObjects.Add(p);

                        if ((PinterestObjects.Any(x => x.Id == p.Id))) //prevent dubs
                            continue;

                        PinterestObjects.Add(p);

                    }
                    break;
                }
            }
            catch (Exception ex)
            {
                string msg = "Error SCREtr170." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log(account.Username, this.GetType().ToString(), msg);
            }
        }
    }
}
