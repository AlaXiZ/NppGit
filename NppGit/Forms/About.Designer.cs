namespace NppGit.Forms
{
    partial class About
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(About));
            this.lPluginName = new System.Windows.Forms.Label();
            this.lVersion = new System.Windows.Forms.Label();
            this.lAuthor = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.llMail = new System.Windows.Forms.LinkLabel();
            this.tbChangeLog = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.llIssue = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lPluginName
            // 
            this.lPluginName.AutoSize = true;
            this.lPluginName.Location = new System.Drawing.Point(142, 12);
            this.lPluginName.Name = "lPluginName";
            this.lPluginName.Size = new System.Drawing.Size(65, 13);
            this.lPluginName.TabIndex = 0;
            this.lPluginName.Text = "Plugin name";
            // 
            // lVersion
            // 
            this.lVersion.AutoSize = true;
            this.lVersion.Location = new System.Drawing.Point(142, 25);
            this.lVersion.Name = "lVersion";
            this.lVersion.Size = new System.Drawing.Size(42, 13);
            this.lVersion.TabIndex = 1;
            this.lVersion.Text = "Version";
            // 
            // lAuthor
            // 
            this.lAuthor.AutoSize = true;
            this.lAuthor.Location = new System.Drawing.Point(142, 38);
            this.lAuthor.Name = "lAuthor";
            this.lAuthor.Size = new System.Drawing.Size(80, 13);
            this.lAuthor.TabIndex = 2;
            this.lAuthor.Text = "Schadin Alexey";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::NppGit.Properties.Resources.NppGit_Logo;
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(124, 202);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // llMail
            // 
            this.llMail.AutoSize = true;
            this.llMail.Location = new System.Drawing.Point(228, 38);
            this.llMail.Name = "llMail";
            this.llMail.Size = new System.Drawing.Size(102, 13);
            this.llMail.TabIndex = 4;
            this.llMail.TabStop = true;
            this.llMail.Text = "schadin@gmail.com";
            this.llMail.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llMail_LinkClicked);
            // 
            // tbChangeLog
            // 
            this.tbChangeLog.BackColor = System.Drawing.Color.White;
            this.tbChangeLog.Location = new System.Drawing.Point(145, 71);
            this.tbChangeLog.Multiline = true;
            this.tbChangeLog.Name = "tbChangeLog";
            this.tbChangeLog.ReadOnly = true;
            this.tbChangeLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbChangeLog.Size = new System.Drawing.Size(306, 143);
            this.tbChangeLog.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(142, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Трекер задача:";
            // 
            // llIssue
            // 
            this.llIssue.AutoSize = true;
            this.llIssue.Location = new System.Drawing.Point(228, 51);
            this.llIssue.Name = "llIssue";
            this.llIssue.Size = new System.Drawing.Size(173, 13);
            this.llIssue.TabIndex = 7;
            this.llIssue.TabStop = true;
            this.llIssue.Text = "bitbucket.org/AlaXiZ/nppgit/issues";
            this.llIssue.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llIssue_LinkClicked);
            // 
            // About
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(463, 226);
            this.Controls.Add(this.llIssue);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbChangeLog);
            this.Controls.Add(this.llMail);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lAuthor);
            this.Controls.Add(this.lVersion);
            this.Controls.Add(this.lPluginName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "About";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "О программе...";
            this.Load += new System.EventHandler(this.About_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lPluginName;
        private System.Windows.Forms.Label lVersion;
        private System.Windows.Forms.Label lAuthor;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.LinkLabel llMail;
        private System.Windows.Forms.TextBox tbChangeLog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel llIssue;
    }
}