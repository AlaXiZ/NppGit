using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace NppGitPlugin
{
    class Plugin
    {
        #region "Fields"
        internal const string PluginName = "NppGitPlugin";
        static GitStatus gitStatusDlg = null;
        static int gitStatusId = -1;

        static Icon tbIcon = null;
        #endregion

        #region " StartUp/CleanUp "
        internal static void CommandMenuInit()
        {
            var isVisiblePanel = Settings.Instance.PanelsSet.StatusPanelVisible;
            
            if (TortoiseGitHelper.Init())
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
                PluginUtils.SetCommand("Readme", TortoiseGitHelper.ReadmeFunc);
            }
            PluginUtils.SetCommand("-", null);
            PluginUtils.SetCommand("Sample context menu", PluginCommands.ContextMenu);
            PluginUtils.SetCommand("-", null);
            gitStatusId = PluginUtils.SetCommand("Status panel", GitStatusDialog, isVisiblePanel);
            PluginCommands.ShowBranchID = PluginUtils.SetCommand("Branch in title", PluginCommands.ShowBranch, Settings.Instance.FuncSet.ShowBranch); 

            if (isVisiblePanel)
                GitStatusDialog();
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
            Settings.Instance.PanelsSet.StatusPanelVisible = gitStatusDlg != null && gitStatusDlg.Visible;
            gitStatusDlg = null;
            //Win32.WritePrivateProfileString("SomeSection", "SomeKey", someSetting ? "1" : "0", iniFilePath);
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

        public static void ChangeTabItem()
        {
            if (gitStatusDlg != null)
                (gitStatusDlg as FormDockable).ChangeContext();

            if (Settings.Instance.FuncSet.ShowBranch)
                PluginCommands.DoShowBranch();
        }
        #endregion
    }
}