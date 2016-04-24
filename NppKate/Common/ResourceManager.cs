using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NppKate.Common
{
    public class ResourceManager : IDisposable
    {
        private IntPtr _hResourceDll = IntPtr.Zero;

        /*private int GetID()
        {
            Encoding.Default.GetString(BitConverter.GetBytes(rID))
        } */

        public ResourceManager()
        {
            var dllPath = Path.Combine(Npp.NppUtils.PluginDir, Properties.Resources.ExternalResourceDll);
            if (!File.Exists(dllPath))
            {
                throw new Exception(string.Format("File with resources {0} not exists!", dllPath));
            }
            else
            {
                _hResourceDll = Interop.Win32.LoadLibraryEx(dllPath, IntPtr.Zero, Interop.LoadLibraryFlags.LOAD_LIBRARY_AS_DATAFILE);
                if (_hResourceDll == IntPtr.Zero)
                {
                    throw new Exception(string.Format("Library {0} not loaded!", dllPath));
                }
            }
        }

        public void Dispose()
        {
            if (_hResourceDll != IntPtr.Zero)
            {
                Interop.Win32.FreeLibrary(_hResourceDll);
            }
        }

        public IntPtr LoadImage(string resourceName, int width, int height)
        {
            return Interop.Win32.LoadImage(_hResourceDll, resourceName, Interop.Win32.IMAGE_BITMAP, 
                width, height, Interop.Win32.LR_LOADMAP3DCOLORS | Interop.Win32.LR_LOADTRANSPARENT);
        }
    }
}
