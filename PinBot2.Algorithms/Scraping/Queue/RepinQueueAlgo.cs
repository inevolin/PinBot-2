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
using PinBot2.Algorithms.Scraping.Queue;
using PinBot2.Algorithms.Helpers;
namespace PinBot2.Algorithms
{
    public class RepinQueueAlgo : Algo
    {
        private DuplicateChecker dupCheck;
        private IDictionary<PinterestObject, Board> Queue;
        public IDictionary<PinterestObject, Board> ScrapedQueue { get { return Queue; } }
        public IList<PinterestObject> InQueue { get; set; }
        public Board Board { get; set; }

        private IDictionary<Board, ScrapeSessionManager> Managers;
        public RepinQueueAlgo(IConfiguration config, IAccount account, IAccountRepository repository)
            : base(repository)
        {
            dupCheck = DuplicateChecker.init();

            if (Queue == null)
                Queue = new Dictionary<PinterestObject, Board>();

            this.account = account;
            Config = (IPinRepinConfiguration)config;
            Running = Config.Enabled;
            Random r = new Random();
            CurrentCount = new Range<int>(0, r.Next(((IPinRepinConfiguration)config).Scrape.Min, ((IPinRepinConfiguration)config).Scrape.Max + 1));

            Managers = new Dictionary<Board, ScrapeSessionManager>();
            foreach (var kv in ((IPinRepinConfiguration)Config).AllQueries)
            {
                IPinRepinConfiguration cnfg = (IPinRepinConfiguration)config;
                if (kv.Value == null || kv.Value.Count == 0)
                    continue;
                cnfg.Queries = kv.Value;
                var sm = new ScrapeSessionManager(this, cnfg, request, this.account);
                var kv2 = new KeyValuePair<Board, ScrapeSessionManager>(kv.Key, sm);
                Managers.Add(kv2);

            }
        }

        public override void Run()
        {
            try
            {
                Running = true;
                if (!ContinueRunning()) { Running = false; return; }

                if (Board == null)
                {
                    ScrapeAllBoards();
                }
                else
                {
                    ScrapeByBoard();
                }

                if (!ContinueRunning()) { Running = false; return; }
            }
            catch (Exception ex)
            {
                string msg = "Error RPQA76." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log(account.Username, this.GetType().ToString(), msg);
            }
            if (((IPinRepinConfiguration)Config).Queue != null)
                foreach (var v in Queue)
                    ((IPinRepinConfiguration)Config).Queue.Add(v);

        }
        private void ScrapeAllBoards()
        {
            //foreach (var kv in Managers)
            {
                Random r = new Random();
                var kv = Managers.ElementAt(r.Next(0, Managers.Count));

                IList<PinterestObject> Pins = new List<PinterestObject>();
                SessionManager = kv.Value;
                Board temp_board = kv.Key;

                IList<PinterestObject> scraped = SessionManager.Scrape("/" + temp_board.Url + "/");
                foreach (PinterestObject pin in scraped)
                {
                    if (!dupCheck.IsDuplicate(account.Email, ((Pin)pin).Image) && !InQueue.Contains(pin))
                        Pins.Add(pin);
                    if (Pins.Count >= CurrentCount.Max)
                        break;
                }
                while (scraped != null && Pins.Count < CurrentCount.Max)
                {
                    scraped = SessionManager.Scrape("/" + temp_board.Url + "/");
                    foreach (PinterestObject pin in scraped)
                    {
                        if (Pins.Count >= CurrentCount.Max)
                            break;
                        else
                        {
                            if (!dupCheck.IsDuplicate(account.Email, ((Pin)pin).Image) && !InQueue.Contains(pin))
                                Pins.Add(pin);
                        }
                    }
                }

                if (Aborted) { return; }

                for (int i = 0; i < Pins.Count; i++)
                {
                    if (!ContinueRunning()) { break; }

                    Pin pin = (Pin)Pins[i];
                    if (SessionManager.UsedPinterestObjects.Contains(pin))
                        continue;
                    if (dupCheck.IsDuplicate(account.Email, pin.Image))
                        continue;

                    if (Aborted) { return; }

                    //dupCheck.Add(account.Email, pin.Image);
                   /* var url = QueueHelper.GetRandomDescUrl((IPinRepinConfiguration)Config);
                    if (url != null && url != "")
                        pin.Description += " " + url;*/

                    Queue.Add(pin, temp_board);

                    ++CurrentCount.Min;
                }
            }
        }
        private void ScrapeByBoard()
        {
            IList<PinterestObject> Pins = null;
            //Random r = new Random();
            Queue = ((IPinRepinConfiguration)Config).Queue;
            if (Queue == null)
                Queue = new Dictionary<PinterestObject, Board>();

            //int index = r.Next(0, Managers.Count);
            SessionManager = Managers[Board];
            Pins = SessionManager.Scrape(Board.SearchQuery);



            if (!ContinueRunning()) { Running = false; return; }

            if (Pins == null) { Status = STATUS.LIMIT_REACHED; Running = false; return; }
            if (Pins.Count <= 0)
                return;

            for (int i = 0; i < Pins.Count; i++)
            {
                Pin pin = (Pin)Pins[i];
                if (SessionManager.UsedPinterestObjects.Contains(pin))
                    continue;
                if (dupCheck.IsDuplicate(account.Email, pin.Image) && !InQueue.Contains(pin))
                    continue;

                if (!ContinueRunning()) { Running = false; return; }

                //dupCheck.Add(account.Email, pin.Image);
                var url = QueueHelper.GetRandomDescUrl((IPinRepinConfiguration)Config);
                if (url != null && url != "")
                    pin.Description += " " + url;

                Queue.Add(pin, Board);

                ++CurrentCount.Min;
            }
        }
        protected override bool ContinueRunning()
        {
            if (CurrentCount.Min >= CurrentCount.Max)
            {
                Random r = new Random();
                CurrentCount = new Range<int>(0, r.Next(((IPinRepinConfiguration)Config).Scrape.Min, ((IPinRepinConfiguration)Config).Scrape.Max + 1));
                return false;
            }

            return true;
        }

        protected override Data GetData(PinterestObject P)
        { return new Data(); }
    }
}
