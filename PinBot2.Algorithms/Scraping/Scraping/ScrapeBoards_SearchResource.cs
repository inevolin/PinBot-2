
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
    internal class ScrapeBoards_SearchResource : ScrapeBoards
    {

        internal ScrapeBoards_SearchResource(string query, http request, IAccount account, IConfiguration config)
            : base(query, request, account, config)
        {
            this.resource = PinterestObjectResources.SearchResource;
        }

        protected override void SetUrlAndRef()
        {
            try
            {
                Url = FirstRequest ? (baseUrl + "/search/boards/?q=" + HttpUtility.UrlEncode(query)) : (baseUrl + GetData());
                Referer = baseUrl;
            }
            catch (Exception ex)
            {
                string msg = "Error SBSR33." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log(account.Username, this.GetType().ToString(), msg);
            }
        }
        protected override string GetData()
        {
            string str =
                        "/resource/SearchResource/get/?source_url="
                        + HttpUtility.UrlEncode("/search/boards/?q=" + HttpUtility.UrlEncode(query))
                        + "&data="
                        + HttpUtility.UrlEncode("{\"options\":{\"layout\":null,\"places\":false,\"constraint_string\":null,\"show_scope_selector\":true,\"query\":\"" + query + "\",\"scope\":\"boards\",\"bookmarks\":[\"" + Bookmark + "\"]},\"context\":{}}")
                        + "&_=" + GetTime();

            str = Regex.Replace(str, "%\\d(\\w)", m => m.ToString().ToUpper());
            return str;
        }

    }
}
