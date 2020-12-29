using System;
using PinBot2.Dal.Interface;
using PinBot2.Model;
using PinBot2.Presenter.Interface;
using PinBot2.Model.Configurations;
using PinBot2.Dal;
using System.Collections.Generic;
using PinBot2.Model.PinterestObjects;
using System.Linq;
using PinBot2.Common;

namespace PinBot2.Presenter.Configurations
{
    public class ConfigPresenter
    {
        private IAccount account;
        private readonly bool IS_PREMIUM;
        private readonly ISelectView _view;
        private readonly InMemoryAccountRepository _repository;

        /////////
        private readonly LikeConfigPresenter LikePresenter;
        private readonly FollowConfigPresenter FollowPresenter;
        private readonly UnfollowConfigPresenter UnfollowPresenter;
        private readonly RepinConfigPresenter RepinPresenter;
        private readonly InviteConfigPresenter InvitePresenter;
        private readonly PinConfigPresenter PinPresenter;
        private readonly CommentConfigPresenter CommentPresenter;
        //...

        public ICampaign SelectedCampaign { get { return _view.SelectedCampaign; } }

        public ConfigPresenter(InMemoryAccountRepository repository, ISelectView view, IConfigureView LikeView, IConfigureView FollowView, IConfigureView UnfollowView, IConfigureQueueView RepinView, IConfigureView InviteView, IConfigureQueueView PinView, IConfigureQueueScrapeView PinQueueView, IConfigureQueueScrapeView RepinQueueView, IConfigureView CommentView, bool IS_PREMIUM)
        {
            this.IS_PREMIUM = IS_PREMIUM;
            _view = view;
            _repository = repository;


            _view.SaveCampaign += SaveCampaign;
            _view.ReloadCampaigns += LoadCampaigns;
            _view.RemoveCampaign += RemoveCampaign;


            ///////
            _view.LikeConfig += LikeConfigShow;
            LikePresenter = new LikeConfigPresenter(_repository, LikeView, IS_PREMIUM);
            LikeView.SaveConfig += SaveCampaign;

            _view.FollowConfig += FollowConfigShow;
            FollowPresenter = new FollowConfigPresenter(_repository, FollowView, IS_PREMIUM);
            FollowView.SaveConfig += SaveCampaign;

            _view.UnfollowConfig += UnfollowConfigShow;
            UnfollowPresenter = new UnfollowConfigPresenter(_repository, UnfollowView, IS_PREMIUM);
            UnfollowView.SaveConfig += SaveCampaign;

            _view.RepinConfig += RepinConfigShow;
            RepinQueueView.SaveConfig += SaveCampaign;
            RepinPresenter = new RepinConfigPresenter(_repository, RepinView, RepinQueueView, IS_PREMIUM);
            RepinView.SaveConfig += SaveCampaign;

            _view.InviteConfig += InviteConfigShow;
            InvitePresenter = new InviteConfigPresenter(_repository, InviteView, IS_PREMIUM);
            InviteView.SaveConfig += SaveCampaign;

            _view.PinConfig += PinConfigShow;
            PinQueueView.SaveConfig += SaveCampaign;
            PinPresenter = new PinConfigPresenter(_repository, PinView, PinQueueView, IS_PREMIUM);
            PinView.SaveConfig += SaveCampaign;

            _view.CommentConfig += CommentConfigShow;
            CommentPresenter = new CommentConfigPresenter(_repository, CommentView, IS_PREMIUM);
            CommentView.SaveConfig += SaveCampaign;
            //...
        }

        public void ShowForm(IAccount account)
        {
            if (account == null)
                return; //no account/row selected

            this.account = account;
            LoadCampaigns(null, null);
            _view.ShowForm(account, IS_PREMIUM);            

            Console.WriteLine("");

            //int campaignID = _repository.SaveCampaign(_view.SelectedCampaign);
            //account.SelectedCampaignId = campaignID;
        }
        private void LoadCampaigns(object sender, EventArgs e)
        {
            try
            {
                _view.Campaigns = new List<ICampaign>(_repository.GetCampaigns(account.Id));
            }
            catch (Exception ex)
            {
                string msg = "Error CP91." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log("user", "CP", msg);
            }
        }


