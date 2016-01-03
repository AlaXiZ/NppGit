using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace NppGitPlugin
{
    class Plugin
    {
        #region "Fields"
        internal const string PluginName = "NppGitPlugin";
        private static readonly string TortoiseGitBin = "TortoiseGit\\bin\\";
        static string iniPath = null;
        static GitStatus gitStatusDlg = null;
        static int gitStatusId = -1;
        static IniFile settings = null;
        //static bool someSetting = false;
        //static frmMyDlg frmMyDlg = null;
        //static int idMyDlg = -1;
        //static Bitmap tbBmp = Properties.Resources.star;
        //static Bitmap tbBmp_tbTab = Properties.Resources.star_bmp;
        static Icon tbIcon = null;
        #endregion

        #region " StartUp/CleanUp "
        internal static void CommandMenuInit()
        {
            iniPath = Path.Combine(PluginUtils.ConfigDir, PluginName + ".ini");
            settings = new IniFile(iniPath);

            var isVisiblePanel = settings.GetValue<int>("Panels", "StatusPanel.Visible", 0);

            InitTortoise();

            if (TortoiseGitHelper.TortoiseGitPath != "")
            {
                PluginUtils.SetCommand("TGit Log file", TortoiseGitHelper.TGitLogFile);
                PluginUtils.SetCommand("TGit Log path", TortoiseGitHelper.TGitLogPath);
                PluginUtils.SetCommand("TGit Log repo", TortoiseGitHelper.TGitLogRepo);
                PluginUtils.SetCommand("TGit Fetch", TortoiseGitHelper.TGitFetch);
                PluginUtils.SetCommand("TGit Pull", TortoiseGitHelper.TGitPull);
                PluginUtils.SetCommand("TGit Push", TortoiseGitHelper.TGitPush);
                PluginUtils.SetCommand("TGit Commit", TortoiseGitHelper.TGitCommit);
                PluginUtils.SetCommand("TGit Blame", TortoiseGitHelper.TGitBlame);
                PluginUtils.SetCommand("TGit Blame line", TortoiseGitHelper.TGitBlameCurrentLine);
                PluginUtils.SetCommand("TGit Switch", TortoiseGitHelper.TGitSwitch);
                PluginUtils.SetCommand("TGit Stash save", TortoiseGitHelper.TGitStashSave);
                PluginUtils.SetCommand("TGit Stash pop", TortoiseGitHelper.TGitStashPop);
                PluginUtils.SetCommand("TGit Repo status", TortoiseGitHelper.TGitRepoStatus);
            }
            else
            {
                PluginUtils.SetCommand("Readme", ReadmeFunc);
            }
            PluginUtils.SetCommand("-", null);
            PluginUtils.SetCommand("Sample context menu", PluginCommands.ContextMenu);
            PluginUtils.SetCommand("-", null);
            gitStatusId = PluginUtils.SetCommand("Git status", GitStatusDialog, isVisiblePanel == 1);

            if (isVisiblePanel == 1)
            {
                GitStatusDialog();
            }
            /*PluginUtils.SetCommand("TGit ", TortoiseGitHelper.TGit);
            PluginUtils.SetCommand("TGit ", TortoiseGitHelper.TGit);
            PluginUtils.SetCommand("TGit ", TortoiseGitHelper.TGit);
            */
            //idMyDlg = PluginUtils.SetCommand("MyDockableDialog", myDockableDialog);
        }

        internal static void ReadmeFunc()
        {
            string text = "Не установлен TortoiseGit или не найдена папка с установленной программой!";
            MessageBox.Show(text, "Ошибка настройки", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        internal static void SetToolBarIcon()
        {

            /*
            toolbarIcons tbIcons = new toolbarIcons();
            tbIcons.hToolbarBmp = tbBmp.GetHbitmap();
            IntPtr pTbIcons = Marshal.AllocHGlobal(Marshal.SizeOf(tbIcons));
            Marshal.StructureToPtr(tbIcons, pTbIcons, false);
            Win32.SendMessage(PluginUtils.nppData._nppHandle, NppMsg.NPPM_ADDTOOLBARICON, PluginUtils._funcItems.Items[idMyDlg]._cmdID, pTbIcons);
            Marshal.FreeHGlobal(pTbIcons);
            */            
        }

        internal static void PluginCleanUp()
        {
            var isVisible = gitStatusDlg != null && gitStatusDlg.Visible ? 1 : 0;
            settings.SetValue<int>("Panels", "StatusPanel.Visible", isVisible);
            gitStatusDlg = null;
            //Win32.WritePrivateProfileString("SomeSection", "SomeKey", someSetting ? "1" : "0", iniFilePath);
        }

        private static bool ExistsTortoiseGit(string programPath)
        {
            return System.IO.Directory.Exists(System.IO.Path.Combine(programPath, TortoiseGitBin));
        }

        internal static void InitTortoise()
        {
            string tortoisePath = settings.GetValue<string>("TortoiseGitProc", "Path", "");
            // Path not set
            if (tortoisePath == "")
            {
                // x64
                if (8 == IntPtr.Size || (!String.IsNullOrEmpty(Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432"))))
                {
                    var path = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
                    if (ExistsTortoiseGit(path))
                    {
                        tortoisePath = System.IO.Path.Combine(path, TortoiseGitBin);
                    }
                }
                if (tortoisePath == "")
                {
                    var path = Environment.GetEnvironmentVariable("ProgramFiles").Replace(" (x86)", "");
                    if (ExistsTortoiseGit(path))
                    {
                        tortoisePath = System.IO.Path.Combine(path, TortoiseGitBin);
                    }
                }
                if (tortoisePath == "")
                {
                    var dlg = new FolderBrowserDialog();
                    dlg.Description = "Select folder with TortoiseGitProc";
                    dlg.ShowNewFolderButton = false;
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        tortoisePath = dlg.SelectedPath;
                    }
                }
                if (tortoisePath != "")
                {
                    settings.SetValue<string>("TortoiseGitProc", "Path", tortoisePath);
                }
            }
            //
            TortoiseGitHelper.TortoiseGitPath = tortoisePath;
        }
        #endregion

        #region " Menu functions "
        static void GitStatusDialog()
        {
            var isVisible = 0;
            if (gitStatusDlg == null)
            {
                gitStatusDlg = new GitStatus();

                tbIcon = PluginUtils.NppBitmapToIcon(Properties.Resources.Git);

                NppTbData _nppTbData = new NppTbData();
                _nppTbData.hClient = gitStatusDlg.Handle;
                _nppTbData.pszName = "Git status";
                _nppTbData.dlgID = gitStatusId;
                _nppTbData.uMask = NppTbMsg.DWS_DF_CONT_RIGHT | NppTbMsg.DWS_ICONTAB | NppTbMsg.DWS_ICONBAR;
                _nppTbData.hIconTab = (uint)tbIcon.Handle;
                _nppTbData.pszModuleName = PluginName;
                IntPtr _ptrNppTbData = Marshal.AllocHGlobal(Marshal.SizeOf(_nppTbData));
                Marshal.StructureToPtr(_nppTbData, _ptrNppTbData, false);

                Win32.SendMessage(PluginUtils.nppData._nppHandle, NppMsg.NPPM_DMMREGASDCKDLG, 0, _ptrNppTbData);
                isVisible = 1;
            }
            else
            {
                if (gitStatusDlg.Visible)
                {
                    Win32.SendMessage(PluginUtils.nppData._nppHandle, NppMsg.NPPM_DMMHIDE, 0, gitStatusDlg.Handle);
                }
                else
                {
                    Win32.SendMessage(PluginUtils.nppData._nppHandle, NppMsg.NPPM_DMMSHOW, 0, gitStatusDlg.Handle);
                    isVisible = 1;
                }
            }
            if (gitStatusDlg != null && gitStatusDlg.Visible)
            {
                (gitStatusDlg as FormDockable).ChangeContext();
            }

           Win32.SendMessage(PluginUtils.nppData._nppHandle, NppMsg.NPPM_SETMENUITEMCHECK, PluginUtils._funcItems.Items[gitStatusId]._cmdID, isVisible);
        }

        public static void UpdateDialogs()
        {
            if (gitStatusDlg != null)
                (gitStatusDlg as FormDockable).ChangeContext();
        }
        #endregion
    }
}