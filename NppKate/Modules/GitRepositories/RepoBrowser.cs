// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
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
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibGit2Sharp;
using NLog;
using NppKate.Common;
using NppKate.Forms;
using NppKate.Modules.TortoiseGitFeatures;
using NppKate.Modules.GitRepositories.RepositoryExt;
using System.Windows.Threading;

namespace NppKate.Modules.GitCore
{
    public partial class RepoBrowser : DockDialog, FormDockable
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();


        #region Node names
        private const string LocalFolder = "LOCAL";
        private const string RemoteFolder = "REMOTE";
        private const string LoadItem = "LOAD";
        private const string CurrentBranch = "CURRENT";
        private const string WorktreeFolder = "WORKTREE";
        #endregion

        #region Image key
        private const string RepoIndex = "REPO";
        private const string BranchIndex = "BRANCH";
        private const string EmptyIndex = "EMPTY";
        private const string CurbranchIndex = "CURRENT_BRANCH";
        private const string BranchFolderIndex = "BRANCH_FOLDER";
        private const string RemoteBranchIndex = "REMOTE_BRANCH";
        private const string LoadingIndex = "LOADING";
        private const string WorktreeFolderIndex = "WORKTREE_FOLDER";
        private const string WorktreeLeafIndex = "WORKTREE_LEAF";
        private const string WorktreeLockIndex = "WORKTREE_LOCK";
        private const string WorktreeNotExistsIndex = "WORKTREE_NOT_EXISTS";
        #endregion

        #region Int Const
        const int NODE_NOLOADED = 0;
        const int NODE_LOADED = 1;
        #endregion

        #region File hook
        private const string FileLock = "head.lock";
        private const string HookFilter = "*.lock";
        #endregion

        private readonly Color _documentRepoColor = Color.DeepSkyBlue;

        private string _lastActiveRepo = null;
        private string _lastDocumentRepo = null;
        private bool _hasDoubleClick = false;
        private DateTime _lastMouseDown = DateTime.Now;
        private bool _isInitialized = false;
        private ITortoiseCommand _commandRuner;

        private readonly Dictionary<string, FileSystemWatcher> _watchers;

        private Dispatcher _UIDispatcher;

        protected override void OnSwitchIn()
        {
            Logger.Trace("Switch in repository browser");
            if (_isInitialized)
                UpdateState();
        }

        protected override void OnSwitchOut()
        {
            Logger.Trace("Switch out repository browser");
        }

        public RepoBrowser()
        {
            InitializeComponent();

            _UIDispatcher = Dispatcher.CurrentDispatcher;

            GitRepository.Instance.OnActiveRepositoryChanged += GitCoreOnActiveRepositoryChanged;
            GitRepository.Instance.OnDocumentReposituryChanged += GitCoreOnDocumentRepositoryChanged;
            GitRepository.Instance.OnRepositoryAdded += GitCoreOnRepositoryAdded;
            GitRepository.Instance.OnRepositoryRemoved += GitCoreOnRepositoryRemoved;

            _watchers = new Dictionary<string, FileSystemWatcher>();
        }

        protected override void AfterInit()
        {
            if (_manager.ModuleManager.ServiceExists(typeof(ITortoiseCommand)))
            {
                _commandRuner = (ITortoiseCommand)_manager.ModuleManager.GetService(typeof(ITortoiseCommand));
                tsGit.Visible = true;
                tortoiseGitToolStripMenuItem.Visible = true;
            }
            findInLogMenuItem.Enabled = _manager.ModuleManager.ServiceExists(typeof(ITortoiseGitSearch));
        }

        private void GitCoreOnRepositoryRemoved(object sender, RepositoryChangedEventArgs e)
        {
            var node = tvRepositories.Nodes[e.RepositoryName];
            if (node.Name == _lastActiveRepo)
            {
                var otherNode = node.PrevVisibleNode ?? node.NextVisibleNode;
                if (!GitRepository.Instance.SwitchByName(otherNode.Name))
                    _lastActiveRepo = null;
            }
            _watchers[node.Name].EnableRaisingEvents = false;
            _watchers[node.Name].Dispose();
            _watchers.Remove(node.Name);
            node.Remove();


        }

