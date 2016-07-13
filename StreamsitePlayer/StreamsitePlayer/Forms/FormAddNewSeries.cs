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
using System.Threading;

namespace SeriesPlayer.Forms
{
    public partial class FormAddNewSeries : Form
    {
        private const string TAG = "FormAddNewSeries";

        private FormMain parent;

        private CancellationTokenSource _currentCancellationTokenSource = null;
        private CancellationTokenSource CurrentCancellationTokenSource
        {
            get
            {
                if (_currentCancellationTokenSource == null)
                {
                    _currentCancellationTokenSource = new CancellationTokenSource();
                }
                return _currentCancellationTokenSource;
            }
            set
            {
                if (_currentCancellationTokenSource != null && !_currentCancellationTokenSource.IsCancellationRequested)
                {
                    _currentCancellationTokenSource.Cancel();
                }
                _currentCancellationTokenSource = value;
            }
        }

        private Dictionary<string, string> _searchDictionary = null;
        private Dictionary<string, string> SearchDictionary {
            get
            {
                if (_searchDictionary == null) _searchDictionary = new Dictionary<string, string>();
                return _searchDictionary;
            }
            set
            {
                _searchDictionary = value;
                textBoxSeries.FuzzyAutoCompleteSource = SearchDictionary.Keys.ToList();
            }
        }
        private Dictionary<string, Dictionary<string, string>> cachedRemoteSearches;
        StreamProvider currentProvider = null;

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

        private async void OpenSeries(string linkExtension)
        {
            if (currentProvider == null) return;
            FormLoadingIndicator.ShowDialog(this, "Loading series. This usually shouldn't take any longer then 30 seconds.");
            int res = await currentProvider.LoadSeriesAsync(linkExtension, comboBoxStreamingProvider);
            FormLoadingIndicator.CloseDialog();
            if (res == StreamProvider.RESULT_OK || res == StreamProvider.RESULT_USE_CACHED)
            {
                Settings.WriteValue(Settings.LAST_PLAYED_SERIES, currentProvider.GetSeries().LinkExtension);
                await parent.LoadCachedSeriesAsync();
                parent.Invoke((MethodInvoker)(() =>
                {
                    parent.SelectSeries(currentProvider.GetSeries());
                }));
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
            string tbText = textBoxSeries.Text;
            if (SearchDictionary.ContainsKey(tbText))
            {
                seriesExtension = SearchDictionary[tbText];
                OpenSeries(seriesExtension);
            }
            else
            {
                MessageBox.Show("The entered name was not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
        }

        private void FormAddNewSeries_FormClosed(object sender, FormClosedEventArgs e)
        {
            CurrentCancellationTokenSource.Cancel();
            SearchDictionary = null;
            cachedRemoteSearches = null;
            parent.Enabled = true;
            parent.Focus();
            parent = null;
        }

        private void comboBoxStreamingProvider_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentProvider = StreamProvider.Create((string)comboBoxStreamingProvider.SelectedItem);
            switch (currentProvider.SupportedSearchMode)
            {
                case StreamProvider.SearchMode.LOCAL:
                    FormLoadingIndicator.ShowDialog(this, "Loading autocomplete list, please be patient ...");
                    currentProvider.RequestSearchIndexAsync(CurrentCancellationTokenSource.Token).ContinueWith((indexTask) => {
                        textBoxSeries.Invoke((MethodInvoker)(() => {
                            if (indexTask.IsCanceled || indexTask.IsFaulted)
                            {
                                Util.ShowUserInformation("Couldn't load search index!");
                                textBoxSeries.UsedAutoCompleteMode = CustomTextBoxTest.CustomAutoCompleteTextBox.AutoCompleteMode.None;
                            }
                            else
                            {
                                SearchDictionary = indexTask.Result;
                                textBoxSeries.UsedAutoCompleteMode = CustomTextBoxTest.CustomAutoCompleteTextBox.AutoCompleteMode.Index;
                                textBoxSeries.FuzzyAutoCompleteSource = SearchDictionary.Keys.ToList();
                                buttonOpenOverview.Enabled = true;
                                textBoxSeries.Enabled = true;
                                textBoxSeries.Select();
                            }
                            FormLoadingIndicator.CloseDialog();
                        }));
                    });
                    break;
                case StreamProvider.SearchMode.REMOTE:
                    textBoxSeries.Invoke((MethodInvoker)(() =>
                    {
                        cachedRemoteSearches = new Dictionary<string, Dictionary<string, string>>();
                        textBoxSeries.UsedAutoCompleteMode = CustomTextBoxTest.CustomAutoCompleteTextBox.AutoCompleteMode.Suggestions;
                        textBoxSeries.FuzzyAutoCompleteSource = new List<string>();
                        buttonOpenOverview.Enabled = true;
                        textBoxSeries.Enabled = true;
                        textBoxSeries.Select();
                    }));
                    break;
                default:
                    Logger.Log(TAG, "Searchmode not handled!");
                    break;
            }
        }

        private void textBoxSeries_TextChanged(object sender, EventArgs e)
        {
            switch (currentProvider.SupportedSearchMode)
            {
                case StreamProvider.SearchMode.LOCAL:
                    buttonAdd.Enabled = SearchDictionary.ContainsKey(((TextBox)sender).Text); ;
                    break;
                case StreamProvider.SearchMode.REMOTE:
                    string currentTextBoxSeriesText = textBoxSeries.Text;
                    if (!cachedRemoteSearches.Keys.Contains(textBoxSeries.Text))
                    {
                        cachedRemoteSearches.Add(currentTextBoxSeriesText, new Dictionary<string, string>());
                        currentProvider.RequestRemoteSearchAsync(currentTextBoxSeriesText, CurrentCancellationTokenSource.Token).ContinueWith((requestedSearchTask) => {
                            textBoxSeries.Invoke((MethodInvoker)(() =>
                            {
                                if (requestedSearchTask.IsCanceled || requestedSearchTask.IsFaulted)
                                {
                                    Util.ShowUserInformation("Couldn't load remote search results!");
                                    textBoxSeries.UsedAutoCompleteMode = CustomTextBoxTest.CustomAutoCompleteTextBox.AutoCompleteMode.None;
                                }
                                else
                                {
                                    textBoxSeries.UsedAutoCompleteMode = CustomTextBoxTest.CustomAutoCompleteTextBox.AutoCompleteMode.Suggestions;
                                    cachedRemoteSearches[currentTextBoxSeriesText] = requestedSearchTask.Result;
                                    if (currentTextBoxSeriesText == textBoxSeries.Text)
                                    {
                                        SearchDictionary = cachedRemoteSearches[currentTextBoxSeriesText];
                                    }
                                }
                                buttonAdd.Enabled = SearchDictionary.ContainsKey(currentTextBoxSeriesText);
                            }));
                        });
                    }
                    else
                    {
                        SearchDictionary = cachedRemoteSearches[currentTextBoxSeriesText];
                        buttonAdd.Enabled = SearchDictionary.ContainsKey(currentTextBoxSeriesText);
                    }
                    break;
                default:
                    Logger.Log(TAG, "Searchmode not handled!");
                    break;
            }
        }

        private void buttonOpenOverview_Click(object sender, EventArgs e)
        {
            Util.OpenLinkInDefaultBrowser(currentProvider.GetWebsiteLink());
        }
    }
}
