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
using System.IO;

namespace NppKate.Common
{
    public class ResourceManager : IDisposable
    {
        private IntPtr _hResourceDll = IntPtr.Zero;

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
