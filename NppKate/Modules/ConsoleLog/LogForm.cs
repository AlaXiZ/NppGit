using System.Drawing;
using NppKate.Common;
using NppKate.Forms;

namespace NppKate.Modules.ConsoleLog
{
    public partial class LogForm : DockDialog, FormDockable
    {
        public LogForm()
        {
            InitializeComponent();
            Text = Title;
        }

        public Bitmap TabIcon => null;

        public string Title => Properties.Resources.CmdConsoleLog;

        public void ChangeContext() { }
    }
}
