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

using NppKate.Npp;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace NppKate.Forms
{
    public partial class SettingsDialog : Form
    {
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

        public SettingsDialog()
        {
            InitializeComponent();
            LoadSettings();
        }

        private void LoadSettings()
        {
            chbTGToolbar.Checked = Settings.TortoiseGitProc.ShowToolbar;
            chlButtons.Enabled = chbTGToolbar.Checked;
            var mask = Settings.TortoiseGitProc.ButtonMask;
            for (int i = 0; i < chlButtons.Items.Count; i++)
            {
                chlButtons.SetItemChecked(i, (mask & (1u << i)) > 0);
            }
            tbTGProcPath.Text = Settings.TortoiseGitProc.Path;
            chbDefaultShortcut.Checked = Settings.InnerSettings.IsSetDefaultShortcut;
            mtxbSHACount.Text = Settings.Functions.SHACount.ToString();
            chbFileInOtherView.Checked = Settings.Functions.OpenFileInOtherView;
            chbAutoExpand.Checked = Settings.GitCore.AutoExpand;

            cbLogLevel.Items.Clear();
            cbLogLevel.Items.AddRange(_logLevel.ToArray());
            cbLogLevel.Text = Settings.InnerSettings.LogLevel;

            chlModules.SetItemChecked(0, Settings.Modules.TortoiseGit);
            chlModules.SetItemChecked(1, Settings.Modules.Git);
            chlModules.SetItemChecked(2, Settings.Modules.SQLIDE);
            chlModules.SetItemChecked(3, Settings.Modules.Snippets);

            //chlModules.SetItemChecked(4, Settings.Modules.PSSE);

            chbGroupByCategory.Checked = Settings.Snippets.IsGroupByCategory;
            chbHideByExt.Checked = Settings.Snippets.IsHideByExtention;
            chbExpand.Checked = Settings.Snippets.IsExpanAfterCreate;
        }

        private uint GetButtonMask()
        {
            uint result = 0u;
            for (int i = 0; i < chlButtons.Items.Count; i++)
            {
                result |= (chlButtons.GetItemChecked(i) ? 1u : 0) << i;
            }
            return result;
        }

        private void chbTGToolbar_CheckedChanged(object sender, EventArgs e)
        {
            chlButtons.Enabled = chbTGToolbar.Checked;
        }

        private void SettingsDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        private void SaveSettings()
        {
            Settings.TortoiseGitProc.ShowToolbar = chbTGToolbar.Checked;
            Settings.TortoiseGitProc.ButtonMask = GetButtonMask();
            Settings.TortoiseGitProc.Path = tbTGProcPath.Text;
            Settings.InnerSettings.IsSetDefaultShortcut = chbDefaultShortcut.Checked;
            Settings.Functions.SHACount = byte.Parse(mtxbSHACount.Text);
            Settings.Functions.OpenFileInOtherView = chbFileInOtherView.Checked;
            Settings.GitCore.AutoExpand = chbAutoExpand.Checked;
            Settings.InnerSettings.LogLevel = cbLogLevel.Text;

            // Modules state
            Settings.Modules.TortoiseGit = chlModules.GetItemChecked(0);
            Settings.Modules.Git = chlModules.GetItemChecked(1);
            Settings.Modules.SQLIDE = chlModules.GetItemChecked(2);
            Settings.Modules.Snippets = chlModules.GetItemChecked(3);
            //Settings.Modules.PSSE = chlModules.GetItemChecked(4);

            // Snippet settings
            Settings.Snippets.IsGroupByCategory = chbGroupByCategory.Checked;
            Settings.Snippets.IsHideByExtention = chbHideByExt.Checked;
            Settings.Snippets.IsExpanAfterCreate = chbExpand.Checked;
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
                MessageBox.Show("Значение должно быть в интервале [1..20]", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                Description = "Выберите папку с TortoiseGitProc.exe",
                ShowNewFolderButton = false,
                SelectedPath = tbTGProcPath.Text
            };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                tbTGProcPath.Text = dlg.SelectedPath;
            }
        }
    }
}
