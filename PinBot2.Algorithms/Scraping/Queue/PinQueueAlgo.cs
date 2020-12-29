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
namespace PinBot2.Algorithms
{
    public class PinQueueAlgo : Algo
    {
        private DuplicateChecker dupCheck;
        private IDictionary<PinterestObject, Board> Queue;
        public IDictionary<PinterestObject, Board> ScrapedQueue { get { return Queue; } }
        public IList<PinterestObject> InQueue { get; set; }
        public Board Board { get; set; }

        private IDictionary<Board, ScrapeSessionManager> Managers;
        public PinQueueAlgo(IConfiguration config, IAccount account, IAccountRepository repository)
            : base(repository)
        {
            dupCheck = DuplicateChecker.init();

            if (Queue == null)
                Queue = new Dictionary<PinterestObject, Board>();

            this.account = account;
            Config = (PinConfiguration)config;
            Running = Config.Enabled;
            Random r = new Random();
            CurrentCount = new Range<int>(0, r.Next(((PinConfiguration)config).Scrape.Min, ((PinConfiguration)config).Scrape.Max + 1));

            Managers = new Dictionary<Board, ScrapeSessionManager>();
            foreach (var kv in ((PinConfiguration)Config).AllQueries)
            {
                PinConfiguration cnfg = (PinConfiguration)config;
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
                string msg = "Error PQA76." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log(account.Username, this.GetType().ToString(), msg);
            }
            if (((PinConfiguration)Config).Queue != null)
                foreach (var v in Queue)
                    ((PinConfiguration)Config).Queue.Add(v);
            
        }
        private void ScrapeAllBoards()
        {
            foreach (var kv in Managers)
            {
                IList<PinterestObject> Pins = new List<PinterestObject>();
                SessionManager = kv.Value;
                Board temp_board = kv.Key;

                IList<PinterestObject> scraped = SessionManager.Scrape("/" + temp_board.Url + "/");
                
                if (scraped == null)
                    continue; //end reached

                foreach (PinterestObject pin in scraped)
                {
                    if (!dupCheck.IsDuplicate(account.Email, ((ExternalPin)pin).ImageFound) && !InQueue.Contains(pin))
                        Pins.Add(pin);
                    if (Pins.Count >= CurrentCount.Max)
                        break;
                }
                while (scraped != null && Pins.Count < CurrentCount.Max)
                {
                    scraped = SessionManager.Scrape("/" + temp_board.Url + "/");
                    if (scraped == null)
                        break;
                    foreach (PinterestObject pin in scraped)
                    {
                        if (Pins.Count >= CurrentCount.Max)
                            break;
                        else
                        {
                            if (!dupCheck.IsDuplicate(account.Email, ((ExternalPin)pin).ImageFound) && !InQueue.Contains(pin))
                                Pins.Add(pin);
                        }
                    }
                }

                if (Aborted) { return; }

                for (int i = 0; i < Pins.Count; i++)
                {
                    if (!ContinueRunning()) { break; }

                    ExternalPin pin = (ExternalPin)Pins[i];
                    if (SessionManager.UsedPinterestObjects.Contains(pin))
                        continue;
                    if (dupCheck.IsDuplicate(account.Email, pin.ImageFound) && !InQueue.Contains(pin))
                        continue;

                    if (Aborted) { return; }

                    //dupCheck.Add(account.Email, pin.Image);
                    Queue.Add(pin, temp_board);

                    ++CurrentCount.Min;
                }
            }
        }
        private void ScrapeByBoard()
        {
            IList<PinterestObject> Pins = null;
            //Random r = new Random();
            Queue = ((PinConfiguration)Config).Queue;
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
                ExternalPin pin = (ExternalPin)Pins[i];

                if (SessionManager.UsedPinterestObjects.Contains(pin))
                    continue;
                if (dupCheck.IsDuplicate(account.Email, pin.ImageFound) && !InQueue.Contains(pin))
                    continue;

                if (!ContinueRunning()) { Running = false; return; }

                //dupCheck.Add(account.Email, pin.Image);
                Queue.Add(pin, Board);

                ++CurrentCount.Min;
            }
        }
        protected override bool ContinueRunning()
        {
            if (CurrentCount.Min >= CurrentCount.Max)
            {
                Random r = new Random();
                CurrentCount = new Range<int>(0, r.Next(((PinConfiguration)Config).Scrape.Min, ((PinConfiguration)Config).Scrape.Max + 1));
                return false;
            }

            return true;
        }

        protected override Data GetData(PinterestObject P)
        { return new Data(); }
    }
}
