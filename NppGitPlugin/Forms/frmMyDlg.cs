using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace NppGitPlugin
{
    public partial class frmMyDlg : Form
    {
        public frmMyDlg()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Button say!");
        }
    }
}
