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
    internal class ScrapePinners_BoardFollowersResource : ScrapePinners
    {

        internal ScrapePinners_BoardFollowersResource(string query, http request, IAccount account, IConfiguration config)
            : base(query, request, account, config)
        {
            this.resource = PinterestObjectResources.BoardFollowersResource;
            baseUsername = query.Split('/')[1];
            boardName = query.Split('/')[2];
        }

        protected override void SetUrlAndRef()
        {
            try
            {
                if (FirstRequest)
                {
                    Url = baseUrl + "/" + baseUsername + "/" + boardName + "/";
                    Referer = baseUrl;
                    MakeRequest();
                }

                if (boardId == null || boardId.Length == 0)
                    boardId = SetBoardId();
                if (boardId == null || boardId.Length == 0)
                {
                    FirstRequest = true;
                    return;
                }

                Url = baseUrl + GetData();
                Referer = baseUrl + "/" + baseUsername + "/" + boardName + "/";
            }
            catch (Exception ex)
            {
                string msg = "Error SPBFR50." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log(account.Username, this.GetType().ToString(), msg);
            }
        }
        protected override string GetData()
        {
            string str = "";

            if (FirstRequest)
                str =
                     "/resource/BoardFollowersResource/get/"
                     + "?source_url="
                     + HttpUtility.UrlEncode("/" + baseUsername + "/" + boardName + "/")
                     + "&data="
                     + HttpUtility.UrlEncode("{\"options\":{\"board_id\":\"" + boardId + "\",\"field_set_key\":\"grid_item\",\"page_size\":9},\"context\":{}}")
                     + "&module_path="
                     + HttpUtility.UrlEncode("App()>BoardPage(inviter_user_id=null, show_follow_memo=null, tab=pins, resource=BoardResource(username=" + baseUsername + ", slug=" + boardName + "))")
                     + "&_=" + GetTime();
            else
                str = "/resource/BoardFollowersResource/get/"
                    + "?source_url="
                    + HttpUtility.UrlEncode("/" + baseUsername + "/" + boardName + "/")
                    + "&data="
                    + HttpUtility.UrlEncode("{\"options\":{\"board_id\":\"" + boardId + "\",\"field_set_key\":\"grid_item\",\"page_size\":9,\"bookmarks\":[\"" + Bookmark + "\"]},\"context\":{}}")
                    + "&module_path="
                    + HttpUtility.UrlEncode("Modal()>BoardFollowers(resource=BoardFollowersResource(board_id=" + boardId + ", page_size=9))>PagedGrid(resource=BoardFollowersResource(board_id=" + boardId + ", page_size=9))>Button(class_name=moreItems, log_element_type=179, text=Load more followers)")
                    + "&_=" + GetTime();

            str = Regex.Replace(str, "%\\d(\\w)", m => m.ToString().ToUpper());
            return str;
        }



    }
}