        private void GitCoreOnRepositoryAdded(object sender, RepositoryChangedEventArgs e)
        {
            if (tvRepositories.Nodes.ContainsKey(e.RepositoryName)) return;
            var node = CreateRepoNode(GitRepository.Instance.Repositories.Find(r => r.Name == e.RepositoryName));
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
                nodeNew.ForeColor = _documentRepoColor;
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

            if (repoName != null && repoName.Equals(_lastActiveRepo, StringComparison.InvariantCultureIgnoreCase) || repoName == _lastActiveRepo) return;
            if (!string.IsNullOrEmpty(_lastActiveRepo))
            {
                var nodeOld = tvRepositories.Nodes[_lastActiveRepo];
                nodeOld.NodeFont = new Font(nodeOld.NodeFont ?? tvRepositories.Font, FontStyle.Regular);
                if (isAutoExpand)
                    nodeOld.Collapse();
            }
            if (!string.IsNullOrEmpty(repoName))
            {
                var nodeNew = tvRepositories.Nodes[repoName];
                nodeNew.NodeFont = new Font(nodeNew.NodeFont ?? tvRepositories.Font, FontStyle.Bold);
                nodeNew.Text += string.Empty;
                if (isAutoExpand)
                    nodeNew.Expand();
            }
            _lastActiveRepo = repoName;
        }

