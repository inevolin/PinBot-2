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
    public class InviteAlgo : Algo
    {
        public InviteAlgo(IAccountRepository repository)
            : base(repository)
        {
            Config = new InviteConfiguration();
        }

        private IDictionary<Board, ScrapeSessionManager> Managers;
        public InviteAlgo(IAccount account, IConfiguration config, IAccountRepository repository)
            : base(repository)
        {
            this.account = account;
            Config = (InviteConfiguration)config;
            Running = Config.Enabled;
            Random r = new Random();
            CurrentCount = new Range<int>(0, r.Next(Config.CurrentCount.Min, Config.CurrentCount.Max + 1));

            Managers = new Dictionary<Board, ScrapeSessionManager>();
            foreach (var kv in ((InviteConfiguration)Config).SelectedBoards)
            {
                if (kv.Value == false)
                    continue;
                InviteConfiguration cnfg = (InviteConfiguration)config;
                cnfg.Queries = new Dictionary<string, PinterestObjectResources>();

                var kv2 = new KeyValuePair<string, PinterestObjectResources>("/" + kv.Key.Url + "/",PinterestObjectResources.BoardFollowersResource);
                cnfg.Queries.Add(kv2);

                var sm = new ScrapeSessionManager(this, cnfg, request, this.account);
                var kv3 = new KeyValuePair<Board, ScrapeSessionManager>(kv.Key, sm);
                Managers.Add(kv3);

            }
        }
        public override void Run()
        {
            Running = true;
            while (Running)
            {
                Console.WriteLine(this.ToString());
                try
                {
                    if (!ContinueRunning()) { Running = false; return; }

                    Random r = new Random();
                    int index = r.Next(0, Managers.Count);
                    SessionManager = Managers.ElementAt(index).Value;
                    Board board = Managers.ElementAt(index).Key;
                    IList<PinterestObject> Pinners = SessionManager.Scrape(board.SearchQuery);

                    if (!ContinueRunning()) { Running = false; return; }

                    if (Pinners == null) { Status = STATUS.LIMIT_REACHED; Running = false; return; }
                    if (Pinners.Count <= 0) continue;

                    for (int i = 0; i < Pinners.Count; i++)
                    {
                        PinterestObject Pinner = Pinners[i];
                        if (SessionManager.UsedPinterestObjects.Contains(Pinner))
                            continue;

                        //if (!((Pinner)Pinner).FollowedByMe)
                           // continue;
                        

                        if (!ContinueRunning()) { Running = false; return; }
                        InvitePinner(Pinner, board);                        
                        if (!ContinueRunning()) { Running = false; return; }
                        TimeOut();
                    }
                }
                catch (Exception ex)
                {
                    string msg = "Error INALGO90." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                    Logging.Log(account.Username, this.GetType().ToString(), msg);
                    
                }
            }
        }

        private void InvitePinner(PinterestObject p, Board board)
        {
            Data data = GetData(p, board);

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
                        "https://www.pinterest.com/resource/BoardInviteResource/create/",
                        data.referer,
                        "application/x-www-form-urlencoded; charset=UTF-8",
                        http_data,
                        headers,
                        account.CookieContainer,
                        account.WebProxy,
                        100000,
                        "application/json, text/javascript, */*; q=0.01");

                Logging.Log(account.Username, this.GetType().ToString(), "invite POST" + Environment.NewLine + Environment.NewLine + strBuilder.ToString());
            }
            catch (Exception ex)
            {
                string msg = "Error INALGO137." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log(account.Username, this.GetType().ToString(), msg);
                
            }

            ProcessResponse(rs, p);
        }
        protected override Data GetData(PinterestObject P)
        { return new Data(); }
        private Data GetData(PinterestObject P, Board board)
        {
            Pinner p = (Pinner)P;
            
            Data data = new Data();
            switch (P.Resource)
            {
                
                case PinterestObjectResources.BoardFollowersResource:
                    data.content =
                        "source_url=" +
                        HttpUtility.UrlEncode("/" + board.Url + "/")
                        + "&data="
                        + HttpUtility.UrlEncode("{\"options\":{\"board_id\":\"" + board.Id + "\",\"invited_user_id\":\"" + p.Id + "\"},\"context\":{}}")
                        + "&module_path="
                        + HttpUtility.UrlEncode(
                                    "App()>BoardPage(resource=BoardResource(username=" + p.BaseUsername + ", slug=" + board.Url.Split('/')[1] + "))"
                                + ">BoardHeader(resource=BoardResource(board_id=" + board.Id + "))"
                                + ">BoardInfoBar(resource=BoardResource(board_id=" + board.Id + "))"
                                + ">ShowModalButton(module=BoardCollaboratorInviter)"
                                + "#Modal(module=BoardCollaboratorInviter(resource=BoardResource(board_id=" + board.Id + ")))"
                            );
                    data.referer = "https://www.pinterest.com/" + board.Url + "/";
                    break;

            }

            return data;
        }

    }
}
