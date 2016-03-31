/*
Copyright (c) 2015-2016, Schadin Alexey (schadin@gmail.com)
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted 
provided that the following conditions are met:

1. Redistributions of source code must retain the above copyright notice, this list of conditions 
and the following disclaimer.

2. Redistributions in binary form must reproduce the above copyright notice, this list of conditions 
and the following disclaimer in the documentation and/or other materials provided with 
the distribution.

3. Neither the name of the copyright holder nor the names of its contributors may be used to endorse 
or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR 
IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND 
FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR 
CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL 
DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, 
DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER 
IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF 
THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using LibGit2Sharp;
using NLog;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace NppKate.Forms
{
    public partial class Status : Form, FormDockable
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
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
            if (Visible)
            {
                ChangeContext();
            }
        }
    }
}
