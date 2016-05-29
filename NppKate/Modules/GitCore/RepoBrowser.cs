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
using System.Collections.Generic;
using System.IO;
using System;

namespace NppKate.Modules.GitCore
{
    public partial class RepoBrowser : DockDialog, FormDockable
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private const string REPO_INDEX = "REPO";
        private const string BRANCH_INDEX = "BRANCH";
        private const string EMPTY_INDEX = "EMPTY";
        private const string CURBRANCH_INDEX = "CURRENT_BRANCH";
        private const string BRANCH_FOLDER_INDEX = "BRANCH_FOLDER";
        private const string REMOTE_BRANCH_INDEX = "REMOTE_BRANCH";
        private const string LOADING_INDEX = "LOADING";

        private const string FILE_LOCK = "head.lock";

        //private readonly Color CurrentBranchColor = Color.FromArgb(10,192,56);
        private readonly Color DocumentRepoColor = Color.DeepSkyBlue;

        private string _lastActiveRepo = null;
        private string _lastDocumentRepo = null;
        private bool _hasDoubleClick = false;
        private DateTime _lastMouseDown = DateTime.Now;
        private bool _isInitialized = false;

        private Dictionary<string, FileSystemWatcher> _watchers;

        protected override void OnSwitchIn()
        {
            _logger.Trace("Switch in repository browser");
            UpdateState();
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
            GitCore.Instance.OnRepositoryRemoved += GitCoreOnRepositoryRemoved;

            _watchers = new Dictionary<string, FileSystemWatcher>();
            LoadTree();
            UpdateState();

            tvRepositories.Update();
        }

        private void GitCoreOnRepositoryRemoved(object sender, RepositoryChangedEventArgs e)
        {
            var node = tvRepositories.Nodes[e.RepositoryName];
            if (node.Name == _lastActiveRepo)
            {
                var otherNode = node.PrevVisibleNode != null ? node.PrevVisibleNode : node.NextVisibleNode;
                GitCore.Instance.SwitchByName(otherNode.Name);
            }
            _watchers[node.Name].EnableRaisingEvents = false;
            _watchers[node.Name].Dispose();
            _watchers.Remove(node.Name);
            node.Remove();

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
                nodeOld.Text = nodeOld.Name;
                nodeOld.ForeColor = tvRepositories.ForeColor;
            }
            if (!string.IsNullOrEmpty(repoName))
            {
                var nodeNew = tvRepositories.Nodes[repoName];
                nodeNew.ForeColor = DocumentRepoColor;
                nodeNew.Text = nodeNew.Name + "*";
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
            var isAutoExpand = Settings.GitCore.AutoExpand;

            if (repoName.Equals(_lastActiveRepo)) return;
            if (!string.IsNullOrEmpty(_lastActiveRepo))
            {
                var nodeOld = tvRepositories.Nodes[_lastActiveRepo];
                nodeOld.NodeFont = new Font(nodeOld.NodeFont ?? tvRepositories.Font, FontStyle.Regular);
                if (isAutoExpand)
                    nodeOld.Collapse();
            }
            var nodeNew = tvRepositories.Nodes[repoName];
            nodeNew.NodeFont = new Font(nodeNew.NodeFont ?? tvRepositories.Font, FontStyle.Bold);
            nodeNew.Text += string.Empty;
            if (isAutoExpand)
                nodeNew.Expand();
            _lastActiveRepo = repoName;
        }

