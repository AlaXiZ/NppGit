using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace NppGitPlugin
{
    class Main
    {
        #region " Fields "
        internal const string PluginName = "NppGitPlugin";
        private static readonly string TortoiseGitBin = "TortoiseGit\\bin\\";
        static string iniPath = null;
        //static bool someSetting = false;
        //static frmMyDlg frmMyDlg = null;
        //static int idMyDlg = -1;
        //static Bitmap tbBmp = Properties.Resources.star;
        //static Bitmap tbBmp_tbTab = Properties.Resources.star_bmp;
        //static Icon tbIcon = null;
        #endregion

        #region " StartUp/CleanUp "
        internal static void CommandMenuInit()
        {
            iniPath = Path.Combine(Plugin.ConfigDir, PluginName + ".ini");
            InitTortoise();

            if (TortoiseGitHelper.TortoiseGitPath != "")
            {
                Plugin.SetCommand("TGit Log file", TortoiseGitHelper.TGitLogFile);
                Plugin.SetCommand("TGit Log path", TortoiseGitHelper.TGitLogPath);
                Plugin.SetCommand("TGit Log repo", TortoiseGitHelper.TGitLogRepo);
                Plugin.SetCommand("TGit Fetch", TortoiseGitHelper.TGitFetch);
                Plugin.SetCommand("TGit Pull", TortoiseGitHelper.TGitPull);
                Plugin.SetCommand("TGit Push", TortoiseGitHelper.TGitPush);
                Plugin.SetCommand("TGit Commit", TortoiseGitHelper.TGitCommit);
                Plugin.SetCommand("TGit Blame", TortoiseGitHelper.TGitBlame);
                Plugin.SetCommand("TGit Blame line", TortoiseGitHelper.TGitBlameCurrentLine);
                Plugin.SetCommand("TGit Switch", TortoiseGitHelper.TGitSwitch);
                Plugin.SetCommand("TGit Stash save", TortoiseGitHelper.TGitStashSave);
                Plugin.SetCommand("TGit Stash pop", TortoiseGitHelper.TGitStashPop);
                Plugin.SetCommand("TGit Repo status", TortoiseGitHelper.TGitRepoStatus);
            }
            else
            {
                Plugin.SetCommand("Readme", ReadmeFunc);
            }
            Plugin.SetCommand("Sample context menu", PluginCommands.ContextMenu);
            /*Plugin.SetCommand("TGit ", TortoiseGitHelper.TGit);
            Plugin.SetCommand("TGit ", TortoiseGitHelper.TGit);
            Plugin.SetCommand("TGit ", TortoiseGitHelper.TGit);
            */
            //idMyDlg = Plugin.SetCommand("MyDockableDialog", myDockableDialog);
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
            Win32.SendMessage(Plugin.nppData._nppHandle, NppMsg.NPPM_ADDTOOLBARICON, Plugin._funcItems.Items[idMyDlg]._cmdID, pTbIcons);
            Marshal.FreeHGlobal(pTbIcons);
            */            
        }

        internal static void PluginCleanUp()
        {
            //Win32.WritePrivateProfileString("SomeSection", "SomeKey", someSetting ? "1" : "0", iniFilePath);
        }

        private static bool ExistsTortoiseGit(string programPath)
        {
            return System.IO.Directory.Exists(System.IO.Path.Combine(programPath, TortoiseGitBin));
        }

        internal static void InitTortoise()
        {
            IniFile iniFile = new IniFile(iniPath);
            string tortoisePath = iniFile.GetValue<string>("TortoiseGitProc", "Path", "");
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
                    iniFile.SetValue<string>("TortoiseGitProc", "Path", tortoisePath);
                }
            }
            //
            TortoiseGitHelper.TortoiseGitPath = tortoisePath;
        }
        #endregion

        #region " Menu functions "
        /*
        internal static void myMenuFunction()
        {
            MessageBox.Show("Hello N++!");
        }
        internal static void myDockableDialog()
        {
            if (frmMyDlg == null)
            {
                frmMyDlg = new frmMyDlg();

                using (Bitmap newBmp = new Bitmap(16, 16))
                {
                    Graphics g = Graphics.FromImage(newBmp);
                    ColorMap[] colorMap = new ColorMap[1];
                    colorMap[0] = new ColorMap();
                    colorMap[0].OldColor = Color.Fuchsia;
                    colorMap[0].NewColor = Color.FromKnownColor(KnownColor.ButtonFace);
                    ImageAttributes attr = new ImageAttributes();
                    attr.SetRemapTable(colorMap);
                    g.DrawImage(tbBmp_tbTab, new Rectangle(0, 0, 16, 16), 0, 0, 16, 16, GraphicsUnit.Pixel, attr);
                    tbIcon = Icon.FromHandle(newBmp.GetHicon());
                }

                NppTbData _nppTbData = new NppTbData();
                _nppTbData.hClient = frmMyDlg.Handle;
                _nppTbData.pszName = "My dockable dialog";
                _nppTbData.dlgID = idMyDlg;
                _nppTbData.uMask = NppTbMsg.DWS_DF_CONT_RIGHT | NppTbMsg.DWS_ICONTAB | NppTbMsg.DWS_ICONBAR;
                _nppTbData.hIconTab = (uint)tbIcon.Handle;
                _nppTbData.pszModuleName = PluginName;
                IntPtr _ptrNppTbData = Marshal.AllocHGlobal(Marshal.SizeOf(_nppTbData));
                Marshal.StructureToPtr(_nppTbData, _ptrNppTbData, false);

                Win32.SendMessage(Plugin.nppData._nppHandle, NppMsg.NPPM_DMMREGASDCKDLG, 0, _ptrNppTbData);
            }
            else
            {
                Win32.SendMessage(Plugin.nppData._nppHandle, NppMsg.NPPM_DMMSHOW, 0, frmMyDlg.Handle);
            }
        }
        */
        #endregion
    }
}