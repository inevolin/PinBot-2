using System;
using System.Collections.Generic;
using PinBot2.Model;
using PinBot2.Presenter.Helper;

namespace PinBot2.Presenter.Interface
{
    public interface IAccountView
    {
        event EventHandler AddAccount;
        event EventHandler EditAccount;
        event EventHandler DeleteAccount;
        event EventHandler Run, RunAll;
        //event EventHandler SaveAcounts;
        event EventHandler Configure;
        event EventHandler Stop, StopAll;
        event EventHandler RefreshAccountInformation;
        event SpecialFeaturesEventHandler ScrapeUsersExportShow;

        void LoadAccounts(IList<IAccount> accounts = null);
        IAccount GetAccount();
        IEnumerable<IAccount> GetSelectedAccounts();
        IEnumerable<IAccount> GetAllAccounts();
        void ShowForm();
        void UpdateDGV();
        bool IsAlreadyStarted(IAccount a);
        void AccountStarted(IAccount a);
        void AccountStopped(IAccount a);
        void AbortAllAccounts();
    }
}