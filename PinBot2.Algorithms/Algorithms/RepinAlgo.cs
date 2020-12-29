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
using System.Text.RegularExpressions;
using PinBot2.Algorithms.Helpers;
using PinBot2.Algorithms.Scraping.Queue;
namespace PinBot2.Algorithms
{
    public class RepinAlgo : Algo
    {
        private List<string> headers;
        //private IList<string> SourceUrls;
        private DuplicateChecker dupChecker;
        private Random r = new Random();

        public RepinAlgo(IAccountRepository repository)
            : base(repository)
        {
            Config = new RepinConfiguration();
        }

        private IDictionary<Board, ScrapeSessionManager> Managers;

        public RepinAlgo(IAccount account, IConfiguration config, IAccountRepository repository)
            : base(repository)
        {
            dupChecker = DuplicateChecker.init();
            this.account = account;
            Config = config;
            Running = Config.Enabled;
            CurrentCount = new Range<int>(0, r.Next(Config.CurrentCount.Min, Config.CurrentCount.Max + 1));

            headers = new List<string>
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

            RepinConfiguration cnfg = (RepinConfiguration)Config;
            Managers = new Dictionary<Board, ScrapeSessionManager>();
            foreach (var kv in cnfg.AllQueries)
            {
                if (kv.Value == null || kv.Value.Count <= 0)
                    continue;
                cnfg.Queries = kv.Value;
                var sm = new ScrapeSessionManager(this, cnfg, request, this.account);
                var kv2 = new KeyValuePair<Board, ScrapeSessionManager>(kv.Key, sm);
                Managers.Add(kv2);
            }

            /*if (cnfg.SourceUrlRate > 0 && cnfg.SourceUrls != null && cnfg.SourceUrls.Count > 0)
                SourceUrls = cnfg.SourceUrls;*/
        }
        public override void Run()
        {
            IDictionary<PinterestObject, Board> Queue = null;
            try
            {

                if (
                    Managers != null && Managers.Count > 0 &&
                    Managers.Any(x => x.Value.Sessions.Any(y => y.PinterestObjects.Any(z => z.Resource.GetType().Equals(PinterestObjectResources.IndividualPin))))
                    &&
                    !Managers.Any(x => x.Value.Sessions.Any(y => y.PinterestObjects.Any(z => !z.Resource.GetType().Equals(PinterestObjectResources.IndividualPin))))
                    )
                {
                    int cnt = Managers.Select(x =>
                        x.Value.Sessions.Select(y =>
                            y.PinterestObjects.Select(z =>
                                z.Resource.GetType().Equals(PinterestObjectResources.IndividualPin)
                            )
                        )
                    ).Count();
                    CurrentCount = new Range<int>(0, cnt);
                }

                Queue = ((RepinConfiguration)Config).Queue;
                if (Queue != null && Queue.Count > 0 && Managers != null && Managers.Count <= 0)
                {
                    CurrentCount = new Range<int>(0, Queue.Count);
                }
            }
            catch (Exception ex)
            {
                string msg = "Error REPALGO133." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log(account.Username, this.GetType().ToString(), msg);
            }

            Running = true;
            while (Running)
            {
                Console.WriteLine(this.ToString());
                try
                {
                    if (!ContinueRunning()) { Running = false; return; }


                    Queue = ((RepinConfiguration)Config).Queue;
                    if (Queue == null || Queue.Count == 0)
                    {
                        RandomRun(Queue);
                    }
                    else
                    {
                        QueueRun(Queue);
                    }

                    if (Status == STATUS.LIMIT_REACHED)
                    {
                        Running = false; return;
                    }

                }
                catch (Exception ex)
                {
                    string msg = "Error REPALGO74." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                    Logging.Log(account.Username, this.GetType().ToString(), msg);

                }
            }


        }
        private void RandomRun(IDictionary<PinterestObject, Board> Queue)
        {
            IList<PinterestObject> Pins = new List<PinterestObject>();
            Board board = null;
            Random r = new Random();

            int index = r.Next(0, Managers.Count);
            SessionManager = Managers.ElementAt(index).Value;
            board = Managers.ElementAt(index).Key;
            Pins = SessionManager.Scrape("/" + board.Url + "/");


            if (!ContinueRunning()) { Running = false; return; }

            if (Pins == null) { Status = STATUS.LIMIT_REACHED; Running = false; return; }
            if (Pins.Count <= 0)
                return;

            for (int i = 0; i < Pins.Count; i++)
            {
                PinterestObject pin = Pins[i];
                if (SessionManager != null && SessionManager.UsedPinterestObjects.Contains(pin))
                    continue;

                if (!ContinueRunning()) { Running = false; return; }
                Repin(pin, board);

                if (!ContinueRunning()) { Running = false; return; }
                TimeOut();
            }
        }
        private void QueueRun(IDictionary<PinterestObject, Board> Queue)
        {
            IList<PinterestObject> Pins = new List<PinterestObject>();
            Board board = null;
            Random r = new Random();

            int index = r.Next(0, Managers.Count);
            SessionManager = Managers.ElementAt(index).Value;

            board = Queue.Values.ElementAt(r.Next(0, Queue.Count));
            foreach (var kv in Queue.Where(x => x.Value.Id == board.Id))
            {
                Pins.Add(kv.Key);
            }

            if (!ContinueRunning()) { Running = false; return; }

            if (Pins == null) { Status = STATUS.LIMIT_REACHED; Running = false; return; }
            if (Pins.Count <= 0)
                return;

            for (int i = 0; i < Pins.Count; i++)
            {
                PinterestObject pin = Pins[i];
                if (SessionManager != null && SessionManager.UsedPinterestObjects.Contains(pin))
                    continue;

                if (!ContinueRunning()) { Running = false; return; }

                Repin(pin, board);

                ICampaign campaign = repository.GetCampaign(account.SelectedCampaignId);
                campaign.ConfigurationContainer.RepinConfiguration.Queue.Remove(pin);
                Config = campaign.ConfigurationContainer.RepinConfiguration;
                repository.SaveCampaign((Campaign)campaign);

                if (!ContinueRunning()) { Running = false; return; }
                TimeOut();
            }
        }

