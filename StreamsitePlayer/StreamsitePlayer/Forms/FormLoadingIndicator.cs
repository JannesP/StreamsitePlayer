using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StreamsitePlayer.Forms
{
    public partial class FormLoadingIndicator : Form
    {
        private static FormLoadingIndicator currentForm;
        private static bool closeForm;
        private Form parent;

        public static void ShowDialog(Form parent, string message)
        {
            FormLoadingIndicator form = new FormLoadingIndicator(parent, message);
            FormLoadingIndicator.currentForm = form;
            form.Show();
        }

        public static void CloseDialog()
        {
            if (currentForm != null)
            {
                closeForm = true;
                currentForm.Close();
                currentForm = null;
            }
        }

        private const int CP_NOCLOSE_BUTTON = 0x200;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
                return myCp;
            }
        }

        private FormLoadingIndicator(Form parent, string message)
        {
            InitializeComponent();
            labelMessage.Text = message;
            this.parent = parent;
        }

        private void FormLoadingIndicator_Shown(object sender, EventArgs e)
        {
            parent.Enabled = false;
            Refresh();
        }

        private void FormLoadingIndicator_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && !closeForm)
            {
                e.Cancel = true;
            }
            else
            {
                parent.Enabled = true;
                closeForm = false;
            }
        }
    }
}
