using System;
using PinBot2.Dal.Interface;
using PinBot2.Model;
using PinBot2.Presenter.Interface;
using PinBot2.Model.Configurations;
using PinBot2.Common;

namespace PinBot2.Presenter.Configurations
{
    public class RepinConfigPresenter
    {
        private readonly bool IS_PREMIUM;
        private readonly IConfigureQueueView _view;
        private readonly IAccountRepository _repository;
        private RepinQueueConfigPresenter _queuePresenter;
        private IRepinConfiguration config;
        private IConfigureQueueScrapeView queue_view;

        public RepinConfigPresenter(IAccountRepository repository, IConfigureQueueView view, IConfigureQueueScrapeView queue_view, bool IS_PREMIUM)
        {
            this.queue_view = queue_view;
            this.IS_PREMIUM = IS_PREMIUM;
            _view = view;
            _repository = repository;

            _view.Queue += Queue;
            _view.SaveConfig += SaveConfig;
        }

        public void ShowForm(IRepinConfiguration con, IAccount account)
        {
            try
            {
                config = con;
                _queuePresenter = new RepinQueueConfigPresenter(account, _repository, queue_view, IS_PREMIUM);

                _view.ShowForm(con);

            }
            catch (Exception ex)
            {
                string msg = "Error RCP39." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log("user", "RCP", msg);
            }
        }

        private void SaveConfig(object sender, EventArgs e)
        {
            this.config = (IRepinConfiguration)_view._config;
        }

        private void Queue(object sender, EventArgs e)
        {
            _queuePresenter.ShowForm(config);
        }
    }
}