        private void Repin(PinterestObject p, Board board)
        {
            Data data = GetData(p, board);

            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append(data.content); //it was appendline before
            byte[] hdrbytes = System.Text.Encoding.UTF8.GetBytes(strBuilder.ToString());
            List<byte[]> http_data = new List<byte[]>();
            http_data.Add(hdrbytes);

            string rs = null;

            try
            {
                rs =
                    request.POST(
                        "https://www.pinterest.com/resource/RepinResource/create/",
                        data.referer,
                        "application/x-www-form-urlencoded; charset=UTF-8",
                        http_data,
                        headers,
                        account.CookieContainer,
                        account.WebProxy,
                        100000,
                        "application/json, text/javascript, */*; q=0.01");

                Logging.Log(account.Username, this.GetType().ToString(), "Repin POST" + Environment.NewLine + Environment.NewLine + strBuilder.ToString());
            }
            catch (Exception ex)
            {
                string msg = "Error REPALGO188." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log(account.Username, this.GetType().ToString(), msg);

            }

            if (ProcessResponse(rs, p))
            {
                dupChecker.Add(account.Email, ((Pin)p).Image);
                //ChangeSourceUrl((Pin)p, board, rs);
            }

        }
        /*private void ChangeSourceUrl(Pin p, Board b, string RS)
        {
            if (SourceUrls != null && (((Pin)p).Link == null || ((Pin)p).Link.Length <= 5))
                p.Link = QueueHelper.GetRandomSourceUrl((RepinConfiguration)Config);
            else if (!http.ValidUrl(p.Link))
                return;

            if (RS == null) return;
            p.Id = Regex.Match(RS, " \"id\": \"(\\d+?)\"}, \"error\"").Groups[1].ToString();

            if (p.Id.Length > 0)
            {
                string hd =
                    "source_url="
                    + HttpUtility.UrlEncode("/" + account.Username + "/pins/")
                    + "&data="
                    + HttpUtility.UrlEncode("{\"options\":{\"board_id\":\""
                    + b.Id
                    + "\",\"description\":\"" + p.Description
                    + "\",\"link\":\"" + p.Link
                    + "\",\"id\":\"" + p.Id
                    + "\"},\"context\":{}}")
                    + "&module_path="
                    + HttpUtility.UrlEncode("App()>UserProfilePage(resource=UserResource(username=" + account.Username
                    + "))>UserProfileContent(resource=UserResource(username=" + account.Username
                    + "))>Grid(resource=UserPinsResource(username=" + account.Username
                    + "))>GridItems(resource=UserPinsResource(username=" + account.Username
                    + "))>Pin(resource=PinResource(id=" + p.Id
                    + "))>ShowModalButton(module=PinEdit)"
                    + "#Modal(module=PinEdit(resource=PinResource(id=" + p.Id + ")))");

                List<byte[]> lb = new List<byte[]>();
                byte[] headerbytes = Encoding.ASCII.GetBytes(hd);
                lb.Add(headerbytes);

                string rs = request.POST(
                    "https://www.pinterest.com/resource/PinResource/update/ ",
                    "https://www.pinterest.com/pin/" + p.Id + "/",
                    "application/x-www-form-urlencoded; charset=UTF-8",
                    lb,
                    headers,
                    account.CookieContainer,
                    account.WebProxy,
                    60000,
                    "application/json, text/javascript"
                );

            }
        }*/

