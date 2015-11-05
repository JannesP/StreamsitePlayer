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
            this.buttonOpenProviderSite = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxStreamingProvider = new System.Windows.Forms.ComboBox();
            this.textBoxAddByExtension = new System.Windows.Forms.TextBox();
            this.labelCurrentlyLoaded = new System.Windows.Forms.Label();
            this.panelAddByExtension = new System.Windows.Forms.Panel();
            this.panelAddBySearch = new System.Windows.Forms.Panel();
            this.textBoxAddBySearch = new System.Windows.Forms.TextBox();
            this.backgroundWorkerLoadAutoComplete = new System.ComponentModel.BackgroundWorker();
            this.panelAddByExtension.SuspendLayout();
            this.panelAddBySearch.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonAdd
            // 
            this.buttonAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonAdd.Enabled = false;
            this.buttonAdd.Location = new System.Drawing.Point(12, 53);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(87, 23);
            this.buttonAdd.TabIndex = 0;
            this.buttonAdd.Text = "Add";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Location = new System.Drawing.Point(352, 53);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(87, 23);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOpenProviderSite
            // 
            this.buttonOpenProviderSite.Location = new System.Drawing.Point(3, 3);
            this.buttonOpenProviderSite.Name = "buttonOpenProviderSite";
            this.buttonOpenProviderSite.Size = new System.Drawing.Size(51, 23);
            this.buttonOpenProviderSite.TabIndex = 24;
            this.buttonOpenProviderSite.Text = "Browse";
            this.buttonOpenProviderSite.UseVisualStyleBackColor = true;
            this.buttonOpenProviderSite.Click += new System.EventHandler(this.buttonOpenProviderSite_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 23;
            this.label1.Text = "Streamprovider:";
            // 
            // comboBoxStreamingProvider
            // 
            this.comboBoxStreamingProvider.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxStreamingProvider.FormattingEnabled = true;
            this.comboBoxStreamingProvider.Location = new System.Drawing.Point(12, 22);
            this.comboBoxStreamingProvider.Name = "comboBoxStreamingProvider";
            this.comboBoxStreamingProvider.Size = new System.Drawing.Size(121, 21);
            this.comboBoxStreamingProvider.TabIndex = 22;
            this.comboBoxStreamingProvider.SelectedIndexChanged += new System.EventHandler(this.comboBoxStreamingProvider_SelectedIndexChanged);
            // 
            // textBoxAddByExtension
            // 
            this.textBoxAddByExtension.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxAddByExtension.Location = new System.Drawing.Point(57, 3);
            this.textBoxAddByExtension.Name = "textBoxAddByExtension";
            this.textBoxAddByExtension.Size = new System.Drawing.Size(243, 20);
            this.textBoxAddByExtension.TabIndex = 26;
            // 
            // labelCurrentlyLoaded
            // 
            this.labelCurrentlyLoaded.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelCurrentlyLoaded.AutoSize = true;
            this.labelCurrentlyLoaded.Location = new System.Drawing.Point(96, 5);
            this.labelCurrentlyLoaded.Name = "labelCurrentlyLoaded";
            this.labelCurrentlyLoaded.Size = new System.Drawing.Size(88, 13);
            this.labelCurrentlyLoaded.TabIndex = 27;
            this.labelCurrentlyLoaded.Text = "Currently loading:";
            this.labelCurrentlyLoaded.Visible = false;
            // 
            // panelAddByExtension
            // 
            this.panelAddByExtension.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelAddByExtension.Controls.Add(this.textBoxAddByExtension);
            this.panelAddByExtension.Controls.Add(this.buttonOpenProviderSite);
            this.panelAddByExtension.Location = new System.Drawing.Point(136, 19);
            this.panelAddByExtension.Name = "panelAddByExtension";
            this.panelAddByExtension.Size = new System.Drawing.Size(303, 29);
            this.panelAddByExtension.TabIndex = 28;
            this.panelAddByExtension.Visible = false;
            // 
            // panelAddBySearch
            // 
            this.panelAddBySearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelAddBySearch.Controls.Add(this.textBoxAddBySearch);
            this.panelAddBySearch.Location = new System.Drawing.Point(136, 19);
            this.panelAddBySearch.Name = "panelAddBySearch";
            this.panelAddBySearch.Size = new System.Drawing.Size(303, 29);
            this.panelAddBySearch.TabIndex = 29;
            this.panelAddBySearch.Visible = false;
            // 
            // textBoxAddBySearch
            // 
            this.textBoxAddBySearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxAddBySearch.Location = new System.Drawing.Point(3, 3);
            this.textBoxAddBySearch.Name = "textBoxAddBySearch";
            this.textBoxAddBySearch.Size = new System.Drawing.Size(297, 20);
            this.textBoxAddBySearch.TabIndex = 26;
            this.textBoxAddBySearch.TextChanged += new System.EventHandler(this.textBoxAddBySearch_TextChanged);
            // 
            // backgroundWorkerLoadAutoComplete
            // 
            this.backgroundWorkerLoadAutoComplete.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerLoadAutoComplete_DoWork);
            this.backgroundWorkerLoadAutoComplete.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerLoadAutoComplete_RunWorkerCompleted);
            // 
            // FormAddNewSeries
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(451, 88);
            this.Controls.Add(this.panelAddBySearch);
            this.Controls.Add(this.panelAddByExtension);
            this.Controls.Add(this.labelCurrentlyLoaded);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxStreamingProvider);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonAdd);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormAddNewSeries";
            this.Text = "Add new Series";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormAddNewSeries_FormClosed);
            this.Load += new System.EventHandler(this.FormAddNewSeries_Load);
            this.panelAddByExtension.ResumeLayout(false);
            this.panelAddByExtension.PerformLayout();
            this.panelAddBySearch.ResumeLayout(false);
            this.panelAddBySearch.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOpenProviderSite;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxStreamingProvider;
        private System.Windows.Forms.TextBox textBoxAddByExtension;
        private System.Windows.Forms.Label labelCurrentlyLoaded;
        private System.Windows.Forms.Panel panelAddByExtension;
        private System.Windows.Forms.Panel panelAddBySearch;
        private System.Windows.Forms.TextBox textBoxAddBySearch;
        private System.ComponentModel.BackgroundWorker backgroundWorkerLoadAutoComplete;
    }
}