using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PinBot2;
using PinBot2.Dal.Interface;
using PinBot2.Model.Configurations;
using PinBot2.Model.PinterestObjects;
using PinBot2.Model;
using PinBot2.Common;
using System.Threading;
using System.Web;
using PinBot2.Algorithms.Scraping;
using System.Threading.Tasks;
namespace PinBot2.Algorithms
{
    public class LikeAlgo : Algo
    {
        public LikeAlgo(IAccountRepository repository)
            : base(repository)
        {
            Config = new LikeConfiguration();
        }

        public LikeAlgo(IAccount account, IConfiguration config, IAccountRepository repository)
            : base(repository)
        {
            this.account = account;
            Config = (LikeConfiguration)config;
            Running = Config.Enabled;
            Random r = new Random();
            CurrentCount = new Range<int>(0, r.Next(Config.CurrentCount.Min, Config.CurrentCount.Max + 1));

            SessionManager = new ScrapeSessionManager(this, Config, request, this.account);
        }
        public override void Run()
        {
            Logging.Log(account.Username, this.GetType().ToString(), "Starting");
            Running = true;
            while (Running)
            {
                Console.WriteLine(this.ToString());
                try
                {
                    if (!ContinueRunning()) { Running = false; return; }
                    IList<PinterestObject> Pins = SessionManager.Scrape(this.GetType().ToString());
                    if (!ContinueRunning()) { Running = false; return; }

                    if (Pins == null) { Status = STATUS.LIMIT_REACHED; Running = false; return; }
                    if (Pins.Count <= 0) continue;

                    for (int i = 0; i < Pins.Count; i++)
                    {
                        PinterestObject pin = Pins[i];
                        if (SessionManager.UsedPinterestObjects.Contains(pin))
                            continue;
                        
                        if (((Pin)pin).LikedByMe)
                            continue;

                        if (!ContinueRunning()) { Running = false; return; }
                        LikePin(pin);
                        if (!ContinueRunning()) { Running = false; return; }
                        TimeOut();
                    }
                }
                catch (Exception ex)
                {
                    string msg = "Error LIALGO67." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");                    
                    Logging.Log(account.Username, this.GetType().ToString(), msg);
                    
                }
            }
        }

