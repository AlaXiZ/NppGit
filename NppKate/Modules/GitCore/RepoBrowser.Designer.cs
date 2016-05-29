namespace NppKate.Modules.GitCore
{
    partial class RepoBrowser
    {
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RepoBrowser));
            this.tvRepositories = new System.Windows.Forms.TreeView();
            this.ilImages = new System.Windows.Forms.ImageList(this.components);
            this.cmRepositories = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miSetActive = new System.Windows.Forms.ToolStripMenuItem();
            this.miAddRepo = new System.Windows.Forms.ToolStripMenuItem();
            this.miRemoveRepo = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.fetchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pullToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.commitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pushToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.syncToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmBranch = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miSwitchTo = new System.Windows.Forms.ToolStripMenuItem();
            this.cmRepositories.SuspendLayout();
            this.cmBranch.SuspendLayout();
            this.SuspendLayout();
            // 
            // tvRepositories
            // 
            this.tvRepositories.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvRepositories.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tvRepositories.ImageIndex = 0;
            this.tvRepositories.ImageList = this.ilImages;
            this.tvRepositories.Location = new System.Drawing.Point(0, 0);
            this.tvRepositories.Name = "tvRepositories";
            this.tvRepositories.SelectedImageIndex = 0;
            this.tvRepositories.Size = new System.Drawing.Size(284, 353);
            this.tvRepositories.TabIndex = 0;
            this.tvRepositories.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvRepositories_BeforeCollapse);
            this.tvRepositories.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvRepositories_BeforeExpand);
            this.tvRepositories.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.tvRepositories_AfterExpand);
            this.tvRepositories.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvRepositories_NodeMouseClick);
            this.tvRepositories.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvRepositories_NodeMouseDoubleClick);
            this.tvRepositories.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tvRepositories_MouseDown);
            // 
            // ilImages
            // 
            this.ilImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilImages.ImageStream")));
            this.ilImages.TransparentColor = System.Drawing.Color.Transparent;
            this.ilImages.Images.SetKeyName(0, "REPO");
            this.ilImages.Images.SetKeyName(1, "EMPTY");
            this.ilImages.Images.SetKeyName(2, "BRANCH");
            this.ilImages.Images.SetKeyName(3, "REMOTE_BRANCH");
            this.ilImages.Images.SetKeyName(4, "BRANCH_FOLDER");
            this.ilImages.Images.SetKeyName(5, "CURRENT_BRANCH");
            this.ilImages.Images.SetKeyName(6, "LOADING");
            // 
            // cmRepositories
            // 
            this.cmRepositories.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miSetActive,
            this.miAddRepo,
            this.miRemoveRepo,
            this.toolStripMenuItem1,
            this.fetchToolStripMenuItem,
            this.pullToolStripMenuItem,
            this.commitToolStripMenuItem,
            this.pushToolStripMenuItem,
            this.syncToolStripMenuItem});
            this.cmRepositories.Name = "cmRepositories";
            this.cmRepositories.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.cmRepositories.Size = new System.Drawing.Size(174, 186);
            this.cmRepositories.Opening += new System.ComponentModel.CancelEventHandler(this.cmRepositories_Opening);
            // 
            // miSetActive
            // 
            this.miSetActive.Image = global::NppKate.Properties.Resources.database__arrow;
            this.miSetActive.Name = "miSetActive";
            this.miSetActive.Size = new System.Drawing.Size(173, 22);
            this.miSetActive.Text = "Set active";
            this.miSetActive.Click += new System.EventHandler(this.miSetActive_Click);
            // 
            // miAddRepo
            // 
            this.miAddRepo.Image = global::NppKate.Properties.Resources.database__plus;
            this.miAddRepo.Name = "miAddRepo";
            this.miAddRepo.Size = new System.Drawing.Size(173, 22);
            this.miAddRepo.Text = "Add repository";
            this.miAddRepo.Click += new System.EventHandler(this.miAddRepo_Click);
            // 
            // miRemoveRepo
            // 
            this.miRemoveRepo.Image = global::NppKate.Properties.Resources.database__minus;
            this.miRemoveRepo.Name = "miRemoveRepo";
            this.miRemoveRepo.Size = new System.Drawing.Size(173, 22);
            this.miRemoveRepo.Text = "Remove repository";
            this.miRemoveRepo.Click += new System.EventHandler(this.miRemoveRepo_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(170, 6);
            this.toolStripMenuItem1.Visible = false;
            // 
            // fetchToolStripMenuItem
            // 
            this.fetchToolStripMenuItem.Enabled = false;
            this.fetchToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("fetchToolStripMenuItem.Image")));
            this.fetchToolStripMenuItem.Name = "fetchToolStripMenuItem";
            this.fetchToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.fetchToolStripMenuItem.Text = "Fetch";
            this.fetchToolStripMenuItem.Visible = false;
            // 
            // pullToolStripMenuItem
            // 
            this.pullToolStripMenuItem.Enabled = false;
            this.pullToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("pullToolStripMenuItem.Image")));
            this.pullToolStripMenuItem.Name = "pullToolStripMenuItem";
            this.pullToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.pullToolStripMenuItem.Text = "Pull";
            this.pullToolStripMenuItem.Visible = false;
            // 
            // commitToolStripMenuItem
            // 
            this.commitToolStripMenuItem.Enabled = false;
            this.commitToolStripMenuItem.Image = global::NppKate.Properties.Resources.commit;
            this.commitToolStripMenuItem.Name = "commitToolStripMenuItem";
            this.commitToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.commitToolStripMenuItem.Text = "Commit";
            this.commitToolStripMenuItem.Visible = false;
            // 
            // pushToolStripMenuItem
            // 
            this.pushToolStripMenuItem.Enabled = false;
            this.pushToolStripMenuItem.Image = global::NppKate.Properties.Resources.push;
            this.pushToolStripMenuItem.Name = "pushToolStripMenuItem";
            this.pushToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.pushToolStripMenuItem.Text = "Push";
            this.pushToolStripMenuItem.Visible = false;
            // 
            // syncToolStripMenuItem
            // 
            this.syncToolStripMenuItem.Enabled = false;
            this.syncToolStripMenuItem.Name = "syncToolStripMenuItem";
            this.syncToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.syncToolStripMenuItem.Text = "Sync";
            this.syncToolStripMenuItem.Visible = false;
            // 
            // cmBranch
            // 
            this.cmBranch.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miSwitchTo});
            this.cmBranch.Name = "cmBranch";
            this.cmBranch.Size = new System.Drawing.Size(124, 26);
            // 
            // miSwitchTo
            // 
            this.miSwitchTo.Enabled = false;
            this.miSwitchTo.Image = global::NppKate.Properties.Resources.checkout;
            this.miSwitchTo.Name = "miSwitchTo";
            this.miSwitchTo.Size = new System.Drawing.Size(123, 22);
            this.miSwitchTo.Text = "Switch to";
            // 
            // RepoBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 353);
            this.Controls.Add(this.tvRepositories);
            this.Name = "RepoBrowser";
            this.Text = "RepoBrowser";
            this.VisibleChanged += new System.EventHandler(this.RepoBrowser_VisibleChanged);
            this.cmRepositories.ResumeLayout(false);
            this.cmBranch.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView tvRepositories;
        private System.Windows.Forms.ImageList ilImages;
        private System.Windows.Forms.ContextMenuStrip cmRepositories;
        private System.Windows.Forms.ToolStripMenuItem miSetActive;
        private System.Windows.Forms.ToolStripMenuItem miAddRepo;
        private System.Windows.Forms.ToolStripMenuItem miRemoveRepo;
        private System.Windows.Forms.ContextMenuStrip cmBranch;
        private System.Windows.Forms.ToolStripMenuItem miSwitchTo;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem fetchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pullToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem commitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pushToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem syncToolStripMenuItem;
    }
}