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
using System.Text;
using System.Windows.Forms;
using NLog;
using NppGit.Common;

namespace NppGit.Modules.TortoiseGitFeatures
{
    public enum TortoiseGitCommand : uint
    {
        Fetch       =  0x0001,
        Log         =  0x0020,
        Commit      =  0x0004,
        Add         ,
        Revert      ,
        Switch      =  0x0200,
        Blame       =  0x0010,
        Pull        =  0x0002,
        Push        =  0x0008,
        StashSave   =  0x0040,
        StashPop    =  0x0080,
        RepoStatus  =  0x0100,
        Diff        =  0x0400,
        Rebase      =  0x0800,
        ShowCompare = 0x1000
    }
    /*
    Fetch
    Pull
    Commit
    Push
    Blame
    Log
    Stash save
    Stash pop
    Repo status
    Switch
    */

    public class TortoiseGitHelper : IModule
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private const string EXE = "TortoiseGitProc.exe";
        private const string PARAM = "/command:{0} {2} /path:\"{1}\"";
        private const string LOG_MSG = "/logmsg:\"{0}\"";
        private const string CLOSE = "/closeonend:{0}";
        private const string TORTOISEGITBIN = "TortoiseGit\\bin\\";

        private static string _tortoiseGitPath = "";
        private static string _tortoiseGitProc = "";

        private Dictionary<int, Bitmap> _icons;
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
            if (string.IsNullOrEmpty(tortoisePath))
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
                    Description = "Select folder with TortoiseGitProc.exe",
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
            } else
            {
                addParam = additionalParam;
            }

            builder.AppendFormat(PARAM, GetCommandName(command), path, addParam);

            if (!string.IsNullOrEmpty(logMsg) && !string.IsNullOrWhiteSpace(logMsg))
            {
                builder.Append(" ").AppendFormat(LOG_MSG, logMsg);
            }

            if (closeParam == null || closeParam > 2)
            {
                builder.Append(" ").AppendFormat(CLOSE, 0);
            } else
            {
                builder.Append(" ").AppendFormat(CLOSE, closeParam);
            }

            return builder.ToString();
        }

        private static void TGitLogFile()
        {
            string filePath = PluginUtils.CurrentFilePath;
            StartCommand(CreateCommand(TortoiseGitCommand.Log, filePath));
        }

        private static void TGitLogPath()
        {
            string dirPath = PluginUtils.CurrentFileDir;
            StartCommand(CreateCommand(TortoiseGitCommand.Log, dirPath));
        }

        private static void TGitLogRepo()
        {
            string dirPath = PluginUtils.GetRootDir(PluginUtils.CurrentFileDir);
            StartCommand(CreateCommand(TortoiseGitCommand.Log, dirPath));
        }

        private static void TGitFetch()
        {
            string dirPath = PluginUtils.GetRootDir(PluginUtils.CurrentFileDir);
            StartCommand(CreateCommand(TortoiseGitCommand.Fetch, dirPath));
        }

        private static void TGitPull()
        {
            string dirPath = PluginUtils.GetRootDir(PluginUtils.CurrentFileDir);
            StartCommand(CreateCommand(TortoiseGitCommand.Pull, dirPath));
        }

        private static void TGitPush()
        {
            string dirPath = PluginUtils.GetRootDir(PluginUtils.CurrentFileDir);
            StartCommand(CreateCommand(TortoiseGitCommand.Push, dirPath));
        }

        private static void TGitCommit()
        {
            string dirPath = PluginUtils.GetRootDir(PluginUtils.CurrentFileDir);
            StartCommand(CreateCommand(TortoiseGitCommand.Commit, dirPath));
        }

        private static void TGitBlame()
        {
            string filePath = PluginUtils.CurrentFilePath;
            StartCommand(CreateCommand(TortoiseGitCommand.Blame, filePath));
        }

        private static void TGitBlameCurrentLine()
        {
            string filePath = PluginUtils.CurrentFilePath;
            string param = string.Format("/line:{0}", PluginUtils.CurrentLine);
            StartCommand(CreateCommand(TortoiseGitCommand.Blame, filePath, additionalParam: param));
        }

        private static void TGitSwitch()
        {
            string dirPath = PluginUtils.GetRootDir(PluginUtils.CurrentFileDir);
            StartCommand(CreateCommand(TortoiseGitCommand.Switch, dirPath));
        }

        private static void TGitStashSave()
        {
            string dirPath = PluginUtils.GetRootDir(PluginUtils.CurrentFileDir);
            string msg = "/msg:" + DateTime.Now.ToString();
            StartCommand(CreateCommand(TortoiseGitCommand.StashSave, dirPath, additionalParam: msg));
        }

        private static void TGitStashPop()
        {
            string dirPath = PluginUtils.GetRootDir(PluginUtils.CurrentFileDir);
            StartCommand(CreateCommand(TortoiseGitCommand.StashPop, dirPath));
        }
    
        private static void TGitRepoStatus()
        {
            string dirPath = PluginUtils.GetRootDir(PluginUtils.CurrentFileDir);
            StartCommand(CreateCommand(TortoiseGitCommand.RepoStatus, dirPath));
        }

        private static void TGitDiff()
        {
            string path = PluginUtils.CurrentFilePath;
            StartCommand(CreateCommand(TortoiseGitCommand.Diff, path));
        }

