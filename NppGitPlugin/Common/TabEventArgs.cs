using System;

namespace NppGit.Common
{
    public class TabEventArgs : EventArgs
    {
        public uint TabId { get; protected set; }
        public TabEventArgs(uint tabId)
        {
            TabId = tabId;
        }
    }
}
