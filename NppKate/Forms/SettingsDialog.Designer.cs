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
            this.label1 = new System.Windows.Forms.Label();
            this.bOk = new System.Windows.Forms.Button();
            this.tbSettings = new System.Windows.Forms.TabControl();
            this.tpCommon = new System.Windows.Forms.TabPage();
            this.gbUsingModules = new System.Windows.Forms.GroupBox();
            this.chlModules = new System.Windows.Forms.CheckedListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbLogLevel = new System.Windows.Forms.ComboBox();
            this.chbDefaultShortcut = new System.Windows.Forms.CheckBox();
            this.tpTortoise = new System.Windows.Forms.TabPage();
            this.tcCommands = new System.Windows.Forms.TabControl();
            this.tpMenu = new System.Windows.Forms.TabPage();
            this.tvMenuCommand = new System.Windows.Forms.TreeView();
            this.tpToolbar = new System.Windows.Forms.TabPage();
            this.tvToolbarCommand = new System.Windows.Forms.TreeView();
            this.bSelectFolder = new System.Windows.Forms.Button();
            this.tbTGProcPath = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tpGitFeatures = new System.Windows.Forms.TabPage();
            this.chbAutoExpand = new System.Windows.Forms.CheckBox();
            this.gbFileInBranch = new System.Windows.Forms.GroupBox();
            this.lSHACount = new System.Windows.Forms.Label();
            this.chbFileInOtherView = new System.Windows.Forms.CheckBox();
            this.mtxbSHACount = new System.Windows.Forms.MaskedTextBox();
            this.tpSnippets = new System.Windows.Forms.TabPage();
            this.label4 = new System.Windows.Forms.Label();
            this.udNestedLEvel = new System.Windows.Forms.DomainUpDown();
            this.chbInsertEmpty = new System.Windows.Forms.CheckBox();
            this.chbExpand = new System.Windows.Forms.CheckBox();
            this.chbHideByExt = new System.Windows.Forms.CheckBox();
            this.chbGroupByCategory = new System.Windows.Forms.CheckBox();
            this.pBottom = new System.Windows.Forms.Panel();
            this.llWiki = new System.Windows.Forms.LinkLabel();
            this.chbRestartNpp = new System.Windows.Forms.CheckBox();
            this.pMain = new System.Windows.Forms.Panel();
            this.tbSettings.SuspendLayout();
            this.tpCommon.SuspendLayout();
            this.gbUsingModules.SuspendLayout();
            this.tpTortoise.SuspendLayout();
            this.tcCommands.SuspendLayout();
            this.tpMenu.SuspendLayout();
            this.tpToolbar.SuspendLayout();
            this.tpGitFeatures.SuspendLayout();
            this.gbFileInBranch.SuspendLayout();
            this.tpSnippets.SuspendLayout();
            this.pBottom.SuspendLayout();
            this.pMain.SuspendLayout();
            this.SuspendLayout();
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
            this.bOk.Location = new System.Drawing.Point(331, 3);
            this.bOk.Margin = new System.Windows.Forms.Padding(5);
            this.bOk.Name = "bOk";
            this.bOk.Size = new System.Drawing.Size(75, 29);
            this.bOk.TabIndex = 3;
            this.bOk.Text = "OK";
            this.bOk.UseVisualStyleBackColor = true;
            this.bOk.Click += new System.EventHandler(this.bOk_Click);
            // 
            // tbSettings
            // 
            this.tbSettings.Controls.Add(this.tpCommon);
            this.tbSettings.Controls.Add(this.tpTortoise);
            this.tbSettings.Controls.Add(this.tpGitFeatures);
            this.tbSettings.Controls.Add(this.tpSnippets);
            this.tbSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbSettings.Location = new System.Drawing.Point(0, 0);
            this.tbSettings.Name = "tbSettings";
            this.tbSettings.SelectedIndex = 0;
            this.tbSettings.Size = new System.Drawing.Size(409, 227);
            this.tbSettings.TabIndex = 4;
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
            this.tpCommon.Size = new System.Drawing.Size(401, 201);
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
            "SQL IDE",
            "Snippets"});
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
            this.label2.Size = new System.Drawing.Size(122, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Уровень логирования:";
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
            this.tpTortoise.Controls.Add(this.tcCommands);
            this.tpTortoise.Controls.Add(this.bSelectFolder);
            this.tpTortoise.Controls.Add(this.tbTGProcPath);
            this.tpTortoise.Controls.Add(this.label3);
            this.tpTortoise.Location = new System.Drawing.Point(4, 22);
            this.tpTortoise.Name = "tpTortoise";
            this.tpTortoise.Padding = new System.Windows.Forms.Padding(3);
            this.tpTortoise.Size = new System.Drawing.Size(401, 201);
            this.tpTortoise.TabIndex = 1;
            this.tpTortoise.Text = "TortoiseGit";
            this.tpTortoise.UseVisualStyleBackColor = true;
            // 
            // tcCommands
            // 
            this.tcCommands.Controls.Add(this.tpMenu);
            this.tcCommands.Controls.Add(this.tpToolbar);
            this.tcCommands.Location = new System.Drawing.Point(8, 46);
            this.tcCommands.Name = "tcCommands";
            this.tcCommands.SelectedIndex = 0;
            this.tcCommands.Size = new System.Drawing.Size(385, 152);
            this.tcCommands.TabIndex = 5;
            // 
            // tpMenu
            // 
            this.tpMenu.Controls.Add(this.tvMenuCommand);
            this.tpMenu.ForeColor = System.Drawing.Color.Black;
            this.tpMenu.Location = new System.Drawing.Point(4, 22);
            this.tpMenu.Name = "tpMenu";
            this.tpMenu.Padding = new System.Windows.Forms.Padding(3);
            this.tpMenu.Size = new System.Drawing.Size(377, 126);
            this.tpMenu.TabIndex = 0;
            this.tpMenu.Text = "In menu *";
            this.tpMenu.UseVisualStyleBackColor = true;
            // 
            // tvMenuCommand
            // 
            this.tvMenuCommand.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvMenuCommand.Location = new System.Drawing.Point(3, 3);
            this.tvMenuCommand.Name = "tvMenuCommand";
            this.tvMenuCommand.Size = new System.Drawing.Size(371, 120);
            this.tvMenuCommand.TabIndex = 0;
            this.tvMenuCommand.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tvMenuCommand_AfterCheck);
            // 
            // tpToolbar
            // 
            this.tpToolbar.Controls.Add(this.tvToolbarCommand);
            this.tpToolbar.Location = new System.Drawing.Point(4, 22);
            this.tpToolbar.Name = "tpToolbar";
            this.tpToolbar.Padding = new System.Windows.Forms.Padding(3);
            this.tpToolbar.Size = new System.Drawing.Size(377, 126);
            this.tpToolbar.TabIndex = 1;
            this.tpToolbar.Text = "In toolbar *";
            this.tpToolbar.UseVisualStyleBackColor = true;
            // 
            // tvToolbarCommand
            // 
            this.tvToolbarCommand.CheckBoxes = true;
            this.tvToolbarCommand.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvToolbarCommand.Location = new System.Drawing.Point(3, 3);
            this.tvToolbarCommand.Name = "tvToolbarCommand";
            this.tvToolbarCommand.Size = new System.Drawing.Size(371, 120);
            this.tvToolbarCommand.TabIndex = 1;
            this.tvToolbarCommand.BeforeCheck += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvToolbarCommand_BeforeCheck);
            this.tvToolbarCommand.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvToolbarCommand_BeforeCheck);
            // 
            // bSelectFolder
            // 
            this.bSelectFolder.Location = new System.Drawing.Point(366, 18);
            this.bSelectFolder.Name = "bSelectFolder";
            this.bSelectFolder.Size = new System.Drawing.Size(28, 23);
            this.bSelectFolder.TabIndex = 4;
            this.bSelectFolder.Text = "...";
            this.bSelectFolder.UseVisualStyleBackColor = true;
            this.bSelectFolder.Click += new System.EventHandler(this.bSelectFolder_Click);
            // 
            // tbTGProcPath
            // 
            this.tbTGProcPath.Location = new System.Drawing.Point(3, 20);
            this.tbTGProcPath.Name = "tbTGProcPath";
            this.tbTGProcPath.ReadOnly = true;
            this.tbTGProcPath.Size = new System.Drawing.Size(357, 20);
            this.tbTGProcPath.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(0, 4);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(146, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Путь до TortoiseGitProc.exe*";
            // 
            // tpGitFeatures
            // 
            this.tpGitFeatures.Controls.Add(this.chbAutoExpand);
            this.tpGitFeatures.Controls.Add(this.gbFileInBranch);
            this.tpGitFeatures.Location = new System.Drawing.Point(4, 22);
            this.tpGitFeatures.Name = "tpGitFeatures";
            this.tpGitFeatures.Padding = new System.Windows.Forms.Padding(3);
            this.tpGitFeatures.Size = new System.Drawing.Size(401, 201);
            this.tpGitFeatures.TabIndex = 2;
            this.tpGitFeatures.Text = "Git";
            this.tpGitFeatures.UseVisualStyleBackColor = true;
            // 
            // chbAutoExpand
            // 
            this.chbAutoExpand.AutoSize = true;
            this.chbAutoExpand.Location = new System.Drawing.Point(15, 77);
            this.chbAutoExpand.Name = "chbAutoExpand";
            this.chbAutoExpand.Size = new System.Drawing.Size(201, 17);
            this.chbAutoExpand.TabIndex = 7;
            this.chbAutoExpand.Text = "Auto expand/collapse after activated";
            this.chbAutoExpand.UseVisualStyleBackColor = true;
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
            this.mtxbSHACount.Leave += new System.EventHandler(this.mtxbSHACount_Leave);
            // 
            // tpSnippets
            // 
            this.tpSnippets.Controls.Add(this.label4);
            this.tpSnippets.Controls.Add(this.udNestedLEvel);
            this.tpSnippets.Controls.Add(this.chbInsertEmpty);
            this.tpSnippets.Controls.Add(this.chbExpand);
            this.tpSnippets.Controls.Add(this.chbHideByExt);
            this.tpSnippets.Controls.Add(this.chbGroupByCategory);
            this.tpSnippets.Location = new System.Drawing.Point(4, 22);
            this.tpSnippets.Name = "tpSnippets";
            this.tpSnippets.Padding = new System.Windows.Forms.Padding(3);
            this.tpSnippets.Size = new System.Drawing.Size(401, 201);
            this.tpSnippets.TabIndex = 3;
            this.tpSnippets.Text = "Snippets";
            this.tpSnippets.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 100);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(87, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Max nested level";
            // 
            // udNestedLEvel
            // 
            this.udNestedLEvel.Location = new System.Drawing.Point(101, 98);
            this.udNestedLEvel.Name = "udNestedLEvel";
            this.udNestedLEvel.ReadOnly = true;
            this.udNestedLEvel.Size = new System.Drawing.Size(65, 20);
            this.udNestedLEvel.TabIndex = 4;
            this.udNestedLEvel.Text = "5";
            // 
            // chbInsertEmpty
            // 
            this.chbInsertEmpty.AutoSize = true;
            this.chbInsertEmpty.Location = new System.Drawing.Point(8, 75);
            this.chbInsertEmpty.Name = "chbInsertEmpty";
            this.chbInsertEmpty.Size = new System.Drawing.Size(136, 17);
            this.chbInsertEmpty.TabIndex = 3;
            this.chbInsertEmpty.Text = "Insert EMPTY_PARAM";
            this.chbInsertEmpty.UseVisualStyleBackColor = true;
            // 
            // chbExpand
            // 
            this.chbExpand.AutoSize = true;
            this.chbExpand.Location = new System.Drawing.Point(8, 52);
            this.chbExpand.Name = "chbExpand";
            this.chbExpand.Size = new System.Drawing.Size(119, 17);
            this.chbExpand.TabIndex = 2;
            this.chbExpand.Text = "Expand after create";
            this.chbExpand.UseVisualStyleBackColor = true;
            // 
            // chbHideByExt
            // 
            this.chbHideByExt.AutoSize = true;
            this.chbHideByExt.Location = new System.Drawing.Point(8, 29);
            this.chbHideByExt.Name = "chbHideByExt";
            this.chbHideByExt.Size = new System.Drawing.Size(118, 17);
            this.chbHideByExt.TabIndex = 1;
            this.chbHideByExt.Text = "Hiding by extension";
            this.chbHideByExt.UseVisualStyleBackColor = true;
            // 
            // chbGroupByCategory
            // 
            this.chbGroupByCategory.AutoSize = true;
            this.chbGroupByCategory.Location = new System.Drawing.Point(8, 6);
            this.chbGroupByCategory.Name = "chbGroupByCategory";
            this.chbGroupByCategory.Size = new System.Drawing.Size(127, 17);
            this.chbGroupByCategory.TabIndex = 0;
            this.chbGroupByCategory.Text = "Grouping by category";
            this.chbGroupByCategory.UseVisualStyleBackColor = true;
            // 
            // pBottom
            // 
            this.pBottom.Controls.Add(this.llWiki);
            this.pBottom.Controls.Add(this.chbRestartNpp);
            this.pBottom.Controls.Add(this.label1);
            this.pBottom.Controls.Add(this.bOk);
            this.pBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pBottom.Location = new System.Drawing.Point(0, 227);
            this.pBottom.Name = "pBottom";
            this.pBottom.Padding = new System.Windows.Forms.Padding(3);
            this.pBottom.Size = new System.Drawing.Size(409, 35);
            this.pBottom.TabIndex = 5;
            // 
            // llWiki
            // 
            this.llWiki.AutoSize = true;
            this.llWiki.Location = new System.Drawing.Point(6, 19);
            this.llWiki.Name = "llWiki";
            this.llWiki.Size = new System.Drawing.Size(28, 13);
            this.llWiki.TabIndex = 5;
            this.llWiki.TabStop = true;
            this.llWiki.Text = "Wiki";
            this.llWiki.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llWiki_LinkClicked);
            // 
            // chbRestartNpp
            // 
            this.chbRestartNpp.AutoSize = true;
            this.chbRestartNpp.Location = new System.Drawing.Point(206, 8);
            this.chbRestartNpp.Name = "chbRestartNpp";
            this.chbRestartNpp.Size = new System.Drawing.Size(116, 17);
            this.chbRestartNpp.TabIndex = 4;
            this.chbRestartNpp.Text = "Restart Notepad++";
            this.chbRestartNpp.UseVisualStyleBackColor = true;
            // 
            // pMain
            // 
            this.pMain.Controls.Add(this.tbSettings);
            this.pMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pMain.Location = new System.Drawing.Point(0, 0);
            this.pMain.Name = "pMain";
            this.pMain.Size = new System.Drawing.Size(409, 227);
            this.pMain.TabIndex = 6;
            // 
            // SettingsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(409, 262);
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
            this.tbSettings.ResumeLayout(false);
            this.tpCommon.ResumeLayout(false);
            this.tpCommon.PerformLayout();
            this.gbUsingModules.ResumeLayout(false);
            this.tpTortoise.ResumeLayout(false);
            this.tpTortoise.PerformLayout();
            this.tcCommands.ResumeLayout(false);
            this.tpMenu.ResumeLayout(false);
            this.tpToolbar.ResumeLayout(false);
            this.tpGitFeatures.ResumeLayout(false);
            this.tpGitFeatures.PerformLayout();
            this.gbFileInBranch.ResumeLayout(false);
            this.gbFileInBranch.PerformLayout();
            this.tpSnippets.ResumeLayout(false);
            this.tpSnippets.PerformLayout();
            this.pBottom.ResumeLayout(false);
            this.pBottom.PerformLayout();
            this.pMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button bOk;
        private System.Windows.Forms.TabControl tbSettings;
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
        private System.Windows.Forms.CheckBox chbRestartNpp;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button bSelectFolder;
        private System.Windows.Forms.TextBox tbTGProcPath;
        private System.Windows.Forms.TabPage tpSnippets;
        private System.Windows.Forms.CheckBox chbHideByExt;
        private System.Windows.Forms.CheckBox chbGroupByCategory;
        private System.Windows.Forms.CheckBox chbExpand;
        private System.Windows.Forms.CheckBox chbAutoExpand;
        private System.Windows.Forms.CheckBox chbInsertEmpty;
        private System.Windows.Forms.LinkLabel llWiki;
        private System.Windows.Forms.TabControl tcCommands;
        private System.Windows.Forms.TabPage tpMenu;
        private System.Windows.Forms.TabPage tpToolbar;
        private System.Windows.Forms.TreeView tvMenuCommand;
        private System.Windows.Forms.TreeView tvToolbarCommand;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DomainUpDown udNestedLEvel;
    }
}