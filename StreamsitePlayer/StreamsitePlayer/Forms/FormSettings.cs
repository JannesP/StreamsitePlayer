using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StreamsitePlayer.Forms
{
    public partial class FormSettings : Form
    {
        private Form parent;

        public FormSettings(Form parent)
        {
            this.parent = parent;
            InitializeComponent();
            
        }

        private void FormSettings_FormClosing(object sender, FormClosingEventArgs e)
        {
            parent.Enabled = true;
            parent.Focus();
        }

        private void FormSettings_Shown(object sender, EventArgs e)
        {
            parent.Enabled = false;
            textBoxJwKey.Text = Settings.GetString(Settings.JW_KEY);
            checkBoxAutoUpdate.Checked = Settings.GetBool(Settings.AUTOCHECK_FOR_UPDATES);
            this.Focus();
        }

        private void linkLabelJwKey_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Util.OpenLinkInDefaultBrowser("http://www.jwplayer.com/pricing/");
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            Settings.WriteValue(Settings.JW_KEY, textBoxJwKey.Text);
            Settings.WriteValue(Settings.AUTOCHECK_FOR_UPDATES, checkBoxAutoUpdate.Checked);
            Settings.SaveFileSettings();
            base.Close();
        }

        private void buttonOpenAppFolder_Click(object sender, EventArgs e)
        {
            Process.Start(Util.GetAppFolder());
        }

        private void buttonResetJwKey_Click(object sender, EventArgs e)
        {
            Settings.RestoreDefault(Settings.JW_KEY);
            textBoxJwKey.Text = Settings.GetString(Settings.JW_KEY);
        }
    }
}
