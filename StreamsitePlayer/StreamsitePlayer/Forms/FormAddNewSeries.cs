using StreamsitePlayer.Streamsites;
using StreamsitePlayer.Streamsites.Providers;
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
    public partial class FormAddNewSeries : Form
    {
        private List<string> streamProviders;
        private FormMain parent;

        public FormAddNewSeries(FormMain parent)
        {
            InitializeComponent();
            this.parent = parent;
        }

        private void FormAddNewSeries_Load(object sender, EventArgs e)
        {
            InitStreamingProviders();
            parent.Enabled = false;
        }

        private void InitStreamingProviders()
        {
            Logger.Log("START", "Adding streaming providers.");
            streamProviders = new List<string>();
            streamProviders.Add(BsToStreamProvider.NAME);
            Logger.Log("START", "Added " + BsToStreamProvider.NAME);
            streamProviders.Add(RyuanimeStreamProvider.NAME);
            Logger.Log("START", "Added " + RyuanimeStreamProvider.NAME);
            streamProviders.Add(DubbedanimehdNetProvider.NAME);
            Logger.Log("START", "Added " + DubbedanimehdNetProvider.NAME);
#if DEBUG
            streamProviders.Add(TestProvider.NAME);
            Logger.Log("START", "Added " + TestProvider.NAME);
#endif
            comboBoxStreamingProvider.Items.Clear();
            Logger.Log("START", "Filling combobox with providers.");
            comboBoxStreamingProvider.Items.AddRange(streamProviders.ToArray());
            comboBoxStreamingProvider.SelectedIndex = 0;
        }

        private void OpenSeries(string linkExtension)
        {
            var currentProvider = StreamProvider.Create((string)comboBoxStreamingProvider.SelectedItem);
            if (currentProvider == null) return;
            this.Enabled = false;
            string oldName = this.Text;
            this.Text = "Working, please be patient ...";
            int res = currentProvider.LoadSeries(linkExtension, comboBoxStreamingProvider);
            this.Text = oldName;
            this.Enabled = true;
            if (res == StreamProvider.RESULT_OK || res == StreamProvider.RESULT_USE_CACHED)
            {
                Settings.WriteValue(Settings.LAST_PLAYED_SERIES, currentProvider.GetSeries().LinkExtension);
                parent.LoadCachedSeries();
                parent.SelectSeries(currentProvider.GetSeries());
            }
            base.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            OpenSeries(textBoxLinkExtension.Text);
        }

        private void FormAddNewSeries_FormClosed(object sender, FormClosedEventArgs e)
        {
            parent.Enabled = true;
            parent.Focus();
        }

        private void buttonOpenProviderSite_Click(object sender, EventArgs e)
        {
            Util.OpenLinkInDefaultBrowser(StreamProvider.Create((string)comboBoxStreamingProvider.SelectedItem).GetWebsiteLink());
        }
    }
}
