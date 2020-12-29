using System;
using PinBot2.Model;

namespace PinBot2.Presenter.Interface
{
  public interface IAddEditAccountView
  {
    event PinBot2.Presenter.Helpers.CustomEventHandler SaveAccount;

    void ShowForm(IAccount account, bool IS_PREMIUM);

    IAccount _Account { get; set; }
  }
}