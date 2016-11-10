using System.Drawing;
using System.IO;
using NppKate.Common;
using NppKate.Forms;

namespace NppKate.Modules.ConsoleLog
{
    public partial class LogForm : DockDialog, FormDockable
    {
        private TextWriter _newWriter;
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
