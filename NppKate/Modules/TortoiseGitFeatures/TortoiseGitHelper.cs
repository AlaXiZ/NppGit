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
using System.Text;
using System.Windows.Forms;
using NLog;
using NppKate.Common;
using NppKate.Npp;
using NppKate.Resources;

namespace NppKate.Modules.TortoiseGitFeatures
{
    public class TortoiseGitHelper : IModule, ITortoiseGit
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

            logger.Debug("Create menu");
            _icons = new Dictionary<int, string>();
            if (SearchTortoiseGit())
            {
                var btnMask = Settings.TortoiseGitProc.ButtonMask;
                logger.Info("TortoiseGit found");

                var cmdId = _manager.RegisterCommandItem(new CommandItem
                {
                    Name = "Pull",
                    Hint = "Pull",
                    ShortcutKey = new ShortcutKey { _isAlt = 1, _key = (byte)Keys.P },
                    Action = GitPull
                });
                if ((btnMask & (uint)TortoiseGitCommand.Pull) > 0)
                    _icons.Add(cmdId, ExternalResourceName.IDB_PULL);

                cmdId = _manager.RegisterCommandItem(new CommandItem
                {
                    Name = "Push",
                    Hint = "Push",
                    ShortcutKey = new ShortcutKey { _isAlt = 1, _isCtrl = 1, _key = (byte)Keys.P },
                    Action = GitPush
                });
                if ((btnMask & (uint)TortoiseGitCommand.Push) > 0)
                    _icons.Add(cmdId, ExternalResourceName.IDB_PUSH);

                cmdId = _manager.RegisterCommandItem(new CommandItem
                {
                    Name = "Commit",
                    Hint = "Commit",
                    ShortcutKey = new ShortcutKey { _isAlt = 1, _isCtrl = 1, _key = (byte)Keys.C },
                    Action = GitCommit
                });
                if ((btnMask & (uint)TortoiseGitCommand.Commit) > 0)
                    _icons.Add(cmdId, ExternalResourceName.IDB_COMMIT);

                cmdId = _manager.RegisterCommandItem(new CommandItem
                {
                    Name = "Fetch",
                    Hint = "Fetch",
                    ShortcutKey = new ShortcutKey { _isAlt = 1, _isCtrl = 1, _isShift = 1, _key = (byte)Keys.F },
                    Action = GitFetch
                });
                if ((btnMask & (uint)TortoiseGitCommand.Fetch) > 0)
                    _icons.Add(cmdId, ExternalResourceName.IDB_PULL);

                /**********************************************************************************/
                _manager.RegisterCommandItem(new CommandItem { Name = "-", Hint = "-", Action = null });

                _manager.RegisterCommandItem(new CommandItem
                {
                    Name = "Diff",
                    Hint = "Diff",
                    ShortcutKey = null,
                    Action = GitDiffUnified
                });

                _manager.RegisterCommandItem(new CommandItem
                {
                    Name = "Compare",
                    Hint = "Compare",
                    ShortcutKey = null,
                    Action = GitDiff
                });

                /**********************************************************************************/
                _manager.RegisterCommandItem(new CommandItem { Name = "-", Hint = "-", Action = null });

                _manager.RegisterCommandItem(new CommandItem
                {
                    Name = "Log file",
                    Hint = "Log file",
                    ShortcutKey = new ShortcutKey { _isCtrl = 1, _isShift = 1, _key = (byte)Keys.L },
                    Action = GitLogFile
                });

                _manager.RegisterCommandItem(new CommandItem
                {
                    Name = "Log path",
                    Hint = "Log path",
                    ShortcutKey = new ShortcutKey { },
                    Action = GitLogPath
                });

                cmdId = _manager.RegisterCommandItem(new CommandItem
                {
                    Name = "Log repository",
                    Hint = "Log repository",
                    ShortcutKey = new ShortcutKey { _isAlt = 1, _isCtrl = 1, _isShift = 1, _key = (byte)Keys.L },
                    Action = GitLogRepo
                });
                if ((btnMask & (uint)TortoiseGitCommand.Log) > 0)
                    _icons.Add(cmdId, ExternalResourceName.IDB_LOG);

                _manager.RegisterCommandItem(new CommandItem
                {
                    Name = "Show Reflog",
                    Hint = "Show Reflog",
                    ShortcutKey = null,
                    Action = GitRefLog
                });

                cmdId = _manager.RegisterCommandItem(new CommandItem
                {
                    Name = "Blame file",
                    Hint = "Blame file",
                    ShortcutKey = new ShortcutKey { _isAlt = 1, _isCtrl = 1, _key = (byte)Keys.B },
                    Action = GitBlame
                });
                if ((btnMask & (uint)TortoiseGitCommand.Blame) > 0)
                    _icons.Add(cmdId, ExternalResourceName.IDB_BLAME);

                _manager.RegisterCommandItem(new CommandItem
                {
                    Name = "Blame line",
                    Hint = "Blame line",
                    ShortcutKey = new ShortcutKey { _isAlt = 1, _key = (byte)Keys.B },
                    Action = GitBlameCurrentLine
                });

                cmdId = _manager.RegisterCommandItem(new CommandItem
                {
                    Name = "Check for modifications",
                    Hint = "Check for modifications",
                    ShortcutKey = null,
                    Action = GitRepoStatus
                });
                if ((btnMask & (uint)TortoiseGitCommand.RepoStatus) > 0)
                    _icons.Add(cmdId, ExternalResourceName.IDB_REPO_BROWSER);

                _manager.RegisterCommandItem(new CommandItem
                {
                    Name = "Rebase...",
                    Hint = "Rebase...",
                    ShortcutKey = null,
                    Action = GitRebase
                });

                _manager.RegisterCommandItem(new CommandItem
                {
                    Name = "Repo-browser",
                    Hint = "Repo-browser",
                    ShortcutKey = null,
                    Action = GitRepoBrowser
                });

                _manager.RegisterCommandItem(new CommandItem
                {
                    Name = "Reference browser",
                    Hint = "Reference browser",
                    ShortcutKey = null,
                    Action = GitRefBrowse
                });

                _manager.RegisterCommandItem(new CommandItem
                {
                    Name = "Revision graph",
                    Hint = "Revision graph",
                    ShortcutKey = null,
                    Action = GitRevisionGraph
                });

                _manager.RegisterCommandItem(new CommandItem
                {
                    Name = "Daemon",
                    Hint = "Daemon",
                    ShortcutKey = null,
                    Action = GitDaemon
                });

                cmdId = _manager.RegisterCommandItem(new CommandItem
                {
                    Name = "Stash save",
                    Hint = "Stash save",
                    ShortcutKey = null,
                    Action = GitStashSave
                });
                if ((btnMask & (uint)TortoiseGitCommand.StashSave) > 0)
                    _icons.Add(cmdId, ExternalResourceName.IDB_STASH_SAVE);

                cmdId = _manager.RegisterCommandItem(new CommandItem
                {
                    Name = "Stash pop",
                    Hint = "Stash pop",
                    ShortcutKey = null,
                    Action = GitStashPop
                });
                if ((btnMask & (uint)TortoiseGitCommand.StashPop) > 0)
                    _icons.Add(cmdId, ExternalResourceName.IDB_STASH_POP);

                _manager.RegisterCommandItem(new CommandItem
                {
                    Name = "Stash apply",
                    Hint = "Stash apply",
                    ShortcutKey = null,
                    Action = GitStashApply
                });

                _manager.RegisterCommandItem(new CommandItem
                {
                    Name = "Stash list",
                    Hint = "Stash list",
                    ShortcutKey = null,
                    Action = GitStashList
                });

                /**********************************************************************************/
                _manager.RegisterCommandItem(new CommandItem { Name = "-", Hint = "-", Action = null });

                _manager.RegisterCommandItem(new CommandItem
                {
                    Name = "Conflict editor",
                    Hint = "Conflict editor",
                    ShortcutKey = null,
                    Action = GitConflictEditor
                });

                _manager.RegisterCommandItem(new CommandItem
                {
                    Name = "Resolve",
                    Hint = "Resolve",
                    ShortcutKey = null,
                    Action = GitResolve
                });

                _manager.RegisterCommandItem(new CommandItem
                {
                    Name = "Add file",
                    Hint = "Add file",
                    ShortcutKey = null,
                    Action = GitAddFile
                });

                _manager.RegisterCommandItem(new CommandItem
                {
                    Name = "Ignore",
                    Hint = "Ignore",
                    ShortcutKey = null,
                    Action = GitIgnore
                });

                _manager.RegisterCommandItem(new CommandItem
                {
                    Name = "Rename",
                    Hint = "Rename",
                    ShortcutKey = null,
                    Action = GitRename
                });

                _manager.RegisterCommandItem(new CommandItem
                {
                    Name = "Remove",
                    Hint = "Remove",
                    ShortcutKey = null,
                    Action = GitRemove
                });

                _manager.RegisterCommandItem(new CommandItem
                {
                    Name = "Revert file",
                    Hint = "Revert file",
                    ShortcutKey = null,
                    Action = GitRevertFile
                });

                _manager.RegisterCommandItem(new CommandItem
                {
                    Name = "Clean up...",
                    Hint = "Clean up",
                    ShortcutKey = null,
                    Action = GitCleanup
                });

                /**********************************************************************************/
                _manager.RegisterCommandItem(new CommandItem { Name = "-", Hint = "-", Action = null });

                cmdId = _manager.RegisterCommandItem(new CommandItem
                {
                    Name = "Switch",
                    Hint = "Switch",
                    ShortcutKey = new ShortcutKey { _isCtrl = 1, _isAlt = 1, _key = (byte)Keys.S },
                    Action = GitSwitch
                });
                if ((btnMask & (uint)TortoiseGitCommand.Switch) > 0)
                    _icons.Add(cmdId, ExternalResourceName.IDB_SWITCH);

                _manager.RegisterCommandItem(new CommandItem
                {
                    Name = "Merge",
                    Hint = "Merge",
                    ShortcutKey = null,
                    Action = GitMerge
                });

                _manager.RegisterCommandItem(new CommandItem
                {
                    Name = "Tag",
                    Hint = "Tag",
                    ShortcutKey = null,
                    Action = GitTag
                });

                _manager.RegisterCommandItem(new CommandItem
                {
                    Name = "Export",
                    Hint = "Export",
                    ShortcutKey = null,
                    Action = GitExport
                });

                /**********************************************************************************/
                _manager.RegisterCommandItem(new CommandItem { Name = "-", Hint = "-", Action = null });
                /*
                _manager.RegisterCommandItem(new CommandItem
                {
                    Name = "Signing Key fingerprint",
                    Hint = "Signing Key fingerprint",
                    ShortcutKey = null,
                    Action =  TGitPGPfp
                });
                */

                _manager.RegisterCommandItem(new CommandItem
                {
                    Name = "Clone",
                    Hint = "Clone",
                    ShortcutKey = null,
                    Action = GitClone
                });

                _manager.RegisterCommandItem(new CommandItem
                {
                    Name = "Create repository",
                    Hint = "Create repository",
                    ShortcutKey = null,
                    Action = GitRepoCreate
                });
                /**********************************************************************************/
                _manager.RegisterCommandItem(new CommandItem { Name = "-", Hint = "-", Action = null });

                cmdId = _manager.RegisterCommandItem(new CommandItem
                {
                    Name = "Create patch",
                    Hint = "Create patch",
                    ShortcutKey = null,
                    Action = GitCreatePatchSerial
                });

                cmdId = _manager.RegisterCommandItem(new CommandItem
                {
                    Name = "Apply patch",
                    Hint = "Apply patch",
                    ShortcutKey = null,
                    Action = GitApplyPatchSerial
                });

                _manager.RegisterCommandItem(new CommandItem { Name = "-", Hint = "-", Action = null });
            }
            else
            {
                logger.Info("TortoiseGit not found");
                _manager.RegisterCommandItem(new CommandItem
                {
                    Name = "TortoiseGit not found",
                    Hint = "-",
                    Action = ReadmeFunc
                });
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
