using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PinBot2.Model.Configurations;
using PinBot2.Model.PinterestObjects;
using PinBot2.Configurations.PinForms;
using PinBot2.Common;

namespace PinBot2.UserControls
{
    public partial class QueueItem : UserControl
    {
        public string UID { get { return imageUrl; } }
        private string imageUrl, description, source_url;
        private bool fullImage = false;
        private IPinRepinConfiguration _config;
        private PinterestObject obj;
        private QueueForm _parent;

        public QueueItem(IConfiguration config, Board board, PinterestObject pinObject, QueueForm parent)
        {
            try
            {
                _parent = parent;
                this.obj = pinObject;
                InitializeComponent();

                _config = (IPinRepinConfiguration)config;

                cboBoard.Items.Clear();
                if (_config.AllQueries != null)
                {
                    foreach (Board b in _config.AllQueries.Keys)
                    {
                        cboBoard.Items.Add(b);
                    }
                    cboBoard.DisplayMember = "Boardname";
                    cboBoard.ValueMember = "Id";
                }
                cboBoard.SelectedItem = board;

                if (pinObject.GetType() == typeof(ExternalPin))
                {
                    ExternalPin pin = (ExternalPin)pinObject;
                    imageUrl = pin.ImageFound;
                    description = pin.Description;
                    source_url = pin.Link;
                }
                else if (pinObject.GetType() == typeof(Pin))
                {
                    Pin pin = (Pin)pinObject;
                    imageUrl = pin.Image;
                    description = pin.Description;
                    source_url = pin.Link;
                }

                pic.ImageLocation = imageUrl;
                txtDesc.Text = description;
                txtLink.Text = source_url == null ? "" : source_url;

            }
            catch (Exception ex)
            {
                string msg = "Error QI68." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log("user", "account action: QueueItem", msg);
                
            }
        }

        public void ChangeSizeMode(int max_w, int max_h)
        {
            System.Drawing.Size size = new System.Drawing.Size();
            size.Width = max_w;
            size.Height = max_h;
            pic.MaximumSize = size;
            pic.Size = size;

            if (fullImage)
            {
                pic.SizeMode = PictureBoxSizeMode.AutoSize;
            }
            else
            {
                pic.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                _config.Queue.Remove(obj);
                _parent.DeleteQueueItem(this);
            }
            catch (Exception ex)
            {
                string msg = "Error QI100." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log("user", "account action: QueueItem", msg);
                
            }
        }

        public PinterestObject GetPinterestObject
        {
            get
            {
                if (obj.GetType() == typeof(ExternalPin))
                {
                    var f = (ExternalPin)obj;
                    f.Description = txtDesc.Text.Trim();
                    f.Link = txtLink.Text.Trim();
                    obj = f;
                }
                else if (obj.GetType() == typeof(Pin))
                {
                    var f = (Pin)obj;
                    f.Description = txtDesc.Text.Trim();
                    f.Link = txtLink.Text.Trim();
                    obj = f;
                }
                return obj;
            }
        }
        public Board GetBoard { get { return (Board)cboBoard.SelectedItem; } }
    }
}