        private void tvRepositories_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Left && e.Node.ImageKey == RepoIndex && !(e.Node.NodeFont?.Bold ?? false))
            {
                GitRepository.Instance.SwitchByName(e.Node.Name);
            }
        }

        private void FillContent(TreeNode node, RepositoryLink link)
        {
            var currentBranch = CreateNode(CurrentBranch, "", CurbranchIndex, node, cmNone);
            var local = CreateNode(LocalFolder, Properties.Resources.RsLocal, BranchFolderIndex, node, cmNone);
            local.Tag = NODE_NOLOADED;
            CreateNode(LoadItem, Properties.Resources.RsLoading, LoadingIndex, local);
            var remote = CreateNode(RemoteFolder, Properties.Resources.RsRemote, BranchFolderIndex, node, cmNone);
            remote.Tag = NODE_NOLOADED;
            CreateNode(LoadItem, Properties.Resources.RsLoading, LoadingIndex, remote);
            using (var r = new Repository(link.Path))
            {
                currentBranch.Text = r.Head.FriendlyName;
            }
            var worktree = CreateNode(WorktreeFolder, Properties.Resources.RsWorktree, WorktreeFolderIndex, node, cmWorktree);
            worktree.Tag = NODE_NOLOADED;
            CreateNode(LoadItem, Properties.Resources.RsLoading, LoadingIndex, worktree);
        }

        private TreeNode CreateRepoNode(RepositoryLink link)
        {
            if (link == null) return null;
            var node = CreateNode(link.Name, link.Name, RepoIndex, null, cmRepositories);
            FillContent(node, link);
            // Create file watcher
            var watcher = new FileSystemWatcher(Path.Combine(link.Path, ".git"), HookFilter);
            watcher.NotifyFilter = NotifyFilters.FileName;
            watcher.Deleted += WatchersOnChange;
            watcher.EnableRaisingEvents = true;
            // save handle
            _watchers.Add(link.Name, watcher);
            return node;
        }

        private void WatchersOnChange(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Deleted && FileLock.Equals(e.Name, System.StringComparison.InvariantCultureIgnoreCase))
            {
                UpdateBranch(GitRepository.GetRepoName(GitRepository.GetRootDir(e.FullPath)));
                UpdateWorktree(GitRepository.GetRepoName(GitRepository.GetRootDir(e.FullPath)));
            }
        }

        private void LoadTree()
        {
            var repoList = GitRepository.Instance.Repositories;
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
                        if (_lastActiveRepo != null)
                            tvRepositories.Nodes[_lastActiveRepo].Text += string.Empty;
                    }
                }));
            });
            task.Start();
        }

        private void UpdateState()
        {
            var repoLink = GitRepository.Instance.ActiveRepository;
            if (repoLink != null)
                ActiverRepositoryUpdate(repoLink.Name);
            repoLink = GitRepository.Instance.DocumentRepository;
            if (repoLink != null)
                DocumentRepositoryUpdate(repoLink.Name);
        }

        private void UpdateBranch(string repoName)
        {
            if (string.IsNullOrEmpty(repoName)) return;
            Logger.Debug($"Update branches for repo {repoName}");
            var node = tvRepositories.Nodes[repoName];
            var link = GitRepository.Instance.GetRepositoryByName(repoName);
            if (link == null)
                return;
            using (var r = new Repository(link.Path))
            {
                var currentBranch = node.Nodes[CurrentBranch];
                var local = node.Nodes[LocalFolder];
                var remote = node.Nodes[RemoteFolder];

                Invoke(new Action(() =>
                {
                    tvRepositories.BeginUpdate();
                    currentBranch.Text = r.Head.FriendlyName;
                }));

                try
                {
                    if ((int)local.Tag != NODE_NOLOADED || (int)remote.Tag != NODE_NOLOADED)
                    {
                        foreach (var b in r.Branches.OrderBy(b => b.FriendlyName))
                        {
                            if (!b.IsRemote && (int)local.Tag == NODE_LOADED)
                            {
                                if (!local.Nodes.ContainsKey(b.FriendlyName))
                                {
                                    Invoke(new Action(() =>
                                    {
                                        CreateNode(b.FriendlyName, b.FriendlyName, BranchIndex, local, cmBranch);
                                    }));
                                }
                                var branch = local.Nodes[b.FriendlyName];
                                if (b.IsCurrentRepositoryHead)
                                {
                                    Invoke(new Action(() =>
                                    {
                                        branch.ImageKey = CurbranchIndex;
                                        branch.SelectedImageKey = CurbranchIndex;
                                        branch.Text += string.Empty;
                                    }));
                                }
                                else
                                {
                                    Invoke(new Action(() =>
                                    {
                                        branch.ImageKey = BranchIndex;
                                        branch.SelectedImageKey = BranchIndex;
                                        branch.Text += string.Empty;
                                    }));
                                }
                            }
                            else if (b.IsRemote && (int)remote.Tag == NODE_LOADED)
                            {
                                if (!b.FriendlyName.EndsWith("/HEAD", StringComparison.InvariantCultureIgnoreCase) &&
                                    !remote.Nodes.ContainsKey(b.FriendlyName))
                                {
                                    Invoke(new Action(() =>
                                    {
                                        CreateNode(b.FriendlyName, b.FriendlyName, RemoteBranchIndex, remote, cmBranch);
                                    }));
                                }
                            }
                        }
                    }
                }
                finally
                {
                    Invoke(new Action(() =>
                    {
                        tvRepositories.EndUpdate();
                    }));
                }
            }
        }

        private void UpdateWorktree(string repoName)
        {
            if (string.IsNullOrEmpty(repoName)) return;
            Logger.Debug($"Update worktree for repo {repoName}");
            var worktree = tvRepositories.Nodes[repoName].Nodes[WorktreeFolder];
            var link = GitRepository.Instance.GetRepositoryByName(repoName);
            if (link == null) return;

            Invoke(new Action(() =>
            {
                tvRepositories.BeginUpdate();
                worktree.Nodes.Clear();
            }));
            try
            {
                foreach(var w in link.Worktrees)
                {
                    Invoke(new Action(() =>
                    {
                        var node = CreateNode(w.Branch, w.Branch, w.IsLocked ? WorktreeLockIndex : WorktreeLeafIndex, worktree, cmWorktree);
                        if (!Directory.Exists(w.Path) && !w.IsLocked)
                        {
                            node.ImageKey = node.SelectedImageKey = WorktreeNotExistsIndex;
                        }
                        node.Tag = w;
                    }));
                }
            }
            finally
            {
                Invoke(new Action(() =>
                {
                    tvRepositories.EndUpdate();
                }));
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
                ContextMenuStrip = menu ?? cmNone
            };
            parentNode?.Nodes?.Add(node);
            return node;
        }

        private void miSetActive_Click(object sender, EventArgs e)
        {
            var node = tvRepositories.SelectedNode;
            GitRepository.Instance.SwitchByName(node?.Name);
        }

        private void cmRepositories_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var node = tvRepositories.SelectedNode;
            miSetActive.Enabled = _lastActiveRepo != node?.Name;
            miRemoveRepo.Enabled = _lastDocumentRepo != node?.Name;

            var inRepo = GitRepository.IsValidGitRepo(Npp.NppUtils.CurrentFileDir);

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
                GitRepository.Instance.SwitchByPath(openDlg.SelectedPath);
            }
        }

        private void miRemoveRepo_Click(object sender, EventArgs e)
        {
            var node = tvRepositories.SelectedNode;
            GitRepository.Instance.RemoveRepository(node.Name);
        }

        private void tvRepositories_Before(object sender, TreeViewCancelEventArgs e)
        {
            e.Cancel = _hasDoubleClick;
            _hasDoubleClick = false;
        }

        private void tvRepositories_MouseDown(object sender, MouseEventArgs e)
        {
            var delta = (int)DateTime.Now.Subtract(_lastMouseDown).TotalMilliseconds;
            _hasDoubleClick = (delta < SystemInformation.DoubleClickTime);
            _lastMouseDown = DateTime.Now;
            if (!_hasDoubleClick) return;
            var node = tvRepositories.GetNodeAt(e.Location);
            _hasDoubleClick = node?.ImageKey == RepoIndex;
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
            if (e.Node.ImageKey == BranchFolderIndex && (int)e.Node.Tag == NODE_NOLOADED)
            {
                e.Node.Tag = NODE_LOADED;
                startProgress();
                var task = Task.Factory.StartNew(() =>
                {
                    UpdateBranch(e.Node.Parent.Name);
                }).ContinueWith((r) =>
                {
                    Invoke(new Action(() =>
                    {
                        e.Node.Nodes.RemoveByKey(LoadItem);
                        stopProgress();
                    }));
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
            else if (e.Node.ImageKey == WorktreeFolderIndex && (int)e.Node.Tag == NODE_NOLOADED)
            {
                e.Node.Tag = NODE_LOADED;
                var task = Task.Factory.StartNew(() =>
                {
                    UpdateWorktree(e.Node.Parent.Name);
                }).ContinueWith((r) =>
                {
                    Invoke(new Action(() =>
                    {
                        stopProgress();
                    }));
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        private void RunTortoiseCommandForRepo(TortoiseGitCommand cmd)
        {
            var node = tvRepositories.SelectedNode;
            var path = GitRepository.Instance.GetRepositoryByName(node?.Name)?.Path;
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

        private void findInLogMenuItem_Click(object sender, EventArgs e)
        {
            var node = tvRepositories.SelectedNode;
            var path = GitRepository.Instance.GetRepositoryByName(node?.Name)?.Path;
            if (path != null)
            {
                var searchDialog = new TortoiseLogSearch();
                searchDialog.Init(_manager, 0);
                searchDialog.RepositoryPath = path;
                searchDialog.Show();
            }
        }

        private void cmBranch_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // TODO: Проверки
            // 1. Проверить, что ветка не выгружена
            //   1.1 Да - недоступны оба пункта
            //   1.2 Нет - доступны оба пункта

            var node = tvRepositories.SelectedNode;
            var nodeBranchName = node?.Name;
            var nodeRepoName = node?.Parent.Parent.Name;
            if (nodeBranchName != null && nodeRepoName != null)
            {
                var t = new Task(new Action(() => 
                {
                    UpdateWorktree(nodeRepoName);
                }));
                t.RunSynchronously();

                var worktree = tvRepositories.Nodes[nodeRepoName].Nodes[WorktreeFolder];
                var enabled = !worktree.Nodes.ContainsKey(nodeBranchName) && node.Parent.PrevNode.Text != node.Text;
                miSwitchTo.Enabled = enabled;
                miToWorktree.Enabled = enabled;
            }
        }

        private void miSwitchTo_Click(object sender, EventArgs e)
        {
            var node = tvRepositories.SelectedNode;
            var nodeBranchName = node?.Name;
            var nodeRepoName = node?.Parent.Parent.Name;
            if (nodeBranchName != null && nodeRepoName != null)
            {
                var t = new Task(new Action(() =>
                {
                    ToLog("In repository '{0}' switching to branch '{1}'", nodeRepoName, nodeBranchName);

                    using (var repo = new Repository(GitRepository.Instance.GetRepositoryByName(nodeRepoName)?.Path))
                    {
                        var branch = repo.Branches.Where(b => b.FriendlyName == nodeBranchName)?.First();
                        if (branch != null)
                        {
                            Branch local = null;
                            if (branch.IsRemote)
                            {
                                if (branch.IsTracking)
                                {
                                    local = branch.TrackedBranch;
                                }
                                else
                                {
                                    var localName = branch.FriendlyName.Replace(branch.RemoteName + "/", "");
                                    try
                                    {
                                        local = repo.CreateBranch(localName, branch.Tip);
                                        ToLog("In repository '{0}' created local branch '{1}'", nodeRepoName, nodeBranchName);
                                    }
                                    catch { }
                                }
                            }
                            else
                                local = branch;
                            try
                            {
                                Commands.Checkout(repo, local);
                                ToLog("Switched to branch '{0}'", nodeBranchName);
                            }
                            catch { }
                        }
                    }
                }));

                t.ContinueWith((r) =>
                {
                    MessageBox.Show($"Switched to branch '{nodeBranchName}'", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    stopProgress();
                }, TaskScheduler.FromCurrentSynchronizationContext());

                startProgress();
                t.Start();
            }
        }

        private void ToLog(string format, params object[] args)
        {
            _UIDispatcher.Invoke(new Action(() => 
            {
                Console.WriteLine(format, args);
            }));
        }

        private void cmWorktree_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var node = tvRepositories.SelectedNode;
            if (node == null || node.ImageKey == WorktreeNotExistsIndex)
            {
                e.Cancel = true;
                return;
            }
            var isLeaf = node.ImageKey == WorktreeLeafIndex || node.ImageKey == WorktreeLockIndex;
            miPrune.Visible = miRefresh.Visible = !isLeaf;
            miRemove.Visible = 
                miLock.Visible = 
                miUnlock.Visible = 
                smiGit.Visible = 
                miWTPull.Visible =
                miWTCommit.Visible =
                miWTPush.Visible =
                miWTLog.Visible =
                isLeaf;

            if (isLeaf)
            {
                var w = (Worktree)node.Tag;
                miRemove.Visible = w != null & !w?.IsLocked ?? false;
                miLock.Visible = !w?.IsLocked ?? false;
                miUnlock.Visible = w?.IsLocked ?? false;

                smiGit.Visible =
                miWTPull.Visible =
                miWTCommit.Visible =
                miWTPush.Visible =
                miWTLog.Visible = Directory.Exists(w?.Path);
            }
        }

        private void miToWorktree_Click(object sender, EventArgs e)
        {
            var node = tvRepositories.SelectedNode;
            var nodeBranchName = node?.Name;
            var nodeRepoName = node?.Parent.Parent.Name;
            if (nodeBranchName != null && nodeRepoName != null)
            {
                var t = new Task(new Action(() =>
                {
                    ToLog("In repository '{0}' branch '{1}' checkout to worktree dir", nodeRepoName, nodeBranchName);

                    using (var repo = new Repository(GitRepository.Instance.GetRepositoryByName(nodeRepoName)?.Path))
                    {
                        var branch = repo.Branches.Where(b => b.FriendlyName == nodeBranchName)?.First();
                        if (branch != null)
                        {
                            repo.AddWorktree(branch);
                        }
                    }
                }));
                t.ContinueWith((r) =>
                {
                    UpdateWorktree(nodeRepoName);
                }).ContinueWith((r) =>
                {
                    MessageBox.Show("Branch has been checkout", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    stopProgress();
                }, TaskScheduler.FromCurrentSynchronizationContext());

                startProgress();
                t.Start();
            }
        }

        private void miLock_Click(object sender, EventArgs e)
        {
            var node = tvRepositories.SelectedNode;
            var nodeWorktreePath = ((Worktree)node?.Tag)?.Path;
            var nodeRepoName = node?.Parent.Parent.Name;
            if (nodeWorktreePath != null && nodeRepoName != null)
            {
                var t = new Task(new Action(() =>
                {
                    ToLog("In repository '{0}' worktree '{1}' will be locked", nodeRepoName, nodeWorktreePath);

                    using (var repo = new Repository(GitRepository.Instance.GetRepositoryByName(nodeRepoName)?.Path))
                    {
                        repo.LockWorktree(nodeWorktreePath);
                    }
                }));
                t.ContinueWith((r) =>
                {
                    UpdateWorktree(nodeRepoName);
                }).ContinueWith((r) =>
                {
                    MessageBox.Show("Worktree locked", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    stopProgress();
                }, TaskScheduler.FromCurrentSynchronizationContext());

                startProgress();
                t.Start();
            }

        }

        private void miUnlock_Click(object sender, EventArgs e)
        {
            var node = tvRepositories.SelectedNode;
            var nodeWorktreePath = ((Worktree)node?.Tag)?.Path;
            var nodeRepoName = node?.Parent.Parent.Name;
            if (nodeWorktreePath != null && nodeRepoName != null)
            {
                var t = new Task(new Action(() =>
                {
                    ToLog("In repository '{0}' worktree '{1}' will be unlocked", nodeRepoName, nodeWorktreePath);

                    using (var repo = new Repository(GitRepository.Instance.GetRepositoryByName(nodeRepoName)?.Path))
                    {
                        repo.UnlockWorktree(nodeWorktreePath);
                    }
                }));
                t.ContinueWith((r) =>
                {
                    UpdateWorktree(nodeRepoName);
                }).ContinueWith((r) =>
                {
                    MessageBox.Show("Worktree unlocked", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    stopProgress();
                }, TaskScheduler.FromCurrentSynchronizationContext()); ;

                stopProgress();
                t.Start();
            }
        }

        private void miPrune_Click(object sender, EventArgs e)
        {
            var node = tvRepositories.SelectedNode;
            var nodeRepoName = node?.Parent.Name;
            if (nodeRepoName != null)
            {
                var t = new Task(new Action(() =>
                {
                    ToLog("In repository '{0}' worktrees will be pruned", nodeRepoName);

                    using (var repo = new Repository(GitRepository.Instance.GetRepositoryByName(nodeRepoName)?.Path))
                    {
                        repo.PruneWorktree();
                    }
                }));
                t.ContinueWith((r) =>
                {
                    UpdateWorktree(nodeRepoName);
                }).ContinueWith((r) =>
                {
                    stopProgress();
                    MessageBox.Show("Worktrees has been pruned", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }, TaskScheduler.FromCurrentSynchronizationContext());

                t.Start();
            }

        }

        private void miRefresh_Click(object sender, EventArgs e)
        {
            var node = tvRepositories.SelectedNode;
            var nodeRepoName = node?.Parent.Name;
            if (nodeRepoName != null)
            {
                var t = new Task(new Action(() =>
                {
                    UpdateWorktree(nodeRepoName);
                }));

                t.ContinueWith((r) =>
                {
                    stopProgress();
                }, TaskScheduler.FromCurrentSynchronizationContext());

                startProgress();
                t.Start();
            }
        }

        private void miRemove_Click(object sender, EventArgs e)
        {
            var node = tvRepositories.SelectedNode;
            var nodeWorktreePath = ((Worktree)node?.Tag)?.Path;
            var nodeRepoName = node?.Parent.Parent.Name;
            if (nodeWorktreePath != null && nodeRepoName != null)
            {
                if (MessageBox.Show($"Remove worktree path '{nodeWorktreePath}'?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Information) != DialogResult.Yes)
                    return;

                var t = new Task(new Action(() =>
                {
                    ToLog("In repository '{0}' worktree '{1}' will be removed", nodeRepoName, nodeWorktreePath);

                    using (var repo = new Repository(GitRepository.Instance.GetRepositoryByName(nodeRepoName)?.Path))
                    {
                        repo.RemoveWorktree(nodeWorktreePath);
                    }
                }));
                t.ContinueWith((r) =>
                {
                    UpdateWorktree(nodeRepoName);
                }).ContinueWith((r) =>
                {
                    MessageBox.Show("Worktree has been removed", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    stopProgress();
                }, TaskScheduler.FromCurrentSynchronizationContext());

                startProgress();
                t.Start();
            }
        }

        private void startProgress()
        {
            pbProgress.Show();
            pbProgress.Style = ProgressBarStyle.Marquee;
            pbProgress.MarqueeAnimationSpeed = 30;
        }

        private void stopProgress()
        {
            pbProgress.MarqueeAnimationSpeed = 0;
            pbProgress.Style = ProgressBarStyle.Continuous;
            pbProgress.Hide();

        }

        private void miWTPull_Click(object sender, EventArgs e)
        {
            var node = tvRepositories.SelectedNode;
            var nodeWorktree = ((Worktree)node?.Tag);

            if (nodeWorktree != null)
            {
                var t = new Task(new Action(() =>
                {
                    nodeWorktree.Pull();
                }));
                t.ContinueWith((r) =>
                {
                    stopProgress();
                }, TaskScheduler.FromCurrentSynchronizationContext());

                startProgress();
                t.Start();
            }
        }

        private void miWTCommit_Click(object sender, EventArgs e)
        {
            var node = tvRepositories.SelectedNode;
            var nodeWorktree = ((Worktree)node?.Tag);

            if (nodeWorktree != null)
            {
                var t = new Task(new Action(() =>
                {
                    nodeWorktree.Commit();
                }));
                t.ContinueWith((r) =>
                {
                    stopProgress();
                }, TaskScheduler.FromCurrentSynchronizationContext());

                startProgress();
                t.Start();
            }
        }

        private void miWTPush_Click(object sender, EventArgs e)
        {
            var node = tvRepositories.SelectedNode;
            var nodeWorktree = ((Worktree)node?.Tag);

            if (nodeWorktree != null)
            {
                var t = new Task(new Action(() =>
                {
                    nodeWorktree.Push();
                }));
                t.ContinueWith((r) =>
                {
                    stopProgress();
                }, TaskScheduler.FromCurrentSynchronizationContext());

                startProgress();
                t.Start();
            }
        }

        private void miWTLog_Click(object sender, EventArgs e)
        {
            var node = tvRepositories.SelectedNode;
            var nodeWorktree = ((Worktree)node?.Tag);

            if (nodeWorktree != null)
            {
                var t = new Task(new Action(() =>
                {
                    nodeWorktree.Log();
                }));
                t.ContinueWith((r) =>
                {
                    stopProgress();
                }, TaskScheduler.FromCurrentSynchronizationContext());

                startProgress();
                t.Start();
            }

        }
    }

}