        private void LikePin(PinterestObject p)
        {
            Data data = GetData(p);

            StringBuilder strBuilder = new StringBuilder();
            strBuilder.AppendLine(data.content);
            byte[] hdrbytes = System.Text.Encoding.UTF8.GetBytes(strBuilder.ToString());

            List<byte[]> http_data = new List<byte[]>();
            http_data.Add(hdrbytes);
            List<string> headers = new List<string>
                {   "X-NEW-APP: 1",
                    "X-APP-VERSION: " + account.AppVersion,
                    "X-Requested-With: XMLHttpRequest",
                    "X-CSRFToken: " + account.CsrfToken,
                    "Accept-Encoding: gzip,deflate,sdch",
                    "Accept-Language: en-US,en;q=0.8",
                    "X-Pinterest-AppState: active",
                    "Pragma: no-cache",
                    "Cache-Control: no-cache"
                    
                };

            string rs = null;

            try
            {
                rs =
                    request.POST(
                        "https://www.pinterest.com/resource/PinLikeResource/create/",
                        data.referer,
                        "application/x-www-form-urlencoded; charset=UTF-8",
                        http_data,
                        headers,
                        account.CookieContainer,
                        account.WebProxy,
                        100000,
                        "application/json, text/javascript, */*; q=0.01");

                Logging.Log(account.Username, this.GetType().ToString(), "like POST" + Environment.NewLine + Environment.NewLine + strBuilder.ToString());
            }
            catch (Exception ex)
            {
                string msg = "Error LIALGO114." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log(account.Username, this.GetType().ToString(), msg);
                
            }

            ProcessResponse(rs,p);
        }
        protected override Data GetData(PinterestObject P)
        {
            Pin p = (Pin)P;
            
            Data data = new Data();
            switch (P.Resource)
            {
                case PinterestObjectResources.UserPinsResource:
                    data.content =
                        "source_url=" +
                        HttpUtility.UrlEncode("/" + p.Username + "/pins/")
                        + "&data="
                        + HttpUtility.UrlEncode("{\"options\":{\"pin_id\":\"" + p.Id + "\",\"source_interest_id\":null},\"context\":{}}")
                        + "&module_path="
                        + HttpUtility.UrlEncode(
                                    "App()>UserProfilePage(resource=UserResource(username=" + p.Username + "))"
                                + ">UserProfileContent(resource=UserResource(username=" + p.Username + "))"
                                + ">Grid(resource=UserPinsResource(username=" + p.Username + "))"
                                + ">GridItems(resource=UserPinsResource(username=" + p.Username + "))"
                                + ">Pin(resource=PinResource(id=" + p.Id + "))"
                                + ">PinLikeButton(liked=false, source_interest_id=null, has_icon=true, text=Like, class_name=likeSmall, pin_id=" + p.Id + ", show_text=false, ga_category=like)"
                            );
                    data.referer = "https://www.pinterest.com/" + p.Username + "/pins/";
                    break;

                case PinterestObjectResources.BoardFeedResource:
                    data.content =
                        "source_url=" +
                        HttpUtility.UrlEncode("/" + p.Username + "/" + p.Boardname + "/")
                        + "&data="
                        + HttpUtility.UrlEncode("{\"options\":{\"pin_id\":\"" + p.Id + "\",\"source_interest_id\":null},\"context\":{}}")
                        + "&module_path="
                        + HttpUtility.UrlEncode(
                                    "App()>BoardPage(resource=BoardResource(username=" + p.Username + ", slug=" + p.Boardname + "))"
                                + ">Grid(resource=BoardFeedResource(board_id=" + p.BoardId + ", board_url=/" + p.Username + "/" + p.Boardname + "/, board_layout=default, prepend=true, page_size=null, access=))"
                                + ">GridItems(resource=BoardFeedResource(board_id=" + p.BoardId + ", board_url=/" + p.Username + "/" + p.Boardname + "/, board_layout=default, prepend=true, page_size=null, access=))"
                                + ">Pin(resource=PinResource(id=" + p.Id + "))"
                                + ">PinLikeButton(ga_category=like, class_name=likeSmall, liked=false, pin_id=" + p.Id + ", has_icon=true, show_text=false, source_interest_id=undefined, text=Like)"
                            );
                    data.referer = "https://www.pinterest.com/" + p.Username + "/" + p.Boardname + "/";
                    break;
                
                case PinterestObjectResources.SearchResource:
                    data.content =
                        "source_url=" +
                        HttpUtility.UrlEncode("/search/pins/?q=" + HttpUtility.UrlEncode(p.SearchQuery))
                        + "&data="
                        + HttpUtility.UrlEncode("{\"options\":{\"pin_id\":\"" + p.Id + "\",\"source_interest_id\":null},\"context\":{}}")
                        + "&module_path="
                        + HttpUtility.UrlEncode(
                                    "App()>SearchPage(resource=BaseSearchResource(constraint_string=null, show_scope_selector=true, restrict=null, scope=pins, query=" + p.SearchQuery + "))"
                                + ">SearchPageContent(resource=SearchResource(layout=null, places=false, constraint_string=null, show_scope_selector=true, query=" + p.SearchQuery + ", scope=pins))"
                                + ">Grid(resource=SearchResource(layout=null, places=false, constraint_string=null, show_scope_selector=true, query=" + p.SearchQuery + ", scope=pins))"
                                + ">GridItems(resource=SearchResource(layout=null, places=false, constraint_string=null, show_scope_selector=true, query=" + p.SearchQuery + ", scope=pins))"
                                + ">Pin(resource=PinResource(id=" + p.Id + "))"
                                + ">PinLikeButton(liked=false, source_interest_id=null, has_icon=true, text=Like, class_name=likeSmall, pin_id=" + p.Id + ", show_text=false, ga_category=like)"
                            );
                    data.referer = "https://www.pinterest.com/search/pins/?q="  + HttpUtility.UrlEncode(p.SearchQuery);
                    break;
            }

            return data;
        }

    }
}
