namespace NppGit.Forms
{
    partial class SettingsDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsDialog));
            this.chbTGToolbar = new System.Windows.Forms.CheckBox();
            this.chlButtons = new System.Windows.Forms.CheckedListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.bOk = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpCommon = new System.Windows.Forms.TabPage();
            this.mtxbSHACount = new System.Windows.Forms.MaskedTextBox();
            this.lSHACount = new System.Windows.Forms.Label();
            this.chbDefaultShortcut = new System.Windows.Forms.CheckBox();
            this.tpTortoise = new System.Windows.Forms.TabPage();
            this.chbFileInOtherView = new System.Windows.Forms.CheckBox();
            this.gbFileInBranch = new System.Windows.Forms.GroupBox();
            this.tabControl1.SuspendLayout();
            this.tpCommon.SuspendLayout();
            this.tpTortoise.SuspendLayout();
            this.gbFileInBranch.SuspendLayout();
            this.SuspendLayout();
            // 
            // chbTGToolbar
            // 
            this.chbTGToolbar.AutoSize = true;
            this.chbTGToolbar.Location = new System.Drawing.Point(6, 6);
            this.chbTGToolbar.Name = "chbTGToolbar";
            this.chbTGToolbar.Size = new System.Drawing.Size(142, 17);
            this.chbTGToolbar.TabIndex = 0;
            this.chbTGToolbar.Text = "Show TortoiseGit toolbar";
            this.chbTGToolbar.UseVisualStyleBackColor = true;
            this.chbTGToolbar.CheckedChanged += new System.EventHandler(this.chbTGToolbar_CheckedChanged);
            // 
            // chlButtons
            // 
            this.chlButtons.CheckOnClick = true;
            this.chlButtons.FormattingEnabled = true;
            this.chlButtons.Items.AddRange(new object[] {
            "Fetch",
            "Pull",
            "Commit",
            "Push",
            "Blame",
            "Log",
            "Stash save",
            "Stash pop",
            "Repo status",
            "Switch"});
            this.chlButtons.Location = new System.Drawing.Point(6, 29);
            this.chlButtons.MultiColumn = true;
            this.chlButtons.Name = "chlButtons";
            this.chlButtons.Size = new System.Drawing.Size(378, 64);
            this.chlButtons.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 96);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "*Only after restart NPP";
            // 
            // bOk
            // 
            this.bOk.Location = new System.Drawing.Point(326, 166);
            this.bOk.Name = "bOk";
            this.bOk.Size = new System.Drawing.Size(70, 25);
            this.bOk.TabIndex = 3;
            this.bOk.Text = "OK";
            this.bOk.UseVisualStyleBackColor = true;
            this.bOk.Click += new System.EventHandler(this.bOk_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tpCommon);
            this.tabControl1.Controls.Add(this.tpTortoise);
            this.tabControl1.Location = new System.Drawing.Point(3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(400, 157);
            this.tabControl1.TabIndex = 4;
            // 
            // tpCommon
            // 
            this.tpCommon.Controls.Add(this.gbFileInBranch);
            this.tpCommon.Controls.Add(this.chbDefaultShortcut);
            this.tpCommon.Location = new System.Drawing.Point(4, 22);
            this.tpCommon.Name = "tpCommon";
            this.tpCommon.Padding = new System.Windows.Forms.Padding(3);
            this.tpCommon.Size = new System.Drawing.Size(392, 131);
            this.tpCommon.TabIndex = 0;
            this.tpCommon.Text = "Common";
            this.tpCommon.UseVisualStyleBackColor = true;
            // 
            // mtxbSHACount
            // 
            this.mtxbSHACount.Location = new System.Drawing.Point(146, 13);
            this.mtxbSHACount.Mask = "00";
            this.mtxbSHACount.Name = "mtxbSHACount";
            this.mtxbSHACount.Size = new System.Drawing.Size(29, 20);
            this.mtxbSHACount.TabIndex = 3;
            this.mtxbSHACount.Leave += new System.EventHandler(this.mtxbSHACount_Leave);
            // 
            // lSHACount
            // 
            this.lSHACount.AutoSize = true;
            this.lSHACount.Location = new System.Drawing.Point(6, 16);
            this.lSHACount.Name = "lSHACount";
            this.lSHACount.Size = new System.Drawing.Size(131, 13);
            this.lSHACount.TabIndex = 2;
            this.lSHACount.Text = "Size SHA in name tmp-file:";
            // 
            // chbDefaultShortcut
            // 
            this.chbDefaultShortcut.AutoSize = true;
            this.chbDefaultShortcut.Location = new System.Drawing.Point(6, 6);
            this.chbDefaultShortcut.Name = "chbDefaultShortcut";
            this.chbDefaultShortcut.Size = new System.Drawing.Size(118, 17);
            this.chbDefaultShortcut.TabIndex = 0;
            this.chbDefaultShortcut.Text = "Set default shortcut";
            this.chbDefaultShortcut.UseVisualStyleBackColor = true;
            // 
            // tpTortoise
            // 
            this.tpTortoise.Controls.Add(this.chbTGToolbar);
            this.tpTortoise.Controls.Add(this.chlButtons);
            this.tpTortoise.Controls.Add(this.label1);
            this.tpTortoise.Location = new System.Drawing.Point(4, 22);
            this.tpTortoise.Name = "tpTortoise";
            this.tpTortoise.Padding = new System.Windows.Forms.Padding(3);
            this.tpTortoise.Size = new System.Drawing.Size(392, 131);
            this.tpTortoise.TabIndex = 1;
            this.tpTortoise.Text = "TortoiseGit";
            this.tpTortoise.UseVisualStyleBackColor = true;
            // 
            // chbFileInOtherView
            // 
            this.chbFileInOtherView.AutoSize = true;
            this.chbFileInOtherView.Location = new System.Drawing.Point(9, 41);
            this.chbFileInOtherView.Name = "chbFileInOtherView";
            this.chbFileInOtherView.Size = new System.Drawing.Size(131, 17);
            this.chbFileInOtherView.TabIndex = 4;
            this.chbFileInOtherView.Text = "Open file in other view";
            this.chbFileInOtherView.UseVisualStyleBackColor = true;
            // 
            // gbFileInBranch
            // 
            this.gbFileInBranch.Controls.Add(this.lSHACount);
            this.gbFileInBranch.Controls.Add(this.chbFileInOtherView);
            this.gbFileInBranch.Controls.Add(this.mtxbSHACount);
            this.gbFileInBranch.Location = new System.Drawing.Point(1, 25);
            this.gbFileInBranch.Name = "gbFileInBranch";
            this.gbFileInBranch.Size = new System.Drawing.Size(388, 65);
            this.gbFileInBranch.TabIndex = 5;
            this.gbFileInBranch.TabStop = false;
            this.gbFileInBranch.Text = "Open file in other branch";
            // 
            // SettingsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(406, 196);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.bOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(3, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Settings";
            this.TopMost = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SettingsDialog_FormClosed);
            this.tabControl1.ResumeLayout(false);
            this.tpCommon.ResumeLayout(false);
            this.tpCommon.PerformLayout();
            this.tpTortoise.ResumeLayout(false);
            this.tpTortoise.PerformLayout();
            this.gbFileInBranch.ResumeLayout(false);
            this.gbFileInBranch.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox chbTGToolbar;
        private System.Windows.Forms.CheckedListBox chlButtons;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button bOk;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpCommon;
        private System.Windows.Forms.TabPage tpTortoise;
        private System.Windows.Forms.CheckBox chbDefaultShortcut;
        private System.Windows.Forms.Label lSHACount;
        private System.Windows.Forms.MaskedTextBox mtxbSHACount;
        private System.Windows.Forms.CheckBox chbFileInOtherView;
        private System.Windows.Forms.GroupBox gbFileInBranch;
    }
}