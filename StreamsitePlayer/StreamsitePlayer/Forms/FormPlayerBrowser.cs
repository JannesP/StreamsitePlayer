using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StreamsitePlayer
{
    public partial class FormPlayerBrowser : Form
    {
        Form parent;
        public FormPlayerBrowser(Form parent)
        {
            this.parent = parent;
            InitializeComponent();
            webBrowserPlayer.Visible = false;
        }

        public WebBrowser GetBrowser()
        {
            return webBrowserPlayer;
        }

        private void FormPlayerBrowser_Shown(object sender, EventArgs e)
        {
            parent.Enabled = false;
            this.Activate();
        }

        private void FormPlayerBrowser_FormClosing(object sender, FormClosingEventArgs e)
        {
            parent.Enabled = true;
            parent.Activate();
        }
    }
}