/*
        private void TGitCompare()
        {
            string path = PluginUtils.CurrentFilePath;
            StartCommand(CreateCommand(TortoiseGitCommand.ShowCompare, path));
        }
*/

        private static void TGitRebase()
        {
            string path = PluginUtils.GetRootDir(PluginUtils.CurrentFileDir);
            StartCommand(CreateCommand(TortoiseGitCommand.Rebase, path));
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
            _icons = new Dictionary<int, Bitmap>();
            if (SearchTortoiseGit())
            {
                var btnMask = Settings.TortoiseGitProc.ButtonMask;
                logger.Info("TortoiseGit found");

                _manager.RegisteCommandItem(new CommandItem
                {
                    Name = "TGit Log file",
                    Hint = "Log file",
                    ShortcutKey = new ShortcutKey { _isCtrl = 1, _isShift = 1, _key = (byte)Keys.L },
                    Action = TGitLogFile
                });

                _manager.RegisteCommandItem(new CommandItem
                {
                    Name = "TGit Log path",
                    Hint = "Log path",
                    ShortcutKey = new ShortcutKey { },
                    Action = TGitLogPath
                });

                var cmdId = _manager.RegisteCommandItem(new CommandItem
                {
                    Name = "TGit Log repo",
                    Hint = "Log repo",
                    ShortcutKey = new ShortcutKey { _isAlt = 1, _isCtrl = 1, _isShift = 1, _key = (byte)Keys.L },
                    Action = TGitLogRepo
                });
                if ((btnMask & (uint)TortoiseGitCommand.Log) > 0)
                    _icons.Add(cmdId, Properties.Resources.log);

                cmdId = _manager.RegisteCommandItem(new CommandItem
                {
                    Name = "TGit Fetch",
                    Hint = "Fetch",
                    ShortcutKey = new ShortcutKey { _isAlt = 1, _isCtrl = 1, _isShift = 1, _key = (byte)Keys.F },
                    Action = TGitFetch
                });
                if ((btnMask & (uint)TortoiseGitCommand.Fetch) > 0)
                    _icons.Add(cmdId, Properties.Resources.pull);

                cmdId = _manager.RegisteCommandItem(new CommandItem
                {
                    Name = "TGit Pull",
                    Hint = "Pull",
                    ShortcutKey = new ShortcutKey { _isAlt = 1, _key = (byte)Keys.P },
                    Action = TGitPull
                });
                if ((btnMask & (uint)TortoiseGitCommand.Pull) > 0)
                    _icons.Add(cmdId, Properties.Resources.pull);

                cmdId = _manager.RegisteCommandItem(new CommandItem
                {
                    Name = "TGit Push",
                    Hint = "Push",
                    ShortcutKey = new ShortcutKey { _isAlt = 1, _isCtrl = 1, _key = (byte)Keys.P },
                    Action = TGitPush
                });
                if ((btnMask & (uint)TortoiseGitCommand.Push) > 0)
                    _icons.Add(cmdId, Properties.Resources.push);

                cmdId = _manager.RegisteCommandItem(new CommandItem
                {
                    Name = "TGit Commit",
                    Hint = "Commit",
                    ShortcutKey = new ShortcutKey { _isAlt = 1, _isCtrl = 1, _key = (byte)Keys.C },
                    Action = TGitCommit
                });
                if ((btnMask & (uint)TortoiseGitCommand.Commit) > 0)
                    _icons.Add(cmdId, Properties.Resources.commit);

                cmdId = _manager.RegisteCommandItem(new CommandItem
                {
                    Name = "TGit Blame",
                    Hint = "Blame",
                    ShortcutKey = new ShortcutKey { _isAlt = 1, _isCtrl = 1, _key = (byte)Keys.B },
                    Action = TGitBlame
                });
                if ((btnMask & (uint)TortoiseGitCommand.Blame) > 0)
                    _icons.Add(cmdId, Properties.Resources.blame);

                _manager.RegisteCommandItem(new CommandItem
                {
                    Name = "TGit Blame line",
                    Hint = "Blame line",
                    ShortcutKey = new ShortcutKey { _isAlt = 1, _key = (byte)Keys.B },
                    Action = TGitBlameCurrentLine
                });

                cmdId = _manager.RegisteCommandItem(new CommandItem
                {
                    Name = "TGit Switch",
                    Hint = "Switch",
                    ShortcutKey = new ShortcutKey { _isCtrl = 1, _isAlt = 1, _key = (byte)Keys.S },
                    Action = TGitSwitch
                });
                if ((btnMask & (uint)TortoiseGitCommand.Switch) > 0)
                    _icons.Add(cmdId, Properties.Resources.checkout);

                cmdId = _manager.RegisteCommandItem(new CommandItem
                {
                    Name = "TGit Stash save",
                    Hint = "Stash save",
                    ShortcutKey = null,
                    Action = TGitStashSave
                });
                if ((btnMask & (uint)TortoiseGitCommand.StashSave) > 0)
                    _icons.Add(cmdId, Properties.Resources.stashsave);

                cmdId = _manager.RegisteCommandItem(new CommandItem
                {
                    Name = "TGit Stash pop",
                    Hint = "Stash pop",
                    ShortcutKey = null,
                    Action = TGitStashPop
                });
                if ((btnMask & (uint)TortoiseGitCommand.StashPop) > 0)
                    _icons.Add(cmdId, Properties.Resources.stashpop);

                cmdId = _manager.RegisteCommandItem(new CommandItem
                {
                    Name = "TGit Repo stastus",
                    Hint = "Repo stastus",
                    ShortcutKey = null,
                    Action = TGitRepoStatus
                });
                if ((btnMask & (uint)TortoiseGitCommand.RepoStatus) > 0)
                    _icons.Add(cmdId, Properties.Resources.repo);

                _manager.RegisteCommandItem(new CommandItem
                {
                    Name = "TGit Diff",
                    Hint = "Diff",
                    ShortcutKey = null,
                    Action = TGitDiff
                });

                /*
                _manager.RegisteCommandItem(new CommandItem
                {
                    Name = "TGit Compare",
                    Hint = "Compare",
                    ShortcutKey = null,
                    Action = TGitCompare
                });
                */

                _manager.RegisteCommandItem(new CommandItem
                {
                    Name = "TGit Rebase",
                    Hint = "Rebase",
                    ShortcutKey = null,
                    Action = TGitRebase
                });

                _manager.RegisteCommandItem(new CommandItem
                {
                    Name = "-",
                    Hint = "-",
                    Action = null
                });
            }
            else
            {
                logger.Info("TortoiseGit not found");
                _manager.RegisteCommandItem(new CommandItem
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
    }
}
