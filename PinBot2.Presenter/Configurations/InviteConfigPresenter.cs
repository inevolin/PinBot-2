﻿using System;
using PinBot2.Dal.Interface;
using PinBot2.Model;
using PinBot2.Presenter.Interface;
using PinBot2.Model.Configurations;

namespace PinBot2.Presenter.Configurations
{
    public class InviteConfigPresenter
    {
        private readonly bool IS_PREMIUM;
        private readonly IConfigureView _view;
        private readonly IAccountRepository _repository;

        public InviteConfigPresenter(IAccountRepository repository, IConfigureView view, bool IS_PREMIUM)
        {
            this.IS_PREMIUM = IS_PREMIUM;
            _view = view;
            _repository = repository;
        }

        public void ShowForm(InviteConfiguration con)
        {
            _view.ShowForm(con);
        }
    }
}