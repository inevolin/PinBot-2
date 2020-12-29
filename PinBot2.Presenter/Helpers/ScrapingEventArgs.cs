using PinBot2.Model.Configurations;
using PinBot2.Model.PinterestObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinBot2
{

    public class ScrapingEventArgs : EventArgs
    {
        public Board SelectedBoard { get; set; } //if null then for each board!
        public IList<PinterestObject> InQueue { get; set; }
    }
}
