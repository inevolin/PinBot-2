using System;
using PinBot2.Dal.Interface;
using PinBot2.Model;
using PinBot2.Presenter.Interface;
using PinBot2.Presenter.Configurations;
using PinBot2.Algorithms;
using System.Threading.Tasks;
using System.Collections.Generic;
using PinBot2.Common;
using System.Threading;
using PinBot2.Presenter.Helper;
using PinBot2.Presenter.Configurations.Interface;

namespace PinBot2.Presenter
{
    public class AccountPresenter
    {
        private readonly bool PREMIUM;
        private readonly IAccountView _view;
        private readonly IAccountRepository _repository;
        private readonly AddEditAccountPresenter _addEditAccountPresenter;
        private readonly ConfigPresenter _configPresenter;


        public AccountPresenter(
            IAccountView view,
            IAccountRepository repository,
            AddEditAccountPresenter addEditAccountPresenter,
            ConfigPresenter configPresenter,
            bool PREMIUM)
        {


            _view = view;
            _repository = repository;
            _addEditAccountPresenter = addEditAccountPresenter;
            _configPresenter = configPresenter;
            this.PREMIUM = PREMIUM;

            _view.AddAccount += AddAccount;
            _view.EditAccount += EditAccount;
            _view.DeleteAccount += DeleteAccount;
            _view.Run += Run;
            _view.RunAll += RunAll;
            _view.StopAll += StopAll;
            _view.Stop += Stop;
            //_view.SaveAcounts += SaveAcounts;
            _view.Configure += Configure;
            _view.RefreshAccountInformation += RefreshAccountInformation;
            _view.ScrapeUsersExportShow += ScrapeUsersExportShow;

            LoadAccounts(true);
        }

        public void ShowForm()
        {
            _view.ShowForm();
        }

        public void LoadAccounts(bool FromDatabase = false)
        {
            try
            {
                IList<IAccount> accounts = _repository.GetAccounts(FromDatabase);

                Mapper m = Mapper.Instance();
                Mapper.repository = _repository;
                m.UpdateDGV += UpdateDGV;

                foreach (IAccount a in accounts)
                {
                    if (!PREMIUM)
                        a.WebProxy = null;

                    a.CheckStatus();
                    a.IsLoggedIn = false;                    
                    m.RegisterUser(a);


                    /*
                   var test = Mapper.repository.GetCampaign(a.SelectedCampaignId);
                   test.ConfigurationContainer.CommentConfiguration.Autopilot =
                       test.ConfigurationContainer.FollowConfiguration.Autopilot =
                       test.ConfigurationContainer.InviteConfiguration.Autopilot =
                       test.ConfigurationContainer.LikeConfiguration.Autopilot =
                       test.ConfigurationContainer.PinConfiguration.Autopilot =
                       test.ConfigurationContainer.RepinConfiguration.Autopilot =
                       test.ConfigurationContainer.UnfollowConfiguration.Autopilot = false;
                   
                   test.ConfigurationContainer.CommentConfiguration.AutoStart =
                       test.ConfigurationContainer.FollowConfiguration.AutoStart =
                       test.ConfigurationContainer.InviteConfiguration.AutoStart =
                       test.ConfigurationContainer.LikeConfiguration.AutoStart =
                       test.ConfigurationContainer.PinConfiguration.AutoStart =
                       test.ConfigurationContainer.RepinConfiguration.AutoStart =
                       test.ConfigurationContainer.UnfollowConfiguration.AutoStart = new Range<DateTime>(DateTime.Now, DateTime.Now.AddMinutes(60));
                   
                    Mapper.repository.SaveCampaign(test);
                     */


                }

                _view.LoadAccounts(accounts);

            }
            catch (Exception ex)
            {
                string msg = "Error AP77." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log("user or auto", "AP", msg);
            }
        }

        public void AddAccount(object sender, EventArgs e)
        {
            try
            {
                _addEditAccountPresenter.ShowForm(new Account());
                LoadAccounts();
            }
            catch (Exception ex)
            {
                string msg = "Error AP92." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log("user or auto", "AP", msg);
            }
        }

        public void EditAccount(object sender, EventArgs e)
        {
            try
            {
                _addEditAccountPresenter.ShowForm(_view.GetAccount());
                LoadAccounts();
            }
            catch (Exception ex)
            {
                string msg = "Error AP98." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log("user or auto", "AP", msg);
            }
        }

