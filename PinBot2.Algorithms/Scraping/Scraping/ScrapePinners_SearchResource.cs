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
    internal class ScrapePinners_SearchResource : ScrapePinners
    {


        internal ScrapePinners_SearchResource(string query, http request, IAccount account, IConfiguration config) 
              : base (query,request,account, config)
        {
            this.resource = PinterestObjectResources.SearchResource;
        }

        protected override void SetUrlAndRef()
        {           
            Url = FirstRequest ? (baseUrl + "/search/people/?q=" + HttpUtility.UrlEncode(query)) : (baseUrl + GetData());
            Referer = baseUrl;
        }
        protected override string GetData()
        {
            string str =
                        "/resource/SearchResource/get/?source_url="
                        + HttpUtility.UrlEncode("/search/people/?q=" + HttpUtility.UrlEncode(query))
                        + "&data="
                        + HttpUtility.UrlEncode("{\"options\":{\"layout\":null,\"places\":false,\"constraint_string\":null,\"show_scope_selector\":true,\"query\":\"" + query + "\",\"scope\":\"people\",\"bookmarks\":[\"" + Bookmark + "\"]},\"context\":{}}")
                        + "&_=" + GetTime();

            str = Regex.Replace(str, "%\\d(\\w)", m => m.ToString().ToUpper());
            return str;
        }


    }
}
