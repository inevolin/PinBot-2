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
    internal class ScrapePins_BoardFeedResource : ScrapePins
    {

        internal ScrapePins_BoardFeedResource(string query, http request, IAccount account, IConfiguration config) 
              : base (query,request,account, config)
        {
            baseUsername = query.Split('/')[1];
            boardName = query.Split('/')[2];
            this.resource = PinterestObjectResources.BoardFeedResource;
        }

        protected override void SetUrlAndRef()
        {
            Referer = baseUrl + "/" + baseUsername + "/boards/";
            if (FirstRequest)
            {
                Url = baseUrl + "/" + baseUsername + "/" + boardName + "/";
            }
            else
            {
                if (boardId == null || boardId.Length == 0)
                    boardId = SetBoardId();
                if (boardId == null || boardId.Length == 0)
                {
                    FirstRequest = true;
                    return;
                }

                    Url = baseUrl + GetData();
            }

            
        }
        protected override string GetData()
        {
            string str =
                        "/resource/BoardFeedResource/get/?source_url="
                        + HttpUtility.UrlEncode("/" + baseUsername + "/" + boardName + "/")
                        + "&data="
                        + HttpUtility.UrlEncode("{\"options\":{\"board_id\":\"" + boardId + "\",\"board_url\":\"/" + baseUsername + "/" + boardName + "/\",\"board_layout\":\"default\",\"prepend\":true,\"page_size\":null,\"access\":[],\"bookmarks\":[\"" + Bookmark + "\"]},\"context\":{}}")
                        + "&module_path="
                        + HttpUtility.UrlEncode(
                              "UserProfilePage(resource=UserResource(username=" + baseUsername + "))"
                            + ">UserProfileContent(resource=UserResource(username=" + baseUsername + "))"
                            + ">UserBoards()>Grid(resource=ProfileBoardsResource(username=" + baseUsername + "))"
                            + ">GridItems(resource=ProfileBoardsResource(username=" + baseUsername + "))"
                            + ">Board(show_board_context=false, view_type=boardCoverImage, component_type=1, show_user_icon=false, resource=BoardResource(board_id=" + boardId + "))")
                        + "&_=" + GetTime();         

            str = Regex.Replace(str, "%\\d(\\w)", m => m.ToString().ToUpper());
            return str;
        }       
        
    }
}
