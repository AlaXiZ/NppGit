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
            set
            {
                var buffer = new StringBuilder(value);
                Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_SWITCHTOFILE, 0, buffer);
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

        public static bool OpenFile(string filePath)
        {
            var buf = new StringBuilder(filePath);
           return 1 == (int)Win32.SendMessage(NppData._nppHandle, NppMsg.NPPM_DOOPEN, 0, buf);
        }

        public static void MoveFileToOtherView()
        {
            Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_MENUCOMMAND, 0, NppMenuCmd.IDM_VIEW_GOTO_ANOTHER_VIEW);
        }

        public static int CurrentBufferId
        {
            get
            {
                return (int)Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_GETCURRENTBUFFERID, 0, 0);
            }
        }

        public static void SetCheckedMenu(int cmdId, bool isChecked)
        {
            Win32.SendMessage(PluginUtils.NppHandle, NppMsg.NPPM_SETMENUITEMCHECK, cmdId, isChecked ? 1 : 0);
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

        static public string GetTextBetween(int start, int end = -1)
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
            var result = new StringBuilder(execute(SciMsg.SCI_GETSELTEXT, 0));
            Win32.SendMessage(CurrentScintilla, SciMsg.SCI_GETSELTEXT, 0, result);
            return result.ToString();
        }

        public static void ReplaceSelectedText(string text)
        {
            StringBuilder buf = new StringBuilder("");
            execute(SciMsg.SCI_BEGINUNDOACTION, 0);
            if (execute(SciMsg.SCI_GETSELECTIONMODE, 0) > 0)
            {
                var startPos = execute(SciMsg.SCI_GETSELECTIONNSTART, 0);
                var line = execute(SciMsg.SCI_LINEFROMPOSITION, startPos);
                Win32.SendMessage(CurrentScintilla, SciMsg.SCI_REPLACESEL, buf.Length, buf);
                foreach (var str in text.Replace(GetEOL(), "\n").Split('\n'))
                {
                    var linePos = execute(SciMsg.SCI_GETLINEENDPOSITION, line);
                    line++;
                    buf.Clear();
                    buf.Append(str);
                    Win32.SendMessage(CurrentScintilla, SciMsg.SCI_INSERTTEXT, linePos, buf);
                }
            } else
            {
                Win32.SendMessage(CurrentScintilla, SciMsg.SCI_REPLACESEL, buf.Length, buf);
                foreach (var str in text.Replace(GetEOL(), "\n").Split('\n'))
                {
                    buf.Clear();
                    buf.Append(str);
                    Win32.SendMessage(CurrentScintilla, SciMsg.SCI_ADDTEXT, buf.Length, buf);
                    NewLine();
                }
            }
            execute(SciMsg.SCI_ENDUNDOACTION, 0);
        }
        #endregion

        static int execute(SciMsg msg, int wParam, int lParam = 0)
        {
            IntPtr sci = CurrentScintilla;
            return (int)Win32.SendMessage(sci, msg, wParam, lParam);
        }

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
                Win32.SendMessage(nppData._nppHandle, (int)WinMsg.WM_GETTEXT, Win32.MAX_PATH, title);
                return title.ToString();
            }
            set
            {   
                var title = new StringBuilder(value);
                Win32.SendMessage(nppData._nppHandle, (int)WinMsg.WM_SETTEXT, (int)WinMsg.WM_SETTEXT, title);
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
