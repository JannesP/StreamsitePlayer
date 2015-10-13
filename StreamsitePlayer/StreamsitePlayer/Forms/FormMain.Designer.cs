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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.menuStripFormMain = new System.Windows.Forms.MenuStrip();
            this.seriesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.downloadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.githubToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkForUpdateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.versionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.labelCurrentlyLoaded = new System.Windows.Forms.Label();
            this.checkBoxAutoplay = new System.Windows.Forms.CheckBox();
            this.numericUpDownSkipEnd = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.numericUpDownSkipStart = new System.Windows.Forms.NumericUpDown();
            this.panelEpisodeButtons = new System.Windows.Forms.Panel();
            this.comboBoxChangeSeries = new System.Windows.Forms.ComboBox();
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripFormMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSkipEnd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSkipStart)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStripFormMain
            // 
            this.menuStripFormMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.seriesToolStripMenuItem,
            this.fileToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStripFormMain.Location = new System.Drawing.Point(0, 0);
            this.menuStripFormMain.Name = "menuStripFormMain";
            this.menuStripFormMain.Size = new System.Drawing.Size(627, 24);
            this.menuStripFormMain.TabIndex = 1;
            this.menuStripFormMain.Text = "menuStripFormMain";
            // 
            // seriesToolStripMenuItem
            // 
            this.seriesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.downloadToolStripMenuItem,
            this.refreshToolStripMenuItem,
            this.removeToolStripMenuItem});
            this.seriesToolStripMenuItem.Enabled = false;
            this.seriesToolStripMenuItem.Name = "seriesToolStripMenuItem";
            this.seriesToolStripMenuItem.Size = new System.Drawing.Size(49, 20);
            this.seriesToolStripMenuItem.Text = "Series";
            // 
            // downloadToolStripMenuItem
            // 
            this.downloadToolStripMenuItem.Enabled = false;
            this.downloadToolStripMenuItem.Name = "downloadToolStripMenuItem";
            this.downloadToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.downloadToolStripMenuItem.Text = "Download";
            this.downloadToolStripMenuItem.Click += new System.EventHandler(this.downloadToolStripMenuItem_Click);
            // 
            // refreshToolStripMenuItem
            // 
            this.refreshToolStripMenuItem.Enabled = false;
            this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            this.refreshToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.refreshToolStripMenuItem.Text = "Recache";
            this.refreshToolStripMenuItem.Click += new System.EventHandler(this.refreshToolStripMenuItem_Click);
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.fileToolStripMenuItem.Text = "General";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.githubToolStripMenuItem,
            this.checkForUpdateToolStripMenuItem,
            this.versionToolStripMenuItem,
            this.helpToolStripMenuItem1});
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.aboutToolStripMenuItem.Text = "About";
            // 
            // githubToolStripMenuItem
            // 
            this.githubToolStripMenuItem.Name = "githubToolStripMenuItem";
            this.githubToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.githubToolStripMenuItem.Text = "Github";
            this.githubToolStripMenuItem.Click += new System.EventHandler(this.githubToolStripMenuItem_Click);
            // 
            // checkForUpdateToolStripMenuItem
            // 
            this.checkForUpdateToolStripMenuItem.Name = "checkForUpdateToolStripMenuItem";
            this.checkForUpdateToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.checkForUpdateToolStripMenuItem.Text = "Check for Update";
            this.checkForUpdateToolStripMenuItem.Click += new System.EventHandler(this.checkForUpdateToolStripMenuItem_Click);
            // 
            // versionToolStripMenuItem
            // 
            this.versionToolStripMenuItem.Name = "versionToolStripMenuItem";
            this.versionToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.versionToolStripMenuItem.Text = "Version";
            this.versionToolStripMenuItem.Click += new System.EventHandler(this.versionToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem1
            // 
            this.helpToolStripMenuItem1.Name = "helpToolStripMenuItem1";
            this.helpToolStripMenuItem1.Size = new System.Drawing.Size(166, 22);
            this.helpToolStripMenuItem1.Text = "Help";
            this.helpToolStripMenuItem1.Click += new System.EventHandler(this.helpToolStripMenuItem1_Click);
            // 
            // toolTip
            // 
            this.toolTip.AutomaticDelay = 100;
            // 
            // labelCurrentlyLoaded
            // 
            this.labelCurrentlyLoaded.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelCurrentlyLoaded.AutoSize = true;
            this.labelCurrentlyLoaded.Location = new System.Drawing.Point(12, 263);
            this.labelCurrentlyLoaded.Name = "labelCurrentlyLoaded";
            this.labelCurrentlyLoaded.Size = new System.Drawing.Size(88, 13);
            this.labelCurrentlyLoaded.TabIndex = 9;
            this.labelCurrentlyLoaded.Text = "Currently loading:";
            this.labelCurrentlyLoaded.Visible = false;
            this.labelCurrentlyLoaded.Click += new System.EventHandler(this.PanelFocus_Click);
            // 
            // checkBoxAutoplay
            // 
            this.checkBoxAutoplay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxAutoplay.AutoSize = true;
            this.checkBoxAutoplay.Location = new System.Drawing.Point(220, 29);
            this.checkBoxAutoplay.Name = "checkBoxAutoplay";
            this.checkBoxAutoplay.Size = new System.Drawing.Size(67, 17);
            this.checkBoxAutoplay.TabIndex = 10;
            this.checkBoxAutoplay.Text = "Autoplay";
            this.checkBoxAutoplay.UseVisualStyleBackColor = true;
            // 
            // numericUpDownSkipEnd
            // 
            this.numericUpDownSkipEnd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownSkipEnd.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownSkipEnd.Location = new System.Drawing.Point(548, 28);
            this.numericUpDownSkipEnd.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.numericUpDownSkipEnd.Name = "numericUpDownSkipEnd";
            this.numericUpDownSkipEnd.Size = new System.Drawing.Size(49, 20);
            this.numericUpDownSkipEnd.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(493, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Skip end";
            this.label3.Click += new System.EventHandler(this.PanelFocus_Click);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(603, 30);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(12, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "s";
            this.label4.Click += new System.EventHandler(this.PanelFocus_Click);
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(453, 30);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(12, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "s";
            this.label5.Click += new System.EventHandler(this.PanelFocus_Click);
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(345, 30);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(51, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "Skip start";
            this.label6.Click += new System.EventHandler(this.PanelFocus_Click);
            // 
            // numericUpDownSkipStart
            // 
            this.numericUpDownSkipStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownSkipStart.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownSkipStart.Location = new System.Drawing.Point(400, 28);
            this.numericUpDownSkipStart.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.numericUpDownSkipStart.Name = "numericUpDownSkipStart";
            this.numericUpDownSkipStart.Size = new System.Drawing.Size(49, 20);
            this.numericUpDownSkipStart.TabIndex = 14;
            // 
            // panelEpisodeButtons
            // 
            this.panelEpisodeButtons.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelEpisodeButtons.AutoScroll = true;
            this.panelEpisodeButtons.Location = new System.Drawing.Point(12, 51);
            this.panelEpisodeButtons.Margin = new System.Windows.Forms.Padding(0);
            this.panelEpisodeButtons.MinimumSize = new System.Drawing.Size(588, 177);
            this.panelEpisodeButtons.Name = "panelEpisodeButtons";
            this.panelEpisodeButtons.Size = new System.Drawing.Size(606, 222);
            this.panelEpisodeButtons.TabIndex = 17;
            this.panelEpisodeButtons.Click += new System.EventHandler(this.PanelFocus_Click);
            // 
            // comboBoxChangeSeries
            // 
            this.comboBoxChangeSeries.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxChangeSeries.FormattingEnabled = true;
            this.comboBoxChangeSeries.Location = new System.Drawing.Point(12, 27);
            this.comboBoxChangeSeries.Name = "comboBoxChangeSeries";
            this.comboBoxChangeSeries.Size = new System.Drawing.Size(202, 21);
            this.comboBoxChangeSeries.TabIndex = 20;
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.removeToolStripMenuItem.Text = "Remove";
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeToolStripMenuItem_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(627, 285);
            this.Controls.Add(this.comboBoxChangeSeries);
            this.Controls.Add(this.labelCurrentlyLoaded);
            this.Controls.Add(this.panelEpisodeButtons);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.numericUpDownSkipStart);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.numericUpDownSkipEnd);
            this.Controls.Add(this.checkBoxAutoplay);
            this.Controls.Add(this.menuStripFormMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStripFormMain;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(643, 323);
            this.Name = "FormMain";
            this.Text = "SeriesPlayer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Shown += new System.EventHandler(this.FormMain_Shown);
            this.Click += new System.EventHandler(this.PanelFocus_Click);
            this.menuStripFormMain.ResumeLayout(false);
            this.menuStripFormMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSkipEnd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSkipStart)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStripFormMain;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Label labelCurrentlyLoaded;
        private System.Windows.Forms.CheckBox checkBoxAutoplay;
        private System.Windows.Forms.NumericUpDown numericUpDownSkipEnd;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numericUpDownSkipStart;
        private System.Windows.Forms.Panel panelEpisodeButtons;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem githubToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkForUpdateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem versionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem1;
        private System.Windows.Forms.ComboBox comboBoxChangeSeries;
        private System.Windows.Forms.ToolStripMenuItem seriesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem downloadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
    }
}

