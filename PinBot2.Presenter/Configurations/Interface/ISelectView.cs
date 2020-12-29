using PinBot2.Model;
using PinBot2.Model.Configurations;
using System;
using System.Collections.Generic;
namespace PinBot2.Presenter.Configurations
{
    public interface ISelectView
    {
        //////
        event EventHandler LikeConfig;
        event EventHandler FollowConfig;
        event EventHandler UnfollowConfig;
        event EventHandler RepinConfig;
        event EventHandler InviteConfig;
        event EventHandler PinConfig;
        event EventHandler CommentConfig;
        //...

        event EventHandler SaveCampaign;
        event EventHandler RemoveCampaign;
        event EventHandler ReloadCampaigns;
        ICampaign SelectedCampaign {get;}
        IList<ICampaign> Campaigns { get; set; }
        void ShowForm(IAccount account,  bool premium);
    }
}