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
using System.IO;
using System.Text.RegularExpressions;
using PinBot2.Algorithms.Helpers;
using PinBot2.Algorithms.Scraping.Queue;
namespace PinBot2.Algorithms
{
    public class PinAlgo : Algo
    {
        private List<string> headers;
        private IList<string> SourceUrls;
        private DuplicateChecker dupChecker;

        private int CountErrors = 0;

        public PinAlgo(IAccountRepository repository)
            : base(repository)
        {
            Config = new PinConfiguration();
        }

        private IDictionary<Board, ScrapeSessionManager> Managers;
        public PinAlgo(IAccount account, IConfiguration config, IAccountRepository repository)
            : base(repository)
        {
            dupChecker = DuplicateChecker.init();
            this.account = account;
            Config = config;
            Running = Config.Enabled;
            Random r = new Random();

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

            PinConfiguration cnfg = (PinConfiguration)Config;
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

            if (cnfg.SourceUrlRate > 0 && cnfg.SourceUrls != null && cnfg.SourceUrls.Count > 0)
                SourceUrls = cnfg.SourceUrls;

            if (Managers.Count == 0)
                CurrentCount = new Range<int>(0, ((PinConfiguration)Config).Queue.Count);
            else
                CurrentCount = new Range<int>(0, r.Next(Config.CurrentCount.Min, Config.CurrentCount.Max + 1));
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

                    IDictionary<PinterestObject, Board> Queue = ((PinConfiguration)Config).Queue;

                    if (Queue == null || Queue.Count == 0)
                    {
                        RandomRun(Queue);
                    }
                    else
                    {
                        QueueRun(Queue);
                    }

                }
                catch (Exception ex)
                {
                    if (++CountErrors > 15) {
                        Logging.Log(account.Username, this.GetType().ToString(), "PinAlgo stopped due to ErrorCount > 15");
                        Running = false;
                        return;
                    }

                    string msg = "Error PINALGO73." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
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
            Pins = SessionManager.Scrape(board.SearchQuery);

            if (!ContinueRunning()) { Running = false; return; }

            if (Pins == null) { Status = STATUS.LIMIT_REACHED; Running = false; return; }
            if (Pins.Count <= 0)
                return;

            for (int i = 0; i < Pins.Count; i++)
            {
                ExternalPin pin = (ExternalPin)Pins[i];
                if (SessionManager.UsedPinterestObjects.Contains(pin))
                    continue;

                if (!ContinueRunning()) { Running = false; return; }

                string img = UploadImage(http.DownloadFile(pin.ImageFound));
                if (img == null || img == "")
                    continue;

                pin.ImageUploaded = img;
                Pin(pin, board);

                if (!ContinueRunning()) { Running = false; return; }

                TimeOut();
            }
        }
        private void QueueRun(IDictionary<PinterestObject, Board> Queue)
        {
            IList<PinterestObject> Pins = new List<PinterestObject>();
            Board board = null;
            Random r = new Random();

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
                ExternalPin pin = (ExternalPin)Pins[i];
                if (!ContinueRunning()) { Running = false; return; }

                string img = (pin.ImageFound.Contains("http://") || pin.ImageFound.Contains("https://")) ?
                   UploadImage(http.DownloadFile(pin.ImageFound)) : img = UploadImage(pin.ImageFound);

                if (img == null || img == "")
                    continue;

                pin.ImageUploaded = img;
                Pin(pin, board);

                ICampaign campaign = repository.GetCampaign(account.SelectedCampaignId);
                campaign.ConfigurationContainer.PinConfiguration.Queue.Remove(pin);
                Config = campaign.ConfigurationContainer.PinConfiguration;
                repository.SaveCampaign((Campaign)campaign);


                if (!ContinueRunning()) { Running = false; return; }
                TimeOut();
            }
        }

        private void Pin(PinterestObject p, Board board)
        {
            Data data = GetData((ExternalPin)p, board);

            StringBuilder strBuilder = new StringBuilder();
            strBuilder.AppendLine(data.content);
            byte[] hdrbytes = System.Text.Encoding.UTF8.GetBytes(strBuilder.ToString());

            List<byte[]> http_data = new List<byte[]>();
            http_data.Add(hdrbytes);

            string rs = null;

            try
            {
                rs =
                    request.POST(
                        "https://www.pinterest.com/resource/PinResource/create/",
                        data.referer,
                        "application/x-www-form-urlencoded; charset=UTF-8",
                        http_data,
                        headers,
                        account.CookieContainer,
                        account.WebProxy,
                        100000,
                        "application/json, text/javascript, */*; q=0.01");

                Logging.Log(account.Username, this.GetType().ToString(), "pin POST" + Environment.NewLine + Environment.NewLine + strBuilder.ToString());
            }
            catch (Exception ex)
            {
                string msg = "Error PINALGO202." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log(account.Username, this.GetType().ToString(), msg);

            }

            if (ProcessResponse(rs, p))
                dupChecker.Add(account.Email, ((ExternalPin)p).ImageFound);
        }

