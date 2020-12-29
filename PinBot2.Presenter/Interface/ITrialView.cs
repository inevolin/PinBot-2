using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinBot2.Presenter.Interface
{
    public interface ITrialView
    {
        void ShowForm();

        bool MayContinue { get; }
    }
}
