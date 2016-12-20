// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
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

using NppKate.Forms;
using NppKate.Npp;
using System;
using System.Drawing;

namespace NppKate
{
    partial class GitStatus : FormDockable
    {
        private const string RS_CURREPO = "Current repo: {0}";
        private const string RS_CURBRANCH = "Current branch: {0}";
        private const string RS_NOREPO = "File \"{0}\" not in git";
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lRepo = new System.Windows.Forms.Label();
            this.lBranch = new System.Windows.Forms.Label();
            this.listView1 = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // lRepo
            // 
            this.lRepo.AutoSize = true;
            this.lRepo.Location = new System.Drawing.Point(12, 9);
            this.lRepo.Name = "lRepo";
            this.lRepo.Size = new System.Drawing.Size(68, 13);
            this.lRepo.TabIndex = 0;
            this.lRepo.Text = "Current repo:";
            // 
            // lBranch
            // 
            this.lBranch.AutoSize = true;
            this.lBranch.Location = new System.Drawing.Point(12, 22);
            this.lBranch.Name = "lBranch";
            this.lBranch.Size = new System.Drawing.Size(80, 13);
            this.lBranch.TabIndex = 1;
            this.lBranch.Text = "Current branch:";
            // 
            // listView1
            // 
            this.listView1.Location = new System.Drawing.Point(12, 142);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(634, 97);
            this.listView1.TabIndex = 2;
            this.listView1.UseCompatibleStateImageBehavior = false;
            // 
            // GitStatus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(658, 441);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.lBranch);
            this.Controls.Add(this.lRepo);
            this.Name = "GitStatus";
            this.Text = "frmMyDlg";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion


        public void ChangeContext()
        {
            var repoDir = NppUtils.GetRootDir(NppUtils.CurrentFileDir);
            if (!string.IsNullOrEmpty(repoDir) && LibGit2Sharp.Repository.IsValid(repoDir))
            {
                using (var repo = new LibGit2Sharp.Repository(repoDir))
                {
                    lRepo.Text = string.Format(RS_CURREPO, new System.IO.DirectoryInfo(repoDir).Name);
                    foreach (var br in repo.Branches)
                    {
                        if (br.IsCurrentRepositoryHead)
                        {
                            lBranch.Text = string.Format(RS_CURBRANCH, br.Name);
                            break;
                        }
                    }
                }
            }
            else
            { 
                lRepo.Text = string.Format(RS_NOREPO, System.IO.Path.GetFileName(NppUtils.CurrentFilePath));
                lBranch.Text = "";
            }
        }
        private System.Windows.Forms.Label lRepo;
        private System.Windows.Forms.Label lBranch;
        private System.Windows.Forms.ListView listView1;

        public string Title
        {
            get
            {
                return "Git Status";
            }
        }

        public Bitmap TabIcon
        {
            get
            {
                return Properties.Resources.Git;
            }
        }
    }
}