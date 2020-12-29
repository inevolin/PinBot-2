using System;
using PinBot2.Dal.Interface;
using PinBot2.Model;
using PinBot2.Presenter.Interface;
using System.Collections.Generic;
using PinBot2.Common;

namespace PinBot2.Presenter
{
    public class AddEditAccountPresenter
    {
        private readonly bool IS_PREMIUM;
        private readonly IAddEditAccountView _view;
        private readonly IAccountRepository _repository;

        public AddEditAccountPresenter(IAccountRepository repository, IAddEditAccountView view, bool IS_PREMIUM)
        {
            this.IS_PREMIUM = IS_PREMIUM;
            _view = view;
            _repository = repository;

            _view.SaveAccount += SaveAccount;

        }

        public void SaveAccount(object sender, PinBot2.Presenter.Helpers.CustomEventArgs e)
        {
            try
            {
                if (_view._Account == null)
                    return;

                if (_view._Account.Id == 0)
                {
                    IList<IAccount> list = _repository.GetAccounts(true);
                    foreach (IAccount a in list)
                    {
                        if (a.Email.Equals(_view._Account.Email, StringComparison.InvariantCultureIgnoreCase))
                        {
                            e.Var = false;
                            return;
                        }
                    }
                    _repository.AddAccount((Account)_view._Account);
                }
                else
                    _repository.SaveAccount((Account)_view._Account);

            }
            catch (Exception ex)
            {
                string msg = "Error AECP50." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log("user", "AECP", msg);
            }
        }


        public void ShowForm(IAccount account)
        {
            _view.ShowForm(account, IS_PREMIUM);
        }
    }
}