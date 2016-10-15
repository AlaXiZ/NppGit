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
