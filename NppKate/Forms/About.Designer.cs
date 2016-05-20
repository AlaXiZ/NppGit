/*
Copyright (c) 2015-2016, Schadin Alexey (schadin@gmail.com)
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted 
provided that the following conditions are met:

1. Redistributions of source code must retain the above copyright notice, this list of conditions 
and the following disclaimer.

2. Redistributions in binary form must reproduce the above copyright notice, this list of conditions 
and the following disclaimer in the documentation and/or other materials provided with 
the distribution.

3. Neither the name of the copyright holder nor the names of its contributors may be used to endorse 
or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR 
IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND 
FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR 
CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL 
DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, 
DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER 
IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF 
THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

namespace NppKate.Forms
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
            this.tbLicense = new System.Windows.Forms.TextBox();
            this.lblIcons = new System.Windows.Forms.Label();
            this.llYusuke = new System.Windows.Forms.LinkLabel();
            this.llCCA30L = new System.Windows.Forms.LinkLabel();
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
            this.pictureBox1.Image = global::NppKate.Properties.Resources.NppGit_Logo;
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
            this.tbChangeLog.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbChangeLog.Location = new System.Drawing.Point(145, 71);
            this.tbChangeLog.Multiline = true;
            this.tbChangeLog.Name = "tbChangeLog";
            this.tbChangeLog.ReadOnly = true;
            this.tbChangeLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbChangeLog.Size = new System.Drawing.Size(335, 143);
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
            this.llIssue.Size = new System.Drawing.Size(204, 13);
            this.llIssue.TabIndex = 7;
            this.llIssue.TabStop = true;
            this.llIssue.Text = "nppgit.myjetbrains.com/youtrack/issues";
            this.llIssue.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llIssue_LinkClicked);
            // 
            // tbLicense
            // 
            this.tbLicense.BackColor = System.Drawing.Color.White;
            this.tbLicense.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbLicense.Location = new System.Drawing.Point(12, 220);
            this.tbLicense.Multiline = true;
            this.tbLicense.Name = "tbLicense";
            this.tbLicense.ReadOnly = true;
            this.tbLicense.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbLicense.Size = new System.Drawing.Size(468, 210);
            this.tbLicense.TabIndex = 8;
            // 
            // lblIcons
            // 
            this.lblIcons.AutoSize = true;
            this.lblIcons.Location = new System.Drawing.Point(12, 433);
            this.lblIcons.Name = "lblIcons";
            this.lblIcons.Size = new System.Drawing.Size(468, 13);
            this.lblIcons.TabIndex = 9;
            this.lblIcons.Text = "Some icons by Yusuke Kamiyamane. Licensed under a Creative Commons Attribution 3." +
    "0 License.";
            // 
            // llYusuke
            // 
            this.llYusuke.AutoSize = true;
            this.llYusuke.Location = new System.Drawing.Point(87, 434);
            this.llYusuke.Name = "llYusuke";
            this.llYusuke.Size = new System.Drawing.Size(106, 13);
            this.llYusuke.TabIndex = 10;
            this.llYusuke.TabStop = true;
            this.llYusuke.Text = "Yusuke Kamiyamane";
            this.llYusuke.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llYusuke_LinkClicked);
            // 
            // llCCA30L
            // 
            this.llCCA30L.AutoSize = true;
            this.llCCA30L.Location = new System.Drawing.Point(277, 434);
            this.llCCA30L.Name = "llCCA30L";
            this.llCCA30L.Size = new System.Drawing.Size(203, 13);
            this.llCCA30L.TabIndex = 11;
            this.llCCA30L.TabStop = true;
            this.llCCA30L.Text = "Creative Commons Attribution 3.0 License";
            this.llCCA30L.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llCCA30L_LinkClicked);
            // 
            // About
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(492, 453);
            this.Controls.Add(this.llCCA30L);
            this.Controls.Add(this.llYusuke);
            this.Controls.Add(this.lblIcons);
            this.Controls.Add(this.tbLicense);
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
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "О программе...";
            this.TopMost = true;
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
        private System.Windows.Forms.TextBox tbLicense;
        private System.Windows.Forms.Label lblIcons;
        private System.Windows.Forms.LinkLabel llYusuke;
        private System.Windows.Forms.LinkLabel llCCA30L;
    }
}