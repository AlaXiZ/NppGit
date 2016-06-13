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

using NLog;
using NppKate.Common;
using NppKate.Npp;
using NppKate.Resources;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace NppKate.Modules.TortoiseGitFeatures
{
    public class TortoiseGitHelper : IModule, ITortoiseCommand
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private const string EXE = "TortoiseGitProc.exe";
        private const string PARAM = "/command:{0} {2} /path:\"{1}\"";
        private const string LOG_MSG = "/logmsg:\"{0}\"";
        private const string CLOSE = "/closeonend:{0}";
        private const string TORTOISEGITBIN = "TortoiseGit\\bin\\";

        private static string _tortoiseGitPath = "";
        private static string _tortoiseGitProc = "";

        private Dictionary<int, string> _icons;
        private IModuleManager _manager;

        public bool IsNeedRun => Settings.Modules.TortoiseGit;

        private static bool ExistsTortoiseGit(string programPath)
        {
            return System.IO.Directory.Exists(System.IO.Path.Combine(programPath, TORTOISEGITBIN));
        }

        private static bool SearchTortoiseGit()
        {
            var tortoisePath = Settings.TortoiseGitProc.Path;
            // Path not set
            if (string.IsNullOrEmpty(tortoisePath) && Settings.TortoiseGitProc.IsFirstSearch)
            {
                // x64
                if (8 == IntPtr.Size || (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432"))))
                {
                    var path = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
                    if (ExistsTortoiseGit(path))
                    {
                        tortoisePath = System.IO.Path.Combine(path, TORTOISEGITBIN);
                    }
                }
                if (string.IsNullOrEmpty(tortoisePath))
                {
                    var path = Environment.GetEnvironmentVariable("ProgramFiles").Replace(" (x86)", "");
                    if (ExistsTortoiseGit(path))
                    {
                        tortoisePath = System.IO.Path.Combine(path, TORTOISEGITBIN);
                    }
                }
                if (string.IsNullOrEmpty(tortoisePath))
                {
                    var dlg = new FolderBrowserDialog
                    {
                        Description = "Выберите папку с TortoiseGitProc.exe",
                        ShowNewFolderButton = false
                    };
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        tortoisePath = dlg.SelectedPath;
                    }
                }
                if (!string.IsNullOrEmpty(tortoisePath))
                {
                    Settings.TortoiseGitProc.Path = tortoisePath;
                }
                Settings.TortoiseGitProc.IsFirstSearch = false;
            }
            _tortoiseGitPath = tortoisePath;
            _tortoiseGitProc = System.IO.Path.Combine(_tortoiseGitPath, EXE);

            return !string.IsNullOrEmpty(_tortoiseGitPath);
        }

        private static string GetCommandName(TortoiseGitCommand command)
        {
            return command.ToString("G").ToLower();
        }

        private static void StartCommand(string param)
        {
            System.Diagnostics.Process.Start(_tortoiseGitProc, param);
        }

        private static string CreateCommand(TortoiseGitCommand command, string path, string logMsg = null, byte? closeParam = null, string additionalParam = null)
        {
            var builder = new StringBuilder();
            string addParam;
            if (string.IsNullOrEmpty(additionalParam) || string.IsNullOrWhiteSpace(additionalParam))
            {
                addParam = "";
            }
            else
            {
                addParam = additionalParam;
            }

            builder.AppendFormat(PARAM, GetCommandName(command), path, addParam);

            if (!string.IsNullOrEmpty(logMsg) && !string.IsNullOrWhiteSpace(logMsg))
            {
                builder.Append(" ").AppendFormat(LOG_MSG, logMsg);
            }

            builder.Append(" ").AppendFormat(CLOSE, closeParam > 2 ? 0 : closeParam);

            return builder.ToString();
        }

        private static bool CheckRepoAndShowError()
        {
            if (GitCore.GitCore.Instance.ActiveRepository == null)
            {
                MessageBox.Show("Нет активного репозитория!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return GitCore.GitCore.Instance.ActiveRepository != null;
        }

        private static void GitLogFile()
        {
            string filePath = NppUtils.CurrentFilePath;
            StartCommand(CreateCommand(TortoiseGitCommand.Log, filePath));
        }

        private static void GitLogPath()
        {
            string dirPath = NppUtils.CurrentFileDir;
            StartCommand(CreateCommand(TortoiseGitCommand.Log, dirPath));
        }

        private static void GitLogRepo()
        {
            if (CheckRepoAndShowError())
            {
                string dirPath = GitCore.GitCore.Instance.ActiveRepository.Path;
                StartCommand(CreateCommand(TortoiseGitCommand.Log, dirPath));
            }
        }

        private static void GitFetch()
        {
            if (CheckRepoAndShowError())
            {
                string dirPath = GitCore.GitCore.Instance.ActiveRepository.Path;
                StartCommand(CreateCommand(TortoiseGitCommand.Fetch, dirPath));
            }
        }

        private static void GitPull()
        {
            if (CheckRepoAndShowError())
            {
                string dirPath = GitCore.GitCore.Instance.ActiveRepository.Path;
                StartCommand(CreateCommand(TortoiseGitCommand.Pull, dirPath));
            }
        }

        private static void GitPush()
        {
            if (CheckRepoAndShowError())
            {
                string dirPath = GitCore.GitCore.Instance.ActiveRepository.Path;
                StartCommand(CreateCommand(TortoiseGitCommand.Push, dirPath));
            }
        }

        private static void GitCommit()
        {
            if (CheckRepoAndShowError())
            {
                string dirPath = GitCore.GitCore.Instance.ActiveRepository.Path;
                StartCommand(CreateCommand(TortoiseGitCommand.Commit, dirPath));
            }
        }

        private static void GitBlame()
        {
            string filePath = NppUtils.CurrentFilePath;
            StartCommand(CreateCommand(TortoiseGitCommand.Blame, filePath));
        }

        private static void GitBlameCurrentLine()
        {
            string filePath = NppUtils.CurrentFilePath;
            string param = string.Format("/line:{0}", NppUtils.CurrentLine);
            StartCommand(CreateCommand(TortoiseGitCommand.Blame, filePath, additionalParam: param));
        }

        private static void GitSwitch()
        {
            if (CheckRepoAndShowError())
            {
                string dirPath = GitCore.GitCore.Instance.ActiveRepository.Path;
                StartCommand(CreateCommand(TortoiseGitCommand.Switch, dirPath));
            }
        }

        private static void GitStashSave()
        {
            if (CheckRepoAndShowError())
            {
                string dirPath = GitCore.GitCore.Instance.ActiveRepository.Path;
                string msg = "/msg:" + DateTime.Now.ToString();
                StartCommand(CreateCommand(TortoiseGitCommand.StashSave, dirPath, additionalParam: msg));
            }
        }

        private static void GitStashPop()
        {
            if (CheckRepoAndShowError())
            {
                string dirPath = GitCore.GitCore.Instance.ActiveRepository.Path;
                StartCommand(CreateCommand(TortoiseGitCommand.StashPop, dirPath));
            }
        }

        private static void GitRepoStatus()
        {
            if (CheckRepoAndShowError())
            {
                string dirPath = GitCore.GitCore.Instance.ActiveRepository.Path;
                StartCommand(CreateCommand(TortoiseGitCommand.RepoStatus, dirPath));
            }
        }

        private static void GitDiff()
        {
            string path = NppUtils.CurrentFilePath;
            StartCommand(CreateCommand(TortoiseGitCommand.Diff, path));
        }

        private static void GitDiffUnified()
        {
            string path = NppUtils.CurrentFilePath;
            StartCommand(CreateCommand(TortoiseGitCommand.Diff, path, additionalParam: "/unified"));
        }

        private static void GitRebase()
        {
            if (CheckRepoAndShowError())
            {
                string path = GitCore.GitCore.Instance.ActiveRepository.Path;
                StartCommand(CreateCommand(TortoiseGitCommand.Rebase, path));
            }
        }

        private static void GitAddFile()
        {
            // TODO: В какой репозиторий добавляется файл?
            if (CheckRepoAndShowError())
            {
                string path = NppUtils.CurrentFilePath;
                StartCommand(CreateCommand(TortoiseGitCommand.Add, path));
            }
        }

        private static void GitRevertFile()
        {
            string path = NppUtils.CurrentFilePath;
            StartCommand(CreateCommand(TortoiseGitCommand.Revert, path));
        }

        private static void GitRepoBrowser()
        {
            if (CheckRepoAndShowError())
            {
                string path = GitCore.GitCore.Instance.ActiveRepository.Path;
                StartCommand(CreateCommand(TortoiseGitCommand.RepoBrowser, path));
            }
        }

        private static void GitStashApply()
        {
            if (CheckRepoAndShowError())
            {
                string path = GitCore.GitCore.Instance.ActiveRepository.Path;
                StartCommand(CreateCommand(TortoiseGitCommand.StashApply, path));
            }
        }

        private static void GitRefBrowse()
        {
            if (CheckRepoAndShowError())
            {
                string path = GitCore.GitCore.Instance.ActiveRepository.Path;
                StartCommand(CreateCommand(TortoiseGitCommand.RefBrowse, path));
            }
        }

        private static void GitIgnore()
        {
            if (CheckRepoAndShowError())
            {
                string path = NppUtils.CurrentFilePath;
                StartCommand(CreateCommand(TortoiseGitCommand.Ignore, path));
            }
        }

        private static void GitExport()
        {
            if (CheckRepoAndShowError())
            {
                string path = GitCore.GitCore.Instance.ActiveRepository.Path;
                StartCommand(CreateCommand(TortoiseGitCommand.Export, path));
            }
        }

        private static void GitMerge()
        {
            if (CheckRepoAndShowError())
            {
                string path = GitCore.GitCore.Instance.ActiveRepository.Path;
                StartCommand(CreateCommand(TortoiseGitCommand.Merge, path));
            }
        }

        private static void GitCleanup()
        {
            if (CheckRepoAndShowError())
            {
                string path = GitCore.GitCore.Instance.ActiveRepository.Path;
                StartCommand(CreateCommand(TortoiseGitCommand.CleanUp, path));
            }
        }

        private static void GitRemove()
        {
            if (CheckRepoAndShowError())
            {
                string path = NppUtils.CurrentFilePath;
                StartCommand(CreateCommand(TortoiseGitCommand.Remove, path));
            }
        }

        private static void GitRename()
        {
            if (CheckRepoAndShowError())
            {
                string path = NppUtils.CurrentFilePath;
                StartCommand(CreateCommand(TortoiseGitCommand.Rename, path));
            }
        }

        private static void GitConflictEditor()
        {
            // TODO: Когда вызывается?
            if (CheckRepoAndShowError())
            {
                string path = NppUtils.CurrentFilePath;
                StartCommand(CreateCommand(TortoiseGitCommand.ConflictEditor, path));
            }
        }

        private static void GitRefLog()
        {
            if (CheckRepoAndShowError())
            {
                string path = GitCore.GitCore.Instance.ActiveRepository.Path;
                StartCommand(CreateCommand(TortoiseGitCommand.RefLog, path));
            }
        }

        private static void GitRevisionGraph()
        {
            if (CheckRepoAndShowError())
            {
                string path = GitCore.GitCore.Instance.ActiveRepository.Path;
                StartCommand(CreateCommand(TortoiseGitCommand.RevisionGraph, path));
            }
        }

        private static void GitTag()
        {
            if (CheckRepoAndShowError())
            {
                string path = GitCore.GitCore.Instance.ActiveRepository.Path;
                StartCommand(CreateCommand(TortoiseGitCommand.Tag, path));
            }
        }

        private static void GitDaemon()
        {
            if (CheckRepoAndShowError())
            {
                string path = GitCore.GitCore.Instance.ActiveRepository.Path;
                StartCommand(CreateCommand(TortoiseGitCommand.Daemon, path));
            }
        }

        private static void GitPGPfp()
        {
            if (CheckRepoAndShowError())
            {
                string path = GitCore.GitCore.Instance.ActiveRepository.Path;
                StartCommand(CreateCommand(TortoiseGitCommand.PGPfp, path));
            }
        }

        private static string SelectFolder(string title)
        {
            var dlg = new FolderBrowserDialog
            {
                ShowNewFolderButton = true,
                Description = title
            };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                return dlg.SelectedPath;
            }
            else
            {
                return "";
            }
        }

        private static void GitClone()
        {
            var path = SelectFolder("Папка назначения");
            if (!string.IsNullOrEmpty(path))
            {
                StartCommand(CreateCommand(TortoiseGitCommand.Clone, path));
            }
        }

        private static void GitRepoCreate()
        {
            var path = SelectFolder("Папка назначения");
            if (!string.IsNullOrEmpty(path))
            {
                StartCommand(CreateCommand(TortoiseGitCommand.RepoCreate, path));
            }
        }

        private static void GitResolve()
        {
            if (CheckRepoAndShowError())
            {
                string path = GitCore.GitCore.Instance.ActiveRepository.Path;
                StartCommand(CreateCommand(TortoiseGitCommand.Resolve, path));
            }
        }

        private static void GitApplyPatchSerial()
        {
            if (CheckRepoAndShowError())
            {
                string path = GitCore.GitCore.Instance.ActiveRepository.Path;
                StartCommand(CreateCommand(TortoiseGitCommand.ImportPatch, path));
            }
        }

        private static void GitCreatePatchSerial()
        {
            if (CheckRepoAndShowError())
            {
                string path = GitCore.GitCore.Instance.ActiveRepository.Path;
                StartCommand(CreateCommand(TortoiseGitCommand.FormatPatch, path));
            }
        }

        private static void GitStashList()
        {
            if (CheckRepoAndShowError())
            {
                string path = GitCore.GitCore.Instance.ActiveRepository.Path;
                StartCommand(CreateCommand(TortoiseGitCommand.RefLog, path, additionalParam: @"/ref:refs/stash"));
            }
        }

        private static void ReadmeFunc()
        {
            const string text = "Не установлен TortoiseGit или не найдена папка с установленной программой!";
            MessageBox.Show(text, "Ошибка настройки", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        void IModule.Init(IModuleManager manager)
        {
            _manager = manager;
            _manager.OnToolbarRegisterEvent += ToolBarInit;
            _manager.RegisterService(typeof(ITortoiseCommand), this);

            logger.Debug("Create menu");
            _icons = new Dictionary<int, string>();
            var selfName = GetType().Name;
            if (SearchTortoiseGit())
            {
                var btnMask = Settings.TortoiseGitProc.ButtonMask;

                logger.Info("TortoiseGit found");

                var cmdId = _manager.CommandManager.RegisterCommand(selfName, "Pull", GitPull, false, new ShortcutKey("Alt+P"));
                if ((btnMask & (uint)TortoiseGitCommand.Pull) > 0)
                    _icons.Add(cmdId, ExternalResourceName.IDB_PULL);

                cmdId = _manager.CommandManager.RegisterCommand(selfName, "Push", GitPush, false, new ShortcutKey("Ctrl+Alt+P"));
                if ((btnMask & (uint)TortoiseGitCommand.Push) > 0)
                    _icons.Add(cmdId, ExternalResourceName.IDB_PUSH);

                cmdId = _manager.CommandManager.RegisterCommand(selfName, "Commit", GitCommit, false, new ShortcutKey("Ctrl+Alt+Shift+C"));
                if ((btnMask & (uint)TortoiseGitCommand.Commit) > 0)
                    _icons.Add(cmdId, ExternalResourceName.IDB_COMMIT);

                cmdId = _manager.CommandManager.RegisterCommand(selfName, "Fetch", GitFetch, false, new ShortcutKey("Ctrl+Alt+Shift+F"));
                if ((btnMask & (uint)TortoiseGitCommand.Fetch) > 0)
                    _icons.Add(cmdId, ExternalResourceName.IDB_PULL);
                /**********************************************************************************/
                _manager.CommandManager.RegisterCommand(selfName, "Diff", GitDiffUnified);
                _manager.CommandManager.RegisterCommand(selfName, "Compare", GitDiff);
                /**********************************************************************************/
                _manager.CommandManager.RegisterCommand(selfName, "Log file", GitLogFile, false, new ShortcutKey("Ctrl+Shift+L"));
                _manager.CommandManager.RegisterCommand(selfName, "Log path", GitLogPath);
                cmdId = _manager.CommandManager.RegisterCommand(selfName, "Log repository", GitLogRepo, false, new ShortcutKey("Ctrl+Alt+Shift+L"));
                if ((btnMask & (uint)TortoiseGitCommand.Log) > 0)
                    _icons.Add(cmdId, ExternalResourceName.IDB_LOG);

                _manager.CommandManager.RegisterCommand(selfName, "Show Reflog", GitRefLog);
                cmdId = _manager.CommandManager.RegisterCommand(selfName, "Blame file", GitBlame, false, new ShortcutKey("Ctrl+Alt+B"));
                if ((btnMask & (uint)TortoiseGitCommand.Blame) > 0)
                    _icons.Add(cmdId, ExternalResourceName.IDB_BLAME);

                _manager.CommandManager.RegisterCommand(selfName, "Blame line", GitBlameCurrentLine, false, new ShortcutKey("Ctrl+Alt+Shift+B"));
                cmdId = _manager.CommandManager.RegisterCommand(selfName, "Check for modifications", GitRepoStatus);
                if ((btnMask & (uint)TortoiseGitCommand.RepoStatus) > 0)
                    _icons.Add(cmdId, ExternalResourceName.IDB_REPO_BROWSER);

                _manager.CommandManager.RegisterCommand(selfName, "Rebase", GitRebase);
                _manager.CommandManager.RegisterCommand(selfName, "Repo-browser", GitRepoBrowser);
                _manager.CommandManager.RegisterCommand(selfName, "Reference browser", GitRefBrowse);
                _manager.CommandManager.RegisterCommand(selfName, "Revision graph", GitRevisionGraph);
                cmdId = _manager.CommandManager.RegisterCommand(selfName, "Stash save", GitStashSave);
                if ((btnMask & (uint)TortoiseGitCommand.StashSave) > 0)
                    _icons.Add(cmdId, ExternalResourceName.IDB_STASH_SAVE);

                cmdId = _manager.CommandManager.RegisterCommand(selfName, "Stash pop", GitStashPop);
                if ((btnMask & (uint)TortoiseGitCommand.StashPop) > 0)
                    _icons.Add(cmdId, ExternalResourceName.IDB_STASH_POP);

                _manager.CommandManager.RegisterCommand(selfName, "Stash apply", GitStashApply);
                _manager.CommandManager.RegisterCommand(selfName, "Stash list", GitStashList);
                /**********************************************************************************/
                _manager.CommandManager.RegisterCommand(selfName, "Conflict editor", GitConflictEditor);
                _manager.CommandManager.RegisterCommand(selfName, "Resolve", GitResolve);
                _manager.CommandManager.RegisterCommand(selfName, "Add file", GitAddFile);
                _manager.CommandManager.RegisterCommand(selfName, "Ignore", GitIgnore);
                _manager.CommandManager.RegisterCommand(selfName, "Rename", GitRename);
                _manager.CommandManager.RegisterCommand(selfName, "Remove", GitRemove);
                _manager.CommandManager.RegisterCommand(selfName, "Revert file", GitRevertFile);
                _manager.CommandManager.RegisterCommand(selfName, "Clean up", GitCleanup);
                /**********************************************************************************/
                cmdId = _manager.CommandManager.RegisterCommand(selfName, "Switch", GitSwitch);
                if ((btnMask & (uint)TortoiseGitCommand.Switch) > 0)
                    _icons.Add(cmdId, ExternalResourceName.IDB_SWITCH);

                _manager.CommandManager.RegisterCommand(selfName, "Merge", GitMerge);
                _manager.CommandManager.RegisterCommand(selfName, "Tag", GitTag);
                _manager.CommandManager.RegisterCommand(selfName, "Export", GitExport);
                _manager.CommandManager.RegisterCommand(selfName, "Clone", GitClone);
                _manager.CommandManager.RegisterCommand(selfName, "Create repository", GitRepoCreate);
                /**********************************************************************************/
                cmdId = _manager.CommandManager.RegisterCommand(selfName, "Create patch", GitCreatePatchSerial);
                cmdId = _manager.CommandManager.RegisterCommand(selfName, "Apply patch", GitApplyPatchSerial);
            }
            else
            {
                logger.Info("TortoiseGit not found");
                _manager.CommandManager.RegisterCommand(selfName, "TortoiseGit not found", ReadmeFunc);
            }
        }

        public void ToolBarInit()
        {
            logger.Debug("Create toolbar");

            if (Settings.TortoiseGitProc.ShowToolbar)
                foreach (var i in _icons)
                {
                    _manager.AddToolbarButton(i.Key, i.Value);
                }
            _icons.Clear();
        }

        public void Final()
        {
            logger.Debug("Finalization");
        }

        public void RunCommand(TortoiseGitCommand command, string path, string logMessage = null, bool isAutoClose = false)
        {
            StartCommand(CreateCommand(command, path, logMessage, (byte)(isAutoClose ? 1 : 0)));
        }
    }
}
