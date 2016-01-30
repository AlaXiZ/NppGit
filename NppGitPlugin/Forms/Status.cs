using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NppGit.Forms
{
    public partial class Status : Form, FormDockable
    {
        public Status()
        {
            InitializeComponent();
        }

        public Bitmap TabIcon
        {
            get { return Properties.Resources.Git; }
        }

        public string Title
        {
            get { return "Status"; }
        }

        public void ChangeContext()
        {
            lvFiles.BeginUpdate();
            try
            {
                lvFiles.Items.Clear();
                var root = PluginUtils.GetRootDir(PluginUtils.CurrentFileDir);
                if (root != null && Repository.IsValid(root))
                {
                    using (var repo = new Repository(root))
                    {
                        var _notFound = new List<string>();

                        foreach (var item in repo.RetrieveStatus())
                        {
                            if (item.State != FileStatus.Ignored)
                            {
                                var ext = System.IO.Path.GetExtension(item.FilePath);
                                if (!ilIcons.Images.ContainsKey(ext))
                                {
                                    Icon iconForFile = SystemIcons.WinLogo;
                                    var fullName = System.IO.Path.Combine(root, item.FilePath);
                                    if (System.IO.File.Exists(fullName))
                                    {
                                        iconForFile = Icon.ExtractAssociatedIcon(fullName);
                                        ilIcons.Images.Add(ext, iconForFile);
                                    }
                                    else
                                    {
                                        if (!_notFound.Contains(ext))
                                        {
                                            _notFound.Add(ext);
                                        }
                                    }
                                }
                                var listItem = lvFiles.Items.Add("", 1);
                                listItem.ImageKey = ext;
                                listItem.SubItems.Add(item.FilePath);
                                switch (item.State)
                                {
                                    case FileStatus.Added: listItem.ForeColor = Color.Gray; break;
                                    case FileStatus.Modified: listItem.ForeColor = Color.Red; break;
                                    case FileStatus.Missing:
                                    case FileStatus.Removed: listItem.ForeColor = Color.Silver; break;
                                    case FileStatus.RenamedInIndex:
                                    case FileStatus.RenamedInWorkDir: listItem.ForeColor = Color.Purple; break;
                                    case FileStatus.Staged: listItem.ForeColor = Color.Green; break;
                                    case FileStatus.Untracked: listItem.ForeColor = Color.RoyalBlue; break;
                                    default: listItem.ForeColor = Color.Black; break;
                                }
                                listItem.SubItems.Add(item.State.ToString());
                            }
                        }
                        foreach (var ext in _notFound)
                        {
                            if (!ilIcons.Images.ContainsKey(ext))
                            {
                                ilIcons.Images.Add(ext, SystemIcons.WinLogo);
                            }
                        }
                    }
                }
            }
            finally
            {
                lvFiles.EndUpdate();
            }
            //throw new NotImplementedException();
        }
        
        private void bRefresh_Click(object sender, EventArgs e)
        {
            ChangeContext();
        }

        private void Status_VisibleChanged(object sender, EventArgs e)
        {
            Settings.Panels.FileStatusPanelVisible = Visible;
            if (Visible)
            {
                ChangeContext();
            }
        }
    }
}
