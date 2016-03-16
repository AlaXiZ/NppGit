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
using NppGit.Forms;
using NppGit.Common;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace NppGit.Modules
{
    public class Git : IModule
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
                    var status = repo.RetrieveStatus(PluginUtils.CurrentFilePath);
                    if (status != FileStatus.Ignored)
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
                            }
                            else
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
            _manager.OnSystemInit += ManagerOnSystemInit;

            _showBranchCmdID = _manager.RegisteCommandItem(new CommandItem
            {
                Name = "Branch in title",
                Hint = "Branch in title",
                ShortcutKey = new ShortcutKey { _isCtrl = 1, _isAlt = 1, _isShift = 1, _key = (byte)System.Windows.Forms.Keys.R },
                Action = ShowBranch,
                Checked = Settings.Functions.ShowBranch
            });

            _showRepoCmdID = _manager.RegisteCommandItem(new CommandItem
            {
                Name = "Repository in title",
                Hint = "Repository in title",
                ShortcutKey = new ShortcutKey {  },
                Action = ShowRepoName,
                Checked = Settings.Functions.ShowRepoName
            });

            _manager.RegisteCommandItem(new CommandItem
            {
                Name = "File in other branch",
                Hint = "File in other branch",
                ShortcutKey = null,
                Action = OpenFileInOtherBranch
            });

            _showStatusFileCmdID = _manager.RegisteCommandItem(new CommandItem
            {
                Name = "File status in title",
                Hint = "File status in title",
                ShortcutKey = null,
                Action = ShowFileStatus,
                Checked = Settings.Functions.ShowStatusFile
            });

            _statusPanelCmdID = _manager.RegisteCommandItem(new CommandItem
            {
                Name = "Status panel",
                Hint = "Status panel",
                ShortcutKey = null,
                Action = DoShowStatus,
                Checked = Settings.Panels.FileStatusPanelVisible
            });

            _manager.RegisterDockForm(typeof(Status), _statusPanelCmdID, true);

            //------------------------------------------------------------------
            _manager.RegisteCommandItem(new CommandItem
            {
                Name = "-",
                Hint = "-",
                Action = null
            });
        }

        private void ManagerOnSystemInit()
        {
            if (Settings.Panels.FileStatusPanelVisible)
            {
                DoShowStatus();
            }
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
                            var outFile = Common.GitHelper.SaveStreamToFile(contentStream, fileName);
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
