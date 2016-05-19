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

using System;
using System.Windows.Forms;

namespace NppKate.Forms
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
