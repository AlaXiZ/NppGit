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

using LibGit2Sharp;
using NLog;
using NppKate.Common;
using NppKate.Forms;
using NppKate.Modules.TortoiseGitFeatures;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NppKate.Modules.GitCore
{
    public partial class RepoBrowser : DockDialog, FormDockable
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();


        #region Node names
        private const string LOCAL_FOLDER = "LOCAL";
        private const string REMOTE_FOLDER = "REMOTE";
        private const string LOAD_ITEM = "LOAD";
        private const string CURRENT_BRANCH = "CURRENT";
        #endregion

        #region Image key
        private const string REPO_INDEX = "REPO";
        private const string BRANCH_INDEX = "BRANCH";
        private const string EMPTY_INDEX = "EMPTY";
        private const string CURBRANCH_INDEX = "CURRENT_BRANCH";
        private const string BRANCH_FOLDER_INDEX = "BRANCH_FOLDER";
        private const string REMOTE_BRANCH_INDEX = "REMOTE_BRANCH";
        private const string LOADING_INDEX = "LOADING";
        #endregion

        #region File hook
        private const string FILE_LOCK = "head.lock";
        private const string HOOK_FILTER = "*.lock";
        #endregion

        private readonly Color DocumentRepoColor = Color.DeepSkyBlue;

        private string _lastActiveRepo = null;
        private string _lastDocumentRepo = null;
        private bool _hasDoubleClick = false;
        private DateTime _lastMouseDown = DateTime.Now;
        private bool _isInitialized = false;
        private ITortoiseCommand _commandRuner;

        private Dictionary<string, FileSystemWatcher> _watchers;

        protected override void OnSwitchIn()
        {
            _logger.Trace("Switch in repository browser");
            if (_isInitialized)
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
        }

        protected override void AfterInit()
        {
            if (_manager.ModuleManager.ServiceExists(typeof(ITortoiseCommand)))
            {
                _commandRuner = (ITortoiseCommand)_manager.ModuleManager.GetService(typeof(ITortoiseCommand));
                tortoiseGitToolStripMenuItem.Visible = true;
            }
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

        public void ChangeContext() { }

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
            var currentBranch = node.Nodes.Add(CURRENT_BRANCH, "", CURBRANCH_INDEX, CURBRANCH_INDEX);
            var local = node.Nodes.Add(LOCAL_FOLDER, Properties.Resources.RsLocal, BRANCH_FOLDER_INDEX, BRANCH_FOLDER_INDEX);
            local.Tag = 0;
            CreateNode(LOAD_ITEM, Properties.Resources.RsLoading, LOADING_INDEX, local);
            var remote = node.Nodes.Add(REMOTE_FOLDER, Properties.Resources.RsRemote, BRANCH_FOLDER_INDEX, BRANCH_FOLDER_INDEX);
            remote.Tag = 0;
            CreateNode(LOAD_ITEM, Properties.Resources.RsLoading, LOADING_INDEX, remote);
            using (var r = new Repository(link.Path))
            {
                currentBranch.Text = r.Head.Name;
            }
        }

        private TreeNode CreateRepoNode(RepositoryLink link)
        {
            if (link == null) return null;
            var node = CreateNode(link.Name, link.Name, REPO_INDEX, null, cmRepositories);
            FillContent(node, link);
            // Create file watcher
            var watcher = new FileSystemWatcher(Path.Combine(link.Path, ".git"), HOOK_FILTER);
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
            var task = new Task<List<TreeNode>>(() =>
            {
                var result = new List<TreeNode>();
                repoList.ForEach(r => result.Add(CreateRepoNode(r)));
                return result;
            });
            task.ContinueWith((t) =>
            {
                Invoke(new Action(() =>
                {
                    tvRepositories.BeginUpdate();
                    try
                    {
                        t.Result.ForEach(n => tvRepositories.Nodes.Add(n));
                        UpdateState();
                    }
                    finally
                    {
                        tvRepositories.EndUpdate();
                        _isInitialized = true;
                        tvRepositories.Nodes[_lastActiveRepo].Text += string.Empty;
                    }
                }));
            });
            task.Start();
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
                var currentBranch = node.Nodes[CURRENT_BRANCH];
                var local = node.Nodes[LOCAL_FOLDER];
                var remote = node.Nodes[REMOTE_FOLDER];

                tvRepositories.Invoke(new Action(() =>
                {
                    tvRepositories.BeginUpdate();
                    currentBranch.Text = r.Head.Name;
                }));

                try
                {
                    if ((int)local.Tag != 0 || (int)remote.Tag != 0)
                    {
                        foreach (var b in r.Branches.OrderBy(b => b.Name))
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
                finally
                {
                    tvRepositories.Invoke(new Action(() =>
                    {
                        tvRepositories.EndUpdate();
                    }));
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

            var inRepo = GitCore.IsValidGitRepo(Npp.NppUtils.CurrentFileDir);

            blameToolStripMenuItem.Enabled = inRepo;
            showLogFileToolStripMenuItem.Enabled = inRepo;

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
                Description = Properties.Resources.RsSelectGitDir,
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
                LoadTree();
            }
        }

        private void tvRepositories_AfterExpand(object sender, TreeViewEventArgs e)
        {
            if (e.Node.ImageKey == BRANCH_FOLDER_INDEX && (int)e.Node.Tag == 0)
            {
                e.Node.Tag = 1;
                e.Node.Nodes.RemoveByKey(LOAD_ITEM);
                new Task(() =>
                {
                    UpdateBranch(e.Node.Parent.Name);
                }).Start();
            }
        }

        private void RunTortoiseCommandForRepo(TortoiseGitCommand cmd)
        {
            var node = tvRepositories.SelectedNode;
            var path = GitCore.Instance.GetRepositoryByName(node?.Name)?.Path;
            if (path != null)
                _commandRuner.RunCommand(cmd, path);
        }

        private void RunTortoiseCommandCurrentFile(TortoiseGitCommand cmd)
        {
            var path = Npp.NppUtils.CurrentFilePath;
            if (path != null)
                _commandRuner.RunCommand(cmd, path);
        }

        private void fetchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RunTortoiseCommandForRepo(TortoiseGitCommand.Fetch);
        }

        private void pullToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RunTortoiseCommandForRepo(TortoiseGitCommand.Pull);
        }

        private void commitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RunTortoiseCommandForRepo(TortoiseGitCommand.Commit);
        }

        private void pushToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RunTortoiseCommandForRepo(TortoiseGitCommand.Push);
        }

        private void syncToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RunTortoiseCommandForRepo(TortoiseGitCommand.Sync);
        }

        private void showLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RunTortoiseCommandForRepo(TortoiseGitCommand.Log);
        }

        private void showReflogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RunTortoiseCommandForRepo(TortoiseGitCommand.RefLog);
        }

        private void stashSaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RunTortoiseCommandForRepo(TortoiseGitCommand.StashSave);
        }

        private void stashPopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RunTortoiseCommandForRepo(TortoiseGitCommand.StashPop);
        }

        private void stashListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // ???
        }

        private void repoBrowserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RunTortoiseCommandForRepo(TortoiseGitCommand.RepoBrowser);
        }

        private void checkForModificationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RunTortoiseCommandForRepo(TortoiseGitCommand.RepoStatus);
        }

        private void browseReferenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RunTortoiseCommandForRepo(TortoiseGitCommand.RefBrowse);
        }

        private void switchCheckoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RunTortoiseCommandForRepo(TortoiseGitCommand.Switch);
        }

        private void blameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RunTortoiseCommandCurrentFile(TortoiseGitCommand.Blame);
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RunTortoiseCommandForRepo(TortoiseGitCommand.Export);
        }

        private void mergeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RunTortoiseCommandForRepo(TortoiseGitCommand.Merge);
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RunTortoiseCommandForRepo(TortoiseGitCommand.Settings);
        }

        private void showLogFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RunTortoiseCommandCurrentFile(TortoiseGitCommand.Log);
        }
    }

}
