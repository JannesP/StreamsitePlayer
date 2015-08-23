namespace StreamsitePlayer
{
    partial class FormPlayerBrowser
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
            this.webBrowserPlayer = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // webBrowserPlayer
            // 
            this.webBrowserPlayer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowserPlayer.Location = new System.Drawing.Point(0, 0);
            this.webBrowserPlayer.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowserPlayer.Name = "webBrowserPlayer";
            this.webBrowserPlayer.ScriptErrorsSuppressed = true;
            this.webBrowserPlayer.Size = new System.Drawing.Size(1122, 652);
            this.webBrowserPlayer.TabIndex = 0;
            // 
            // FormPlayerBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1122, 652);
            this.Controls.Add(this.webBrowserPlayer);
            this.Name = "FormPlayerBrowser";
            this.Text = "FormPlayerBrowser";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormPlayerBrowser_FormClosing);
            this.Shown += new System.EventHandler(this.FormPlayerBrowser_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser webBrowserPlayer;
    }
}