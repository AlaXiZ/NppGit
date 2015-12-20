using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace NppGitPlugin
{
    public partial class Plugin
    {
        #region " Fields "
        public static NppData nppData;
        internal static FuncItems _funcItems = new FuncItems();
        internal static int index = 0;
        #endregion

        #region " Helper "
        public static int SetCommand(string commandName, Action functionPointer)
        {
            return SetCommand(commandName, functionPointer, new ShortcutKey(), false);
        }
        public static int SetCommand(string commandName, Action functionPointer, ShortcutKey shortcut)
        {
            return SetCommand(commandName, functionPointer, shortcut, false);
        }
        public static int SetCommand(string commandName, Action functionPointer, bool checkOnInit)
        {
            return SetCommand(commandName, functionPointer, new ShortcutKey(), checkOnInit);
        }
        public static int SetCommand(string commandName, Action functionPointer, ShortcutKey shortcut, bool checkOnInit)
        {
            FuncItem funcItem = new FuncItem();
            funcItem._cmdID = ++index;
            funcItem._itemName = commandName;
            if (functionPointer != null)
                funcItem._pFunc = functionPointer;
            if (shortcut._key != 0)
                funcItem._pShKey = shortcut;
            funcItem._init2Check = checkOnInit;
            _funcItems.Add(funcItem);
            return funcItem._cmdID;
        }

        public static IntPtr CurrentScintilla
        {
            get
            {
                int curScintilla;
                Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_GETCURRENTSCINTILLA, 0, out curScintilla);
                return (curScintilla == 0) ? nppData._scintillaMainHandle : nppData._scintillaSecondHandle;
            }
        }

        public static string ConfigDir
        {
            get
            {
                var buffer = new StringBuilder(Win32.MAX_PATH);
                Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_GETPLUGINSCONFIGDIR, Win32.MAX_PATH, buffer);
                return buffer.ToString();
            }
        }

        public static string PluginDir
        {
            get
            {
                string assemblyFile = Assembly.GetExecutingAssembly().Location;
                return Path.Combine(Path.GetDirectoryName(assemblyFile), Path.GetFileNameWithoutExtension(assemblyFile));
            }
        }

        static public void SetToolbarImage(Bitmap image, int pluginId)
        {
            var tbIcons = new toolbarIcons();
            tbIcons.hToolbarBmp = image.GetHbitmap();
            IntPtr pTbIcons = Marshal.AllocHGlobal(Marshal.SizeOf(tbIcons));
            Marshal.StructureToPtr(tbIcons, pTbIcons, false);
            Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_ADDTOOLBARICON, _funcItems.Items[pluginId]._cmdID, pTbIcons);
            Marshal.FreeHGlobal(pTbIcons);
        }

        public static string CurrentFilePath
        {
            get
            {
                var buffer = new StringBuilder(Win32.MAX_PATH);
                Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_GETFULLCURRENTPATH, Win32.MAX_PATH, buffer);
                return buffer.ToString();
            }
        }

        public static string CurrentFileDir
        {
            get
            {
                var buffer = new StringBuilder(Win32.MAX_PATH);
                Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_GETFULLCURRENTPATH, Win32.MAX_PATH, buffer);
                return System.IO.Path.GetDirectoryName(buffer.ToString());
            }
        }

        public static long CurrentLine
        {
            get
            {
                return ((long)Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_GETCURRENTLINE, 0, 0)) + 1;
            }
        }
        #endregion
    }
}
