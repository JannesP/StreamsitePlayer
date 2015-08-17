﻿namespace StreamsitePlayer
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
            this.checkBoxAutoplay = new System.Windows.Forms.CheckBox();
            this.numericUpDownSkipEnd = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.numericUpDownSkipStart = new System.Windows.Forms.NumericUpDown();
            this.panelEpisodeButtons = new System.Windows.Forms.Panel();
            this.menuStripFormMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSkipEnd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSkipStart)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStripFormMain
            // 
            this.menuStripFormMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStripFormMain.Location = new System.Drawing.Point(0, 0);
            this.menuStripFormMain.Name = "menuStripFormMain";
            this.menuStripFormMain.Size = new System.Drawing.Size(627, 24);
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
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // textBoxSeriesExtension
            // 
            this.textBoxSeriesExtension.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSeriesExtension.Location = new System.Drawing.Point(139, 46);
            this.textBoxSeriesExtension.Name = "textBoxSeriesExtension";
            this.textBoxSeriesExtension.Size = new System.Drawing.Size(271, 20);
            this.textBoxSeriesExtension.TabIndex = 2;
            this.textBoxSeriesExtension.TextChanged += new System.EventHandler(this.textBoxSeriesExtension_TextChanged);
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
            // 
            // buttonOpenSeries
            // 
            this.buttonOpenSeries.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOpenSeries.Location = new System.Drawing.Point(416, 45);
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
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(497, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Streaming site:";
            // 
            // comboBoxStreamingSites
            // 
            this.comboBoxStreamingSites.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxStreamingSites.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxStreamingSites.FormattingEnabled = true;
            this.comboBoxStreamingSites.Location = new System.Drawing.Point(497, 45);
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
            // checkBoxAutoplay
            // 
            this.checkBoxAutoplay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxAutoplay.AutoSize = true;
            this.checkBoxAutoplay.Location = new System.Drawing.Point(551, 72);
            this.checkBoxAutoplay.Name = "checkBoxAutoplay";
            this.checkBoxAutoplay.Size = new System.Drawing.Size(67, 17);
            this.checkBoxAutoplay.TabIndex = 10;
            this.checkBoxAutoplay.Text = "Autoplay";
            this.checkBoxAutoplay.UseVisualStyleBackColor = true;
            this.checkBoxAutoplay.CheckedChanged += new System.EventHandler(this.checkBoxAutoplay_CheckedChanged);
            // 
            // numericUpDownSkipEnd
            // 
            this.numericUpDownSkipEnd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownSkipEnd.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownSkipEnd.Location = new System.Drawing.Point(475, 71);
            this.numericUpDownSkipEnd.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.numericUpDownSkipEnd.Name = "numericUpDownSkipEnd";
            this.numericUpDownSkipEnd.Size = new System.Drawing.Size(49, 20);
            this.numericUpDownSkipEnd.TabIndex = 11;
            this.numericUpDownSkipEnd.ValueChanged += new System.EventHandler(this.numericUpDownSkipEnd_ValueChanged);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(420, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Skip end";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(530, 73);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(12, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "s";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(404, 73);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(12, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "s";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(294, 73);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(51, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "Skip start";
            // 
            // numericUpDownSkipStart
            // 
            this.numericUpDownSkipStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownSkipStart.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownSkipStart.Location = new System.Drawing.Point(349, 71);
            this.numericUpDownSkipStart.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.numericUpDownSkipStart.Name = "numericUpDownSkipStart";
            this.numericUpDownSkipStart.Size = new System.Drawing.Size(49, 20);
            this.numericUpDownSkipStart.TabIndex = 14;
            this.numericUpDownSkipStart.ValueChanged += new System.EventHandler(this.numericUpDownSkipStart_ValueChanged);
            // 
            // panelEpisodeButtons
            // 
            this.panelEpisodeButtons.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelEpisodeButtons.AutoScroll = true;
            this.panelEpisodeButtons.Location = new System.Drawing.Point(12, 95);
            this.panelEpisodeButtons.Margin = new System.Windows.Forms.Padding(0);
            this.panelEpisodeButtons.MinimumSize = new System.Drawing.Size(588, 177);
            this.panelEpisodeButtons.Name = "panelEpisodeButtons";
            this.panelEpisodeButtons.Size = new System.Drawing.Size(606, 280);
            this.panelEpisodeButtons.TabIndex = 17;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(627, 387);
            this.Controls.Add(this.panelEpisodeButtons);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.numericUpDownSkipStart);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.numericUpDownSkipEnd);
            this.Controls.Add(this.checkBoxAutoplay);
            this.Controls.Add(this.labelCurrentlyLoaded);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBoxStreamingSites);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonOpenSeries);
            this.Controls.Add(this.comboBoxStreamingProvider);
            this.Controls.Add(this.labelSeriesExtensionHelp);
            this.Controls.Add(this.textBoxSeriesExtension);
            this.Controls.Add(this.menuStripFormMain);
            this.MainMenuStrip = this.menuStripFormMain;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(625, 323);
            this.Name = "FormMain";
            this.Text = "Serieswatcher";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
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
        private System.Windows.Forms.TextBox textBoxSeriesExtension;
        private System.Windows.Forms.Label labelSeriesExtensionHelp;
        private System.Windows.Forms.ComboBox comboBoxStreamingProvider;
        private System.Windows.Forms.Button buttonOpenSeries;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxStreamingSites;
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
    }
}
