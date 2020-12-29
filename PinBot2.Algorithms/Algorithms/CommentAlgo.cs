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
    public class CommentAlgo : Algo
    {

        private IList<string> Comments { get; set; }

        public CommentAlgo(IAccountRepository repository)
            : base(repository)
        {
            Config = new CommentConfiguration();
        }

        public CommentAlgo(IAccount account, IConfiguration config, IAccountRepository repository)
            : base(repository)
        {
            this.account = account;
            Config = (CommentConfiguration)config;
            Running = Config.Enabled;
            Random r = new Random();
            CurrentCount = new Range<int>(0, r.Next(Config.CurrentCount.Min, Config.CurrentCount.Max + 1));

            SessionManager = new ScrapeSessionManager(this, Config, request, this.account);
            Comments = ((CommentConfiguration)Config).Comments;
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
                    IList<PinterestObject> Pins = SessionManager.Scrape(this.GetType().ToString());
                    if (!ContinueRunning()) { Running = false; return; }

                    if (Pins == null) { Status = STATUS.LIMIT_REACHED; Running = false; return; }
                    if (Pins.Count <= 0) continue;

                    for (int i = 0; i < Pins.Count; i++)
                    {
                        PinterestObject pin = Pins[i];

                        if (SessionManager.UsedPinterestObjects.Contains(pin))
                            continue;

                        if (AlreadyCommentedOnPin(pin))
                            continue;

                        if (!ContinueRunning()) { Running = false; return; }
                        CommentPin(pin);
                        if (!ContinueRunning()) { Running = false; return; }
                        TimeOut();
                    }
                }
                catch (Exception ex)
                {
                    string msg = "Error COMALGO71." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                    Logging.Log(account.Username, this.GetType().ToString(), msg);
                    
                }
            }
        }

        private bool AlreadyCommentedOnPin(PinterestObject p)
        {
            IList<PinterestObject> list = SessionManager.ScrapeComments(p);
            foreach (PinterestObject o in list)
            {
                if (((Comment)o).Username.Trim() == account.Username.Trim())
                    return true;
            }
            return false;
        }
        private void CommentPin(PinterestObject p)
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
                        "https://www.pinterest.com/resource/PinCommentResource/create/",
                        data.referer,
                        "application/x-www-form-urlencoded; charset=UTF-8",
                        http_data,
                        headers,
                        account.CookieContainer,
                        account.WebProxy,
                        100000,
                        "application/json, text/javascript, */*; q=0.01");

                Logging.Log(account.Username, this.GetType().ToString(), "comment POST" + Environment.NewLine + Environment.NewLine + strBuilder.ToString());
            }
            catch (Exception ex)
            {
                string msg = "Error COMALGO128." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log(account.Username, this.GetType().ToString(), msg);
                
            }

            ProcessResponse(rs, p);
        }
        private string GetComment()
        {
            Random r = new Random();
            return Comments[r.Next(0, Comments.Count)];
        }
        protected override Data GetData(PinterestObject P)
        {
            Pin p = (Pin)P;

            string COMMENT = GetComment();
            COMMENT = COMMENT.Replace("\"", "\\\"");
 
            Data data = new Data();
            //switch (P.Resource)
            //{
               // case PinterestObjectResources.SearchResource:
                    data.content =
                        "source_url=" +
                        HttpUtility.UrlEncode("/pin/" + p.Id + "/")
                        + "&data="
                        + HttpUtility.UrlEncode("{\"options\":{\"pin_id\":\"" + p.Id + "\",\"text\":\"" + COMMENT + "\"},\"context\":{}}")
                        + "&module_path="
                        + HttpUtility.UrlEncode(
                                    "App>Closeup>CloseupContent>Pin>PinCommentsPage>PinDescriptionComment(image_src=https://s-media-cache-ak0.pinimg.com/avatars/"+ DateTime.Now.Ticks +".jpg, username=" + account.Username + ", full_name=" + account.Username + ", content=null, show_comment_form=true, view_type=detailed, subtitle=That's you!, pin_id=" + p.Id + ", is_description=false)"
                            );
                    data.referer = "https://www.pinterest.com/pin/" + p.Id + "/";
               //     break;
           // }

            return data;
        }

    }
}
