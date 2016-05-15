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

using System.Drawing;
using System.Windows.Forms;
using LibGit2Sharp;
using NLog;
using NppKate.Common;
using NppKate.Forms;

namespace NppKate.Modules.GitCore
{
    public partial class RepoBrowser : DockDialog, FormDockable
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private const string REPO_INDEX = "REPO";
        private const string BRANCH_INDEX = "BRANCH";
        private const string EMPTY_INDEX = "EMPTY";
        private string _lastActiveRepo = null;
        private string _lastDocumentRepo = null;

        protected override void OnSwitchIn()
        {
            _logger.Trace("Switch in repository browser");
        }

        protected override void OnSwitchOut()
        {
            _logger.Trace("Switch out repository browser");
        }

        public RepoBrowser()
        {
            InitializeComponent();

            GitCore.Instance.OnActiveRepositoryChanged += GitCoreOnActiveRepositoryChanged;
            GitCore.Instance.OnDocumentReposituryChanged += GitCoreOnDocumentRepositoryChanged;
            GitCore.Instance.OnRepositoryAdded += GitCoreOnRepositoryAdded;

            LoadTree();
            UpdateState();

            tvRepositories.Update();
        }

        private void GitCoreOnRepositoryAdded(object sender, RepositoryChangedEventArgs e)
        {
            if (tvRepositories.Nodes.ContainsKey(e.RepositoryName)) return;
            var node = CreateRepoNode(GitCore.Instance.Repositories.Find(r => r.Name == e.RepositoryName));
            if (node == null) return;
            tvRepositories.Nodes.Add(node);
        }

        private void GitCoreOnDocumentRepositoryChanged(object sender, RepositoryChangedEventArgs e)
        {
            DocumentRepositoryUpdate(e.RepositoryName);
        }

        private void DocumentRepositoryUpdate(string repoName)
        {
            if (!string.IsNullOrEmpty(repoName) && repoName.Equals(_lastDocumentRepo)) return;
            if (!string.IsNullOrEmpty(_lastDocumentRepo))
            {
                var nodeOld = tvRepositories.Nodes[_lastDocumentRepo];
                nodeOld.ForeColor = tvRepositories.ForeColor;
            }
            if (!string.IsNullOrEmpty(repoName))
            {
                var nodeNew = tvRepositories.Nodes[repoName];
                nodeNew.ForeColor = Color.Red;
            }
            _lastDocumentRepo = repoName;
        }

        public Bitmap TabIcon => Properties.Resources.repositories;

        public string Title => "Repository browser";

        public void ChangeContext()
        { 
            // TODO: ? 
        }

        private void GitCoreOnActiveRepositoryChanged(object sender, RepositoryChangedEventArgs e)
        {
            var repoName = e.RepositoryName;
            ActiverRepositoryUpdate(repoName);
        }

        private void ActiverRepositoryUpdate(string repoName)
        {
            if (repoName.Equals(_lastActiveRepo)) return;
            if (!string.IsNullOrEmpty(_lastActiveRepo))
            {
                var nodeOld = tvRepositories.Nodes[_lastActiveRepo];
                nodeOld.NodeFont = new Font(nodeOld.NodeFont ?? tvRepositories.Font, FontStyle.Regular);
            }
            var nodeNew = tvRepositories.Nodes[repoName];
            nodeNew.NodeFont = new Font(nodeNew.NodeFont ?? tvRepositories.Font, FontStyle.Bold);
            nodeNew.Text += string.Empty;
            _lastActiveRepo = repoName;
        }

        private void tvRepositories_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            GitCore.Instance.SwitchByName(e.Node.Name);
        }

        private void FillBranch(TreeNode node, RepositoryLink link)
        {
            var local = node.Nodes.Add("LOCAL", "local", BRANCH_INDEX, BRANCH_INDEX);
            var remote = node.Nodes.Add("REMOTE", "remote", BRANCH_INDEX, BRANCH_INDEX);
            using (var r = new Repository(link.Path))
           {
                foreach(var b in r.Branches)
                {
                    if (b.IsRemote)
                    {
                        if (!b.Name.EndsWith("/HEAD", System.StringComparison.InvariantCultureIgnoreCase))
                        {
                            remote.Nodes.Add(b.Name, b.Name, BRANCH_INDEX, BRANCH_INDEX);
                        }
                    }
                    else
                    {
                        var branchNode = local.Nodes.Add(b.Name, b.Name, BRANCH_INDEX, BRANCH_INDEX);
                        if (b.IsCurrentRepositoryHead)
                        {
                            branchNode.ForeColor = Color.DeepSkyBlue;
                        }
                    }
                }
            }
        }

        private TreeNode CreateRepoNode(RepositoryLink link)
        {
            if (link == null) return null;
            var node = new TreeNode(link.Name)
            {
                Name = link.Name,
                Text = link.Name,
                ImageKey = REPO_INDEX
            };
            FillBranch(node, link);
            return node;
        }

        private void LoadTree()
        {
            var repoList = GitCore.Instance.Repositories;
            repoList.ForEach(r => tvRepositories.Nodes.Add(CreateRepoNode(r)));
        }

        private void UpdateState()
        {
            var repoLink = GitCore.Instance.ActiveRepository;
            if (repoLink != null)
                ActiverRepositoryUpdate(repoLink.Name);
            repoLink = GitCore.Instance.DocumentRepository;
            if (repoLink != null)
                DocumentRepositoryUpdate(repoLink.Name);
        }

        private void tvRepositories_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Name == "LOCAL" && (!e.Node.Tag?.Equals(100) ?? false))
            {
                foreach (TreeNode n in e.Node.Nodes)
                {
                    n.Text += string.Empty;
                }
                e.Node.Tag = 100;
            }
        }
    }

}
