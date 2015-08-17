using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
            string jwKey = Program.settings.GetString(Settings.JW_KEY);
            textBoxJwKey.Text = jwKey;
            textBoxJwKey.TextChanged += textBoxJwKey_TextChanged;
            this.Focus();
        }

        private void textBoxJwKey_TextChanged(object sender, EventArgs e)
        {
            Program.settings.WriteValue(Settings.JW_KEY, ((TextBox)sender).Text);
        }
    }
}
