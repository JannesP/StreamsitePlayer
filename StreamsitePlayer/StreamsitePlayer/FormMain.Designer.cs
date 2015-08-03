namespace StreamsitePlayer
{
    partial class FormMain
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
            this.components = new System.ComponentModel.Container();
            this.menuStripFormMain = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.textBoxSeriesExtension = new System.Windows.Forms.TextBox();
            this.labelSeriesExtensionHelp = new System.Windows.Forms.Label();
            this.comboBoxStreamingProvider = new System.Windows.Forms.ComboBox();
            this.buttonOpenSeries = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxStreamingSites = new System.Windows.Forms.ComboBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.labelCurrentlyLoaded = new System.Windows.Forms.Label();
            this.menuStripFormMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStripFormMain
            // 
            this.menuStripFormMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStripFormMain.Location = new System.Drawing.Point(0, 0);
            this.menuStripFormMain.Name = "menuStripFormMain";
            this.menuStripFormMain.Size = new System.Drawing.Size(459, 24);
            this.menuStripFormMain.TabIndex = 1;
            this.menuStripFormMain.Text = "menuStripFormMain";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // textBoxSeriesExtension
            // 
            this.textBoxSeriesExtension.Location = new System.Drawing.Point(139, 46);
            this.textBoxSeriesExtension.Name = "textBoxSeriesExtension";
            this.textBoxSeriesExtension.Size = new System.Drawing.Size(100, 20);
            this.textBoxSeriesExtension.TabIndex = 2;
            // 
            // labelSeriesExtensionHelp
            // 
            this.labelSeriesExtensionHelp.AutoSize = true;
            this.labelSeriesExtensionHelp.Location = new System.Drawing.Point(139, 27);
            this.labelSeriesExtensionHelp.Name = "labelSeriesExtensionHelp";
            this.labelSeriesExtensionHelp.Size = new System.Drawing.Size(29, 13);
            this.labelSeriesExtensionHelp.TabIndex = 3;
            this.labelSeriesExtensionHelp.Text = "Help";
            // 
            // comboBoxStreamingProvider
            // 
            this.comboBoxStreamingProvider.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxStreamingProvider.FormattingEnabled = true;
            this.comboBoxStreamingProvider.Location = new System.Drawing.Point(12, 45);
            this.comboBoxStreamingProvider.Name = "comboBoxStreamingProvider";
            this.comboBoxStreamingProvider.Size = new System.Drawing.Size(121, 21);
            this.comboBoxStreamingProvider.TabIndex = 4;
            this.comboBoxStreamingProvider.SelectedIndexChanged += new System.EventHandler(this.comboBoxStreamingProvider_SelectedIndexChanged);
            // 
            // buttonOpenSeries
            // 
            this.buttonOpenSeries.Location = new System.Drawing.Point(245, 45);
            this.buttonOpenSeries.Name = "buttonOpenSeries";
            this.buttonOpenSeries.Size = new System.Drawing.Size(75, 23);
            this.buttonOpenSeries.TabIndex = 5;
            this.buttonOpenSeries.Text = "Go!";
            this.buttonOpenSeries.UseVisualStyleBackColor = true;
            this.buttonOpenSeries.Click += new System.EventHandler(this.buttonOpenSeries_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Streamprovider:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(326, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Streaming site:";
            // 
            // comboBoxStreamingSites
            // 
            this.comboBoxStreamingSites.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxStreamingSites.FormattingEnabled = true;
            this.comboBoxStreamingSites.Location = new System.Drawing.Point(326, 45);
            this.comboBoxStreamingSites.Name = "comboBoxStreamingSites";
            this.comboBoxStreamingSites.Size = new System.Drawing.Size(121, 21);
            this.comboBoxStreamingSites.TabIndex = 7;
            // 
            // toolTip
            // 
            this.toolTip.AutomaticDelay = 100;
            // 
            // labelCurrentlyLoaded
            // 
            this.labelCurrentlyLoaded.AutoSize = true;
            this.labelCurrentlyLoaded.Location = new System.Drawing.Point(12, 73);
            this.labelCurrentlyLoaded.Name = "labelCurrentlyLoaded";
            this.labelCurrentlyLoaded.Size = new System.Drawing.Size(88, 13);
            this.labelCurrentlyLoaded.TabIndex = 9;
            this.labelCurrentlyLoaded.Text = "Currently loading:";
            this.labelCurrentlyLoaded.Visible = false;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(459, 93);
            this.Controls.Add(this.labelCurrentlyLoaded);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBoxStreamingSites);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonOpenSeries);
            this.Controls.Add(this.comboBoxStreamingProvider);
            this.Controls.Add(this.labelSeriesExtensionHelp);
            this.Controls.Add(this.textBoxSeriesExtension);
            this.Controls.Add(this.menuStripFormMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStripFormMain;
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.Text = "Serieswatcher";
            this.menuStripFormMain.ResumeLayout(false);
            this.menuStripFormMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStripFormMain;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.TextBox textBoxSeriesExtension;
        private System.Windows.Forms.Label labelSeriesExtensionHelp;
        private System.Windows.Forms.ComboBox comboBoxStreamingProvider;
        private System.Windows.Forms.Button buttonOpenSeries;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxStreamingSites;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Label labelCurrentlyLoaded;
    }
}

