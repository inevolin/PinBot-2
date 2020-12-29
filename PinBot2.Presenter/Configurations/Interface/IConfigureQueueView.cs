using PinBot2.Model.Configurations;
using System;
namespace PinBot2.Presenter.Configurations
{
    public interface IConfigureQueueView
    {
        event EventHandler SaveConfig;
        event EventHandler Queue;
        IPinRepinConfiguration _config { get; set; }
        void ShowForm(IConfiguration config);
    }
}
