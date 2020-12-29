using PinBot2.Model;
using PinBot2.Model.Configurations;
using PinBot2.Model.PinterestObjects;
using PinBot2.Presenter.Configurations.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinBot2.Presenter.Helper
{
    public delegate void SpecialFeaturesEventHandler(object sender, SpecialFeaturesEventArgs e);
    public class SpecialFeaturesEventArgs : EventArgs
    {
        public ISpecialFeaturesView Var { get; set; }

        public SpecialFeaturesEventArgs(ISpecialFeaturesView var)
        {
            this.Var = var;
        }
    }
}
