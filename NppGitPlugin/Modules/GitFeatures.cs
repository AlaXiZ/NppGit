using LibGit2Sharp;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NppGit
{
    public class GitFeatures : IModule
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private IModuleManager _manager;
        private int showBranchCmdID = -1;
        private int showRepoCmdID = -1;

        private void ShowBranch()
        {
            Settings.Functions.ShowBranch = !Settings.Functions.ShowBranch;
            _manager.SetCheckedMenu(showBranchCmdID, Settings.Functions.ShowBranch);
            DoShowBranch();
        }

        private void ShowRepoName()
        {
            Settings.Functions.ShowRepoName = !Settings.Functions.ShowRepoName;
            _manager.SetCheckedMenu(showRepoCmdID, Settings.Functions.ShowRepoName);
            DoShowBranch();
        }

        private void DoShowBranch()
        {
            bool isShowBranch = Settings.Functions.ShowBranch,
                 isShowRepo = Settings.Functions.ShowRepoName;
            var repoDir = PluginUtils.GetRootDir(PluginUtils.CurrentFileDir);
            if (!string.IsNullOrEmpty(repoDir) && LibGit2Sharp.Repository.IsValid(repoDir))
            {
                using (var repo = new LibGit2Sharp.Repository(repoDir))
                {
                    var title = PluginUtils.WindowTitle;
                    title = title.Substring(0, title.LastIndexOf("++") + 2);
                    var strBuild = new StringBuilder();
                    if (isShowRepo)
                    {
                        string remote = "";
                        if (repo.Network.Remotes.Count() > 0)
                        {
                            var remoteUrl = repo.Network.Remotes.First().Url;
                            if (!string.IsNullOrEmpty(remoteUrl))
                            {
                                remote = remoteUrl.Substring(remoteUrl.LastIndexOf('/') + 1, remoteUrl.Length - remoteUrl.LastIndexOf('/') - 1).Replace(".git", "");
                            }
                        } else
                        {
                            remote = new DirectoryInfo(repoDir).Name;
                        }
                        strBuild.Append(remote);
                    }
                    if (isShowBranch)
                    {
                        if (strBuild.Length > 0)
                            strBuild.Append(" / ");
                        strBuild.Append(repo.Head.Name);
                    }
                    if (strBuild.Length > 0)
                        title += " [ " + strBuild.ToString() + " ]";
                    PluginUtils.WindowTitle = title;
                }
            }
        }

        public void ChangeContext(TabEventArgs args)
        {
            if (Settings.Functions.ShowBranch || Settings.Functions.ShowRepoName)
                DoShowBranch();
        }

        public void Final()
        {
        }

        public void Init(IModuleManager manager)
        {
            _manager = manager;
            _manager.OnTabChangeEvent += ChangeContext;

            showBranchCmdID = _manager.RegisterMenuItem(new MenuItem
            {
                Name = "Branch in title",
                Hint = "Branch in title",
                ShortcutKey = new ShortcutKey { _isCtrl = 1, _isAlt = 1, _isShift = 1, _key = (byte)System.Windows.Forms.Keys.R },
                Action = ShowBranch,
                Checked = Settings.Functions.ShowBranch
            });

            showRepoCmdID = _manager.RegisterMenuItem(new MenuItem
            {
                Name = "Repository in title",
                Hint = "Repository in title",
                ShortcutKey = new ShortcutKey {  },
                Action = ShowRepoName,
                Checked = Settings.Functions.ShowRepoName
            });

            _manager.RegisterMenuItem(new MenuItem
            {
                Name = "File in other branch",
                Hint = "File in other branch",
                ShortcutKey = null,
                Action = OpenFileInOtherBranch
            });

        }

        private async void OpenFileInOtherBranch()
        {
            var repoDir = PluginUtils.GetRootDir(PluginUtils.CurrentFileDir);
            try
            {
                using (var repo = new Repository(repoDir))
                {
                    var dlg = new Forms.BranchList();
                    dlg.RepoDirectory = repoDir;
                    // Replace "\" to "/" and delete first "/"
                    var fileInRepo = PluginUtils.CurrentFilePath.Replace(repoDir, "").Replace("\\", "/").Remove(0, 1);

                    if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        Branch branch = null;
                        foreach (var b in repo.Branches)
                        {
                            if (b.Name == dlg.SelectedItem)
                            {
                                branch = b;
                                break;
                            }
                        }
                        if (branch != null)
                        {
                            var commit = repo.Lookup<Commit>(branch.Tip.Id);
                            var blob = (Blob)commit[fileInRepo].Target;
                            var fileName = commit.Sha.Substring(1, Settings.Functions.SHACount) + "_" + PluginUtils.CurrentFileName;
                            var contentStream = blob.GetContentStream();
                            var outFile = await Utils.Git.SaveStreamToFileAsync(contentStream, fileName);
                            if (outFile != null)
                            {
                                var curFile = PluginUtils.CurrentFilePath;
                                PluginUtils.OpenFile(outFile);
                                if (Settings.Functions.OpenFileInOtherView)
                                {
                                    PluginUtils.MoveFileToOtherView();
                                    PluginUtils.CurrentFilePath = curFile;
                                }
                            }
                        }
                    }
                }
            }
            catch(Exception e)
            {
                logger.Debug(e, "Directory {0} isn't git repository!", repoDir);
            }
        }

    }
}