        protected override Data GetData(PinterestObject P)
        { return new Data(); }
        protected Data GetData(PinterestObject P, Board board)
        {
            Pin p = (Pin)P;

            Data data = new Data();
            switch (P.Resource)
            {

                case PinterestObjectResources.UserPinsResource:
                    if (p.SearchQuery.Contains("/"))
                        p.Username = p.SearchQuery.Split('/')[1];
                    data.content =
                        "source_url=" +
                        HttpUtility.UrlEncode("/" + p.Username + "/pins/")
                        + "&data="
                        + HttpUtility.UrlEncode("{\"options\":{\"board_id\":\"" + board.Id + "\",\"description\":\"" + p.Description + "\",\"link\":\"" + p.Link + "\",\"is_video\":false,\"pin_id\":\"" + p.Id + "\"},\"context\":{}}")
                        + "&module_path="
                        + HttpUtility.UrlEncode(
                                    "App()>UserProfilePage(resource=UserResource(username=" + p.Username + "))"
                                + ">UserProfileContent(resource=UserResource(username=" + p.Username + "))"
                                + ">Grid(resource=UserPinsResource(username=" + p.Username + "))"
                                + ">GridItems(resource=UserPinsResource(username=" + p.Username + "))"
                                + ">Pin(resource=PinResource(id=" + p.Id + "))"
                                + ">ShowModalButton(module=PinCreate)#Modal(module=PinCreate(resource=PinResource(id=" + p.Id + ")))"
                            );
                    data.referer = "https://www.pinterest.com/" + p.Username + "/pins/";
                    break;

                case PinterestObjectResources.BoardFeedResource:
                    if (p.SearchQuery.Contains("/"))
                    {
                        p.Username = p.SearchQuery.Split('/')[1];
                        p.Boardname = p.SearchQuery.Split('/')[2];
                    }
                    data.content =
                        "source_url=" +
                        HttpUtility.UrlEncode("/" + p.Username + "/" + p.Boardname + "/")
                        + "&data="
                        + HttpUtility.UrlEncode("{\"options\":{\"pin_id\":\"" + p.Id + "\",\"description\":\"" + p.Description + "\",\"link\":" + (p.Link == null || p.Link == "" ? "null" : "\"" + p.Link + "\"") + ",\"is_video\":false,\"board_id\":\"" + board.Id + "\"},\"context\":{}}")
                        + "&module_path="
                        + HttpUtility.UrlEncode(
                                    "App>ModalManager>Modal>PinCreate>BoardPicker>SelectList(view_type=pinCreate, selected_section_index=undefined, selected_item_index=undefined, highlight_matched_text=true, suppress_hover_events=undefined, scroll_selected_item_into_view=true, select_first_item_after_update=false, item_module=[object Object])"
                            );
                    data.referer = "https://www.pinterest.com/" + p.Username + "/" + p.Boardname + "/";
                    break;

                case PinterestObjectResources.SearchResource:
                    data.content =
                        "source_url=" +
                        HttpUtility.UrlEncode("/search/pins/?q=" + HttpUtility.UrlEncode(p.SearchQuery))
                        + "&data="
                        + HttpUtility.UrlEncode("{\"options\":{\"board_id\":\"" + board.Id + "\",\"description\":\"" + p.Description + "\",\"link\":\"" + p.Link + "\",\"is_video\":false,\"pin_id\":\"" + p.Id + "\"},\"context\":{}}")
                        + "&module_path="
                        + HttpUtility.UrlEncode(
                                    "App()>SearchPage(resource=BaseSearchResource(constraint_string=null, show_scope_selector=true, restrict=null, scope=pins, query=" + p.SearchQuery + "))"
                                + ">SearchPageContent(resource=SearchResource(layout=null, places=false, constraint_string=null, show_scope_selector=true, query=" + p.SearchQuery + ", scope=pins))"
                                + ">Grid(resource=SearchResource(layout=null, places=false, constraint_string=null, show_scope_selector=true, query=" + p.SearchQuery + ", scope=pins))"
                                + ">GridItems(resource=SearchResource(layout=null, places=false, constraint_string=null, show_scope_selector=true, query=" + p.SearchQuery + ", scope=pins))"
                                + ">Pin(resource=PinResource(id=" + p.Id + "))"
                                + ">ShowModalButton(module=PinCreate)#Modal(module=PinCreate(resource=PinResource(id=" + p.Id + ")))"
                            );
                    data.referer = "https://www.pinterest.com/search/pins/?q=" + HttpUtility.UrlEncode(p.SearchQuery);
                    break;
            }

            return data;
        }

    }
}
