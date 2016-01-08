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

        private void ShowBranch()
        {
            Settings.Functions.ShowBranch = !Settings.Functions.ShowBranch;
            _manager.SetCheckedMenu(showBranchCmdID, Settings.Functions.ShowBranch);
            DoShowBranch(Settings.Functions.ShowBranch);
        }

        private void DoShowBranch(bool isShow = true)
        {
            var repoDir = PluginUtils.GetRootDir(PluginUtils.CurrentFileDir);
            if (!string.IsNullOrEmpty(repoDir) && LibGit2Sharp.Repository.IsValid(repoDir))
            {
                using (var repo = new LibGit2Sharp.Repository(repoDir))
                {
                    var branch = " [Branch: " + repo.Head.Name + "]";
                    if (isShow)
                    {
                        PluginUtils.WindowTitle += branch;
                    }
                    else
                    {
                        var title = PluginUtils.WindowTitle;
                        PluginUtils.WindowTitle = title.Replace(branch, "");
                    }
                }
            }
        }

        public void ChangeContext(TabEventArgs args)
        {
            if (Settings.Functions.ShowBranch)
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
