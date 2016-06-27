using System;
using System.Windows.Forms;

namespace NppKate.Common
{
    public class SimpleDialog : Form
    {
        protected IDockableManager _manager;

        public void Init(IDockableManager manager)
        {
            _manager = manager;
            AfterInit();
        }

        protected virtual void AfterInit() { }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (Visible)
            {
                Npp.NppUtils.RegisterAsDialog(Handle);
            }
            else
            {
                Npp.NppUtils.UnregisterAsDialog(Handle);
            }
        }
    }
}
