using LibGit2Sharp;
using NLog;
using NppGit.Forms;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace NppGit
{
    public class GitFeatures : IModule
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private IModuleManager _manager;
        private int _showBranchCmdID = -1;
        private int _showRepoCmdID = -1;
        private int _showStatusFileCmdID = -1;
        private int _statusPanelCmdID = -1;
        private static string _ending = "";

        public bool IsNeedRun
        {
            get { return Settings.Modules.Git; }
        }

        private void ShowBranch()
        {
            Settings.Functions.ShowBranch = !Settings.Functions.ShowBranch;
            _manager.SetCheckedMenu(_showBranchCmdID, Settings.Functions.ShowBranch);
            DoTitleUpdate();
        }

        private void ShowRepoName()
        {
            Settings.Functions.ShowRepoName = !Settings.Functions.ShowRepoName;
            _manager.SetCheckedMenu(_showRepoCmdID, Settings.Functions.ShowRepoName);
            DoTitleUpdate();
        }

        private void ShowFileStatus()
        {
            Settings.Functions.ShowStatusFile = !Settings.Functions.ShowStatusFile;
            _manager.SetCheckedMenu(_showStatusFileCmdID, Settings.Functions.ShowStatusFile);
            DoTitleUpdate();
        }

        private void DoTitleUpdate()
        {
            bool isShowBranch = Settings.Functions.ShowBranch,
                 isShowRepo = Settings.Functions.ShowRepoName,
                 isShowStatus = Settings.Functions.ShowStatusFile;

            var repoDir = PluginUtils.GetRootDir(PluginUtils.CurrentFileDir);
            if (!string.IsNullOrEmpty(repoDir) && Repository.IsValid(repoDir))
            {
                using (var repo = new Repository(repoDir))
                {
                    var title = PluginUtils.WindowTitle;
                    // Заголовок может заканчиваться на Notepad++ или на [Administrator]
                    // но всегда есть разделительный дефис между имененем файла и Notepad++
                    if (string.IsNullOrEmpty(_ending))
                    {
                        // Ищем последний дефис
                        var pos = title.LastIndexOf(" - ") + 3;
                        // Получаем окончание для заголовка
                        _ending = title.Substring(pos, title.Length - pos);
                    }
                    // Вдруг на пришел заголовок с нашими дописками,
                    // сначала их порежем
                    title = title.Substring(0, title.LastIndexOf(_ending) + _ending.Length);
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
                    if (isShowStatus)
                    {
                        if (strBuild.Length > 0)
                            strBuild.Append(" / ");
                        strBuild.Append(repo.RetrieveStatus(PluginUtils.CurrentFilePath).ToString());
                    }
                    if (strBuild.Length > 0)
                        title += " [ " + strBuild.ToString() + " ]";
                    PluginUtils.WindowTitle = title;
                }
            }
        }

        private void DoShowStatus()
        {
            Settings.Panels.FileStatusPanelVisible = _manager.ToogleFormState(_statusPanelCmdID);
        }

        public void Final()
        {
        }

        public void Init(IModuleManager manager)
        {
            _manager = manager;
            _manager.OnTitleChangingEvent += TitleChanging;

            _showBranchCmdID = _manager.RegisterMenuItem(new MenuItem
            {
                Name = "Branch in title",
                Hint = "Branch in title",
                ShortcutKey = new ShortcutKey { _isCtrl = 1, _isAlt = 1, _isShift = 1, _key = (byte)System.Windows.Forms.Keys.R },
                Action = ShowBranch,
                Checked = Settings.Functions.ShowBranch
            });

            _showRepoCmdID = _manager.RegisterMenuItem(new MenuItem
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

            _showStatusFileCmdID = _manager.RegisterMenuItem(new MenuItem
            {
                Name = "File status in title",
                Hint = "File status in title",
                ShortcutKey = null,
                Action = ShowFileStatus,
                Checked = Settings.Functions.ShowStatusFile
            });

            _statusPanelCmdID = _manager.RegisterMenuItem(new MenuItem
            {
                Name = "Status panel",
                Hint = "Status panel",
                ShortcutKey = null,
                Action = DoShowStatus,
                Checked = Settings.Panels.FileStatusPanelVisible
            });

            _manager.RegisterDockForm(typeof(Status), _statusPanelCmdID, true);

            if (Settings.Panels.FileStatusPanelVisible)
            {
                DoShowStatus();
            }
            //------------------------------------------------------------------
            _manager.RegisterMenuItem(new MenuItem
            {
                Name = "-",
                Hint = "-",
                Action = null
            });
        }

        private void TitleChanging()
        {
            if (Settings.Functions.ShowBranch || Settings.Functions.ShowRepoName || Settings.Functions.ShowStatusFile)
                DoTitleUpdate();
        }

        private void OpenFileInOtherBranch()
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
                            var outFile = Utils.Git.SaveStreamToFile(contentStream, fileName);
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
