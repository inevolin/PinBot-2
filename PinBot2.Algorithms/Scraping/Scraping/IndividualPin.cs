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
    internal class IndividualPin : ScrapeSession
    {
        protected string pinId;

        internal IndividualPin(string query, http request, IAccount account, IConfiguration config)
            : base(query, request, account, config)
        {
            pinId = query.Split('/')[2];
        }

        internal override void Scrape()
        {
            try
            {
                SetUrlAndRef();
                MakeRequest();
                Parse();
                if (Bookmark != "") FirstRequest = false;
            }
            catch (Exception ex)
            {
                string msg = "Error ipSPNS37." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log(account.Username, this.GetType().ToString(), msg);
            }
        }

        protected override void SetUrlAndRef()
        {
            try
            {
                Url = baseUrl + "/pin/" + pinId + "/";
                Referer = baseUrl;

            }
            catch (Exception ex)
            {
                string msg = "Error ipSPNS39." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log(account.Username, this.GetType().ToString(), msg);
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
                foreach (string pattern in ParsePatterns.IndividualPins)
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
                        string desc = m.Groups["desc"].ToString();
                        string src = m.Groups["source"].ToString();
                        if (src == "null") { src = "\"\""; }
                        Pin p = new Pin(pinId, "", query, "", "", "", resource);
                        p.Description = desc;

                        if (config.GetType().Equals(typeof(RepinConfiguration)))
                        {
                            var descUrl = QueueHelper.GetRandomDescUrl((RepinConfiguration)config);
                            if (descUrl != null && descUrl != "")
                                p.Description += " " + descUrl;
                        }


                        this.EndReached = true;
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
                string msg = "Error ipSPNS102." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log(account.Username, this.GetType().ToString(), msg);
            }
        }

        protected override string GetData()
        {
            throw new NotImplementedException();
        }

    }
}