        private void ChangeSourceUrl(ExternalPin p, Board b, string RS)
        {
            if (SourceUrls != null)
                p.Link = QueueHelper.GetRandomSourceUrl((PinConfiguration)Config);
            else if (!http.ValidUrl(p.Link))
                return;

            if (RS == null) return;
            p.Id = Regex.Match(RS, "\"id\":\"(\\d+?)\"},\"error\"").Groups[1].ToString();

            if (p.Id.Length > 0)
            {
                string hd =
                    "source_url="
                    + HttpUtility.UrlEncode("/" + account.Username + "/pins/")
                    + "&data="
                    + HttpUtility.UrlEncode("{\"options\":{\"board_id\":\""
                    + b.Id
                    + "\",\"description\":\"" + p.Description.Replace("\n", "\\\\n").Replace("\"", "\\\"")
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
                    "application/json, text/javascript, */*; q=0.01"
                );
                Logging.Log(account.Username, this.GetType().ToString(), "pin POST" + Environment.NewLine + Environment.NewLine + hd.ToString());

            }
        }

        private string UploadImage(string file)
        {
            List<byte[]> http_data = new List<byte[]>();
            string boundary = http.getBoundary();

            StringBuilder strBuilder = new StringBuilder();
            strBuilder.AppendLine("--" + boundary);
            strBuilder.AppendLine("Content-Disposition: form-data; name=\"img\"; filename=\"" + Path.GetFileName(file) + "\"");
            string ext = Path.GetExtension(file).Replace(".", "").ToLower();
            ext = ext.Equals("jpg") ? "jpeg" : ext;
            strBuilder.Append("Content-Type: image/" + ext);
            strBuilder.AppendLine();
            strBuilder.AppendLine();

            byte[] hdrbytes = System.Text.Encoding.UTF8.GetBytes(strBuilder.ToString());
            byte[] filedata = null;
            using (BinaryReader breader = new BinaryReader(File.OpenRead(file)))
                filedata = breader.ReadBytes((int)breader.BaseStream.Length);
            byte[] endboundarybytes = System.Text.Encoding.ASCII.GetBytes(Environment.NewLine + "--" + boundary + "--" + Environment.NewLine);

            http_data.Add(hdrbytes);
            http_data.Add(filedata);
            http_data.Add(endboundarybytes);

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
                        "https://www.pinterest.com/upload-image/?img=" + HttpUtility.UrlEncode(Path.GetFileName(file)),
                        baseUrl,
                        "multipart/form-data; boundary=" + boundary,
                        http_data,
                        headers,
                        account.CookieContainer,
                        account.WebProxy,
                        100000,
                        "*/*");

                Logging.Log(account.Username, this.GetType().ToString(), "pin POST" + Environment.NewLine + Environment.NewLine + strBuilder.ToString());
            }
            catch (Exception ex)
            {
                string msg = "Error PINALGO267." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log(account.Username, this.GetType().ToString(), msg);

            }

            if (rs == null) return null;
            string pattern = "http(?:s*?)://(.+?)\",";
            if (Regex.IsMatch(rs, pattern))
            {
                Match match = Regex.Match(rs, pattern, RegexOptions.Singleline);
                string upload_url = "https://" + match.Groups[1].Value.ToString();
                return upload_url;
            }

            return "";
        }
        protected override Data GetData(PinterestObject P)
        { return new Data(); }
        protected Data GetData(ExternalPin p, Board board)
        {

            Data data = new Data();
            switch (p.Resource)
            {
                case PinterestObjectResources.External:
                    data.content =
                        "source_url=" +
                        HttpUtility.UrlEncode("/" + board.Url + "/")
                        + "&data="
                        + HttpUtility.UrlEncode("{\"options\":{\"board_id\":\"" + board.Id + "\",\"description\":\"" + p.Description.Replace("\n", "\\\\n").Replace("\"", "\\\"") + "\",\"link\":\"" + p.Link + "\",\"image_url\":\"" + p.ImageUploaded + "\",\"method\":\"uploaded\"},\"context\":{}}")
                        + "&module_path="
                        + HttpUtility.UrlEncode(
                                    "PinUploader(default_board_id=" + board.Id + ")#Modal(module=PinCreate())"
                            );
                    data.referer = "https://www.pinterest.com/" + board.Url + "/";
                    break;
            }

            return data;
        }
    }
}
