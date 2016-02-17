using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace NppGit.Forms
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
            chbDefaultShortcut.Checked = Settings.InnerSettings.IsSetDefaultShortcut;
            mtxbSHACount.Text = Settings.Functions.SHACount.ToString();
            chbFileInOtherView.Checked = Settings.Functions.OpenFileInOtherView;

            cbLogLevel.Items.Clear();
            cbLogLevel.Items.AddRange(_logLevel.ToArray());
            cbLogLevel.Text = Settings.InnerSettings.LogLevel;

            chlModules.SetItemChecked(0, Settings.Modules.TortoiseGit);
            chlModules.SetItemChecked(1, Settings.Modules.Git);
            chlModules.SetItemChecked(2, Settings.Modules.SQLIDE);
            chlModules.SetItemChecked(3, Settings.Modules.Snippets);
            chlModules.SetItemChecked(4, Settings.Modules.PSSE);
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
            Settings.InnerSettings.IsSetDefaultShortcut = chbDefaultShortcut.Checked;
            Settings.Functions.SHACount = byte.Parse(mtxbSHACount.Text);
            Settings.Functions.OpenFileInOtherView = chbFileInOtherView.Checked;
            Settings.InnerSettings.LogLevel = cbLogLevel.Text;

            // Modules state
            Settings.Modules.TortoiseGit = chlModules.GetItemChecked(0);
            Settings.Modules.Git = chlModules.GetItemChecked(1);
            Settings.Modules.SQLIDE = chlModules.GetItemChecked(2);
            Settings.Modules.Snippets = chlModules.GetItemChecked(3);
            Settings.Modules.PSSE = chlModules.GetItemChecked(4);
        }

        private void bOk_Click(object sender, EventArgs e)
        {
            SaveSettings();
            if (chbRestartNpp.Checked)
            {
                // TODO: Restart app
                PluginUtils.Restart();
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
                MessageBox.Show("Value in [1..20]", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                mtxbSHACount.Focus();
            }
        }

        private void chlModules_SelectedValueChanged(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(sender);
        }
    }
}
