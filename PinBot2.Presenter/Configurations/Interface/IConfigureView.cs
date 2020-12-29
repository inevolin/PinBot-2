using PinBot2.Model.Configurations;
using System;
namespace PinBot2.Presenter.Configurations
{
    public interface IConfigureView
    {
        event EventHandler SaveConfig;
        void ShowForm(IConfiguration config);
    }
}
