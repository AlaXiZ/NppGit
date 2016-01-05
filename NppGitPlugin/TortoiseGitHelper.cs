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

        private static readonly string EXE = "TortoiseGitProc.exe";
        private static readonly string PARAM = "/command:{0} {2} /path:\"{1}\"";
        private static readonly string LOG_MSG = "/logmsg:\"{0}\"";
        private static readonly string CLOSE = "/closeonend:{0}";
        private static readonly string TORTOISEGITBIN = "TortoiseGit\\bin\\";

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
            string tortoisePath = Settings.Instance.TortoiseGit.Path;
            // Path not set
            if (tortoisePath == "")
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
                if (tortoisePath == "")
                {
                    var path = Environment.GetEnvironmentVariable("ProgramFiles").Replace(" (x86)", "");
                    if (ExistsTortoiseGit(path))
                    {
                        tortoisePath = System.IO.Path.Combine(path, TORTOISEGITBIN);
                    }
                }
                if (tortoisePath == "")
                {
                    var dlg = new FolderBrowserDialog();
                    dlg.Description = "Select folder with TortoiseGitProc.exe";
                    dlg.ShowNewFolderButton = false;
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        tortoisePath = dlg.SelectedPath;
                    }
                }
                if (tortoisePath != "")
                {
                    Settings.Instance.TortoiseGit.Path = tortoisePath;
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
            logger.Debug("Create menu");
            _icons = new Dictionary<int, Bitmap>();
            if (InitTG())
            {
                var btnMask = Settings.Instance.TortoiseGit.ButtonMask;
                logger.Info("TortoiseGit found");
                PluginUtils.SetCommand("TGit Log file", TGitLogFile);
                PluginUtils.SetCommand("TGit Log path", TGitLogPath);
                var cmdID = PluginUtils.SetCommand("TGit Log repo", TGitLogRepo);
                if ((btnMask & (uint)TortoiseGitCommand.Log) > 0)
                    _icons.Add(cmdID, Properties.Resources.log);
                cmdID = PluginUtils.SetCommand("TGit Fetch", TGitFetch);
                if ((btnMask & (uint)TortoiseGitCommand.Fetch) > 0)
                    _icons.Add(cmdID, Properties.Resources.pull);
                cmdID = PluginUtils.SetCommand("TGit Pull", TGitPull);
                if ((btnMask & (uint)TortoiseGitCommand.Pull) > 0)
                    _icons.Add(cmdID, Properties.Resources.pull);
                cmdID = PluginUtils.SetCommand("TGit Push", TGitPush);
                if ((btnMask & (uint)TortoiseGitCommand.Push) > 0)
                    _icons.Add(cmdID, Properties.Resources.push);
                cmdID = PluginUtils.SetCommand("TGit Commit", TGitCommit);
                if ((btnMask & (uint)TortoiseGitCommand.Commit) > 0)
                    _icons.Add(cmdID, Properties.Resources.commit);
                cmdID = PluginUtils.SetCommand("TGit Blame", TGitBlame);
                if ((btnMask & (uint)TortoiseGitCommand.Blame) > 0)
                    _icons.Add(cmdID, Properties.Resources.blame);
                PluginUtils.SetCommand("TGit Blame line", TGitBlameCurrentLine);
                cmdID = PluginUtils.SetCommand("TGit Switch", TGitSwitch);
                if ((btnMask & (uint)TortoiseGitCommand.Switch) > 0)
                    _icons.Add(cmdID, Properties.Resources.checkout);
                cmdID = PluginUtils.SetCommand("TGit Stash save", TGitStashSave);
                if ((btnMask & (uint)TortoiseGitCommand.StashSave) > 0)
                    _icons.Add(cmdID, Properties.Resources.stashsave);
                cmdID = PluginUtils.SetCommand("TGit Stash pop", TGitStashPop);
                if ((btnMask & (uint)TortoiseGitCommand.StashPop) > 0)
                    _icons.Add(cmdID, Properties.Resources.stashpop);
                cmdID = PluginUtils.SetCommand("TGit Repo stastus", TGitRepoStatus);
                if ((btnMask & (uint)TortoiseGitCommand.RepoStatus) > 0)
                    _icons.Add(cmdID, Properties.Resources.repo);
            }
            else
            {
                logger.Info("TortoiseGit not found");
                PluginUtils.SetCommand("Readme", ReadmeFunc);
            }
        }

        public void ToolBarInit()
        { 
            logger.Debug("Create toolbar");

            if (Settings.Instance.TortoiseGit.ShowToolbar)
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

        public void ChangeContext()
        {
            logger.Debug("ChangeContext");
        }
    }
}
