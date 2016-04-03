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
using NppKate.Forms;
using NppKate.Common;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace NppKate.Modules
{
    public class Git : IModule
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private IModuleManager _manager;
        private int _statusPanelCmdID = -1;

        public bool IsNeedRun
        {
            get { return Settings.Modules.Git; }
        }

        private void DoTitleUpdate()
        {
            _manager.ManualTitleUpdate();
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
            _manager.OnTitleChangedEvent += OnTitleChangedEvent;
            _manager.OnSystemInit += ManagerOnSystemInit;

            _manager.RegisteCommandItem(new CommandItem
            {
                Name = "File in other branch",
                Hint = "File in other branch",
                ShortcutKey = null,
                Action = OpenFileInOtherBranch
            });

            /*
            _statusPanelCmdID = _manager.RegisteCommandItem(new CommandItem
            {
                Name = "Status panel",
                Hint = "Status panel",
                ShortcutKey = null,
                Action = DoShowStatus,
                Checked = Settings.Panels.FileStatusPanelVisible
            });
            */

            _manager.RegisterDockForm(typeof(Status), _statusPanelCmdID, true, NppTbMsg.DWS_PARAMSALL | NppTbMsg.DWS_DF_CONT_BOTTOM);

            //------------------------------------------------------------------
            _manager.RegisteCommandItem(new CommandItem
            {
                Name = "-",
                Hint = "-",
                Action = null
            });
        }

        private void OnTitleChangedEvent(object sender, TitleChangedEventArgs e)
        {
            if (Settings.Functions.ShowBranch || Settings.Functions.ShowRepoName || Settings.Functions.ShowStatusFile)
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
                            var title = new StringBuilder();
                            if (isShowRepo)
                            {                                
                                string remote = GitCore.GitCore.GetRepoName(repoDir);
                                title.Append(remote);
                            }
                            if (isShowBranch)
                            {
                                if (title.Length > 0)
                                    title.Append(":");
                                title.Append(repo.Head.FriendlyName);
                            }
                            if (isShowStatus)
                            {
                                if (title.Length > 0)
                                    title.Append(":");
                                title.Append(repo.RetrieveStatus(PluginUtils.CurrentFilePath).ToString());
                            }
                            e.AddTitleItem("File in repo: " + title.ToString());
                        }
                    }
                }
            }
        }

        private void ManagerOnSystemInit()
        {
            if (Settings.Panels.FileStatusPanelVisible)
            {
                DoShowStatus();
            }
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
                            if (b.FriendlyName == dlg.SelectedItem)
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
            catch (Exception e)
            {
                logger.Debug(e, "Directory {0} isn't git repository!", repoDir);
            }
        }

    }
}
