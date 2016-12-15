// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using System.Drawing;

namespace NppKate.Common
{
    public interface IResourceManager
    {
        Icon LoadToolbarIcon(string resourceName);
        IntPtr LoadImage(string resourceName, int width, int height);
        void AddImageLibrary(string path);
    }
}
