using PinBot2.Presenter.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinBot2.Presenter
{
    public class SplashPresenter
    {
        public bool IsValid { get { return _view.IsValid; } }
        public bool MayContinue { get { return _view.MayContinue; } }
        public bool IsTrialMember { get { return _view.IsTrialMember; } }

        private ISplashView _view;
        public SplashPresenter(ISplashView view)
        {
            _view = view;
        }

        public void ShowForm()
        {
            _view.ShowForm();
        }
    }
}
