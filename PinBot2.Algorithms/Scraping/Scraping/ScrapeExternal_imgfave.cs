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
    internal class ScrapeExternal_imgfave : ScrapeSession
    {
        private DuplicateChecker dupChecker;
        int page = 1;
        internal ScrapeExternal_imgfave(string query, http request, IAccount account, IConfiguration config)
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
                string msg = "Error SCREif36." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log(account.Username, this.GetType().ToString(), msg);
            }
        }

        protected override void SetUrlAndRef()
        {
            if (FirstRequest)
                page = 1;

            Url = "http://imgfave.com/search/" + HttpUtility.UrlEncode(query);
            if (page > 1) Url += "/page:" + (page);

            Referer = "http://imgfave.com/search/" + HttpUtility.UrlEncode(query);
            if (page > 1) Referer += "/page:" + (page - 1);

            ++page;
        }
        protected override string GetData()
        {
            return "";
        }
        protected override void SetBookmark()
        {
            try
            {
                if (Regex.IsMatch(rs, "<a class=\"btn btn-custom\" href=\"/search/(.+?)/page:2\">Next Page"))
                    FirstRequest = false;
                else if (rs.Contains("No results found") || rs.Contains("</form>of 1</div><span class=\"next_Prev\">"))
                    EndReached = true;
            }
            catch (Exception ex)
            {
                string msg = "Error SEif71." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
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
                string msg = "Error SEif84." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
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
                foreach (string pattern in ParsePatterns.ImgFave)
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
                        //else
                        //    dupChecker.Add(account.Email, img);

                        string title = m.Groups["title"].ToString();
                        List<string> tags = new List<string>();
                        tags.Add(title);
                        string temp_tags = m.Groups["tags"].ToString();
                        foreach (Match mm in Regex.Matches(temp_tags, "\">([a-zA-Z0-9]+)</a>"))
                        {
                            tags.Add(mm.Groups[1].ToString());

                            //Console.WriteLine(mm);
                        }

                        ExternalPin p = new ExternalPin(GenerateDescription(tags), img, tags, PinterestObjectResources.External);
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
                string msg = "Error SEif170." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log(account.Username, this.GetType().ToString(), msg);
            }
        }
    }
}
