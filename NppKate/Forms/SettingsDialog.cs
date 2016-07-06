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

using NppKate.Common;
using NppKate.Modules.TortoiseGitFeatures;
using NppKate.Npp;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace NppKate.Forms
{
    public partial class SettingsDialog : Form
    {
        readonly System.Drawing.Color DisableColor = System.Drawing.Color.Gray;

        private static readonly List<NLog.LogLevel> _logLevel = new List<NLog.LogLevel>
        {
            NLog.LogLevel.Off,
            NLog.LogLevel.Fatal,
            NLog.LogLevel.Error,
            NLog.LogLevel.Warn,
            NLog.LogLevel.Info,
            NLog.LogLevel.Debug,
            NLog.LogLevel.Trace
        };

        private CommandManager _commandManager;

        public SettingsDialog(CommandManager commandManager = null)
        {
            InitializeComponent();

            for (short i = 1; i < 1025; i++)
            {
                udNestedLEvel.Items.Add(i);
            }
            _commandManager = commandManager;
            LoadSettings();
        }

        private void LoadSettings()
        {
            //chbTGToolbar.Checked = Settings.TortoiseGitProc.ShowToolbar;
            //chlButtons.Enabled = chbTGToolbar.Checked;
            //var mask = Settings.TortoiseGitProc.ButtonMask;
            //for (int i = 0; i < chlButtons.Items.Count; i++)
            //{
            //    chlButtons.SetItemChecked(i, (mask & (1u << i)) > 0);
            //}
            tbTGProcPath.Text = Settings.TortoiseGitProc.Path;
            LoadTortoiseCommands();

            chbDefaultShortcut.Checked = Settings.CommonSettings.IsSetDefaultShortcut;
            mtxbSHACount.Text = Settings.Functions.SHACount.ToString();
            chbFileInOtherView.Checked = Settings.Functions.OpenFileInOtherView;
            chbAutoExpand.Checked = Settings.GitCore.AutoExpand;

            cbLogLevel.Items.Clear();
            cbLogLevel.Items.AddRange(_logLevel.ToArray());
            cbLogLevel.Text = Settings.CommonSettings.LogLevel;

            chlModules.SetItemChecked(0, Settings.Modules.TortoiseGit);
            chlModules.SetItemChecked(1, Settings.Modules.Git);
            chlModules.SetItemChecked(2, Settings.Modules.SQLIDE);
            chlModules.SetItemChecked(3, Settings.Modules.Snippets);

            //chlModules.SetItemChecked(4, Settings.Modules.PSSE);

            chbGroupByCategory.Checked = Settings.Snippets.IsGroupByCategory;
            chbHideByExt.Checked = Settings.Snippets.IsHideByExtention;
            chbExpand.Checked = Settings.Snippets.IsExpanAfterCreate;
            chbInsertEmpty.Checked = Settings.Snippets.InsertEmpty;
            udNestedLEvel.SelectedIndex = Settings.Snippets.MaxLevel - 1;

        }

        private uint GetButtonMask()
        {
            uint result = 0u;
            //for (int i = 0; i < chlButtons.Items.Count; i++)
            //{
            //    result |= (chlButtons.GetItemChecked(i) ? 1u : 0) << i;
            //}
            return result;
        }

        private void LoadTortoiseCommands()
        {
            // TODO: Load command in tree view
            if (_commandManager == null) return;
            var tgName = typeof(TortoiseGitHelper).Name;
            var commands = _commandManager.GetCommandsByModule(tgName);

            tvMenuCommand.BeginUpdate();
            tvToolbarCommand.BeginUpdate();
            try
            {
                foreach (var cmd in commands)
                {
                    var node2 = tvToolbarCommand.Nodes.Add(cmd.Name, cmd.Name);
                    var node = tvMenuCommand.Nodes.Add(cmd.Name, cmd.Name);
                    node2.Checked = Settings.CommonSettings.GetToolbarCommandState(tgName, cmd.Name);
                    node.Checked = Settings.CommonSettings.GetCommandState(tgName, cmd.Name);
                }
            }
            finally
            {
                tvMenuCommand.EndUpdate();
                tvToolbarCommand.EndUpdate();
            }
            tvMenuCommand.CheckBoxes = true;
            tvToolbarCommand.CheckBoxes = true;
        }

        private void chbTGToolbar_CheckedChanged(object sender, EventArgs e)
        {
            //chlButtons.Enabled = chbTGToolbar.Checked;
        }

        private void SettingsDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        private void SaveSettings()
        {
            Settings.CommonSettings.IsSetDefaultShortcut = chbDefaultShortcut.Checked;
            Settings.CommonSettings.LogLevel = cbLogLevel.Text;
            //Settings.TortoiseGitProc.ShowToolbar = chbTGToolbar.Checked;
            //Settings.TortoiseGitProc.ButtonMask = GetButtonMask();
            Settings.TortoiseGitProc.Path = tbTGProcPath.Text;
            SaveTortoiseGitCommand();
            Settings.Functions.SHACount = byte.Parse(mtxbSHACount.Text);
            Settings.Functions.OpenFileInOtherView = chbFileInOtherView.Checked;
            Settings.GitCore.AutoExpand = chbAutoExpand.Checked;

            // Modules state
            Settings.Modules.TortoiseGit = chlModules.GetItemChecked(0);
            Settings.Modules.Git = chlModules.GetItemChecked(1);
            Settings.Modules.SQLIDE = chlModules.GetItemChecked(2);
            Settings.Modules.Snippets = chlModules.GetItemChecked(3);

            // Snippet settings
            Settings.Snippets.IsGroupByCategory = chbGroupByCategory.Checked;
            Settings.Snippets.IsHideByExtention = chbHideByExt.Checked;
            Settings.Snippets.IsExpanAfterCreate = chbExpand.Checked;
            Settings.Snippets.InsertEmpty = chbInsertEmpty.Checked;
            Settings.Snippets.MaxLevel = (ushort)(udNestedLEvel.SelectedIndex + 1);
        }

        private void SaveTortoiseGitCommand()
        {
            if (_commandManager == null) return;
            var tgName = typeof(TortoiseGitHelper).Name;

            foreach (TreeNode node in tvMenuCommand.Nodes)
            {
                Settings.CommonSettings.SetCommandState(tgName, node.Text, node.Checked);
            }
            foreach (TreeNode node in tvToolbarCommand.Nodes)
            {
                Settings.CommonSettings.SetToolbarCommandState(tgName, node.Text, node.Checked);
            }
        }

        private void bOk_Click(object sender, EventArgs e)
        {
            SaveSettings();
            if (chbRestartNpp.Checked)
            {
                NppUtils.Restart();
            }
            else
            {
                Close();
            }
        }

        private void mtxbSHACount_Leave(object sender, EventArgs e)
        {
            byte result;
            if (!byte.TryParse(mtxbSHACount.Text, out result) || result > 20 || result == 0)
            {
                MessageBox.Show("�������� ������ ���� � ��������� [1..20]", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                mtxbSHACount.Focus();
            }
        }

        private void chlModules_SelectedValueChanged(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(sender);
        }

        private void bSelectFolder_Click(object sender, EventArgs e)
        {
            var dlg = new FolderBrowserDialog
            {
                Description = "�������� ����� � TortoiseGitProc.exe",
                ShowNewFolderButton = false,
                SelectedPath = tbTGProcPath.Text
            };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                tbTGProcPath.Text = dlg.SelectedPath;
            }
        }

        private void llWiki_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            llWiki.LinkVisited = true;
            System.Diagnostics.Process.Start("https://github.com/schadin/NppKate/wiki");
        }

        private void tvMenuCommand_AfterCheck(object sender, TreeViewEventArgs e)
        {
            var node = tvToolbarCommand.Nodes[e.Node.Name];
            node.ForeColor = e.Node.Checked ? tvToolbarCommand.ForeColor : DisableColor;
        }

        private void tvToolbarCommand_BeforeCheck(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.ForeColor == DisableColor)
            {
                e.Cancel = true;
            }
        }
    }
}
