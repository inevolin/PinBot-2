using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
    internal abstract class ScrapeBoards : ScrapeSession
    {

        internal ScrapeBoards(string query, http request, IAccount account, IConfiguration config)
            : base(query, request, account, config)
        { }
        internal override void Scrape()
        {
            try
            {
                SetUrlAndRef();
                MakeRequest();
                SetBookmark();
                Parse();
                if (Bookmark != "") FirstRequest = false;
            }
            catch (Exception ex)
            {
                string msg = "Error SCRB33." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log(account.Username, this.GetType().ToString(), msg);
            }
        }

        /*protected override void Parse()
        {
            try
            {
                PinterestObjects.Clear(); //clear previous ones

                if (rs == null) return;
                rs = Regex.Unescape(rs);
                rs = HttpUtility.HtmlDecode(rs);
                rs = rs.Replace("\\", "");
                foreach (string pattern in ParsePatterns.Boards)
                {
                    List<Match> matches = Regex.Matches(rs, pattern, RegexOptions.Multiline).Cast<Match>().ToList();
                    if (matches.Count == 0)
                    {
                        EndReached = true;
                        continue;
                    }
                    EndReached = false;
                    matches.Shuffle();
                    Random r = new Random();
                    //int keepRange = r.Next(0, matches.Count / 2);
                    //matches.RemoveRange(keepRange + 1, matches.Count - keepRange - 1);

                    foreach (Match m in matches)
                    {
                        string boardId = m.Groups["boardId"].ToString();
                        string userId = m.Groups["userId"].ToString();
                        string username = m.Groups["username"].ToString();
                        string url = m.Groups["url"].ToString();
                        int pincount = int.Parse(m.Groups["pins"].Value);
                        bool FollowedByMe = bool.Parse(m.Groups["followed_by_me"].Value);

                        Board p = new Board(boardId, userId, username, "", resource);
                        p.PinsCount = pincount;
                        p.FollowedByMe = FollowedByMe;
                        p.Url = url;

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
                string msg = "Error SCRB85." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log(account.Username, this.GetType().ToString(), msg);
            }

        }*/

        protected override void Parse()
        {
            try
            {
                PinterestObjects.Clear(); //clear previous ones

                if (rs == null) return;
                List<Board> boards = jsonBoards(rs);
                if (boards.Count == 0)
                {
                    EndReached = true;
                    return;
                }
                EndReached = false;
                boards.Shuffle();

            }
            catch (Exception ex)
            {
                string msg = "Error SCRB85." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log(account.Username, this.GetType().ToString(), msg);
            }

        }

        private List<Board> jsonBoards(string rs)
        {
            var lst = new List<Board>();
            dynamic d = JsonConvert.DeserializeObject(rs);
            JArray a1 = d["resource_data_cache"];
            JToken a2 = a1.FirstOrDefault(o => o["data"] != null && o["data"].GetType().Equals(typeof(JArray)));
            JArray a3 = null;
            if (a2 != null)
            {
                a3 = (JArray)a2["data"];
            }
            else
            {
                a2 = a1.FirstOrDefault(o => o["data"]["results"] != null && o["data"]["results"].GetType().Equals(typeof(JArray)));
                if (a2 != null)
                {
                    a3 = (JArray)a2["data"]["results"];
                }
            }

            if (a3 == null) return null;
            foreach (var a4 in a3)
            {
                string boardId = (string)a4["id"];
                dynamic user = a4["owner"];
                string userId = (string)user["id"];
                string username = (string)user["username"];
                string url = (string)a4["url"];
                int pincount = (int)a4["pin_count"];
                bool FollowedByMe = (bool)a4["followed_by_me"];

                Board p = new Board(boardId, userId, username, "", resource);
                p.PinsCount = pincount;
                p.FollowedByMe = FollowedByMe;
                p.Url = url;

                if ((UsedObjects.Any(x => x.Id == p.Id))) //prevent dubs
                    continue;

                UsedObjects.Add(p);

                if ((PinterestObjects.Any(x => x.Id == p.Id))) //prevent dubs
                    continue;

                PinterestObjects.Add(p);

            }
            return lst;
        }

    }
}
