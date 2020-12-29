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
    internal class ScrapePinners_UserFollowersResource : ScrapePinners
    {


        internal ScrapePinners_UserFollowersResource(string query, http request, IAccount account, IConfiguration config)
            : base(query, request, account, config)
        {
            this.resource = PinterestObjectResources.UserFollowersResource;
            baseUsername = query.Split('/')[1];
        }

        protected override void SetUrlAndRef()
        {
            try
            {
                if (FirstRequest)
                {
                    Url = baseUrl + "/" + baseUsername + "/followers/";
                    Referer = baseUrl;
                    MakeRequest();
                }

                Url = baseUrl + GetData();
                Referer = baseUrl + "/" + baseUsername + "/followers/";
            }
            catch (Exception ex)
            {
                string msg = "Error SPUFR41." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log(account.Username, this.GetType().ToString(), msg);
            }
        }
        protected override string GetData()
        {
            string str = "";

            if (FirstRequest)
                str = "/resource/UserResource/get/"
                    + "?source_url="
                    + HttpUtility.UrlEncode("/" + baseUsername + "/followers/")
                    + "&data="
                    + HttpUtility.UrlEncode("{\"options\":{\"username\":\"" + baseUsername + "\"},"
                    + "\"context\":{},"
                    + "\"module\":{\"name\":\"UserProfileContent\","
                    + "\"options\":{\"tab\":\"followers\"}},"
                    + "\"render_type\":1,"
                    + "\"error_strategy\":0}")
                    + "&_=" + GetTime();
            else
                str =
                     "/resource/UserFollowersResource/get/"
                     + "?source_url="
                     + HttpUtility.UrlEncode("/" + baseUsername + "/followers/")
                     + "&data="
                     + HttpUtility.UrlEncode("{\"options\":{\"username\":\"" + baseUsername + "\","
                     + "\"bookmarks\":[\"" + Bookmark + "\"]},"
                     + "\"context\":{}}")
                     + "&_=" + GetTime();

            str = Regex.Replace(str, "%\\d(\\w)", m => m.ToString().ToUpper());
            return str;
        }

    }
}
