namespace SeriesPlayer.Forms
{
    partial class FormAddNewSeries
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAddNewSeries));
            this.buttonAdd = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxStreamingProvider = new System.Windows.Forms.ComboBox();
            this.labelCurrentlyLoaded = new System.Windows.Forms.Label();
            this.textBoxAddBySearch = new System.Windows.Forms.TextBox();
            this.backgroundWorkerLoadAutoComplete = new System.ComponentModel.BackgroundWorker();
            this.buttonOpenOverview = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonAdd
            // 
            this.buttonAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonAdd.Enabled = false;
            this.buttonAdd.Location = new System.Drawing.Point(18, 85);
            this.buttonAdd.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(130, 35);
            this.buttonAdd.TabIndex = 3;
            this.buttonAdd.Text = "Add";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Location = new System.Drawing.Point(537, 85);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(130, 35);
            this.buttonCancel.TabIndex = 5;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 9);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(121, 20);
            this.label1.TabIndex = 23;
            this.label1.Text = "Streamprovider:";
            // 
            // comboBoxStreamingProvider
            // 
            this.comboBoxStreamingProvider.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxStreamingProvider.FormattingEnabled = true;
            this.comboBoxStreamingProvider.Location = new System.Drawing.Point(18, 34);
            this.comboBoxStreamingProvider.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.comboBoxStreamingProvider.Name = "comboBoxStreamingProvider";
            this.comboBoxStreamingProvider.Size = new System.Drawing.Size(180, 28);
            this.comboBoxStreamingProvider.TabIndex = 1;
            this.comboBoxStreamingProvider.SelectedIndexChanged += new System.EventHandler(this.comboBoxStreamingProvider_SelectedIndexChanged);
            // 
            // labelCurrentlyLoaded
            // 
            this.labelCurrentlyLoaded.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelCurrentlyLoaded.AutoSize = true;
            this.labelCurrentlyLoaded.Location = new System.Drawing.Point(144, 11);
            this.labelCurrentlyLoaded.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelCurrentlyLoaded.Name = "labelCurrentlyLoaded";
            this.labelCurrentlyLoaded.Size = new System.Drawing.Size(131, 20);
            this.labelCurrentlyLoaded.TabIndex = 27;
            this.labelCurrentlyLoaded.Text = "Currently loading:";
            this.labelCurrentlyLoaded.Visible = false;
            // 
            // textBoxAddBySearch
            // 
            this.textBoxAddBySearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxAddBySearch.Enabled = false;
            this.textBoxAddBySearch.Location = new System.Drawing.Point(208, 34);
            this.textBoxAddBySearch.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBoxAddBySearch.Name = "textBoxAddBySearch";
            this.textBoxAddBySearch.Size = new System.Drawing.Size(457, 26);
            this.textBoxAddBySearch.TabIndex = 2;
            this.textBoxAddBySearch.TextChanged += new System.EventHandler(this.textBoxAddBySearch_TextChanged);
            // 
            // backgroundWorkerLoadAutoComplete
            // 
            this.backgroundWorkerLoadAutoComplete.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerLoadAutoComplete_DoWork);
            this.backgroundWorkerLoadAutoComplete.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerLoadAutoComplete_RunWorkerCompleted);
            // 
            // buttonOpenOverview
            // 
            this.buttonOpenOverview.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOpenOverview.Enabled = false;
            this.buttonOpenOverview.Location = new System.Drawing.Point(242, 85);
            this.buttonOpenOverview.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonOpenOverview.Name = "buttonOpenOverview";
            this.buttonOpenOverview.Size = new System.Drawing.Size(194, 35);
            this.buttonOpenOverview.TabIndex = 4;
            this.buttonOpenOverview.Text = "Open Series Overview";
            this.buttonOpenOverview.UseVisualStyleBackColor = true;
            this.buttonOpenOverview.Click += new System.EventHandler(this.buttonOpenOverview_Click);
            // 
            // FormAddNewSeries
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(686, 138);
            this.Controls.Add(this.textBoxAddBySearch);
            this.Controls.Add(this.buttonOpenOverview);
            this.Controls.Add(this.labelCurrentlyLoaded);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxStreamingProvider);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonAdd);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "FormAddNewSeries";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Add new Series";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormAddNewSeries_FormClosed);
            this.Load += new System.EventHandler(this.FormAddNewSeries_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxStreamingProvider;
        private System.Windows.Forms.Label labelCurrentlyLoaded;
        private System.Windows.Forms.TextBox textBoxAddBySearch;
        private System.ComponentModel.BackgroundWorker backgroundWorkerLoadAutoComplete;
        private System.Windows.Forms.Button buttonOpenOverview;
    }
}