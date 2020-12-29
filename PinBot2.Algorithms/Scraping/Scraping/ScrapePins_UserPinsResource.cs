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
    internal class ScrapePins_UserPinsResource : ScrapePins
    {

        internal ScrapePins_UserPinsResource(string query, http request, IAccount account, IConfiguration config)
            : base(query, request, account, config)
        {
            baseUsername = query.Split('/')[1];
        }

        protected override void SetUrlAndRef()
        {
            try
            {
                if (FirstRequest)
                {
                    Url = baseUrl + "/" + baseUsername + "/pins/";
                    Referer = baseUrl;
                    MakeRequest();
                }

                Url = baseUrl + GetData();
                Referer = baseUrl + "/" + baseUsername + "/pins/";
            }
            catch (Exception ex)
            {
                string msg = "Error SPUPR39." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log(account.Username, this.GetType().ToString(), msg);
            }
        }
        protected override string GetData()
        {
            string str = "";

            if (FirstRequest)
                str =
                        "/resource/UserResource/get/?source_url="
                        + HttpUtility.UrlEncode("/" + baseUsername + "/pins/")
                        + "&data="
                        + HttpUtility.UrlEncode("{\"options\":{\"username\":\"" + baseUsername + "\"},\"context\":{},\"module\":{\"name\":\"UserProfileContent\",\"options\":{\"tab\":\"pins\"}},\"render_type\":1,\"error_strategy\":0}")
                        + "&_=" + GetTime();
            else
                str =
                        "/resource/UserPinsResource/get/?source_url="
                        + HttpUtility.UrlEncode("/" + baseUsername + "/pins/")
                        + "&data="
                        + HttpUtility.UrlEncode("{\"options\":{\"username\":\"" + baseUsername + "\",\"bookmarks\":[\"" + Bookmark + "\"]},\"context\":{}}")
                        + "&_=" + GetTime();
            str = Regex.Replace(str, "%\\d(\\w)", m => m.ToString().ToUpper());
            return str;
        }

    }
}
