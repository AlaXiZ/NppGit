// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
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
using System.IO;
using System.Runtime.InteropServices;

namespace NppKate.Common
{
    public class ResourceManager : IResourceManager, IDisposable
    {
        private const int ToolbarIconSize = 16;
        private readonly Dictionary<string, IntPtr> _resourceCache;
        private readonly List<IntPtr> _hResources;

        public ResourceManager()
        {
            _resourceCache = new Dictionary<string, IntPtr>();
            _hResources = new List<IntPtr>();
            var path = Path.Combine(Npp.NppUtils.PluginDir, Properties.Resources.ExternalResourceDll);
            AddImageLibrary(path);
        }

        public void Dispose()
        {
            foreach(var h in _hResources)
            {
                Marshal.FreeHGlobal(h);
            }
            foreach (var h in _resourceCache.Values)
            {
                Marshal.FreeHGlobal(h);
            }
            _resourceCache.Clear();
        }

        public Icon LoadToolbarIcon(string resourceName)
        {
            return Npp.NppUtils.NppBitmapToIcon(LoadImage(resourceName, ToolbarIconSize, ToolbarIconSize));
        }

        public IntPtr LoadImage(string resourceName, int width, int height)
        {
            if (_resourceCache.ContainsKey(resourceName))
                return _resourceCache[resourceName];

            IntPtr handleLibrary = IntPtr.Zero;
            foreach (var h in _hResources)
            {
                var fHandle = Interop.Win32.FindResource(h, resourceName, Interop.Win32.RT_BITMAP);
                if (fHandle != IntPtr.Zero)
                {
                    handleLibrary = h;
                    break;
                }
            }
            if (handleLibrary != IntPtr.Zero)
            {
                var handle = Interop.Win32.LoadImage(handleLibrary, resourceName, Interop.Win32.IMAGE_BITMAP,
                    width, height, Interop.Win32.LR_LOADMAP3DCOLORS | Interop.Win32.LR_LOADTRANSPARENT);
                _resourceCache.Add(resourceName, handle);
                return handle;
            }
            else
            {
                return IntPtr.Zero;
            }
        }

        public void AddImageLibrary(string path)
        {
            if (File.Exists(path))
            {
                var handle = Interop.Win32.LoadLibraryEx(path, IntPtr.Zero, Interop.LoadLibraryFlags.LOAD_LIBRARY_AS_DATAFILE);
                if (handle == IntPtr.Zero)
                {
                    System.Diagnostics.Debug.WriteLine($"AddImageLibrary::Library {path} not loaded!");
                }
                else
                {
                    _hResources.Add(handle);
                }
            }
        }
    }
}
