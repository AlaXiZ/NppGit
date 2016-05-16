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
using System.Linq;
using System.Threading;
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

        private const string INDEX_LOCK = "index.lock";

        private string _lastActiveRepo = null;
        private string _lastDocumentRepo = null;

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

            _watchers = new Dictionary<string, FileSystemWatcher>();
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

        private void FillContent(TreeNode node, RepositoryLink link)
        {
            var currentBranch = node.Nodes.Add("CURRENT", "", CURBRANCH_INDEX, CURBRANCH_INDEX);
            var local = node.Nodes.Add("LOCAL", "local", BRANCH_FOLDER_INDEX, BRANCH_FOLDER_INDEX);
            var remote = node.Nodes.Add("REMOTE", "remote", BRANCH_FOLDER_INDEX, BRANCH_FOLDER_INDEX);
            using (var r = new Repository(link.Path))
            {
                foreach (var b in r.Branches)
                {
                    if (b.IsRemote)
                    {
                        if (!b.Name.EndsWith("/HEAD", System.StringComparison.InvariantCultureIgnoreCase))
                        {
                            remote.Nodes.Add(b.Name, b.Name, REMOTE_BRANCH_INDEX, REMOTE_BRANCH_INDEX);
                        }
                    }
                    else
                    {
                        var branchNode = local.Nodes.Add(b.Name, b.Name, BRANCH_INDEX, BRANCH_INDEX);
                        if (b.IsCurrentRepositoryHead)
                        {
                            branchNode.ForeColor = Color.DeepSkyBlue;
                            branchNode.ImageKey = CURBRANCH_INDEX;
                            branchNode.SelectedImageKey = CURBRANCH_INDEX;
                            currentBranch.Text = b.Name;
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
            if (e.ChangeType == WatcherChangeTypes.Deleted && INDEX_LOCK.Equals(e.Name, System.StringComparison.InvariantCultureIgnoreCase))
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
                foreach(var b in r.Branches)
                {
                    if (!b.IsRemote)
                    {
                        if (!local.Nodes.ContainsKey(b.Name))
                        {
                            tvRepositories.Invoke(new Action(() =>
                            {
                                local.Nodes.Add(b.Name, b.Name, BRANCH_INDEX, BRANCH_INDEX);
                            }));
                        }
                        var branch = local.Nodes[b.Name];
                        if (b.IsCurrentRepositoryHead)
                        {
                            tvRepositories.Invoke(new Action(() =>
                            {
                                currentBranch.Text = b.Name;
                                branch.ForeColor = Color.DeepSkyBlue;
                                branch.ImageKey = CURBRANCH_INDEX;
                                branch.SelectedImageKey = CURBRANCH_INDEX;
                            }));
                        }
                        else
                        {
                            tvRepositories.Invoke(new Action(() =>
                            {
                                branch.ForeColor = Color.Black;
                                branch.ImageKey = BRANCH_INDEX;
                                branch.SelectedImageKey = BRANCH_INDEX;
                            }));
                        }
                    }
                    else
                    {
                        if (!b.Name.EndsWith("/HEAD", System.StringComparison.InvariantCultureIgnoreCase) && 
                            !remote.Nodes.ContainsKey(b.Name))
                        {
                            tvRepositories.Invoke(new Action(() =>
                            {
                                remote.Nodes.Add(b.Name, b.Name, REMOTE_BRANCH_INDEX, REMOTE_BRANCH_INDEX);
                            }));
                        }
                    }
                }
            }
        }

    }

}
