
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
    internal class IndividualUser : ScrapeSession
    {
        protected string username;

        internal IndividualUser(string query, http request, IAccount account, IConfiguration config)
            : base(query, request, account, config)
        {
            username = query.Split('/')[1];
            this.resource = PinterestObjectResources.IndividualUser;
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
                string msg = "Error iuSCRPNS36." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log(account.Username, this.GetType().ToString(), msg);
            }
        }


        protected override void SetUrlAndRef()
        {
            try
            {
                Url = baseUrl + "/" + username + "/";
                Referer = baseUrl;

            }
            catch (Exception ex)
            {
                string msg = "Error iuSCRPNS39." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
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
                foreach (string pattern in ParsePatterns.IndividualUsers)
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

                    foreach (Match m in matches)
                    {
                        string id = m.Groups["id"].ToString();                     
                        bool FollowedByMe = bool.Parse(m.Groups["followed_by_me"].Value);

                        Pinner p = new Pinner(id, username, baseUsername, "", resource);
                        p.FollowedByMe = FollowedByMe;

                        this.EndReached = true;
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
                string msg = "Error iuSCRPNS90." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log(account.Username, this.GetType().ToString(), msg);
            }
        }


        protected override string GetData()
        {
            throw new NotImplementedException();
        }
    }
}
