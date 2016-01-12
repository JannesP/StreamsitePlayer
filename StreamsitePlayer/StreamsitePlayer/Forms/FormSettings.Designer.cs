namespace SeriesPlayer.Forms
{
    partial class FormSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSettings));
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxJwKey = new System.Windows.Forms.TextBox();
            this.linkLabelJwKey = new System.Windows.Forms.LinkLabel();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.checkBoxAutoUpdate = new System.Windows.Forms.CheckBox();
            this.buttonOpenAppFolder = new System.Windows.Forms.Button();
            this.buttonResetJwKey = new System.Windows.Forms.Button();
            this.numericUpDownPort = new System.Windows.Forms.NumericUpDown();
            this.checkBoxRememberLocation = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBoxRemoteControl = new System.Windows.Forms.CheckBox();
            this.linkLabelAndroidApk = new System.Windows.Forms.LinkLabel();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPort)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "JwPlayer Key:";
            // 
            // textBoxJwKey
            // 
            this.textBoxJwKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxJwKey.Location = new System.Drawing.Point(91, 12);
            this.textBoxJwKey.Name = "textBoxJwKey";
            this.textBoxJwKey.Size = new System.Drawing.Size(284, 20);
            this.textBoxJwKey.TabIndex = 1;
            // 
            // linkLabelJwKey
            // 
            this.linkLabelJwKey.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.linkLabelJwKey.AutoSize = true;
            this.linkLabelJwKey.Location = new System.Drawing.Point(457, 15);
            this.linkLabelJwKey.Name = "linkLabelJwKey";
            this.linkLabelJwKey.Size = new System.Drawing.Size(109, 13);
            this.linkLabelJwKey.TabIndex = 2;
            this.linkLabelJwKey.TabStop = true;
            this.linkLabelJwKey.Text = "Get your own for free!";
            this.linkLabelJwKey.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelJwKey_LinkClicked);
            // 
            // buttonSave
            // 
            this.buttonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSave.Location = new System.Drawing.Point(491, 181);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 3;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Location = new System.Drawing.Point(410, 181);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // checkBoxAutoUpdate
            // 
            this.checkBoxAutoUpdate.AutoSize = true;
            this.checkBoxAutoUpdate.Checked = true;
            this.checkBoxAutoUpdate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxAutoUpdate.Location = new System.Drawing.Point(12, 38);
            this.checkBoxAutoUpdate.Name = "checkBoxAutoUpdate";
            this.checkBoxAutoUpdate.Size = new System.Drawing.Size(166, 17);
            this.checkBoxAutoUpdate.TabIndex = 5;
            this.checkBoxAutoUpdate.Text = "Check for updates at startup?";
            this.checkBoxAutoUpdate.UseVisualStyleBackColor = true;
            // 
            // buttonOpenAppFolder
            // 
            this.buttonOpenAppFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOpenAppFolder.Location = new System.Drawing.Point(258, 181);
            this.buttonOpenAppFolder.Name = "buttonOpenAppFolder";
            this.buttonOpenAppFolder.Size = new System.Drawing.Size(117, 23);
            this.buttonOpenAppFolder.TabIndex = 6;
            this.buttonOpenAppFolder.Text = "Open Program Folder";
            this.buttonOpenAppFolder.UseVisualStyleBackColor = true;
            this.buttonOpenAppFolder.Click += new System.EventHandler(this.buttonOpenAppFolder_Click);
            // 
            // buttonResetJwKey
            // 
            this.buttonResetJwKey.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonResetJwKey.Location = new System.Drawing.Point(381, 10);
            this.buttonResetJwKey.Name = "buttonResetJwKey";
            this.buttonResetJwKey.Size = new System.Drawing.Size(70, 23);
            this.buttonResetJwKey.TabIndex = 7;
            this.buttonResetJwKey.Text = "Reset";
            this.buttonResetJwKey.UseVisualStyleBackColor = true;
            this.buttonResetJwKey.Click += new System.EventHandler(this.buttonResetJwKey_Click);
            // 
            // numericUpDownPort
            // 
            this.numericUpDownPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.numericUpDownPort.Location = new System.Drawing.Point(513, 151);
            this.numericUpDownPort.Maximum = new decimal(new int[] {
            49151,
            0,
            0,
            0});
            this.numericUpDownPort.Minimum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.numericUpDownPort.Name = "numericUpDownPort";
            this.numericUpDownPort.Size = new System.Drawing.Size(53, 20);
            this.numericUpDownPort.TabIndex = 8;
            this.numericUpDownPort.Value = new decimal(new int[] {
            8003,
            0,
            0,
            0});
            // 
            // checkBoxRememberLocation
            // 
            this.checkBoxRememberLocation.AutoSize = true;
            this.checkBoxRememberLocation.Checked = true;
            this.checkBoxRememberLocation.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxRememberLocation.Location = new System.Drawing.Point(12, 61);
            this.checkBoxRememberLocation.Name = "checkBoxRememberLocation";
            this.checkBoxRememberLocation.Size = new System.Drawing.Size(164, 17);
            this.checkBoxRememberLocation.TabIndex = 8;
            this.checkBoxRememberLocation.Text = "Remember last play location?";
            this.checkBoxRememberLocation.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(404, 153);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(103, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Remote control port:";
            // 
            // checkBoxRemoteControl
            // 
            this.checkBoxRemoteControl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxRemoteControl.AutoSize = true;
            this.checkBoxRemoteControl.Location = new System.Drawing.Point(12, 152);
            this.checkBoxRemoteControl.Name = "checkBoxRemoteControl";
            this.checkBoxRemoteControl.Size = new System.Drawing.Size(223, 17);
            this.checkBoxRemoteControl.TabIndex = 10;
            this.checkBoxRemoteControl.Text = "Activate remote control? (via android app)";
            this.checkBoxRemoteControl.UseVisualStyleBackColor = true;
            // 
            // linkLabelAndroidApk
            // 
            this.linkLabelAndroidApk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.linkLabelAndroidApk.AutoSize = true;
            this.linkLabelAndroidApk.Location = new System.Drawing.Point(241, 153);
            this.linkLabelAndroidApk.Name = "linkLabelAndroidApk";
            this.linkLabelAndroidApk.Size = new System.Drawing.Size(148, 13);
            this.linkLabelAndroidApk.TabIndex = 11;
            this.linkLabelAndroidApk.TabStop = true;
            this.linkLabelAndroidApk.Text = "Download APK (Android 3.0+)";
            this.linkLabelAndroidApk.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelAndroidApk_LinkClicked);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 115);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(482, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Experimantal (These options might crash the program. When you\'ve got any queststi" +
    "ons, simply ask.):";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 128);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(336, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "Also, these functions might be complex to configure for the time being.";
            // 
            // FormSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(578, 214);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.linkLabelAndroidApk);
            this.Controls.Add(this.checkBoxRemoteControl);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.numericUpDownPort);
            this.Controls.Add(this.checkBoxRememberLocation);
            this.Controls.Add(this.buttonResetJwKey);
            this.Controls.Add(this.buttonOpenAppFolder);
            this.Controls.Add(this.checkBoxAutoUpdate);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.linkLabelJwKey);
            this.Controls.Add(this.textBoxJwKey);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormSettings";
            this.ShowIcon = false;
            this.Text = "SeriesPlayer Settings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormSettings_FormClosing);
            this.Shown += new System.EventHandler(this.FormSettings_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPort)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxJwKey;
        private System.Windows.Forms.LinkLabel linkLabelJwKey;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.CheckBox checkBoxAutoUpdate;
        private System.Windows.Forms.Button buttonOpenAppFolder;
        private System.Windows.Forms.Button buttonResetJwKey;
        private System.Windows.Forms.NumericUpDown numericUpDownPort;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox checkBoxRemoteControl;
        private System.Windows.Forms.CheckBox checkBoxRememberLocation;
        private System.Windows.Forms.LinkLabel linkLabelAndroidApk;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}