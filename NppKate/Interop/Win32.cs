/*
Copyright (c) 2015-2016, Schadin Alexey (schadin@gmail.com)
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted 
provided that the following conditions are met:

1. Redistributions of source code must retain the above copyright notice, this list of conditions 
and the following disclaimer.

2. Redistributions in binary form must reproduce the above copyright notice, this list of conditions 
and the following disclaimer in the documentation and/or other materials provided with 
the distribution.

3. Neither the name of the copyright holder nor the names of its contributors may be used to endorse 
or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR 
IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND 
FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR 
CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL 
DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, 
DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER 
IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF 
THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace NppKate.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    public struct CWPSTRUCT
    {
        public IntPtr lParam;
        public IntPtr wParam;
        public uint message;
        public IntPtr hwnd;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CWPRETSTRUCT
    {
        public IntPtr lResult;
        public IntPtr lParam;
        public IntPtr wParam;
        public uint message;
        public IntPtr hwnd;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MSG
    {
        public IntPtr hwnd;
        public UInt32 message;
        public IntPtr wParam;
        public IntPtr lParam;
        public UInt32 time;
        public POINT pt;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct HWND
    {
        public IntPtr h;

        public static HWND Cast(IntPtr h)
        {
            HWND hTemp = new HWND();
            hTemp.h = h;
            return hTemp;
        }

        public static implicit operator IntPtr(HWND h)
        {
            return h.h;
        }

        public static HWND NULL
        {
            get
            {
                HWND hTemp = new HWND();
                hTemp.h = IntPtr.Zero;
                return hTemp;
            }
        }

        public static bool operator ==(HWND hl, HWND hr)
        {
            return hl.h == hr.h;
        }

        public static bool operator !=(HWND hl, HWND hr)
        {
            return hl.h != hr.h;
        }

        override public bool Equals(object oCompare)
        {
            HWND hr = Cast((HWND)oCompare);
            return h == hr.h;
        }

        public override int GetHashCode()
        {
            return (int)h;
        }
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;

        public RECT(int left, int top, int right, int bottom)
        {
            this.left = left;
            this.top = top;
            this.right = right;
            this.bottom = bottom;
        }

        public bool IsEmpty
        {
            get
            {
                return left >= right || top >= bottom;
            }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int x;
        public int y;

        public POINT(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    public enum WinEvent : uint
    {
        EVENT_MIN = 0,
        EVENT_MAX = 0x7FFFFFFF,

        EVENT_SYSTEM_MENUSTART = 0x0004,
        EVENT_SYSTEM_MENUEND = 0x0005,
        EVENT_SYSTEM_MENUPOPUPSTART = 0x0006,
        EVENT_SYSTEM_MENUPOPUPEND = 0x0007,
        EVENT_SYSTEM_CAPTURESTART = 0x0008,
        EVENT_SYSTEM_CAPTUREEND = 0x0009,
        EVENT_SYSTEM_SWITCHSTART = 0x0014,
        EVENT_SYSTEM_SWITCHEND = 0x0015,

        EVENT_OBJECT_CREATE = 0x8000,
        EVENT_OBJECT_DESTROY = 0x8001,
        EVENT_OBJECT_SHOW = 0x8002,
        EVENT_OBJECT_HIDE = 0x8003,
        EVENT_OBJECT_FOCUS = 0x8005,
        EVENT_OBJECT_STATECHANGE = 0x800A,
        EVENT_OBJECT_LOCATIONCHANGE = 0x800B
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MENUINFO
    {
        public int cbSize;
        public int fMask;
        public int dwStyle;
        public int cyMax;
        public IntPtr hbrBack;
        public int dwContextHelpID;
        public int dwMenuData;
    }

    public enum WinEventContext : uint
    {
        WINEVENT_OUTOFCONTEXT = 0x0000,
        WINEVENT_SKIPOWNTHREAD = 0x0001,
        WINEVENT_SKIPOWNPROCESS = 0x0002,
        WINEVENT_INCONTEXT = 0x0004
    }

    public delegate void WinEventDelegate(IntPtr hWinEventHook, WinEvent eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

    [System.Flags]
    public enum LoadLibraryFlags : uint
    {
        DONT_RESOLVE_DLL_REFERENCES = 0x00000001,
        LOAD_IGNORE_CODE_AUTHZ_LEVEL = 0x00000010,
        LOAD_LIBRARY_AS_DATAFILE = 0x00000002,
        LOAD_LIBRARY_AS_DATAFILE_EXCLUSIVE = 0x00000040,
        LOAD_LIBRARY_AS_IMAGE_RESOURCE = 0x00000020,
        LOAD_WITH_ALTERED_SEARCH_PATH = 0x00000008
    }

    [System.Flags]
    public enum ExtendedNameFormat : int
    {
        /// <summary>
        /// An unknown name type.
        /// </summary>
        NameUnknown = 0,

        /// <summary>
        /// The fully qualified distinguished name
        /// (for example, CN=Jeff Smith,OU=Users,DC=Engineering,DC=Microsoft,DC=Com).
        /// </summary>
        NameFullyQualifiedDN = 1,

        /// <summary>
        /// A legacy account name (for example, Engineering\JSmith).
        /// The domain-only version includes trailing backslashes (\\).
        /// </summary>
        NameSamCompatible = 2,

        /// <summary>
        /// A "friendly" display name (for example, Jeff Smith).
        /// The display name is not necessarily the defining relative distinguished name (RDN).
        /// </summary>
        NameDisplay = 3,

        /// <summary>
        /// A GUID string that the IIDFromString function returns
        /// (for example, {4fa050f0-f561-11cf-bdd9-00aa003a77b6}).
        /// </summary>
        NameUniqueId = 6,

        /// <summary>
        /// The complete canonical name (for example, engineering.microsoft.com/software/someone).
        /// The domain-only version includes a trailing forward slash (/).
        /// </summary>
        NameCanonical = 7,

        /// <summary>
        /// The user principal name (for example, someone@example.com).
        /// </summary>
        NameUserPrincipal = 8,

        /// <summary>
        /// The same as NameCanonical except that the rightmost forward slash (/)
        /// is replaced with a new line character (\n), even in a domain-only case
        /// (for example, engineering.microsoft.com/software\nJSmith).
        /// </summary>
        NameCanonicalEx = 9,

        /// <summary>
        /// The generalized service principal name
        /// (for example, www/www.microsoft.com@microsoft.com).
        /// </summary>
        NameServicePrincipal = 10,

        /// <summary>
        /// The DNS domain name followed by a backward-slash and the SAM user name.
        /// </summary>
        NameDnsDomain = 12
    }

    public partial class Win32
    {
        [DllImport("user32.dll")]
        static public extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        public static IntPtr SendMenuCmd(IntPtr hWnd, NppMenuCmd wParam, int lParam)
        {
            return Win32.SendMessage(hWnd, (int)WinMsg.WM_COMMAND, (int)wParam, lParam);
        }

        [DllImport("user32")]
        public static extern IntPtr SendMessage(IntPtr hWnd, NppMsg Msg, int wParam, NppMenuCmd lParam);

        [DllImport("user32")]
        public static extern IntPtr SendMessage(IntPtr hWnd, NppMsg Msg, int wParam, IntPtr lParam);

        [DllImport("user32")]
        public static extern IntPtr SendMessage(IntPtr hWnd, NppMsg Msg, int wParam, int lParam);

        [DllImport("user32")]
        public static extern IntPtr SendMessage(IntPtr hWnd, NppMsg Msg, int wParam, out int lParam);

        [DllImport("user32")]
        public static extern IntPtr SendMessage(IntPtr hWnd, NppMsg Msg, IntPtr wParam, int lParam);

        [DllImport("user32")]
        public static extern IntPtr SendMessage(IntPtr hWnd, NppMsg Msg, int wParam, ref LangType lParam);

        [DllImport("user32")]
        public static extern IntPtr SendMessage(IntPtr hWnd, NppMsg Msg, int wParam, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder lParam);

        [DllImport("user32")]
        public static extern IntPtr SendMessage(IntPtr hWnd, NppMsg Msg, int wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);

        [DllImport("user32")]
        public static extern IntPtr SendMessage(IntPtr hWnd, NppMsg Msg, IntPtr wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);

        [DllImport("user32")]
        public static extern IntPtr SendMessage(IntPtr hWnd, NppMsg Msg, IntPtr wParam, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder lParam);

        public static IntPtr SendMessage(IntPtr hWnd, NppMsg Msg, int wParam, out string lParam)
        {
            var text = new StringBuilder(Win32.MAX_PATH);
            IntPtr retval = Win32.SendMessage(hWnd, Msg, 0, text);
            lParam = text.ToString();
            return retval;
        }

        public static IntPtr SendMessage(IntPtr hWnd, NppMsg Msg, int wParam, out List<string> lParam)
        {
            lParam = new List<string>();

            using (var cStrArray = new ClikeStringArray(wParam, Win32.MAX_PATH))
            {
                if (Win32.SendMessage(hWnd, Msg, cStrArray.NativePointer, wParam) != IntPtr.Zero)
                    foreach (string item in cStrArray.ManagedStringsUnicode)
                        lParam.Add(item);
            }

            return IntPtr.Zero;
        }

        [DllImport("user32")]
        public static extern IntPtr SendMessage(IntPtr hWnd, SciMsg Msg, int wParam, IntPtr lParam);

        [DllImport("user32")]
        public static extern IntPtr SendMessage(IntPtr hWnd, SciMsg Msg, int wParam, string lParam);

        [DllImport("user32")]
        public static extern IntPtr SendMessage(IntPtr hWnd, SciMsg Msg, int wParam, [MarshalAs(UnmanagedType.LPStr)] StringBuilder lParam);

        [DllImport("user32")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, [MarshalAs(UnmanagedType.LPStr)] StringBuilder lParam);

        [DllImport("user32")]
        public static extern IntPtr SendMessage(IntPtr hWnd, SciMsg Msg, int wParam, int lParam);

        public static IntPtr SendMessage(IntPtr hWnd, SciMsg Msg, int wParam, out string lParam)
        {
            var text = new StringBuilder(Win32.MAX_PATH);
            IntPtr retval = Win32.SendMessage(hWnd, Msg, 0, text);
            lParam = text.ToString();
            return retval;
        }

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool PostMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);


        [DllImport("Shell32.dll")]
        public extern static int ExtractIconEx(string libName, int iconIndex, IntPtr[] largeIcon, IntPtr[] smallIcon, int nIcons);

        public static Icon ExtractIcon(string file, int index, bool small = true)
        {
            IntPtr[] icons = new IntPtr[index + 1];
            if (small)
                ExtractIconEx(file, 0, null, icons, icons.Length);
            else
                ExtractIconEx(file, 0, icons, null, icons.Length);

            return Icon.FromHandle(icons[index]);
        }

        public const int MAX_PATH = 260;

        [DllImport("kernel32")]
        public static extern int GetPrivateProfileInt(string lpAppName, string lpKeyName, int nDefault, string lpFileName);

        [DllImport("kernel32")]
        public static extern bool WritePrivateProfileString(string lpAppName, string lpKeyName, string lpString, string lpFileName);

        public const int MF_BYCOMMAND = 0;
        public const int MF_BYPOSITION = 0x00000400;
        public const int MF_CHECKED = 8;
        public const int MF_UNCHECKED = 0;

        [DllImport("user32")]
        public static extern IntPtr GetMenu(IntPtr hWnd);

        [DllImport("user32")]
        public static extern int CheckMenuItem(IntPtr hmenu, int uIDCheckItem, int uCheck);

        public const int WM_CREATE = 1;

        [DllImport("user32")]
        public static extern bool ClientToScreen(IntPtr hWnd, ref Point lpPoint);

        [DllImport("kernel32")]
        public static extern void OutputDebugString(string lpOutputString);


        [DllImport("user32.dll")]
        public static extern IntPtr SetWinEventHook(WinEvent eventMin, WinEvent eventMax, IntPtr hmodWinEventProc, WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);

        [DllImport("user32.dll")]
        public static extern bool UnhookWinEvent(IntPtr hWinEventHook);

        [DllImport("user32.dll")]
        public static extern int GetMenuString(IntPtr hMenu, uint uIDItem, [Out, MarshalAs(UnmanagedType.LPStr)] StringBuilder lpString, int nMaxCount, uint uFlag);

        [DllImport("kernel32.dll")]
        public static extern IntPtr LoadLibrary(string pathToDll);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr LoadLibraryEx(string lpFileName, IntPtr hReservedNull, LoadLibraryFlags dwFlags);


        [DllImport("kernel32.dll")]
        public static extern bool FreeLibrary(IntPtr hModule);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr LoadImage(IntPtr hInstance, string lpszName, uint uType, int cxDesired, int cyDesired, uint fuLoad);
        // uType
        public const uint IMAGE_BITMAP = 0; // Loads a bitmap.
        public const uint IMAGE_CURSOR = 2; // Loads a cursor.
        public const uint IMAGE_ICON = 1; // Loads an icon.
        // fuLoad
        public const uint LR_CREATEDIBSECTION = 0x00002000; 
        public const uint LR_DEFAULTCOLOR = 0x00000000;
        public const uint LR_DEFAULTSIZE = 0x00000040;
        public const uint LR_LOADFROMFILE = 0x00000010;
        public const uint LR_LOADMAP3DCOLORS = 0x00001000;
        public const uint LR_LOADTRANSPARENT = 0x00000020;
        public const uint LR_MONOCHROME = 0x00000001;
        public const uint LR_SHARED = 0x00008000;
        public const uint LR_VGACOLOR = 0x00000080;

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("secur32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetUserNameEx(ExtendedNameFormat nameFormat, StringBuilder userName, ref int userNameSize);

        public static string GetUserNameEx(ExtendedNameFormat nameFormat)
        {
            var buf = new StringBuilder(MAX_PATH);
            int size = buf.Capacity;
            if (GetUserNameEx(nameFormat, buf, ref size) > 0)
            {
                buf.Capacity = size;
                return buf.ToString();
            }
            else
            {
                return "";
            }
        }
    }
}
