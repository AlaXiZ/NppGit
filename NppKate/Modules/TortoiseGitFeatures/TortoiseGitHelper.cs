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
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private const string TortoiseProcExe = "TortoiseGitProc.exe";
        private const string MainParamTemplate = "/command:{0} {2} /path:\"{1}\"";
        private const string LogMsgTemplate = "/logmsg:\"{0}\"";
        private const string CloseEndTemplate = "/closeonend:{0}";
        private const string TortoiseBinPath = "TortoiseGit\\bin\\";

        private static string _tortoiseGitPath = "";
        private static string _tortoiseGitProc = "";

        private Dictionary<int, string> _icons;
        private IModuleManager _manager;

        public bool IsNeedRun => Settings.Modules.TortoiseGit;

        void IModule.Init(IModuleManager manager)
        {
            _manager = manager;
            _manager.OnToolbarRegisterEvent += ToolBarInit;

            _icons = new Dictionary<int, string>();
            var selfName = GetType().Name;
            if (SearchTortoiseGit())
            {
                Logger.Info("TortoiseGit found");

                _manager.RegisterService(typeof(ITortoiseCommand), this);

                var cmdId = _manager.CommandManager.RegisterCommand(selfName, "Pull", GitPull, false, new ShortcutKey("Alt+P"));
                _icons.Add(cmdId, ExternalResourceName.IDB_PULL);

                cmdId = _manager.CommandManager.RegisterCommand(selfName, "Push", GitPush, false, new ShortcutKey("Ctrl+Alt+P"));
                _icons.Add(cmdId, ExternalResourceName.IDB_PUSH);

                cmdId = _manager.CommandManager.RegisterCommand(selfName, "Commit", GitCommit, false, new ShortcutKey("Ctrl+Alt+Shift+C"));
                _icons.Add(cmdId, ExternalResourceName.IDB_COMMIT);

                cmdId = _manager.CommandManager.RegisterCommand(selfName, "Fetch", GitFetch, false, new ShortcutKey("Ctrl+Alt+Shift+F"));
                _icons.Add(cmdId, ExternalResourceName.IDB_PULL);
                /**********************************************************************************/
                _manager.CommandManager.RegisterCommand(selfName, "Diff", GitDiffUnified);
                cmdId = _manager.CommandManager.RegisterCommand(selfName, "Compare", GitDiff);
                _icons.Add(cmdId, ExternalResourceName.IDB_COMPARE);
                /**********************************************************************************/
                _manager.CommandManager.RegisterCommand(selfName, "Log file", GitLogFile, false, new ShortcutKey("Ctrl+Shift+L"));
                _manager.CommandManager.RegisterCommand(selfName, "Log path", GitLogPath);
                cmdId = _manager.CommandManager.RegisterCommand(selfName, "Log repository", GitLogRepo, false, new ShortcutKey("Ctrl+Alt+Shift+L"));
                _icons.Add(cmdId, ExternalResourceName.IDB_LOG);

                _manager.CommandManager.RegisterCommand(selfName, "Show Reflog", GitRefLog);
                cmdId = _manager.CommandManager.RegisterCommand(selfName, "Blame file", GitBlame, false, new ShortcutKey("Ctrl+Alt+B"));
                _icons.Add(cmdId, ExternalResourceName.IDB_BLAME);

                _manager.CommandManager.RegisterCommand(selfName, "Blame line", GitBlameCurrentLine, false, new ShortcutKey("Ctrl+Alt+Shift+B"));
                cmdId = _manager.CommandManager.RegisterCommand(selfName, "Check for modifications", GitRepoStatus);
                _icons.Add(cmdId, ExternalResourceName.IDB_REPO_BROWSER);

                _manager.CommandManager.RegisterCommand(selfName, "Rebase", GitRebase);
                _manager.CommandManager.RegisterCommand(selfName, "Repo-browser", GitRepoBrowser);
                _manager.CommandManager.RegisterCommand(selfName, "Reference browser", GitRefBrowse);
                _manager.CommandManager.RegisterCommand(selfName, "Revision graph", GitRevisionGraph);
                cmdId = _manager.CommandManager.RegisterCommand(selfName, "Stash save", GitStashSave);
                _icons.Add(cmdId, ExternalResourceName.IDB_STASH_SAVE);

                cmdId = _manager.CommandManager.RegisterCommand(selfName, "Stash pop", GitStashPop);
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
                _icons.Add(cmdId, ExternalResourceName.IDB_SWITCH);

                cmdId = _manager.CommandManager.RegisterCommand(selfName, "Merge", GitMerge);
                _icons.Add(cmdId, ExternalResourceName.IDB_MERGE);
                _manager.CommandManager.RegisterCommand(selfName, "Tag", GitTag);
                cmdId = _manager.CommandManager.RegisterCommand(selfName, "Export", GitExport);
                _icons.Add(cmdId, ExternalResourceName.IDB_EXPORT);
                _manager.CommandManager.RegisterCommand(selfName, "Clone", GitClone);
                _manager.CommandManager.RegisterCommand(selfName, "Create repository", GitRepoCreate);
                /**********************************************************************************/
                cmdId = _manager.CommandManager.RegisterCommand(selfName, "Create patch", GitCreatePatchSerial);
                _icons.Add(cmdId, ExternalResourceName.IDB_PATCH_CREATE);
                cmdId = _manager.CommandManager.RegisterCommand(selfName, "Apply patch", GitApplyPatchSerial);
                _icons.Add(cmdId, ExternalResourceName.IDB_PATCH_APPLY);
            }
            else
            {
                Logger.Info("TortoiseGit not found");
                _manager.CommandManager.RegisterCommand(selfName, "TortoiseGit not found", ReadmeFunc);
            }
        }

        public void ToolBarInit()
        {
            Logger.Debug("Create toolbar");

            if (Settings.TortoiseGitProc.ShowToolbar)
                foreach (var i in _icons)
                {
                    _manager.AddToolbarButton(i.Key, i.Value);
                }
            _icons.Clear();
        }

        public void Final()
        {
            Logger.Debug("Finalization");
        }

        public void RunCommand(TortoiseGitCommand command, string path, string logMessage = null, bool isAutoClose = false)
        {

            StartCommand(BuildCommandString(command, path, logMessage, (byte)(isAutoClose ? 1 : 0)));
        }

        private static bool SearchTortoiseGit()
        {
            var tortoisePath = Settings.TortoiseGitProc.Path;
            // Path not set and first run
            if (string.IsNullOrEmpty(tortoisePath) && Settings.TortoiseGitProc.IsFirstSearch)
            {
                // If OS x64, then search in "Program Files (x86)"
                if (8 == IntPtr.Size || !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432")))
                {
                    var path = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
                    if (ExistsTortoiseGit(path))
                    {
                        tortoisePath = System.IO.Path.Combine(path, TortoiseBinPath);
                    }
                }
                // if not found or OS x32, then search in "Program Files"
                if (string.IsNullOrEmpty(tortoisePath))
                {
                    var environmentVariable = Environment.GetEnvironmentVariable("ProgramFiles");
                    if (environmentVariable != null)
                    {
                        // But npp is 32bit process,
                        // then Environment.GetEnvironmentVariable("ProgramFiles") return "Program Files (x86)"
                        var path = environmentVariable.Replace(" (x86)", "");
                        if (ExistsTortoiseGit(path))
                        {
                            tortoisePath = System.IO.Path.Combine(path, TortoiseBinPath);
                        }
                    }
                }
                // TG not found
                if (string.IsNullOrEmpty(tortoisePath))
                {
                    var dlg = new FolderBrowserDialog
                    {
                        Description = "Please select TortoiseGit folder's",
                        ShowNewFolderButton = false
                    };
                    if (dlg.ShowDialog() == DialogResult.OK)
                        tortoisePath = dlg.SelectedPath;
                }
                // If found then save path in setting
                if (!string.IsNullOrEmpty(tortoisePath))
                    Settings.TortoiseGitProc.Path = tortoisePath;
                Settings.TortoiseGitProc.IsFirstSearch = false;
            }
            _tortoiseGitPath = tortoisePath;
            if (_tortoiseGitPath != null)
                _tortoiseGitProc = System.IO.Path.Combine(_tortoiseGitPath, TortoiseProcExe);
            return !string.IsNullOrEmpty(_tortoiseGitPath);
        }

        private static bool ExistsTortoiseGit(string programPath)
        {
            return System.IO.Directory.Exists(System.IO.Path.Combine(programPath, TortoiseBinPath));
        }

        private static void ReadmeFunc()
        {
            const string text = "Не установлен TortoiseGit или не найдена папка с установленной программой!";
            MessageBox.Show(text, "Ошибка настройки", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private static void GitLogFile()
        {
            var filePath = NppUtils.CurrentFilePath;
            StartCommand(BuildCommandString(TortoiseGitCommand.Log, filePath));
        }

        private static void GitLogPath()
        {
            var dirPath = NppUtils.CurrentFileDir;
            StartCommand(BuildCommandString(TortoiseGitCommand.Log, dirPath));
        }

        private static void GitLogRepo()
        {
            if (CheckRepoAndShowError())
            {
                var dirPath = GitCore.GitCore.Instance.ActiveRepository.Path;
                StartCommand(BuildCommandString(TortoiseGitCommand.Log, dirPath));
            }
        }

        private static void GitFetch()
        {
            if (CheckRepoAndShowError())
            {
                var dirPath = GitCore.GitCore.Instance.ActiveRepository.Path;
                StartCommand(BuildCommandString(TortoiseGitCommand.Fetch, dirPath));
            }
        }

        private static void GitPull()
        {
            if (CheckRepoAndShowError())
            {
                var dirPath = GitCore.GitCore.Instance.ActiveRepository.Path;
                StartCommand(BuildCommandString(TortoiseGitCommand.Pull, dirPath));
            }
        }

        private static void GitPush()
        {
            if (CheckRepoAndShowError())
            {
                var dirPath = GitCore.GitCore.Instance.ActiveRepository.Path;
                StartCommand(BuildCommandString(TortoiseGitCommand.Push, dirPath));
            }
        }

        private static void GitCommit()
        {
            if (CheckRepoAndShowError())
            {
                var dirPath = GitCore.GitCore.Instance.ActiveRepository.Path;
                StartCommand(BuildCommandString(TortoiseGitCommand.Commit, dirPath));
            }
        }

        private static void GitBlame()
        {
            var filePath = NppUtils.CurrentFilePath;
            StartCommand(BuildCommandString(TortoiseGitCommand.Blame, filePath));
        }

        private static void GitBlameCurrentLine()
        {
            var filePath = NppUtils.CurrentFilePath;
            var param = string.Format("/line:{0}", NppUtils.CurrentLine);
            StartCommand(BuildCommandString(TortoiseGitCommand.Blame, filePath, additionalParam: param));
        }

        private static void GitSwitch()
        {
            if (CheckRepoAndShowError())
            {
                var dirPath = GitCore.GitCore.Instance.ActiveRepository.Path;
                StartCommand(BuildCommandString(TortoiseGitCommand.Switch, dirPath));
            }
        }

        private static void GitStashSave()
        {
            if (CheckRepoAndShowError())
            {
                var dirPath = GitCore.GitCore.Instance.ActiveRepository.Path;
                var msg = "/msg:" + DateTime.Now.ToString();
                StartCommand(BuildCommandString(TortoiseGitCommand.StashSave, dirPath, additionalParam: msg));
            }
        }

        private static void GitStashPop()
        {
            if (CheckRepoAndShowError())
            {
                var dirPath = GitCore.GitCore.Instance.ActiveRepository.Path;
                StartCommand(BuildCommandString(TortoiseGitCommand.StashPop, dirPath));
            }
        }

        private static void GitRepoStatus()
        {
            if (CheckRepoAndShowError())
            {
                var dirPath = GitCore.GitCore.Instance.ActiveRepository.Path;
                StartCommand(BuildCommandString(TortoiseGitCommand.RepoStatus, dirPath));
            }
        }

        private static void GitDiff()
        {
            var path = NppUtils.CurrentFilePath;
            StartCommand(BuildCommandString(TortoiseGitCommand.Diff, path));
        }

        private static void GitDiffUnified()
        {
            var path = NppUtils.CurrentFilePath;
            StartCommand(BuildCommandString(TortoiseGitCommand.Diff, path, additionalParam: "/unified"));
        }

        private static void GitRebase()
        {
            if (CheckRepoAndShowError())
            {
                var path = GitCore.GitCore.Instance.ActiveRepository.Path;
                StartCommand(BuildCommandString(TortoiseGitCommand.Rebase, path));
            }
        }

        private static void GitAddFile()
        {
            // TODO: В какой репозиторий добавляется файл?
            if (CheckRepoAndShowError())
            {
                var path = NppUtils.CurrentFilePath;
                StartCommand(BuildCommandString(TortoiseGitCommand.Add, path));
            }
        }

        private static void GitRevertFile()
        {
            var path = NppUtils.CurrentFilePath;
            StartCommand(BuildCommandString(TortoiseGitCommand.Revert, path));
        }

        private static void GitRepoBrowser()
        {
            if (CheckRepoAndShowError())
            {
                var path = GitCore.GitCore.Instance.ActiveRepository.Path;
                StartCommand(BuildCommandString(TortoiseGitCommand.RepoBrowser, path));
            }
        }

        private static void GitStashApply()
        {
            if (CheckRepoAndShowError())
            {
                var path = GitCore.GitCore.Instance.ActiveRepository.Path;
                StartCommand(BuildCommandString(TortoiseGitCommand.StashApply, path));
            }
        }

        private static void GitRefBrowse()
        {
            if (CheckRepoAndShowError())
            {
                var path = GitCore.GitCore.Instance.ActiveRepository.Path;
                StartCommand(BuildCommandString(TortoiseGitCommand.RefBrowse, path));
            }
        }

        private static void GitIgnore()
        {
            if (CheckRepoAndShowError())
            {
                var path = NppUtils.CurrentFilePath;
                StartCommand(BuildCommandString(TortoiseGitCommand.Ignore, path));
            }
        }

        private static void GitExport()
        {
            if (CheckRepoAndShowError())
            {
                var path = GitCore.GitCore.Instance.ActiveRepository.Path;
                StartCommand(BuildCommandString(TortoiseGitCommand.Export, path));
            }
        }

        private static void GitMerge()
        {
            if (CheckRepoAndShowError())
            {
                var path = GitCore.GitCore.Instance.ActiveRepository.Path;
                StartCommand(BuildCommandString(TortoiseGitCommand.Merge, path));
            }
        }

        private static void GitCleanup()
        {
            if (CheckRepoAndShowError())
            {
                var path = GitCore.GitCore.Instance.ActiveRepository.Path;
                StartCommand(BuildCommandString(TortoiseGitCommand.CleanUp, path));
            }
        }

        private static void GitRemove()
        {
            if (CheckRepoAndShowError())
            {
                var path = NppUtils.CurrentFilePath;
                StartCommand(BuildCommandString(TortoiseGitCommand.Remove, path));
            }
        }

        private static void GitRename()
        {
            if (CheckRepoAndShowError())
            {
                var path = NppUtils.CurrentFilePath;
                StartCommand(BuildCommandString(TortoiseGitCommand.Rename, path));
            }
        }

        private static void GitConflictEditor()
        {
            // TODO: Когда вызывается?
            if (CheckRepoAndShowError())
            {
                var path = NppUtils.CurrentFilePath;
                StartCommand(BuildCommandString(TortoiseGitCommand.ConflictEditor, path));
            }
        }

        private static void GitRefLog()
        {
            if (CheckRepoAndShowError())
            {
                var path = GitCore.GitCore.Instance.ActiveRepository.Path;
                StartCommand(BuildCommandString(TortoiseGitCommand.RefLog, path));
            }
        }

        private static void GitRevisionGraph()
        {
            if (CheckRepoAndShowError())
            {
                var path = GitCore.GitCore.Instance.ActiveRepository.Path;
                StartCommand(BuildCommandString(TortoiseGitCommand.RevisionGraph, path));
            }
        }

        private static void GitTag()
        {
            if (CheckRepoAndShowError())
            {
                var path = GitCore.GitCore.Instance.ActiveRepository.Path;
                StartCommand(BuildCommandString(TortoiseGitCommand.Tag, path));
            }
        }

        private static void GitDaemon()
        {
            if (CheckRepoAndShowError())
            {
                var path = GitCore.GitCore.Instance.ActiveRepository.Path;
                StartCommand(BuildCommandString(TortoiseGitCommand.Daemon, path));
            }
        }

/*
        private static void GitPGPfp()
        {
            if (CheckRepoAndShowError())
            {
                var path = GitCore.GitCore.Instance.ActiveRepository.Path;
                StartCommand(BuildCommandString(TortoiseGitCommand.PGPfp, path));
            }
        }
*/

        private static string SelectFolder(string title)
        {
            var dlg = new FolderBrowserDialog
            {
                ShowNewFolderButton = true,
                Description = title
            };
            return dlg.ShowDialog() == DialogResult.OK ? dlg.SelectedPath : "";
        }

        private static void GitClone()
        {
            var path = SelectFolder("Папка назначения");
            if (!string.IsNullOrEmpty(path))
            {
                StartCommand(BuildCommandString(TortoiseGitCommand.Clone, path));
            }
        }

        private static void GitRepoCreate()
        {
            var path = SelectFolder("Папка назначения");
            if (!string.IsNullOrEmpty(path))
            {
                StartCommand(BuildCommandString(TortoiseGitCommand.RepoCreate, path));
            }
        }

        private static void GitResolve()
        {
            if (CheckRepoAndShowError())
            {
                var path = GitCore.GitCore.Instance.ActiveRepository.Path;
                StartCommand(BuildCommandString(TortoiseGitCommand.Resolve, path));
            }
        }

        private static void GitApplyPatchSerial()
        {
            if (CheckRepoAndShowError())
            {
                var path = GitCore.GitCore.Instance.ActiveRepository.Path;
                StartCommand(BuildCommandString(TortoiseGitCommand.ImportPatch, path));
            }
        }

        private static void GitCreatePatchSerial()
        {
            if (CheckRepoAndShowError())
            {
                var path = GitCore.GitCore.Instance.ActiveRepository.Path;
                StartCommand(BuildCommandString(TortoiseGitCommand.FormatPatch, path));
            }
        }

        private static void GitStashList()
        {
            if (CheckRepoAndShowError())
            {
                var path = GitCore.GitCore.Instance.ActiveRepository.Path;
                StartCommand(BuildCommandString(TortoiseGitCommand.RefLog, path, additionalParam: @"/ref:refs/stash"));
            }
        }

        private static bool CheckRepoAndShowError()
        {
            // TODO: Hmm... Need "common console"
            if (GitCore.GitCore.Instance.ActiveRepository == null)
            {
                MessageBox.Show("Нет активного репозитория!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return GitCore.GitCore.Instance.ActiveRepository != null;
        }

        private static string BuildCommandString(TortoiseGitCommand command, string path, string logMsg = null, byte? closeParam = null, string additionalParam = null)
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

            builder.AppendFormat(MainParamTemplate, GetCommandName(command), path, addParam);

            if (!string.IsNullOrEmpty(logMsg) && !string.IsNullOrWhiteSpace(logMsg))
            {
                builder.Append(" ").AppendFormat(LogMsgTemplate, logMsg);
            }

            builder.Append(" ").AppendFormat(CloseEndTemplate, closeParam > 2 ? 0 : closeParam);

            return builder.ToString();
        }

        private static string GetCommandName(TortoiseGitCommand command)
        {
            return command.ToString("G").ToLower();
        }

        private static void StartCommand(string command)
        {
            System.Diagnostics.Process.Start(_tortoiseGitProc, command);
        }
    }
}