        private void tvRepositories_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Left && e.Node.ImageKey == REPO_INDEX && !(e.Node.NodeFont?.Bold ?? false))
            {
                GitCore.Instance.SwitchByName(e.Node.Name);
            }
        }

        private void FillContent(TreeNode node, RepositoryLink link)
        {
            var currentBranch = node.Nodes.Add("CURRENT", "", CURBRANCH_INDEX, CURBRANCH_INDEX);
            //currentBranch.ForeColor = CurrentBranchColor;
            var local = node.Nodes.Add("LOCAL", "local", BRANCH_FOLDER_INDEX, BRANCH_FOLDER_INDEX);
            local.Tag = 0;
            CreateNode("LOAD_LOCAL", "Loading...", LOADING_INDEX, local);
            var remote = node.Nodes.Add("REMOTE", "remote", BRANCH_FOLDER_INDEX, BRANCH_FOLDER_INDEX);
            remote.Tag = 0;
            CreateNode("LOAD_REMOTE", "Loading...", LOADING_INDEX, remote);
            using (var r = new Repository(link.Path))
            {
                currentBranch.Text = r.Head.Name;
                /*
                foreach (var b in r.Branches)
                {
                    if (b.IsRemote)
                    {
                        if (!b.Name.EndsWith("/HEAD", StringComparison.InvariantCultureIgnoreCase))
                        {
                            CreateNode(b.Name, b.Name, REMOTE_BRANCH_INDEX, remote, null);
                        }
                    }
                    else
                    {
                        var branchNode = CreateNode(b.Name, b.Name, BRANCH_INDEX, local, cmBranch);
                        if (b.IsCurrentRepositoryHead)
                        {
                            branchNode.ImageKey = CURBRANCH_INDEX;
                            branchNode.SelectedImageKey = CURBRANCH_INDEX;
                            currentBranch.Text = b.Name;
                        }
                    }
                }
                */
            }
        }

        private TreeNode CreateRepoNode(RepositoryLink link)
        {
            if (link == null) return null;

            var node = CreateNode(link.Name, link.Name, REPO_INDEX, null, cmRepositories);
            FillContent(node, link);
            // Create file watcher
            var watcher = new FileSystemWatcher(Path.Combine(link.Path, ".git"), "*.lock");
            watcher.NotifyFilter = NotifyFilters.FileName;
            watcher.Deleted += WatchersOnChange;
            watcher.EnableRaisingEvents = true;
            // save handle
            _watchers.Add(link.Name, watcher);
            return node;
        }

        private void WatchersOnChange(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Deleted && FILE_LOCK.Equals(e.Name, System.StringComparison.InvariantCultureIgnoreCase))
            {
                UpdateBranch(GitCore.GetRepoName(GitCore.GetRootDir(e.FullPath)));
            }
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

        private void UpdateBranch(string repoName)
        {
            if (string.IsNullOrEmpty(repoName)) return;
            _logger.Debug($"Update branches for repo {repoName}");
            var node = tvRepositories.Nodes[repoName];
            var link = GitCore.Instance.GetRepositoryByName(repoName);
            using (var r = new Repository(link.Path))
            {
                var currentBranch = node.Nodes["CURRENT"];
                var local = node.Nodes["LOCAL"];
                var remote = node.Nodes["REMOTE"];

                tvRepositories.Invoke(new Action(() =>
                {
                    currentBranch.Text = r.Head.Name;
                }));

                if ((int)local.Tag != 0 || (int)remote.Tag != 0)
                {
                    foreach (var b in r.Branches)
                    {
                        if (!b.IsRemote && (int)local.Tag == 1)
                        {
                            if (!local.Nodes.ContainsKey(b.Name))
                            {
                                tvRepositories.Invoke(new Action(() =>
                                {
                                    CreateNode(b.Name, b.Name, BRANCH_INDEX, local);
                                }));
                            }
                            var branch = local.Nodes[b.Name];
                            if (b.IsCurrentRepositoryHead)
                            {
                                tvRepositories.Invoke(new Action(() =>
                                {
                                    branch.ImageKey = CURBRANCH_INDEX;
                                    branch.SelectedImageKey = CURBRANCH_INDEX;
                                    branch.Text += string.Empty;
                                }));
                            }
                            else
                            {
                                tvRepositories.Invoke(new Action(() =>
                                {
                                    branch.ImageKey = BRANCH_INDEX;
                                    branch.SelectedImageKey = BRANCH_INDEX;
                                    branch.Text += string.Empty;
                                }));
                            }
                        }
                        else if (b.IsRemote && (int)remote.Tag == 1)
                        {
                            if (!b.Name.EndsWith("/HEAD", StringComparison.InvariantCultureIgnoreCase) &&
                                !remote.Nodes.ContainsKey(b.Name))
                            {
                                tvRepositories.Invoke(new Action(() =>
                                {
                                    CreateNode(b.Name, b.Name, REMOTE_BRANCH_INDEX, remote);
                                }));
                            }
                        }
                    }
                }
            }
        }

        private TreeNode CreateNode(string name, string text, string imageKey, TreeNode parentNode = null, ContextMenuStrip menu = null)
        {
            var node = new TreeNode()
            {
                Name = name,
                Text = text,
                ImageKey = imageKey,
                SelectedImageKey = imageKey,
                ContextMenuStrip = menu
            };
            parentNode?.Nodes?.Add(node);
            return node;
        }

        private void miSetActive_Click(object sender, EventArgs e)
        {
            var node = tvRepositories.SelectedNode;
            GitCore.Instance.SwitchByName(node?.Name);
        }

        private void cmRepositories_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var node = tvRepositories.SelectedNode;
            miSetActive.Enabled = _lastActiveRepo != node?.Name;
            miRemoveRepo.Enabled = _lastDocumentRepo != node?.Name;
        }

        private void tvRepositories_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (tvRepositories.SelectedNode != e.Node)
            {
                tvRepositories.SelectedNode = e.Node;
            }
        }

        private void miAddRepo_Click(object sender, EventArgs e)
        {
            var openDlg = new FolderBrowserDialog()
            {
                Description = "Select git-repository folder",
                ShowNewFolderButton = false
            };
            if (openDlg.ShowDialog() == DialogResult.OK)
            {
                GitCore.Instance.SwitchByPath(openDlg.SelectedPath);
            }
        }

        private void miRemoveRepo_Click(object sender, EventArgs e)
        {
            var node = tvRepositories.SelectedNode;
            GitCore.Instance.RemoveRepository(node.Name);
        }

        private void tvRepositories_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            e.Cancel = _hasDoubleClick;
            _hasDoubleClick = false;
        }

        private void tvRepositories_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            e.Cancel = _hasDoubleClick;
            _hasDoubleClick = false;
        }

        private void tvRepositories_MouseDown(object sender, MouseEventArgs e)
        {
            int delta = (int)DateTime.Now.Subtract(_lastMouseDown).TotalMilliseconds;
            _hasDoubleClick = (delta < SystemInformation.DoubleClickTime);
            _lastMouseDown = DateTime.Now;
            if (_hasDoubleClick)
            {
                var node = tvRepositories.GetNodeAt(e.Location);
                _hasDoubleClick = node?.ImageKey == REPO_INDEX;
            }
        }

        private void RepoBrowser_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible && !_isInitialized)
            {
                _isInitialized = true;
                tvRepositories.Nodes[_lastActiveRepo].Text += string.Empty;
            }
        }

        private void tvRepositories_AfterExpand(object sender, TreeViewEventArgs e)
        {
            if (e.Node.ImageKey == BRANCH_FOLDER_INDEX && (int)e.Node.Tag == 0)
            {

            }
        }
    }

}
