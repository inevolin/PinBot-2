using PinBot2.Presenter.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinBot2.Presenter
{
    public class TrialPresenter
    {
        public bool MayContinue { get { return _view.MayContinue; } }
        

        private ITrialView _view;
        public TrialPresenter(ITrialView view)
        {
            _view = view;
        }

        public void ShowForm()
        {
            _view.ShowForm();
        }
    }
}
