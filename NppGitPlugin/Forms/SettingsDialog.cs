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
            var cfg = Settings.Instance;
            chbTGToolbar.Checked = cfg.TortoiseGit.ShowToolbar;
            chlButtons.Enabled = chbTGToolbar.Checked;
            var mask = cfg.TortoiseGit.ButtonMask;
            for (int i = 0; i < chlButtons.Items.Count; i++)
            {
                chlButtons.SetItemChecked(i, (mask & (1u << i)) > 0);
            }
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
            Settings.Instance.TortoiseGit.ShowToolbar = chbTGToolbar.Checked;
            Settings.Instance.TortoiseGit.ButtonMask = GetButtonMask();
        }

        private void bOk_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
