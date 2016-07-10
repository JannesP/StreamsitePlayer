using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomTextBoxTest
{
    class CustomAutoCompleteTextBox : TextBox
    {
        private List<string> currentAutoCompletion = new List<string>();
        private ListBox listBoxAutoCompletionEntries;
        private string[] indexSplitters = { "-", " ", "#", ";", ".", ":", ",", "_", "+", "\"", "'" };
        private char[] searchSplitters = { '-', ' ', '#', ';', '.', ':', ',', '_', '+', '\"', '\'' };
        private int oldChildIndex = -1;
        private int oldHeight = -1;

        public CustomAutoCompleteTextBox()
        {
            base.AutoSize = false;
            base.Margin = new Padding(0);

            listBoxAutoCompletionEntries = new ListBox();
            listBoxAutoCompletionEntries.Location = new System.Drawing.Point(0, base.Height);
            listBoxAutoCompletionEntries.Visible = false;
            listBoxAutoCompletionEntries.Width = base.Width;
            listBoxAutoCompletionEntries.TabStop = false;
            listBoxAutoCompletionEntries.GotFocus += ListBoxAutoCompletionEntries_GotFocus;
            listBoxAutoCompletionEntries.BorderStyle = BorderStyle.None;
            listBoxAutoCompletionEntries.IntegralHeight = true;
            listBoxAutoCompletionEntries.Cursor = Cursors.Arrow;
            listBoxAutoCompletionEntries.MouseMove += ListBoxAutoCompletionEntries_MouseMove;
            listBoxAutoCompletionEntries.MouseDown += ListBoxAutoCompletionEntries_MouseDown;
            base.Controls.Add(listBoxAutoCompletionEntries);
            base.Controls.SetChildIndex(listBoxAutoCompletionEntries, 0);
        }

        private void ListBoxAutoCompletionEntries_MouseDown(object sender, MouseEventArgs e)
        {
            base.Text = (string)listBoxAutoCompletionEntries.SelectedItem;
            HideAutoCompletionList();
        }

        private void ListBoxAutoCompletionEntries_MouseMove(object sender, MouseEventArgs e)
        {
            listBoxAutoCompletionEntries.SelectedIndex = listBoxAutoCompletionEntries.IndexFromPoint(e.Location);
        }

        protected void ShowAutoCompletionList()
        {
            Control parent = base.Parent;
            if (parent != null && listBoxAutoCompletionEntries.Items.Count > 0)
            {
                if (oldChildIndex == -1) oldChildIndex = parent.Controls.GetChildIndex(this);
                if (oldHeight == -1) oldHeight = base.Height;
                parent.Controls.SetChildIndex(this, 0);
                base.Height = base.PreferredHeight + listBoxAutoCompletionEntries.Height;

                listBoxAutoCompletionEntries.Visible = true;
            }
        }

        protected void HideAutoCompletionList()
        {
            Control parent = base.Parent;
            if (parent != null)
            {
                parent.Controls.SetChildIndex(this, oldChildIndex);
                base.Height = oldHeight <= 0 ? base.PreferredHeight : oldHeight;
                listBoxAutoCompletionEntries.Visible = false;
                oldChildIndex = -1;
                oldHeight = -1;
            }
        }

        private void ListBoxAutoCompletionEntries_GotFocus(object sender, EventArgs e)
        {
            base.Focus();
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            HideAutoCompletionList();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            listBoxAutoCompletionEntries.Width = base.Width;
        }

        protected override void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
        {
            if (listBoxAutoCompletionEntries.Visible && e.KeyCode == Keys.Tab)
            {
                base.Text = (string)listBoxAutoCompletionEntries.SelectedItem;
            }
            base.OnPreviewKeyDown(e);
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (listBoxAutoCompletionEntries.Visible)
            {
                e.Handled = true;
                switch (e.KeyCode)
                {
                    case Keys.Escape:
                        HideAutoCompletionList();
                        e.SuppressKeyPress = true;
                        break;
                    case Keys.Up:
                        int newIndex = listBoxAutoCompletionEntries.SelectedIndex - 1;
                        if (newIndex < 0) newIndex += listBoxAutoCompletionEntries.Items.Count;
                        listBoxAutoCompletionEntries.SelectedIndex = newIndex;
                        break;
                    case Keys.Down:
                        listBoxAutoCompletionEntries.SelectedIndex = (listBoxAutoCompletionEntries.SelectedIndex + 1) % listBoxAutoCompletionEntries.Items.Count;
                        break;
                    case Keys.Enter:
                        base.Text = (string)listBoxAutoCompletionEntries.SelectedItem;
                        HideAutoCompletionList();
                        e.SuppressKeyPress = true;
                        break;
                    default:
                        e.Handled = false;
                        break;
                }
            }
            else
            {
                if (currentAutoCompletion.Count == 0)
                {
                    switch (e.KeyCode)
                    {
                        case Keys.Up:
                        case Keys.Down:
                            ShowAutoCompletionList();
                            break;
                    }
                }
            }
            base.OnKeyDown(e);
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            RefreshAutoCompletion();
        }

        protected void RefreshAutoCompletion()
        {
            listBoxAutoCompletionEntries.Items.Clear();
            if (base.Text.Length > 0)
            {
                List<string> currentAutoCompletion = GetAutoCompleteList(base.Text);
                if (currentAutoCompletion.Count > 0)
                {
                    foreach (string currentEntry in currentAutoCompletion)
                    {
                        listBoxAutoCompletionEntries.Items.Add(currentEntry);
                    }
                    listBoxAutoCompletionEntries.SelectedIndex = 0;
                    ShowAutoCompletionList();
                    base.ForeColor = System.Drawing.Color.Black;
                }
                else
                {
                    HideAutoCompletionList();
                }
            }
            else
            {
                HideAutoCompletionList();
                if (UsedAutoCompleteMode != AutoCompleteMode.None)
                {
                    base.ForeColor = System.Drawing.Color.Red;
                }
            }

        }

        protected List<string> GetAutoCompleteList(string searchText)
        {
            List<string> result = new List<string>();

            switch (UsedAutoCompleteMode)
            {
                case AutoCompleteMode.Index:
                    foreach (string currentEntry in FuzzyAutoCompleteSource)
                    {
                        string entrySearch = ReplaceAll(currentEntry, indexSplitters, "").ToLower();

                        string[] searchTextParts = searchText.Split(searchSplitters);
                        bool[] textPartExists = new bool[searchTextParts.Length];
                        for (int i = 0; i < textPartExists.Length; i++)
                        {
                            textPartExists[i] = false;
                        }
                        for (int i = 0; i < searchTextParts.Length; i++)
                        {
                            if (entrySearch.Contains(searchTextParts[i].ToLower()))
                            {
                                textPartExists[i] = true;
                            }
                        }
                        bool isMatch = true;
                        for (int i = 0; i < textPartExists.Length; i++)
                        {
                            if (!textPartExists[i])
                            {
                                isMatch = false;
                                break;
                            }
                        }
                        if (isMatch) result.Add(currentEntry);
                    }
                    break;
                case AutoCompleteMode.Suggestions:
                    result = FuzzyAutoCompleteSource;
                    break;
                case AutoCompleteMode.None:
                    break;
            }

            return result;
        }

        private static string ReplaceAll(string str, string[] strings, string replacement)
        {
            string buffer = str;
            foreach (string s in strings)
            {
                buffer = buffer.Replace(s, replacement);
            }
            return buffer;
        }

        private List<string> _searchIndex = new List<string>();
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        public List<string> FuzzyAutoCompleteSource
        {
            get
            {
                return _searchIndex;
            }
            set
            {
                if (value == null)
                {
                    _searchIndex = new List<string>();
                }
                else
                {
                    _searchIndex = value;
                }
                RefreshAutoCompletion();
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new AutoCompleteStringCollection AutoCompleteCustomSource
        {
            get { return new AutoCompleteStringCollection(); }
            set { throw new InvalidOperationException("This method is not allowed, use FuzzyAutoCompleteSource!"); }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new AutoCompleteSource AutoCompleteSource
        {
            get { return AutoCompleteSource.None; }
            set { throw new InvalidOperationException("This method is not allowed, use FuzzyAutoCompleteSource!"); }
        }

        private AutoCompleteMode _usedAutoCompleteMode = AutoCompleteMode.Index;
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        [DefaultValue(AutoCompleteMode.None)]
        public AutoCompleteMode UsedAutoCompleteMode
        {
            get
            {
                return _usedAutoCompleteMode;
            }
            set
            {
                _usedAutoCompleteMode = value;
                RefreshAutoCompletion();
            }
        }
        public new enum AutoCompleteMode
        {
            /// <summary>
            /// The FuzzyAutoCompleteSource is used as the search index.
            /// </summary>
            Index,
            /// <summary>
            /// The FuzzyAutoCompleteSource is used as suggestions.
            /// </summary>
            Suggestions,
            /// <summary>
            /// The search index is turned off. Usable as a normal TextBox.
            /// </summary>
            None
        }

        //inactive for now, just an idea for later
        private class FormListBox : Form
        {
            private const int CS_DROPSHADOW = 0x0002;
            public FormListBox()
            {
                this.ShowIcon = false;
                this.ControlBox = false;
                this.MinimizeBox = false;
                this.MaximizeBox = false;
                this.ShowInTaskbar = false;
                this.FormBorderStyle = FormBorderStyle.None;
                this.LostFocus += FormListBox_LostFocus;
            }

            protected override CreateParams CreateParams
            {
                get
                {
                    CreateParams createParams = base.CreateParams;
                    createParams.ClassStyle |= CS_DROPSHADOW;
                    return createParams;
                }
            }

            private void FormListBox_LostFocus(object sender, EventArgs e)
            {
                this.Close();
            }
        }

    }
}