using System;
using System.Windows.Forms;

namespace NppGit.Forms
{
    public partial class SettingsDialog : Form
    {
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
            Settings.TortoiseGitProc.ShowToolbar = chbTGToolbar.Checked;
            Settings.TortoiseGitProc.ButtonMask = GetButtonMask();
            Settings.InnerSettings.IsSetDefaultShortcut = chbDefaultShortcut.Checked;
            Settings.Functions.SHACount = byte.Parse(mtxbSHACount.Text);
            Settings.Functions.OpenFileInOtherView = chbFileInOtherView.Checked;
        }

        private void bOk_Click(object sender, EventArgs e)
        {
            Close();
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
    }
}
