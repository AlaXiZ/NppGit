﻿using NppGit.Interop;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace NppGit
{
    public partial class PluginUtils
    {
        #region "Notepad++"
        
        public static IntPtr CurrentScintilla
        {
            get
            {
                int curScintilla;
                Win32.SendMessage(NppHandle, NppMsg.NPPM_GETCURRENTSCINTILLA, 0, out curScintilla);
                return (curScintilla == 0) ? nppData._scintillaMainHandle : nppData._scintillaSecondHandle;
            }
        }

        public static string ConfigDir
        {
            get
            {
                var buffer = new StringBuilder(Win32.MAX_PATH);
                Win32.SendMessage(NppHandle, NppMsg.NPPM_GETPLUGINSCONFIGDIR, Win32.MAX_PATH, buffer);
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
            Win32.SendMessage(NppHandle, NppMsg.NPPM_ADDTOOLBARICON, _funcItems.Items[pluginId]._cmdID, pTbIcons);
            Marshal.FreeHGlobal(pTbIcons);
        }

        public static string CurrentFilePath
        {
            get
            {
                var buffer = new StringBuilder(Win32.MAX_PATH);
                Win32.SendMessage(NppHandle, NppMsg.NPPM_GETFULLCURRENTPATH, Win32.MAX_PATH, buffer);
                return buffer.ToString();
            }
            set
            {
                var buffer = new StringBuilder(value);
                Win32.SendMessage(NppHandle, NppMsg.NPPM_SWITCHTOFILE, 0, buffer);
            }
        }

        public static string CurrentFileName
        {
            get
            {
                return Path.GetFileName(CurrentFilePath);
            }
        }

        public static string CurrentFileDir
        {
            get
            {
                var buffer = new StringBuilder(Win32.MAX_PATH);
                Win32.SendMessage(NppHandle, NppMsg.NPPM_GETFULLCURRENTPATH, Win32.MAX_PATH, buffer);
                return Path.GetDirectoryName(buffer.ToString());
            }
        }

        public static long CurrentLine
        {
            get { return ((long)Win32.SendMessage(NppHandle, NppMsg.NPPM_GETCURRENTLINE, 0, 0)) + 1; }
        }

        public static void NewFile()
        {
            Win32.SendMessage(NppHandle, NppMsg.NPPM_MENUCOMMAND, 0, NppMenuCmd.IDM_FILE_NEW);
        }

        public static void SetLang(LangType langType)
        {
            Win32.SendMessage(NppHandle, NppMsg.NPPM_SETCURRENTLANGTYPE, 0, (int)langType);
        }

        public static bool OpenFile(string filePath)
        {
            var buf = new StringBuilder(filePath);
           return 1 == (int)Win32.SendMessage(NppHandle, NppMsg.NPPM_DOOPEN, 0, buf);
        }

        public static void MoveFileToOtherView()
        {
            Win32.SendMessage(NppHandle, NppMsg.NPPM_MENUCOMMAND, 0, NppMenuCmd.IDM_VIEW_GOTO_ANOTHER_VIEW);
        }

        public static int CurrentBufferId
        {
            get
            {
                return (int)Win32.SendMessage(NppHandle, NppMsg.NPPM_GETCURRENTBUFFERID, 0, 0);
            }
        }

        public static void SetCheckedMenu(int cmdId, bool isChecked)
        {
            Win32.SendMessage(NppHandle, NppMsg.NPPM_SETMENUITEMCHECK, cmdId, isChecked ? 1 : 0);
        }

        #endregion

        #region SCI Command
        public static void AppendText(string text)
        {
            SendText(SciMsg.SCI_ADDTEXT, text);
        }

        public static void NewLine()
        {
            Win32.SendMessage(CurrentScintilla, SciMsg.SCI_NEWLINE, 0, 0);
        }

        public static string GetEOL()
        {
           switch(execute(SciMsg.SCI_GETEOLMODE, 0))
            {
                case (int)SciMsg.SC_EOL_CRLF: return "\r\n";
                case (int)SciMsg.SC_EOL_CR: return "\r";
                case (int)SciMsg.SC_EOL_LF: return "\n";
                default: return "\r\n";
            }
        }

        public static string GetTextBetween(int start, int end = -1)
        {
            IntPtr sci = CurrentScintilla;

            if (end == -1)
                end = (int)Win32.SendMessage(sci, SciMsg.SCI_GETLENGTH, 0, 0);

            using (var tr = new Sci_TextRange(start, end, end - start + 1)) //+1 for null termination
            {
                Win32.SendMessage(sci, SciMsg.SCI_GETTEXTRANGE, 0, tr.NativePointer);
                return tr.lpstrText;
            }
        }

        public static string GetSelectedText()
        {
            return GetText(SciMsg.SCI_GETSELTEXT);
        }
        
        public static void ReplaceSelectedText(string[] lines)
        {
            StringBuilder buf = new StringBuilder("");

            execute(SciMsg.SCI_BEGINUNDOACTION, 0);
            if (execute(SciMsg.SCI_GETSELECTIONMODE, 0) > 0)
            {
                var startPos = execute(SciMsg.SCI_GETSELECTIONNSTART, 0);
                var line = execute(SciMsg.SCI_LINEFROMPOSITION, startPos);
                Win32.SendMessage(CurrentScintilla, SciMsg.SCI_REPLACESEL, buf.Length, buf);
                foreach (var str in lines)
                {
                    var linePos = execute(SciMsg.SCI_GETLINEENDPOSITION, line);
                    line++;
                    SendText(SciMsg.SCI_INSERTTEXT, str, linePos);
                }
            }
            else
            {
                Win32.SendMessage(CurrentScintilla, SciMsg.SCI_REPLACESEL, buf.Length, buf);
                var count = lines.Length - 1;
                for (int i = 0; i <= count; i++)
                {
                    SendText(SciMsg.SCI_ADDTEXT, lines[i]);
                    if (i < count)
                        NewLine();
                }
            }
            execute(SciMsg.SCI_ENDUNDOACTION, 0);
        }

        public static void ReplaceSelectedText(string text)
        {
            ReplaceSelectedText(text.Replace(GetEOL(), "\n").Split('\n'));
        }

        static int execute(SciMsg msg, int wParam, int lParam = 0)
        {
            IntPtr sci = CurrentScintilla;
            return (int)Win32.SendMessage(sci, msg, wParam, lParam);
        }

        /* True method for sending text with correct codepage */
        static void SendText(SciMsg msg, string text, int pos = 0)
        {
            var buf = Encoding.UTF8.GetBytes(text + (pos > 0 ? "\0" : ""));
            IntPtr ptr = Marshal.AllocHGlobal(buf.Length);
            try
            {
                Marshal.Copy(buf, 0, ptr, buf.Length);
                Win32.SendMessage(CurrentScintilla, msg, pos == 0 ? buf.Length : pos, ptr);
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }
        /* True method for getting text with correct codepage */
        static string GetText(SciMsg msg)
        {
            int length = execute(msg, 0) + 1;
            if (length == 1)
            {
                return "";
            }

            byte[] buffer = new byte[length];
            IntPtr ptr = Marshal.AllocHGlobal(length);
            try
            {
                Win32.SendMessage(CurrentScintilla, msg,     length, ptr);
                Marshal.Copy(ptr, buffer, 0, length);
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
            return Encoding.UTF8.GetString(buffer).TrimEnd('\0');
        }
        #endregion


        public static string GetRootDir(string path)
        {
            var search = Path.Combine(path, ".git");
            if (Directory.Exists(search) || File.Exists(search))
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

        public static string WindowTitle
        {
            get
            {
                var title = new StringBuilder(Win32.MAX_PATH);
                Win32.SendMessage(NppHandle, (int)WinMsg.WM_GETTEXT, Win32.MAX_PATH, title);
                return title.ToString();
            }
            set
            {   
                var title = new StringBuilder(value);
                Win32.SendMessage(NppHandle, (int)WinMsg.WM_SETTEXT, (int)WinMsg.WM_SETTEXT, title);
            }
        }

        public static int NppPID
        {
            get
            {
                var pid = 0;
                foreach (var p in System.Diagnostics.Process.GetProcessesByName("notepad++"))
                {
                    pid = p.Id;
                    break;
                }
                return pid;
            }
        }
    }
}
