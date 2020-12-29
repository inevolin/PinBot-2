using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PinBot2.Helpers
{
    public static class StatusLabel
    {
        public static void SetStatus(ToolStripStatusLabel label, bool visible, string s = "", Color? g = null)
        {
            if (g.HasValue) label.ForeColor = g.Value;
            label.Text = s;
            label.Visible = visible;
        }
    }
}
