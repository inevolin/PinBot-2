using System;
using PinBot2.Dal.Interface;
using PinBot2.Presenter.Interface;
using PinBot2.Model.Configurations;
using System.Threading.Tasks;
using PinBot2.Algorithms;
using PinBot2.Model.PinterestObjects;
using System.Collections.Generic;
using PinBot2.Model;
using PinBot2.Common;

namespace PinBot2.Presenter.Configurations
{
    public class PinQueueConfigPresenter
    {
        private readonly bool IS_PREMIUM;
        private readonly IConfigureQueueScrapeView _view;
        private readonly IAccountRepository _repository;
        private IAccount account;
        private IPinConfiguration config;
        private PinQueueAlgo ScrapeAlgo;

        public PinQueueConfigPresenter(IAccount account, IAccountRepository repository, IConfigureQueueScrapeView view, bool IS_PREMIUM)
        {
            this.account = account;
            this.IS_PREMIUM = IS_PREMIUM;
            _view = view;
            _repository = repository;

            _view.Scrape += Scrape;
        }

        public void ShowForm(IPinConfiguration con)
        {
            this.config = con;

            _view.ShowForm(config);
        }

        private void Scrape(object sender, EventArgs e)
        {
            try
            {
                ScrapeAlgo = new PinQueueAlgo(config, account, _repository);

                ScrapingEventArgs ee = (ScrapingEventArgs)e;

                ScrapeAlgo.Board = ee.SelectedBoard;
                ScrapeAlgo.InQueue = ee.InQueue;

                Task t = Task.Factory.StartNew(() =>
                {
                    ScrapeAlgo.Run();
                });

                t.Wait();
                foreach (var kv in ScrapeAlgo.ScrapedQueue)
                {
                    if (config.Queue == null)
                        config.Queue = new Dictionary<PinterestObject, Board>();

                    if (!config.Queue.ContainsKey(kv.Key))
                        config.Queue.Add(kv);
                }

            }
            catch (Exception ex)
            {
                string msg = "Error PQCP66." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log("user", "PQCP", msg);
            }
        }
    }
}