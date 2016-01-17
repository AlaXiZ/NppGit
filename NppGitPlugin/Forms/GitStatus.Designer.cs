using System;
using System.Drawing;

namespace NppGit
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
            // GitStatus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(371, 441);
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
            var repoDir = PluginUtils.GetRootDir(PluginUtils.CurrentFileDir);
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
                lRepo.Text = string.Format(RS_NOREPO, System.IO.Path.GetFileName(PluginUtils.CurrentFilePath));
                lBranch.Text = "";
            }
        }
        private System.Windows.Forms.Label lRepo;
        private System.Windows.Forms.Label lBranch;

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