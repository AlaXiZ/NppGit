using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using NppKate.Interop;
using NppKate.Npp;

namespace NppKate.Common
{
    public class DockDialog : Form, IDockDialog
    {
        protected IDockableManager _manager;
        protected int _cmdId;

        public void init(IDockableManager manager, int commandId)
        {
            _manager = manager;
            _cmdId = commandId;
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == (int)WinMsg.WM_NOTIFY)
            {
                tagNMHDR ntf = (tagNMHDR)Marshal.PtrToStructure(m.LParam, typeof(tagNMHDR));
                if (ntf.code == (uint)DockMgrMsg.DMN_CLOSE)
                {
                    Win32.SendMessage(NppInfo.Instance.NppHandle, (int)WinMsg.WM_COMMAND, _cmdId, 0);
                    return;
                }
            }
            base.WndProc(ref m);
        }
    }
}
