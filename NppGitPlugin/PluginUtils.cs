using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace NppGitPlugin
{
    public partial class PluginUtils
    {
        #region "Notepad++"
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

        public static void SetToolbarImage(Bitmap image, int pluginId)
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

        public static void NewFile()
        {
            Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_MENUCOMMAND, 0, NppMenuCmd.IDM_FILE_NEW);
        }

        public static void SetLang(LangType langType)
        {
            Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_SETCURRENTLANGTYPE, 0, (int)langType);
        }

        #endregion

        #region SCI Command
        public static void AppendText(string text)
        {
            var buffer = new StringBuilder(text.Length);
            buffer.Append(text);
            Win32.SendMessage(CurrentScintilla, SciMsg.SCI_ADDTEXT, buffer.Length, buffer);
        }

        public static void NewLine()
        {
            Win32.SendMessage(CurrentScintilla, SciMsg.SCI_NEWLINE, 0, 0);
        }
        #endregion

        public static string GetRootDir(string path)
        {
            if (Directory.Exists(Path.Combine(path, ".git")))
            {
                return path;
            }
            else
            {
                if (!string.IsNullOrEmpty(path) && Directory.GetParent(path) != null)
                {
                    return GetRootDir(Directory.GetParent(path).FullName);
                }
                else {
                    return null;
                }
            }
        }



        public static Icon NppBitmapToIcon(Bitmap bitmap)
        {
            using (Bitmap newBmp = new Bitmap(16, 16))
            {
                Graphics g = Graphics.FromImage(newBmp);
                ColorMap[] colorMap = new ColorMap[1];
                colorMap[0] = new ColorMap();
                colorMap[0].OldColor = Color.Fuchsia;
                colorMap[0].NewColor = Color.FromKnownColor(KnownColor.ButtonFace);
                ImageAttributes attr = new ImageAttributes();
                attr.SetRemapTable(colorMap);
                g.DrawImage(bitmap, new Rectangle(0, 0, 16, 16), 0, 0, 16, 16, GraphicsUnit.Pixel, attr);
                return Icon.FromHandle(newBmp.GetHicon());
            }
        }
    }
}
