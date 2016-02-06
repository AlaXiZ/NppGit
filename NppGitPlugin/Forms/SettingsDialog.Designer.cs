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
            this.gbUsingModules = new System.Windows.Forms.GroupBox();
            this.chlModules = new System.Windows.Forms.CheckedListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbLogLevel = new System.Windows.Forms.ComboBox();
            this.chbDefaultShortcut = new System.Windows.Forms.CheckBox();
            this.tpTortoise = new System.Windows.Forms.TabPage();
            this.tpGitFeatures = new System.Windows.Forms.TabPage();
            this.gbFileInBranch = new System.Windows.Forms.GroupBox();
            this.lSHACount = new System.Windows.Forms.Label();
            this.chbFileInOtherView = new System.Windows.Forms.CheckBox();
            this.mtxbSHACount = new System.Windows.Forms.MaskedTextBox();
            this.pBottom = new System.Windows.Forms.Panel();
            this.pMain = new System.Windows.Forms.Panel();
            this.tabControl1.SuspendLayout();
            this.tpCommon.SuspendLayout();
            this.gbUsingModules.SuspendLayout();
            this.tpTortoise.SuspendLayout();
            this.tpGitFeatures.SuspendLayout();
            this.gbFileInBranch.SuspendLayout();
            this.pBottom.SuspendLayout();
            this.pMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // chbTGToolbar
            // 
            this.chbTGToolbar.AutoSize = true;
            this.chbTGToolbar.Location = new System.Drawing.Point(6, 6);
            this.chbTGToolbar.Name = "chbTGToolbar";
            this.chbTGToolbar.Size = new System.Drawing.Size(170, 17);
            this.chbTGToolbar.TabIndex = 0;
            this.chbTGToolbar.Text = "Показать тулбар TortoiseGit*";
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
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Location = new System.Drawing.Point(3, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(167, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "*После перезапуска Notepad++";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // bOk
            // 
            this.bOk.Dock = System.Windows.Forms.DockStyle.Right;
            this.bOk.Location = new System.Drawing.Point(330, 3);
            this.bOk.Margin = new System.Windows.Forms.Padding(5);
            this.bOk.Name = "bOk";
            this.bOk.Size = new System.Drawing.Size(75, 25);
            this.bOk.TabIndex = 3;
            this.bOk.Text = "OK";
            this.bOk.UseVisualStyleBackColor = true;
            this.bOk.Click += new System.EventHandler(this.bOk_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tpCommon);
            this.tabControl1.Controls.Add(this.tpTortoise);
            this.tabControl1.Controls.Add(this.tpGitFeatures);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(408, 196);
            this.tabControl1.TabIndex = 4;
            // 
            // tpCommon
            // 
            this.tpCommon.Controls.Add(this.gbUsingModules);
            this.tpCommon.Controls.Add(this.label2);
            this.tpCommon.Controls.Add(this.cbLogLevel);
            this.tpCommon.Controls.Add(this.chbDefaultShortcut);
            this.tpCommon.Location = new System.Drawing.Point(4, 22);
            this.tpCommon.Name = "tpCommon";
            this.tpCommon.Padding = new System.Windows.Forms.Padding(3);
            this.tpCommon.Size = new System.Drawing.Size(400, 170);
            this.tpCommon.TabIndex = 0;
            this.tpCommon.Text = "Общие";
            this.tpCommon.UseVisualStyleBackColor = true;
            // 
            // gbUsingModules
            // 
            this.gbUsingModules.Controls.Add(this.chlModules);
            this.gbUsingModules.Location = new System.Drawing.Point(8, 56);
            this.gbUsingModules.Name = "gbUsingModules";
            this.gbUsingModules.Size = new System.Drawing.Size(389, 65);
            this.gbUsingModules.TabIndex = 8;
            this.gbUsingModules.TabStop = false;
            this.gbUsingModules.Text = "Включенные модули*";
            // 
            // chlModules
            // 
            this.chlModules.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chlModules.FormattingEnabled = true;
            this.chlModules.Items.AddRange(new object[] {
            "Tortoise Git",
            "Git Features",
            "SQL IDE"});
            this.chlModules.Location = new System.Drawing.Point(3, 16);
            this.chlModules.MultiColumn = true;
            this.chlModules.Name = "chlModules";
            this.chlModules.Size = new System.Drawing.Size(383, 46);
            this.chlModules.TabIndex = 0;
            this.chlModules.SelectedValueChanged += new System.EventHandler(this.chlModules_SelectedValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(127, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Уровень логгирования:";
            // 
            // cbLogLevel
            // 
            this.cbLogLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLogLevel.FormattingEnabled = true;
            this.cbLogLevel.Location = new System.Drawing.Point(136, 29);
            this.cbLogLevel.Name = "cbLogLevel";
            this.cbLogLevel.Size = new System.Drawing.Size(84, 21);
            this.cbLogLevel.TabIndex = 6;
            // 
            // chbDefaultShortcut
            // 
            this.chbDefaultShortcut.AutoSize = true;
            this.chbDefaultShortcut.Location = new System.Drawing.Point(6, 6);
            this.chbDefaultShortcut.Name = "chbDefaultShortcut";
            this.chbDefaultShortcut.Size = new System.Drawing.Size(184, 17);
            this.chbDefaultShortcut.TabIndex = 0;
            this.chbDefaultShortcut.Text = "Горячие кнопки по умолчанию*";
            this.chbDefaultShortcut.UseVisualStyleBackColor = true;
            // 
            // tpTortoise
            // 
            this.tpTortoise.Controls.Add(this.chbTGToolbar);
            this.tpTortoise.Controls.Add(this.chlButtons);
            this.tpTortoise.Location = new System.Drawing.Point(4, 22);
            this.tpTortoise.Name = "tpTortoise";
            this.tpTortoise.Padding = new System.Windows.Forms.Padding(3);
            this.tpTortoise.Size = new System.Drawing.Size(400, 170);
            this.tpTortoise.TabIndex = 1;
            this.tpTortoise.Text = "TortoiseGit";
            this.tpTortoise.UseVisualStyleBackColor = true;
            // 
            // tpGitFeatures
            // 
            this.tpGitFeatures.Controls.Add(this.gbFileInBranch);
            this.tpGitFeatures.Location = new System.Drawing.Point(4, 22);
            this.tpGitFeatures.Name = "tpGitFeatures";
            this.tpGitFeatures.Padding = new System.Windows.Forms.Padding(3);
            this.tpGitFeatures.Size = new System.Drawing.Size(400, 170);
            this.tpGitFeatures.TabIndex = 2;
            this.tpGitFeatures.Text = "GitFeatures";
            this.tpGitFeatures.UseVisualStyleBackColor = true;
            // 
            // gbFileInBranch
            // 
            this.gbFileInBranch.Controls.Add(this.lSHACount);
            this.gbFileInBranch.Controls.Add(this.chbFileInOtherView);
            this.gbFileInBranch.Controls.Add(this.mtxbSHACount);
            this.gbFileInBranch.Location = new System.Drawing.Point(6, 6);
            this.gbFileInBranch.Name = "gbFileInBranch";
            this.gbFileInBranch.Size = new System.Drawing.Size(388, 65);
            this.gbFileInBranch.TabIndex = 6;
            this.gbFileInBranch.TabStop = false;
            this.gbFileInBranch.Text = "Открыть файл из др. ветки";
            // 
            // lSHACount
            // 
            this.lSHACount.AutoSize = true;
            this.lSHACount.Location = new System.Drawing.Point(6, 16);
            this.lSHACount.Name = "lSHACount";
            this.lSHACount.Size = new System.Drawing.Size(217, 13);
            this.lSHACount.TabIndex = 2;
            this.lSHACount.Text = "Размер SHA в имени временного файла:";
            // 
            // chbFileInOtherView
            // 
            this.chbFileInOtherView.AutoSize = true;
            this.chbFileInOtherView.Location = new System.Drawing.Point(9, 41);
            this.chbFileInOtherView.Name = "chbFileInOtherView";
            this.chbFileInOtherView.Size = new System.Drawing.Size(172, 17);
            this.chbFileInOtherView.TabIndex = 4;
            this.chbFileInOtherView.Text = "Открывать в другой области";
            this.chbFileInOtherView.UseVisualStyleBackColor = true;
            // 
            // mtxbSHACount
            // 
            this.mtxbSHACount.Location = new System.Drawing.Point(229, 13);
            this.mtxbSHACount.Mask = "00";
            this.mtxbSHACount.Name = "mtxbSHACount";
            this.mtxbSHACount.Size = new System.Drawing.Size(29, 20);
            this.mtxbSHACount.TabIndex = 3;
            // 
            // pBottom
            // 
            this.pBottom.Controls.Add(this.label1);
            this.pBottom.Controls.Add(this.bOk);
            this.pBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pBottom.Location = new System.Drawing.Point(0, 196);
            this.pBottom.Name = "pBottom";
            this.pBottom.Padding = new System.Windows.Forms.Padding(3);
            this.pBottom.Size = new System.Drawing.Size(408, 31);
            this.pBottom.TabIndex = 5;
            // 
            // pMain
            // 
            this.pMain.Controls.Add(this.tabControl1);
            this.pMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pMain.Location = new System.Drawing.Point(0, 0);
            this.pMain.Name = "pMain";
            this.pMain.Size = new System.Drawing.Size(408, 196);
            this.pMain.TabIndex = 6;
            // 
            // SettingsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(408, 227);
            this.Controls.Add(this.pMain);
            this.Controls.Add(this.pBottom);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(3, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Настройки";
            this.TopMost = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SettingsDialog_FormClosed);
            this.tabControl1.ResumeLayout(false);
            this.tpCommon.ResumeLayout(false);
            this.tpCommon.PerformLayout();
            this.gbUsingModules.ResumeLayout(false);
            this.tpTortoise.ResumeLayout(false);
            this.tpTortoise.PerformLayout();
            this.tpGitFeatures.ResumeLayout(false);
            this.gbFileInBranch.ResumeLayout(false);
            this.gbFileInBranch.PerformLayout();
            this.pBottom.ResumeLayout(false);
            this.pBottom.PerformLayout();
            this.pMain.ResumeLayout(false);
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
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbLogLevel;
        private System.Windows.Forms.TabPage tpGitFeatures;
        private System.Windows.Forms.Panel pBottom;
        private System.Windows.Forms.Panel pMain;
        private System.Windows.Forms.MaskedTextBox mtxbSHACount;
        private System.Windows.Forms.CheckBox chbFileInOtherView;
        private System.Windows.Forms.Label lSHACount;
        private System.Windows.Forms.GroupBox gbFileInBranch;
        private System.Windows.Forms.GroupBox gbUsingModules;
        private System.Windows.Forms.CheckedListBox chlModules;
    }
}