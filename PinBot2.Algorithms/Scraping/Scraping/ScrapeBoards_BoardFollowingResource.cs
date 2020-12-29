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
    internal class ScrapeBoards_BoardFollowingResource : ScrapeBoards
    {

        internal ScrapeBoards_BoardFollowingResource(string query, http request, IAccount account, IConfiguration config)
            : base(query, request, account, config)
        {
            baseUsername = query.Split('/')[1];
            this.resource = PinterestObjectResources.BoardFollowingResource;
        }

        protected override void SetUrlAndRef()
        {
            try
            {
                if (FirstRequest)
                {
                    Url = baseUrl + "/" + baseUsername + "/following/";
                    Referer = baseUrl;
                    MakeRequest();
                }

                Url = baseUrl + GetData();
                Referer = baseUrl + "/" + baseUsername + "/following/";
            }
            catch (Exception ex)
            {
                string msg = "Error SBBF40." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log(account.Username, this.GetType().ToString(), msg);
            }
        }
        protected override string GetData()
        {
            string str = "";

            if (FirstRequest)
                str =
                     "/resource/BoardFollowingResource/get/"
                     + "?source_url="
                     + HttpUtility.UrlEncode("/" + baseUsername + "/following/")
                     + "&data="
                     + HttpUtility.UrlEncode("{\"options\":{\"username\":\"" + baseUsername + "\"},"
                     + "\"context\":{}}")
                     + "&module_path="
                     + HttpUtility.UrlEncode("App()>UserProfilePage(resource=UserResource(username=" + baseUsername + "))"
                     + ">UserProfileContent(resource=UserResource(username=" + baseUsername + "))"
                     + ">FollowingSwitcher()>Button(class_name=rightRounded navScopeBtn, text=Boards, element_type=a, rounded=false)")
                     + "&_=" + GetTime();
            else
                str = "/resource/BoardFollowingResource/get/"
                    + "?source_url="
                    + HttpUtility.UrlEncode("/" + baseUsername + "/following/")
                    + "&data="
                    + HttpUtility.UrlEncode("{\"options\":{\"username\":\"" + baseUsername + "\","
                    + "\"bookmarks\":[\"" + Bookmark + "\"]},"
                    + "\"context\":{}}")
                    + "&module_path="
                    + HttpUtility.UrlEncode("App(module=[object Object])")
                    + "&_=" + GetTime();

            str = Regex.Replace(str, "%\\d(\\w)", m => m.ToString().ToUpper());
            return str;
        }

    }
}
