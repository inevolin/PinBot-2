
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
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace PinBot2.Algorithms.Scraping
{
    internal abstract class ScrapePinners : ScrapeSession
    {
        protected string boardName;
        protected string boardId;

        internal ScrapePinners(string query, http request, IAccount account, IConfiguration config)
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
                string msg = "Error SCRPNS36." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
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
                foreach (string pattern in ParsePatterns.Pinners)
                {
                    List<Match> matches = Regex.Matches(rs, pattern).Cast<Match>().ToList();
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
                        string id = m.Groups["id"].ToString();
                        string username = m.Groups["username"].ToString();
                        int pincount = m.Groups["pins"].Success ? int.Parse(m.Groups["pins"].Value) : 0;
                        int followerscount = m.Groups["followers"].Success ? int.Parse(m.Groups["followers"].Value) : 0;
                        bool FollowedByMe = m.Groups["followed_by_me"].Success ? bool.Parse(m.Groups["followed_by_me"].Value) : true;

                        Pinner p = new Pinner(id, username, baseUsername, "", resource);
                        p.PinsCount = pincount;
                        p.FollowersCount = followerscount;
                        p.FollowedByMe = FollowedByMe;
                        p.SearchQuery = boardName;

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
                string msg = "Error SCRPNS90." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log(account.Username, this.GetType().ToString(), msg);
            }
        }*/

        protected override void Parse()
        {
            try
            {
                PinterestObjects.Clear(); //clear previous ones
                if (rs == null) return;

                List<Pinner> pinners = jsonPinners(rs);
                if (pinners == null || pinners.Count == 0)
                {
                    EndReached = true;
                    return;
                }
                EndReached = false;
                pinners.Shuffle();
                 
            }
            catch (Exception ex)
            {
                string msg = "Error SCRPNS90." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log(account.Username, this.GetType().ToString(), msg);
            }
        }

        private List<Pinner> jsonPinners(string rs) {
            var lst = new List<Pinner>();
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
                string id = (string)a4["id"];
                string username = (string)a4["username"];
                int pincount = (int)a4["pin_count"];
                int followerscount = (int)a4["follower_count"];
                bool FollowedByMe = (bool)a4["explicitly_followed_by_me"];

                Pinner p = new Pinner(id, username, "", "", resource);
                p.PinsCount = pincount;
                p.FollowersCount = followerscount;
                p.FollowedByMe = FollowedByMe;

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