        public void DeleteAccount(object sender, EventArgs e)
        {
            try
            {
                bool removedAny = false;
                foreach (var account in _view.GetSelectedAccounts())
                {
                    _repository.DeleteAccount(account.Id);
                    removedAny = true;
                }

                if (removedAny)
                    LoadAccounts();

            }
            catch (Exception ex)
            {
                string msg = "Error AP112." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log("user or auto", "AP", msg);
            }
        }

        public void Run(object sender, EventArgs e)
        {
            Logging.Log("user", "account action", "clicked run");
            try
            {
                foreach (var account in _view.GetSelectedAccounts())
                {
                    if (!_view.IsAlreadyStarted(account))
                        StartAccount((IAccount)account);
                }
            }
            catch (Exception ex)
            {
                string msg = "Error AP120." + Environment.NewLine + ex.Message;
                
                Logging.Log("user", "account action", msg);
            }
        }

        private void UpdateDGV(object sender, EventArgs e)
        {
            _view.UpdateDGV();
        }

        public void RunAll(object sender, EventArgs e)
        {
            Logging.Log("user", "account action", "clicked run all");
            try
            {
                foreach (var account in _view.GetAllAccounts())
                {
                    if (!_view.IsAlreadyStarted(account))
                        StartAccount((IAccount)account);

                    var r = new Random();
                    Thread.Sleep(r.Next(50, 100));
                }
            }
            catch (Exception ex)
            {
                string msg = "Error AP128." + Environment.NewLine + ex.Message;
                
                Logging.Log("user", "account action", msg);
            }
        }

        private void StartAccount(IAccount account)
        {
            try
            {
                if (!(account.ValidCredentials && account.IsConfigured && account.ValidProxy))
                    return;

                //TaskCreationOptions.LongRunning;
                Task.Factory.StartNew(() =>
                {
                    Mapper m = Mapper.Instance();
                    Mapper.repository = _repository;
                    _view.AccountStarted(account);
                    m.StartAllAlgos(account); //throws exception here
                },CancellationToken.None, TaskCreationOptions.LongRunning,TaskScheduler.Default);
            }
            catch (Exception ex)
            {
                string msg = "Error AP142." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log("user or auto", "AP", msg);
            }
        }

        public void StopAll(object sender, EventArgs e)
        {
            Logging.Log("user", "account action", "clicked stop all");
            try
            {
                _view.AbortAllAccounts();
                Mapper m = Mapper.Instance();
                Mapper.repository = _repository;
                m.AbortAll();
            }
            catch (Exception ex)
            {
                string msg = "Error AP149." + Environment.NewLine + ex.Message;
                
                Logging.Log("user", "account action", msg);
            }
        }
        public void Stop(object sender, EventArgs e)
        {
            Logging.Log("user", "account action", "clicked stop");
            try
            {
                foreach (var account in _view.GetSelectedAccounts())
                {
                    _view.AccountStopped(account);
                    Mapper m = Mapper.Instance();
                    Mapper.repository = _repository;
                    m.AbortSelected(account);
                }
            }
            catch (Exception ex)
            {
                string msg = "Error AP158." + Environment.NewLine + ex.Message;
                
                Logging.Log("user", "account action", msg);
            }
        }

        public void Configure(object sender, EventArgs e)
        {
            try
            {
                var a = _view.GetAccount();
                _configPresenter.ShowForm(a);
                _repository.SaveAccount((Account)a);
                LoadAccounts();
            }
            catch (Exception ex)
            {
                string msg = "Error AP164." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log("user or auto", "AP", msg);
            }
        }

        public void SaveAcounts(object sender, EventArgs e)
        {
            try
            {
                int i = DateTime.Now.Second % 10;
                foreach (var account in _view.GetAllAccounts())
                {
                    if (i == 0) //TimerTick every second, every 10 seconds SaveAccount
                        _repository.SaveAccount((Account)account);
                }
            }
            catch (Exception ex)
            {
                string msg = "Error AP175." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log("user or auto", "AP", msg);
            }
        }

        public void RefreshAccountInformation(object sender, EventArgs e)
        {
            try
            {
                foreach (var account in _view.GetSelectedAccounts())
                {
                    ((IAccount)account).RefreshAccountInformation();
                }
            }
            catch (Exception ex)
            {
                string msg = "Error AP183." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log("user or auto", "AP", msg);
            }
        }

        public void ScrapeUsersExportShow(object sender, SpecialFeaturesEventArgs e)
        {
            var view = (ISpecialFeaturesView)e.Var;
            view.ShowForm(_view.GetAccount());
        }
    }
}