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
            this.tortoiseGitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmTortoiseGit = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.fetchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pullToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.commitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pushToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.syncToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showLogFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showReflogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stashSaveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stashPopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stashListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.repoBrowserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkForModificationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.browseReferenceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.switchCheckoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.blameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mergeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmBranch = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miSwitchTo = new System.Windows.Forms.ToolStripMenuItem();
            this.cmRepositories.SuspendLayout();
            this.cmTortoiseGit.SuspendLayout();
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
            this.tortoiseGitToolStripMenuItem});
            this.cmRepositories.Name = "cmRepositories";
            this.cmRepositories.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.cmRepositories.Size = new System.Drawing.Size(174, 98);
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
            // 
            // tortoiseGitToolStripMenuItem
            // 
            this.tortoiseGitToolStripMenuItem.DropDown = this.cmTortoiseGit;
            this.tortoiseGitToolStripMenuItem.Image = global::NppKate.Properties.Resources.Tortoise;
            this.tortoiseGitToolStripMenuItem.Name = "tortoiseGitToolStripMenuItem";
            this.tortoiseGitToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.tortoiseGitToolStripMenuItem.Text = "Tortoise Git";
            this.tortoiseGitToolStripMenuItem.Visible = false;
            // 
            // cmTortoiseGit
            // 
            this.cmTortoiseGit.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fetchToolStripMenuItem,
            this.pullToolStripMenuItem,
            this.commitToolStripMenuItem,
            this.pushToolStripMenuItem,
            this.syncToolStripMenuItem,
            this.showLogToolStripMenuItem,
            this.showLogFileToolStripMenuItem,
            this.showReflogToolStripMenuItem,
            this.stashSaveToolStripMenuItem,
            this.stashPopToolStripMenuItem,
            this.stashListToolStripMenuItem,
            this.repoBrowserToolStripMenuItem,
            this.checkForModificationToolStripMenuItem,
            this.browseReferenceToolStripMenuItem,
            this.switchCheckoutToolStripMenuItem,
            this.blameToolStripMenuItem,
            this.exportToolStripMenuItem,
            this.mergeToolStripMenuItem,
            this.settingsToolStripMenuItem});
            this.cmTortoiseGit.Name = "cmTortoiseGit";
            this.cmTortoiseGit.Size = new System.Drawing.Size(197, 444);
            // 
            // fetchToolStripMenuItem
            // 
            this.fetchToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("fetchToolStripMenuItem.Image")));
            this.fetchToolStripMenuItem.Name = "fetchToolStripMenuItem";
            this.fetchToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.fetchToolStripMenuItem.Text = "Fetch";
            this.fetchToolStripMenuItem.Click += new System.EventHandler(this.fetchToolStripMenuItem_Click);
            // 
            // pullToolStripMenuItem
            // 
            this.pullToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("pullToolStripMenuItem.Image")));
            this.pullToolStripMenuItem.Name = "pullToolStripMenuItem";
            this.pullToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.pullToolStripMenuItem.Text = "Pull";
            this.pullToolStripMenuItem.Click += new System.EventHandler(this.pullToolStripMenuItem_Click);
            // 
            // commitToolStripMenuItem
            // 
            this.commitToolStripMenuItem.Image = global::NppKate.Properties.Resources.commit;
            this.commitToolStripMenuItem.Name = "commitToolStripMenuItem";
            this.commitToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.commitToolStripMenuItem.Text = "Commit";
            this.commitToolStripMenuItem.Click += new System.EventHandler(this.commitToolStripMenuItem_Click);
            // 
            // pushToolStripMenuItem
            // 
            this.pushToolStripMenuItem.Image = global::NppKate.Properties.Resources.push;
            this.pushToolStripMenuItem.Name = "pushToolStripMenuItem";
            this.pushToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.pushToolStripMenuItem.Text = "Push";
            this.pushToolStripMenuItem.Click += new System.EventHandler(this.pushToolStripMenuItem_Click);
            // 
            // syncToolStripMenuItem
            // 
            this.syncToolStripMenuItem.Image = global::NppKate.Properties.Resources.menurelocate;
            this.syncToolStripMenuItem.Name = "syncToolStripMenuItem";
            this.syncToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.syncToolStripMenuItem.Text = "Sync";
            this.syncToolStripMenuItem.Click += new System.EventHandler(this.syncToolStripMenuItem_Click);
            // 
            // showLogToolStripMenuItem
            // 
            this.showLogToolStripMenuItem.Image = global::NppKate.Properties.Resources.log;
            this.showLogToolStripMenuItem.Name = "showLogToolStripMenuItem";
            this.showLogToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.showLogToolStripMenuItem.Text = "Show log";
            this.showLogToolStripMenuItem.Click += new System.EventHandler(this.showLogToolStripMenuItem_Click);
            // 
            // showLogFileToolStripMenuItem
            // 
            this.showLogFileToolStripMenuItem.Image = global::NppKate.Properties.Resources.log;
            this.showLogFileToolStripMenuItem.Name = "showLogFileToolStripMenuItem";
            this.showLogFileToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.showLogFileToolStripMenuItem.Text = "Show log file";
            this.showLogFileToolStripMenuItem.Click += new System.EventHandler(this.showLogFileToolStripMenuItem_Click);
            // 
            // showReflogToolStripMenuItem
            // 
            this.showReflogToolStripMenuItem.Image = global::NppKate.Properties.Resources.log;
            this.showReflogToolStripMenuItem.Name = "showReflogToolStripMenuItem";
            this.showReflogToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.showReflogToolStripMenuItem.Text = "Show Reflog";
            this.showReflogToolStripMenuItem.Click += new System.EventHandler(this.showReflogToolStripMenuItem_Click);
            // 
            // stashSaveToolStripMenuItem
            // 
            this.stashSaveToolStripMenuItem.Image = global::NppKate.Properties.Resources.stashsave;
            this.stashSaveToolStripMenuItem.Name = "stashSaveToolStripMenuItem";
            this.stashSaveToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.stashSaveToolStripMenuItem.Text = "Stash Save";
            this.stashSaveToolStripMenuItem.Click += new System.EventHandler(this.stashSaveToolStripMenuItem_Click);
            // 
            // stashPopToolStripMenuItem
            // 
            this.stashPopToolStripMenuItem.Image = global::NppKate.Properties.Resources.stashpop;
            this.stashPopToolStripMenuItem.Name = "stashPopToolStripMenuItem";
            this.stashPopToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.stashPopToolStripMenuItem.Text = "Stash Pop";
            this.stashPopToolStripMenuItem.Click += new System.EventHandler(this.stashPopToolStripMenuItem_Click);
            // 
            // stashListToolStripMenuItem
            // 
            this.stashListToolStripMenuItem.Enabled = false;
            this.stashListToolStripMenuItem.Image = global::NppKate.Properties.Resources.log;
            this.stashListToolStripMenuItem.Name = "stashListToolStripMenuItem";
            this.stashListToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.stashListToolStripMenuItem.Text = "Stash List";
            this.stashListToolStripMenuItem.Visible = false;
            this.stashListToolStripMenuItem.Click += new System.EventHandler(this.stashListToolStripMenuItem_Click);
            // 
            // repoBrowserToolStripMenuItem
            // 
            this.repoBrowserToolStripMenuItem.Image = global::NppKate.Properties.Resources.repo;
            this.repoBrowserToolStripMenuItem.Name = "repoBrowserToolStripMenuItem";
            this.repoBrowserToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.repoBrowserToolStripMenuItem.Text = "Repo browser";
            this.repoBrowserToolStripMenuItem.Click += new System.EventHandler(this.repoBrowserToolStripMenuItem_Click);
            // 
            // checkForModificationToolStripMenuItem
            // 
            this.checkForModificationToolStripMenuItem.Image = global::NppKate.Properties.Resources.menushowchanged;
            this.checkForModificationToolStripMenuItem.Name = "checkForModificationToolStripMenuItem";
            this.checkForModificationToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.checkForModificationToolStripMenuItem.Text = "Check for modification";
            this.checkForModificationToolStripMenuItem.Click += new System.EventHandler(this.checkForModificationToolStripMenuItem_Click);
            // 
            // browseReferenceToolStripMenuItem
            // 
            this.browseReferenceToolStripMenuItem.Image = global::NppKate.Properties.Resources.repo;
            this.browseReferenceToolStripMenuItem.Name = "browseReferenceToolStripMenuItem";
            this.browseReferenceToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.browseReferenceToolStripMenuItem.Text = "Browse Reference";
            this.browseReferenceToolStripMenuItem.Click += new System.EventHandler(this.browseReferenceToolStripMenuItem_Click);
            // 
            // switchCheckoutToolStripMenuItem
            // 
            this.switchCheckoutToolStripMenuItem.Image = global::NppKate.Properties.Resources.menuswitch;
            this.switchCheckoutToolStripMenuItem.Name = "switchCheckoutToolStripMenuItem";
            this.switchCheckoutToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.switchCheckoutToolStripMenuItem.Text = "Switch/Checkout";
            this.switchCheckoutToolStripMenuItem.Click += new System.EventHandler(this.switchCheckoutToolStripMenuItem_Click);
            // 
            // blameToolStripMenuItem
            // 
            this.blameToolStripMenuItem.Image = global::NppKate.Properties.Resources.blame;
            this.blameToolStripMenuItem.Name = "blameToolStripMenuItem";
            this.blameToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.blameToolStripMenuItem.Text = "Blame";
            this.blameToolStripMenuItem.Click += new System.EventHandler(this.blameToolStripMenuItem_Click);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Image = global::NppKate.Properties.Resources.menuexport;
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.exportToolStripMenuItem.Text = "Export...";
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
            // 
            // mergeToolStripMenuItem
            // 
            this.mergeToolStripMenuItem.Image = global::NppKate.Properties.Resources.menumerge;
            this.mergeToolStripMenuItem.Name = "mergeToolStripMenuItem";
            this.mergeToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.mergeToolStripMenuItem.Text = "Merge";
            this.mergeToolStripMenuItem.Click += new System.EventHandler(this.mergeToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Image = global::NppKate.Properties.Resources.menusettings;
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
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
            this.Text = "Repositories";
            this.VisibleChanged += new System.EventHandler(this.RepoBrowser_VisibleChanged);
            this.cmRepositories.ResumeLayout(false);
            this.cmTortoiseGit.ResumeLayout(false);
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
        private System.Windows.Forms.ToolStripMenuItem tortoiseGitToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip cmTortoiseGit;
        private System.Windows.Forms.ToolStripMenuItem fetchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pullToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem commitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pushToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem syncToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showLogToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showLogFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showReflogToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stashSaveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stashPopToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stashListToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem repoBrowserToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkForModificationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem browseReferenceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem switchCheckoutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem blameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mergeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
    }
}