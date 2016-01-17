using System;
using System.Runtime.InteropServices;
using NppPlugin.DllExport;

namespace NppGit
{
    class UnmanagedExports
    {
        [DllExport(CallingConvention=CallingConvention.Cdecl)]
        static bool isUnicode()
        {
            return true;
        }

        [DllExport(CallingConvention = CallingConvention.Cdecl)]
        static void setInfo(NppData notepadPlusData)
        {
            PluginUtils.NppData = notepadPlusData;
            AssemblyLoader.Init();

            Plugin.Init();
        }
    
        [DllExport(CallingConvention = CallingConvention.Cdecl)]
        static IntPtr getFuncsArray(ref int nbF)
        {
            nbF = PluginUtils._funcItems.Items.Count;
            return PluginUtils._funcItems.NativePointer;
        }

        [DllExport(CallingConvention = CallingConvention.Cdecl)]
        static uint messageProc(uint Message, IntPtr wParam, IntPtr lParam)
        {
            return 1;
        }

        static IntPtr _ptrPluginName = IntPtr.Zero;
        [DllExport(CallingConvention = CallingConvention.Cdecl)]
        static IntPtr getName()
        {
            if (_ptrPluginName == IntPtr.Zero)
                _ptrPluginName = Marshal.StringToHGlobalUni(Properties.Resources.PluginName);
            return _ptrPluginName;
        }

        [DllExport(CallingConvention = CallingConvention.Cdecl)]
        static void beNotified(IntPtr notifyCode)
        {
            SCNotification nc = (SCNotification)Marshal.PtrToStructure(notifyCode, typeof(SCNotification));
            if (nc.nmhdr.code == (uint)NppMsg.NPPN_TBMODIFICATION)
            {
                PluginUtils._funcItems.RefreshItems();
                Plugin.ToolBarInit();
            }
            else if (nc.nmhdr.code == (uint)NppMsg.NPPN_SHUTDOWN)
            {
                Plugin.PluginCleanUp();
                Marshal.FreeHGlobal(_ptrPluginName);
            }
            else
            {
                Plugin.MessageProc(nc);
            }
        }
    }
}
