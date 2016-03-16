using System;

namespace NppGit.Common
{
    public class MessageEventArgs : EventArgs
    {
        public MessageEventArgs(uint message, int lParam, int wParam)
        {
            Message = message;
            LParam = lParam;
            WParam = wParam;
        }

        public uint Message { get; set; }
        public int LParam { get; set; }
        public int WParam { get; set; }
    }
}
