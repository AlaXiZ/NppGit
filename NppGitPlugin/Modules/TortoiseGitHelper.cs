using NLog;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace NppGit
{
    public enum TortoiseGitCommand : uint
    {
        Fetch      = 0x001,
        Log        = 0x020,
        Commit     = 0x004,
        Add        ,
        Revert     ,
        Switch     = 0x200,
        Blame      = 0x010,
        Pull       = 0x002,
        Push       = 0x008,
        StashSave  = 0x040,
        StashPop   = 0x080,
        RepoStatus = 0x100
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

        private static string tortoiseGitPath = "";
        private static string tortoiseGitProc = "";

        private Dictionary<int, Bitmap> _icons;
        private IModuleManager _manager;
        
        private static bool ExistsTortoiseGit(string programPath)
        {
            return System.IO.Directory.Exists(System.IO.Path.Combine(programPath, TORTOISEGITBIN));
        }

        private bool InitTG()
        {
            string tortoisePath = Settings.TortoiseGitProc.Path;
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
                    var dlg = new FolderBrowserDialog();
                    dlg.Description = "Select folder with TortoiseGitProc.exe";
                    dlg.ShowNewFolderButton = false;
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
            tortoiseGitPath = tortoisePath;
            tortoiseGitProc = System.IO.Path.Combine(tortoiseGitPath, EXE);

            return !string.IsNullOrEmpty(tortoiseGitPath);
        }

        private static string GetCommandName(TortoiseGitCommand command)
        {
            return command.ToString("G").ToLower();
        }

        private static void StartCommand(string param)
        {
            System.Diagnostics.Process.Start(tortoiseGitProc, param);
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

        private void TGitLogFile()
        {
            string filePath = PluginUtils.CurrentFilePath;
            StartCommand(CreateCommand(TortoiseGitCommand.Log, filePath));
        }

        private void TGitLogPath()
        {
            string dirPath = PluginUtils.CurrentFileDir;
            StartCommand(CreateCommand(TortoiseGitCommand.Log, dirPath));
        }

        private void TGitLogRepo()
        {
            string dirPath = PluginUtils.GetRootDir(PluginUtils.CurrentFileDir);
            StartCommand(CreateCommand(TortoiseGitCommand.Log, dirPath));
        }

        private void TGitFetch()
        {
            string dirPath = PluginUtils.GetRootDir(PluginUtils.CurrentFileDir);
            StartCommand(CreateCommand(TortoiseGitCommand.Fetch, dirPath));
        }

        private void TGitPull()
        {
            string dirPath = PluginUtils.GetRootDir(PluginUtils.CurrentFileDir);
            StartCommand(CreateCommand(TortoiseGitCommand.Pull, dirPath));
        }

        private void TGitPush()
        {
            string dirPath = PluginUtils.GetRootDir(PluginUtils.CurrentFileDir);
            StartCommand(CreateCommand(TortoiseGitCommand.Push, dirPath));
        }

        private void TGitCommit()
        {
            string dirPath = PluginUtils.GetRootDir(PluginUtils.CurrentFileDir);
            StartCommand(CreateCommand(TortoiseGitCommand.Commit, dirPath));
        }

        private void TGitBlame()
        {
            string filePath = PluginUtils.CurrentFilePath;
            StartCommand(CreateCommand(TortoiseGitCommand.Blame, filePath));
        }

        private void TGitBlameCurrentLine()
        {
            string filePath = PluginUtils.CurrentFilePath;
            string param = string.Format("/line:{0}", PluginUtils.CurrentLine);
            StartCommand(CreateCommand(TortoiseGitCommand.Blame, filePath, additionalParam: param));
        }

        private void TGitSwitch()
        {
            string dirPath = PluginUtils.GetRootDir(PluginUtils.CurrentFileDir);
            StartCommand(CreateCommand(TortoiseGitCommand.Switch, dirPath));
        }

        private void TGitStashSave()
        {
            string dirPath = PluginUtils.GetRootDir(PluginUtils.CurrentFileDir);
            string msg = "/msg:" + DateTime.Now.ToString();
            StartCommand(CreateCommand(TortoiseGitCommand.StashSave, dirPath, additionalParam: msg));
        }

        private void TGitStashPop()
        {
            string dirPath = PluginUtils.GetRootDir(PluginUtils.CurrentFileDir);
            StartCommand(CreateCommand(TortoiseGitCommand.StashPop, dirPath));
        }
    
        private void TGitRepoStatus()
        {
            string dirPath = PluginUtils.GetRootDir(PluginUtils.CurrentFileDir);
            StartCommand(CreateCommand(TortoiseGitCommand.RepoStatus, dirPath));
        }

        private void ReadmeFunc()
        {
            string text = "Не установлен TortoiseGit или не найдена папка с установленной программой!";
            MessageBox.Show(text, "Ошибка настройки", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        void IModule.Init(IModuleManager manager)
        {
            _manager = manager;
            _manager.OnToolbarRegisterEvent += ToolBarInit;

            logger.Debug("Create menu");
            _icons = new Dictionary<int, Bitmap>();
            if (InitTG())
            {
                var btnMask = Settings.TortoiseGitProc.ButtonMask;
                logger.Info("TortoiseGit found");

                _manager.RegisterMenuItem(new MenuItem
                {
                    Name = "TGit Log file",
                    Hint = "Log file",
                    ShortcutKey = new ShortcutKey { _isCtrl = 1, _isShift = 1, _key = (byte)Keys.L },
                    Action = TGitLogFile
                });

                _manager.RegisterMenuItem(new MenuItem
                {
                    Name = "TGit Log path",
                    Hint = "Log path",
                    ShortcutKey = new ShortcutKey { },
                    Action = TGitLogPath
                });

                var cmdID = _manager.RegisterMenuItem(new MenuItem
                {
                    Name = "TGit Log repo",
                    Hint = "Log repo",
                    ShortcutKey = new ShortcutKey { _isAlt = 1, _isCtrl = 1, _isShift = 1, _key = (byte)Keys.L },
                    Action = TGitLogRepo
                });
                if ((btnMask & (uint)TortoiseGitCommand.Log) > 0)
                    _icons.Add(cmdID, Properties.Resources.log);

                cmdID = _manager.RegisterMenuItem(new MenuItem
                {
                    Name = "TGit Fetch",
                    Hint = "Fetch",
                    ShortcutKey = new ShortcutKey { _isAlt = 1, _isCtrl = 1, _isShift = 1, _key = (byte)Keys.F },
                    Action = TGitFetch
                });
                if ((btnMask & (uint)TortoiseGitCommand.Fetch) > 0)
                    _icons.Add(cmdID, Properties.Resources.pull);

                cmdID = _manager.RegisterMenuItem(new MenuItem
                {
                    Name = "TGit Pull",
                    Hint = "Pull",
                    ShortcutKey = new ShortcutKey { _isAlt = 1, _key = (byte)Keys.P },
                    Action = TGitPull
                });
                if ((btnMask & (uint)TortoiseGitCommand.Pull) > 0)
                    _icons.Add(cmdID, Properties.Resources.pull);

                cmdID = _manager.RegisterMenuItem(new MenuItem
                {
                    Name = "TGit Push",
                    Hint = "Push",
                    ShortcutKey = new ShortcutKey { _isAlt = 1, _isCtrl = 1, _key = (byte)Keys.P },
                    Action = TGitPush
                });
                if ((btnMask & (uint)TortoiseGitCommand.Push) > 0)
                    _icons.Add(cmdID, Properties.Resources.push);

                cmdID = _manager.RegisterMenuItem(new MenuItem
                {
                    Name = "TGit Commit",
                    Hint = "Commit",
                    ShortcutKey = new ShortcutKey { _isAlt = 1, _isCtrl = 1, _key = (byte)Keys.C },
                    Action = TGitCommit
                });
                if ((btnMask & (uint)TortoiseGitCommand.Commit) > 0)
                    _icons.Add(cmdID, Properties.Resources.commit);

                cmdID = _manager.RegisterMenuItem(new MenuItem
                {
                    Name = "TGit Blame",
                    Hint = "Blame",
                    ShortcutKey = new ShortcutKey { _isAlt = 1, _isCtrl = 1, _key = (byte)Keys.B },
                    Action = TGitBlame
                });
                if ((btnMask & (uint)TortoiseGitCommand.Blame) > 0)
                    _icons.Add(cmdID, Properties.Resources.blame);

                _manager.RegisterMenuItem(new MenuItem
                {
                    Name = "TGit Blame line",
                    Hint = "Blame line",
                    ShortcutKey = new ShortcutKey { _isAlt = 1, _key = (byte)Keys.B },
                    Action = TGitBlameCurrentLine
                });

                cmdID = _manager.RegisterMenuItem(new MenuItem
                {
                    Name = "TGit Switch",
                    Hint = "Switch",
                    ShortcutKey = new ShortcutKey { _isCtrl = 1, _isAlt = 1, _key = (byte)Keys.S },
                    Action = TGitSwitch
                });
                if ((btnMask & (uint)TortoiseGitCommand.Switch) > 0)
                    _icons.Add(cmdID, Properties.Resources.checkout);

                cmdID = _manager.RegisterMenuItem(new MenuItem
                {
                    Name = "TGit Stash save",
                    Hint = "Stash save",
                    ShortcutKey = null,
                    Action = TGitStashSave
                });
                if ((btnMask & (uint)TortoiseGitCommand.StashSave) > 0)
                    _icons.Add(cmdID, Properties.Resources.stashsave);

                cmdID = _manager.RegisterMenuItem(new MenuItem
                {
                    Name = "TGit Stash pop",
                    Hint = "Stash pop",
                    ShortcutKey = null,
                    Action = TGitStashPop
                });
                if ((btnMask & (uint)TortoiseGitCommand.StashPop) > 0)
                    _icons.Add(cmdID, Properties.Resources.stashpop);

                cmdID = _manager.RegisterMenuItem(new MenuItem
                {
                    Name = "TGit Repo stastus",
                    Hint = "Repo stastus",
                    ShortcutKey = null,
                    Action = TGitRepoStatus
                });
                if ((btnMask & (uint)TortoiseGitCommand.RepoStatus) > 0)
                    _icons.Add(cmdID, Properties.Resources.repo);
            }
            else
            {
                logger.Info("TortoiseGit not found");
                _manager.RegisterMenuItem(new MenuItem
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
