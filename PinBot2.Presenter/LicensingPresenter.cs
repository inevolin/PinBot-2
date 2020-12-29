using PinBot2.Presenter.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinBot2.Presenter
{
    public class LicensingPresenter
    {
        public bool IS_PREMIUM { get { return _view.IS_PREMIUM; } }
        public bool MayContinue { get { return _view.MayContinue; } }

        private ILicensingView _view;
        public LicensingPresenter(ILicensingView view)
        {
            _view = view;
        }

        public void ShowForm()
        {
            _view.ShowForm();
        }
    }
}
