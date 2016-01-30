using System;
using System.Windows.Forms;

namespace NppGit.Forms
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
        }

        private void About_Load(object sender, EventArgs e)
        {
            lPluginName.Text = Properties.Resources.PluginName;
            lVersion.Text = this.GetType().Assembly.GetName().Version.ToString();
            tbChangeLog.Text = Properties.Resources.ChangeLog;
        }

        private void llMail_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            llMail.LinkVisited = true; 
            System.Diagnostics.Process.Start("mailto:" + llMail.Text + "?subject=NppGit");
        }

        private void llIssue_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            llIssue.LinkVisited = true;
            System.Diagnostics.Process.Start("https://bitbucket.org/AlaXiZ/nppgitplugin/issues?status=new&status=open");

        }
    }
}
