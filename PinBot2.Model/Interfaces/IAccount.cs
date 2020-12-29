using PinBot2.Common;
using PinBot2.Model.PinterestObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PinBot2.Model
{
    public interface IAccount
    {
        PinBot2.Model.AccountInformation AccountInfo { get; set; }


        http Request { get; }

        int Id { get; set; }
        string Email { get; set; }
        string Password { get; set; }
        PinBot2.Proxy WebProxy { get; set; }
        CookieContainer CookieContainer { get; set; }
        string CsrfToken { get; set; }
        string AppVersion { get; set; }
        PinBot2.Model.Account.STATUS Status { get; set; }
        int SelectedCampaignId { get; set; }
        string Username { get; set; }
        string UsernameSlug { get; set; }
        bool IsConfigured { get; set; }
        bool ValidCredentials { get; set; }
        bool ValidProxy { get; set; }
        bool IsLoggedIn { get; set; }
        HashSet<Board> Boards { get; set; }
        DateTime LastLogin { get; set; }

        string getStatus { get; }
        void LoginSync(bool FullLogin, http request = null);
        void LoadBoards(http request);
       // void GetCsrfToken();
        void CheckStatus();
        void RefreshAccountInformation();

    }
}
