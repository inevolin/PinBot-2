using PinBot2.Model.Configurations;
using System;

namespace PinBot2.Presenter.Configurations
{
    public interface IConfigureQueueScrapeView
    {
        event EventHandler SaveConfig;
        event EventHandler<ScrapingEventArgs> Scrape;
        void ShowForm(IConfiguration config);
        void SetMessage(string msg, string color);
    }
}
