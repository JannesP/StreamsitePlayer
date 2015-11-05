using SeriesPlayer.Streamsites;
using SeriesPlayer.Streamsites.Providers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SeriesPlayer.Utility.Extensions;

namespace SeriesPlayer.Forms
{
    public partial class FormAddNewSeries : Form
    {
        private FormMain parent;
        private bool addBySearch = false;
        private Dictionary<string, string> searchDictionary;

        public FormAddNewSeries(FormMain parent)
        {
            InitializeComponent();
            this.parent = parent;
        }

        private void FormAddNewSeries_Load(object sender, EventArgs e)
        {
            comboBoxStreamingProvider.Items.AddRange(StreamProvider.VALID_PROVIDERS.ToArray());
            parent.Enabled = false;
        }

        private void OpenSeries(string linkExtension)
        {
            var currentProvider = StreamProvider.Create((string)comboBoxStreamingProvider.SelectedItem);
            if (currentProvider == null) return;
            FormLoadingIndicator.ShowDialog(this, "Loading series. This usually shouldn't take any longer then 30 seconds.");
            int res = currentProvider.LoadSeries(linkExtension, comboBoxStreamingProvider);
            FormLoadingIndicator.CloseDialog();
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
            parent.SelectSeries(null);
            base.Close();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            string seriesExtension = "";
            if (addBySearch)
            {
                seriesExtension = searchDictionary[textBoxAddBySearch.Text];
            }
            else
            {
                seriesExtension = textBoxAddByExtension.Text;
            }
            OpenSeries(seriesExtension);
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

        private void comboBoxStreamingProvider_SelectedIndexChanged(object sender, EventArgs e)
        {
            var currentProvider = StreamProvider.Create((string)comboBoxStreamingProvider.SelectedItem);
            if (currentProvider.IsSearchSupported())
            {
                addBySearch = true;
                backgroundWorkerLoadAutoComplete.RunWorkerAsync(currentProvider);
                FormLoadingIndicator.ShowDialog(this, "Loading autocomplete list, please be patient ...");
            }
            else
            {
                addBySearch = false;
            }
            buttonAdd.Enabled = !addBySearch;
            panelAddByExtension.Visible = !addBySearch;
            panelAddBySearch.Visible = addBySearch;
        }

        private void textBoxAddBySearch_TextChanged(object sender, EventArgs e)
        {
            bool foundSeriesWithName = searchDictionary.ContainsKey(((TextBox)sender).Text);
            buttonAdd.Enabled = foundSeriesWithName;
        }

        private void backgroundWorkerLoadAutoComplete_DoWork(object sender, DoWorkEventArgs e)
        {
            searchDictionary = ((StreamProvider)e.Argument).GetSearchIndex();
            e.Result = searchDictionary;
        }

        private void backgroundWorkerLoadAutoComplete_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            searchDictionary = (Dictionary<string, string>)e.Result;
            textBoxAddBySearch.AutoCompleteMode = AutoCompleteMode.Suggest;
            textBoxAddBySearch.AutoCompleteSource = AutoCompleteSource.CustomSource;
            textBoxAddBySearch.AutoCompleteCustomSource = searchDictionary.Keys.ToAutoCompleteStringCollection();
            FormLoadingIndicator.CloseDialog();
        }
    }
}
