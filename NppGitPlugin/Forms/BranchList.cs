using System;
using System.Windows.Forms;

namespace NppGit.Forms
{
    public partial class BranchList : Form
    {
        public string SelectedItem
        {
            get; private set;
        }

        public BranchList()
        {
            InitializeComponent();
        }

        private string _repoDir;

        public string RepoDirectory
        {
            get { return _repoDir; }
            set
            {
                _repoDir = value;
                ReloadBranched();    
            }
        }

        private void ReloadBranched()
        {
            var showRemoteBranch = rbAll.Checked || rbRemote.Checked;
            var showLocal = rbAll.Checked || rbLocal.Checked;

            lbItems.BeginUpdate();
            lbItems.Items.Clear();
            using (var repo = new LibGit2Sharp.Repository(_repoDir))
            {
                foreach(var b in repo.Branches)
                {
                    if (b.IsRemote == showRemoteBranch || !b.IsRemote == showLocal)
                    {
                        lbItems.Items.Add(b.Name);
                    }
                }
            }
            lbItems.EndUpdate();
        }

        private void lbItems_DoubleClick(object sender, EventArgs e)
        {
            OK();
        }

        private void lbItems_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && lbItems.SelectedItem != null)
            {
                OK();
            }
        }

        private void OK()
        {
            SelectedItem = (string)lbItems.SelectedItem;
            DialogResult = DialogResult.OK;
        }

        private void bOk_Click(object sender, EventArgs e)
        {
            OK();
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void CheckedChanged(object sender, EventArgs e)
        {
            ReloadBranched();
        }
    }
}
