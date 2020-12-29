using System.Collections.Generic;
using PinBot2.Model;
using PinBot2.Model.Configurations;

namespace PinBot2.Dal.Interface
{
  public interface IAccountRepository
  {
    IAccount GetAccount(int id);
    IList<IAccount> GetAccounts(bool FromDatabase);
    void SaveAccount(IAccount account);
    void AddAccount(IAccount account);
    void DeleteAccount(int id);

    IList<ICampaign> GetCampaigns(int accountId);
    ICampaign GetCampaign(int Id);
    int SaveCampaign(ICampaign campaign);
    void DeleteCampaign(ICampaign campaign);
  }
}