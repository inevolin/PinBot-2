using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinBot2.Presenter.Helpers
{
    public delegate void CustomEventHandler(object sender, CustomEventArgs e);
    public class CustomEventArgs : EventArgs
    {
        public bool Var { get; set; }

        public CustomEventArgs(bool var)
        {
            this.Var = var;
        }
    }

}
