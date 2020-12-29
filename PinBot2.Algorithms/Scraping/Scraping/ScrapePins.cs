using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace PinBot2.Algorithms.Scraping
{
    internal abstract class ScrapePins : ScrapeSession
    {
        private DuplicateChecker dupChecker;
        protected string boardName;
        protected string boardId;

        internal ScrapePins(string query, http request, IAccount account, IConfiguration config)
            : base(query, request, account, config)
        {
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
                if (Bookmark != "") FirstRequest = false;
            }
            catch (Exception ex)
            {
                string msg = "Error SPNS37." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log(account.Username, this.GetType().ToString(), msg);
            }
        }

        protected override void Parse()
        {
            try
            {
                PinterestObjects.Clear(); //clear previous ones
                if (rs == null) return;                

                List<Pin> pins = jsonPins(rs);
                if (pins.Count == 0)
                {
                    EndReached = true;
                    return;
                }
                EndReached = false;
                pins.Shuffle();
            }
            catch (Exception ex)
            {
                string msg = "Error SPNS102." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
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
                foreach (string pattern in ParsePatterns.Pins)
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

                    foreach (Match m in matches)
                    {
                        string pinId = m.Groups["id"].ToString();
                        string image = m.Groups["image"].ToString();

                        if (dupChecker.IsDuplicate(account.Email, image))
                            continue;

                        Pin p = new Pin(pinId, "", query, "", "", image, resource);
                        p.Description = m.Groups["description"].ToString();
                        p.LikedByMe = bool.Parse(m.Groups["liked_by_me"].ToString());

                        if (config.GetType().Equals(typeof(PinConfiguration)))
                        {
                            var url = QueueHelper.GetRandomSourceUrl((PinConfiguration)config);
                            if (url == null || url == "")
                                p.Link = m.Groups["link"].ToString();
                            else
                                p.Link = url;
                        }
                        else if (config.GetType().Equals(typeof(RepinConfiguration)))
                        {
                            var descUrl = QueueHelper.GetRandomDescUrl((RepinConfiguration)config);
                            if (descUrl != null && descUrl != "")
                                p.Description += " " + descUrl;
                        }



                        if ((UsedObjects.Any(x => x.Id == p.Id))) //prevent dubs
                            continue;

                        UsedObjects.Add(p);

                        if ((PinterestObjects.Any(x => x.Id == p.Id))) //prevent dubs
                            continue;
                        else
                            PinterestObjects.Add(p);
                    }
                    break;
                }

            }
            catch (Exception ex)
            {
                string msg = "Error SPNS102." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log(account.Username, this.GetType().ToString(), msg);
            }
        }*/

        private List<Pin> jsonPins(string rs)
        {
            var lst = new List<Pin>();
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
            foreach (var a5 in a3)
            {
                string id = (string)a5["id"];
                dynamic imgs = a5["images"];
                dynamic orig = imgs["orig"];
                string img = (string)orig["url"];
                string desc = (string)a5["description"];
                string link = (string)a5["link"];
                bool liked = (bool)a5["liked_by_me"];

                Pin p = new Pin(id, "", query, "", "", img, resource);
                p.Description = desc;
                p.LikedByMe = liked;

                if (config.GetType().Equals(typeof(PinConfiguration)))
                {
                    var url = QueueHelper.GetRandomSourceUrl((PinConfiguration)config);
                    if (url == null || url == "")
                        p.Link = link;
                    else
                        p.Link = url;
                }
                else if (config.GetType().Equals(typeof(RepinConfiguration)))
                {
                    var descUrl = QueueHelper.GetRandomDescUrl((RepinConfiguration)config);
                    if (descUrl != null && descUrl != "")
                        p.Description += " " + descUrl;
                }
                if ((UsedObjects.Any(x => x.Id == p.Id))) //prevent dubs
                    continue;
                UsedObjects.Add(p);
                if ((PinterestObjects.Any(x => x.Id == p.Id))) //prevent dubs
                    continue;
                else
                    PinterestObjects.Add(p);
                lst.Add(p);
            }
            return lst;
        }

    }
}
