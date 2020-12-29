using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinBot2.Presenter.Interface
{
    public interface ISplashView
    {
        void ShowForm();
        bool IsValid { get; }
        bool MayContinue { get; }
        bool IsTrialMember { get; }
    }
}