        //////////
        public void LikeConfigShow(object sender, EventArgs e)
        {
            try
            {
                if ((LikeConfiguration)_view.SelectedCampaign.ConfigurationContainer.LikeConfiguration == null)
                    _view.SelectedCampaign.ConfigurationContainer.LikeConfiguration = new LikeConfiguration();
                LikePresenter.ShowForm((LikeConfiguration)_view.SelectedCampaign.ConfigurationContainer.LikeConfiguration);
            }
            catch (Exception ex)
            {
                string msg = "Error CP109." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log("user", "CP", msg);
            }
        }
        public void FollowConfigShow(object sender, EventArgs e)
        {
            try
            {
                if ((FollowConfiguration)_view.SelectedCampaign.ConfigurationContainer.FollowConfiguration == null)
                    _view.SelectedCampaign.ConfigurationContainer.FollowConfiguration = new FollowConfiguration();
                FollowPresenter.ShowForm((FollowConfiguration)_view.SelectedCampaign.ConfigurationContainer.FollowConfiguration);
            }
            catch (Exception ex)
            {
                string msg = "Error CP122." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log("user", "CP", msg);
            }
        }
        public void UnfollowConfigShow(object sender, EventArgs e)
        {
            try
            {
                if ((UnfollowConfiguration)_view.SelectedCampaign.ConfigurationContainer.UnfollowConfiguration == null)
                    _view.SelectedCampaign.ConfigurationContainer.UnfollowConfiguration = new UnfollowConfiguration();
                UnfollowPresenter.ShowForm((UnfollowConfiguration)_view.SelectedCampaign.ConfigurationContainer.UnfollowConfiguration);
            }
            catch (Exception ex)
            {
                string msg = "Error CP135." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log("user", "CP", msg);
            }
        }
        public void RepinConfigShow(object sender, EventArgs e)
        {

            try
            {
                RepinConfiguration config = (RepinConfiguration)_view.SelectedCampaign.ConfigurationContainer.RepinConfiguration;

                if (config == null)
                    config = new RepinConfiguration();

                if (config.AllQueries == null)
                {
                    config.AllQueries = new Dictionary<Board, IDictionary<string, PinterestObjectResources>>();
                    if (account.Boards == null || account.Boards.Count == 0)
                    {
                        account.LoadBoards(new http());
                    }
                    foreach (Board b in account.Boards)
                    {
                        var kv = new KeyValuePair<Board, IDictionary<string, PinterestObjectResources>>(b, null);
                        config.AllQueries.Add(kv);
                    }
                }
                else if (config.AllQueries.Keys.Count < account.Boards.Count)
                {
                    //add all new boards
                    foreach (Board b in account.Boards)
                    {
                        if (!config.AllQueries.Any(x => x.Key.Id == b.Id))
                        {
                            var kv = new KeyValuePair<Board, IDictionary<string, PinterestObjectResources>>(b, null);
                            config.AllQueries.Add(kv);
                        }
                    }
                }


                //let's check if any boards with diff. name
                foreach (Board b in account.Boards)
                {
                    if (config.AllQueries.Any(x => x.Key.Id == b.Id && x.Key.Boardname != b.Boardname))
                    {
                        config.AllQueries.Remove(b);
                        var kv = new KeyValuePair<Board, IDictionary<string, PinterestObjectResources>>(b, null);
                        config.AllQueries.Add(kv);
                    }
                }



                _view.SelectedCampaign.ConfigurationContainer.RepinConfiguration = config;

                RepinPresenter.ShowForm((RepinConfiguration)_view.SelectedCampaign.ConfigurationContainer.RepinConfiguration, account);
            }
            catch (Exception ex)
            {
                string msg = "Error CP181." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log("user", "CP", msg);
            }

        }
        public void InviteConfigShow(object sender, EventArgs e)
        {
            try
            {
                InviteConfiguration config = (InviteConfiguration)_view.SelectedCampaign.ConfigurationContainer.InviteConfiguration;

                if (config == null)
                    config = new InviteConfiguration();

                if (config.SelectedBoards == null)
                {
                    config.SelectedBoards = new Dictionary<Board, bool>();
                    if (account.Boards == null || account.Boards.Count == 0)
                    {
                        account.LoadBoards(new http());
                    }
                    foreach (Board b in account.Boards)
                    {
                        var kv = new KeyValuePair<Board, bool>(b, false);
                        config.SelectedBoards.Add(kv);
                    }
                }
                else if (config.SelectedBoards.Keys.Count < account.Boards.Count)
                {
                    //add all new boards
                    foreach (Board b in account.Boards)
                    {
                        if (!config.SelectedBoards.Any(x => x.Key.Id == b.Id))
                        {
                            var kv = new KeyValuePair<Board, bool>(b, false);
                            config.SelectedBoards.Add(kv);
                        }
                    }
                }

                //let's check if any boards with diff. name
                foreach (Board b in account.Boards)
                {
                    if (config.SelectedBoards.Any(x => x.Key.Id == b.Id && x.Key.Boardname != b.Boardname))
                    {
                        config.SelectedBoards.Remove(b);
                        var kv = new KeyValuePair<Board, bool>(b, false);
                        config.SelectedBoards.Add(kv);
                    }
                }

                _view.SelectedCampaign.ConfigurationContainer.InviteConfiguration = config;

                InvitePresenter.ShowForm((InviteConfiguration)_view.SelectedCampaign.ConfigurationContainer.InviteConfiguration);
            }
            catch (Exception ex)
            {
                string msg = "Error CP226." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log("user", "CP", msg);
            }
        }
        public void PinConfigShow(object sender, EventArgs e)
        {
            try
            {
                PinConfiguration config = (PinConfiguration)_view.SelectedCampaign.ConfigurationContainer.PinConfiguration;

                if (config == null)
                    config = new PinConfiguration();

                if (config.AllQueries == null)
                {
                    config.AllQueries = new Dictionary<Board, IDictionary<string, PinterestObjectResources>>();
                    if (account.Boards == null || account.Boards.Count == 0)
                    {
                        account.LoadBoards(new http());
                    }
                    foreach (Board b in account.Boards)
                    {
                        var kv = new KeyValuePair<Board, IDictionary<string, PinterestObjectResources>>(b, null);
                        config.AllQueries.Add(kv);
                    }
                }
                else if (config.AllQueries.Keys.Count < account.Boards.Count)
                {
                    //add all new boards
                    foreach (Board b in account.Boards)
                    {
                        if (!config.AllQueries.Any(x => x.Key.Id == b.Id))
                        {
                            var kv = new KeyValuePair<Board, IDictionary<string, PinterestObjectResources>>(b, null);
                            config.AllQueries.Add(kv);
                        }
                    }
                }

                //let's check if any boards with diff. name
                foreach (Board b in account.Boards)
                {
                    if (config.AllQueries.Any(x => x.Key.Id == b.Id && x.Key.Boardname != b.Boardname))
                    {
                        config.AllQueries.Remove(b);
                        var kv = new KeyValuePair<Board, IDictionary<string, PinterestObjectResources>>(b, null);
                        config.AllQueries.Add(kv);
                    }
                }

                _view.SelectedCampaign.ConfigurationContainer.PinConfiguration = config;

                PinPresenter.ShowForm((PinConfiguration)_view.SelectedCampaign.ConfigurationContainer.PinConfiguration, account);
            }
            catch (Exception ex)
            {
                string msg = "Error CP270." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log("user", "CP", msg);
            }
        }
        public void CommentConfigShow(object sender, EventArgs e)
        {
            try
            {
                if ((CommentConfiguration)_view.SelectedCampaign.ConfigurationContainer.CommentConfiguration == null)
                    _view.SelectedCampaign.ConfigurationContainer.CommentConfiguration = new CommentConfiguration();
                CommentPresenter.ShowForm((CommentConfiguration)_view.SelectedCampaign.ConfigurationContainer.CommentConfiguration);
            }
            catch (Exception ex)
            {
                string msg = "Error CP288." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log("user", "CP", msg);
            }
        }
        //...


        public void SaveCampaign(object sender, EventArgs e)
        {

            _view.SelectedCampaign.ID = _repository.SaveCampaign((Campaign)_view.SelectedCampaign);
            
        }

        public void RemoveCampaign(object sender, EventArgs e)
        {
            _repository.DeleteCampaign((Campaign)_view.SelectedCampaign);
        }

    }
}