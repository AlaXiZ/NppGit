using System;
using System.Text;
using System.Windows.Forms;

namespace NppGitPlugin
{
    public enum TortoiseGitCommand
    {
        Fetch,
        Log,
        Commit,
        Add,
        Revert,
        Switch,
        Blame,
        Pull,
        Push,
        StashSave,
        StashPop,
        RepoStatus
    }

    public class TortoiseGitHelper
    {
        private static readonly string EXE = "TortoiseGitProc.exe";
        private static readonly string PARAM = "/command:{0} {2} /path:\"{1}\"";
        private static readonly string LOG_MSG = "/logmsg:\"{0}\"";
        private static readonly string CLOSE = "/closeonend:{0}";
        private static readonly string TORTOISEGITBIN = "TortoiseGit\\bin\\";

        private static string tortoiseGitPath = "";
        private static string tortoiseGitProc = "";
        
        private static bool ExistsTortoiseGit(string programPath)
        {
            return System.IO.Directory.Exists(System.IO.Path.Combine(programPath, TORTOISEGITBIN));
        }

        public static bool Init()
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

        public static void TGitLogFile()
        {
            string filePath = PluginUtils.CurrentFilePath;
            StartCommand(CreateCommand(TortoiseGitCommand.Log, filePath));
        }

        public static void TGitLogPath()
        {
            string dirPath = PluginUtils.CurrentFileDir;
            StartCommand(CreateCommand(TortoiseGitCommand.Log, dirPath));
        }

        public static void TGitLogRepo()
        {
            string dirPath = PluginUtils.GetRootDir(PluginUtils.CurrentFileDir);
            StartCommand(CreateCommand(TortoiseGitCommand.Log, dirPath));
        }

        public static void TGitFetch()
        {
            string dirPath = PluginUtils.GetRootDir(PluginUtils.CurrentFileDir);
            StartCommand(CreateCommand(TortoiseGitCommand.Fetch, dirPath));
        }

        public static void TGitPull()
        {
            string dirPath = PluginUtils.GetRootDir(PluginUtils.CurrentFileDir);
            StartCommand(CreateCommand(TortoiseGitCommand.Pull, dirPath));
        }

        public static void TGitPush()
        {
            string dirPath = PluginUtils.GetRootDir(PluginUtils.CurrentFileDir);
            StartCommand(CreateCommand(TortoiseGitCommand.Push, dirPath));
        }

        public static void TGitCommit()
        {
            string dirPath = PluginUtils.GetRootDir(PluginUtils.CurrentFileDir);
            StartCommand(CreateCommand(TortoiseGitCommand.Commit, dirPath));
        }

        public static void TGitBlame()
        {
            string filePath = PluginUtils.CurrentFilePath;
            StartCommand(CreateCommand(TortoiseGitCommand.Blame, filePath));
        }

        public static void TGitBlameCurrentLine()
        {
            string filePath = PluginUtils.CurrentFilePath;
            string param = string.Format("/line:{0}", PluginUtils.CurrentLine);
            StartCommand(CreateCommand(TortoiseGitCommand.Blame, filePath, additionalParam: param));
        }

        public static void TGitSwitch()
        {
            string dirPath = PluginUtils.GetRootDir(PluginUtils.CurrentFileDir);
            StartCommand(CreateCommand(TortoiseGitCommand.Switch, dirPath));
        }

        public static void TGitStashSave()
        {
            string dirPath = PluginUtils.GetRootDir(PluginUtils.CurrentFileDir);
            string msg = "/msg:" + DateTime.Now.ToString();
            StartCommand(CreateCommand(TortoiseGitCommand.StashSave, dirPath, additionalParam: msg));
        }

        public static void TGitStashPop()
        {
            string dirPath = PluginUtils.GetRootDir(PluginUtils.CurrentFileDir);
            StartCommand(CreateCommand(TortoiseGitCommand.StashPop, dirPath));
        }
    
        public static void TGitRepoStatus()
        {
            string dirPath = PluginUtils.GetRootDir(PluginUtils.CurrentFileDir);
            StartCommand(CreateCommand(TortoiseGitCommand.RepoStatus, dirPath));
        }

        public static void ReadmeFunc()
        {
            string text = "Не установлен TortoiseGit или не найдена папка с установленной программой!";
            MessageBox.Show(text, "Ошибка настройки", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
