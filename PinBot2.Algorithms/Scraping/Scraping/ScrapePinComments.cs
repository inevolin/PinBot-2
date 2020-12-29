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
    internal class ScrapePinComments : ScrapeSession
    {

        Pin pin;
        internal ScrapePinComments(Pin pin, http request, IAccount account, IConfiguration config)
            : base("", request, account, config)
        {
            this.pin = pin;
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
                string msg = "Error SPC37." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log(account.Username, this.GetType().ToString(), msg);
            }
        }

        protected override void SetUrlAndRef()
        {
            Url =
                baseUrl
                + "/resource/PinCommentListResource/get/?source_url="
                + HttpUtility.UrlEncode("/pin/" + pin.Id + "/")
                + "&data="
                + HttpUtility.UrlEncode("{\"options\":{\"field_set_key\":\"pin_detailed\",\"pin_id\":\"" + pin.Id + "\",\"page_size\":5},\"context\":{}}")
                + "&module_path="
                + HttpUtility.UrlEncode("App()>Closeup(show_reg=null, sender=null, resource=PinResource(link_selection=true, fetch_visual_search_objects=true, id=" + pin.Id + "))")
                + "&_="
                ;
            Referer = baseUrl + "/pin/" + pin.Id + "/";
        }
        protected override string GetData()
        {
            return "";
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
                foreach (string pattern in ParsePatterns.Comments)
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
                        string id = m.Groups["id"].ToString();
                        string text = m.Groups["text"].ToString();
                        string username = m.Groups["username"].ToString();

                        Comment p = new Comment(id, username, text, PinterestObjectResources.PinCommentListResource);

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
                string msg = "Error SPC101." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log(account.Username, this.GetType().ToString(), msg);
            }
        }
    }
}
