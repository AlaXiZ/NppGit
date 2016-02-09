using NppGit.Interop;
using NppGit.Modules;
using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace NppGit
{
    class Plugin
    {
        #region "Fields"
        //internal const string PluginName = "NppGit";
        private static ModuleManager mm = new ModuleManager();
        static GitStatus gitStatusDlg = null;
        static int gitStatusId = -1;

        static Icon tbIcon = null;
        #endregion

        #region " StartUp/CleanUp "
        internal static void Init()
        {
            mm.AddModule(new TortoiseGitHelper());
            mm.AddModule(new GitFeatures());
            mm.AddModule(new SqlIde());
            mm.AddModule(new Snippets());

            mm.Init();
            var isVisiblePanel = Settings.Panels.StatusPanelVisible;
            
            // PluginUtils.SetCommand("-", null);
            // gitStatusId = PluginUtils.SetCommand("Status panel", GitStatusDialog, isVisiblePanel);
            // PluginCommands.ShowBranchID = PluginUtils.SetCommand("Branch in title", PluginCommands.ShowBranch, Settings.Instance.FuncSet.ShowBranch); 

            // if (isVisiblePanel)
            //     GitStatusDialog();
            //PluginUtils.SetCommand("-", null);
            //PluginUtils.SetCommand("Sample context menu", mm.ContextMenu);
            PluginUtils.SetCommand("Settings", DoSettings);
            PluginUtils.SetCommand("About", DoAbout);
        }

        internal static void ToolBarInit()
        {
            mm.ToolBarInit();          
        }

        internal static void PluginCleanUp()
        {
            mm.Final();
            Settings.Panels.StatusPanelVisible = gitStatusDlg != null && gitStatusDlg.Visible;
            gitStatusDlg = null;
        }

        internal static void MessageProc (SCNotification sn)
        {
            mm.MessageProc(sn);
        }


        #endregion

        #region " Menu functions "
        internal static void DoSettings()
        {
            var dlg = new Forms.SettingsDialog();
            dlg.ShowDialog();
        }

        internal static void DoAbout()
        {
            var dlg = new Forms.About();
            dlg.ShowDialog();
        }

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
                _nppTbData.pszModuleName = Properties.Resources.PluginName;
                IntPtr _ptrNppTbData = Marshal.AllocHGlobal(Marshal.SizeOf(_nppTbData));
                Marshal.StructureToPtr(_nppTbData, _ptrNppTbData, false);

                Win32.SendMessage(PluginUtils.NppHandle, NppMsg.NPPM_DMMREGASDCKDLG, 0, _ptrNppTbData);
                isVisible = 1;
            }
            else
            {
                if (gitStatusDlg.Visible)
                {
                    Win32.SendMessage(PluginUtils.NppHandle, NppMsg.NPPM_DMMHIDE, 0, gitStatusDlg.Handle);
                }
                else
                {
                    Win32.SendMessage(PluginUtils.NppHandle, NppMsg.NPPM_DMMSHOW, 0, gitStatusDlg.Handle);
                    isVisible = 1;
                }
            }
            if (gitStatusDlg != null && gitStatusDlg.Visible)
            {
                (gitStatusDlg as FormDockable).ChangeContext();
            }

           Win32.SendMessage(PluginUtils.NppHandle, NppMsg.NPPM_SETMENUITEMCHECK, PluginUtils._funcItems.Items[gitStatusId]._cmdID, isVisible);
        }

        public static void ChangeTabItem()
        {
            if (gitStatusDlg != null)
                (gitStatusDlg as FormDockable).ChangeContext();
        }
        #endregion
    }
}