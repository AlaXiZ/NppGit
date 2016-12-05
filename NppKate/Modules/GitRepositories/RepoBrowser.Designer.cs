// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
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
            this.cmTreeView = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miAddRepo2 = new System.Windows.Forms.ToolStripMenuItem();
            this.ilImages = new System.Windows.Forms.ImageList(this.components);
            this.cmRepositories = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miSetActive = new System.Windows.Forms.ToolStripMenuItem();
            this.miAddRepo = new System.Windows.Forms.ToolStripMenuItem();
            this.miRemoveRepo = new System.Windows.Forms.ToolStripMenuItem();
            this.tsGit = new System.Windows.Forms.ToolStripSeparator();
            this.tortoiseGitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmTortoiseGit = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.findInLogMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.miToWorktree = new System.Windows.Forms.ToolStripMenuItem();
            this.cmNone = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmWorktree = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miPrune = new System.Windows.Forms.ToolStripMenuItem();
            this.miRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.smiWorktree = new System.Windows.Forms.ToolStripSeparator();
            this.miLock = new System.Windows.Forms.ToolStripMenuItem();
            this.miUnlock = new System.Windows.Forms.ToolStripMenuItem();
            this.miRemove = new System.Windows.Forms.ToolStripMenuItem();
            this.smiGit = new System.Windows.Forms.ToolStripSeparator();
            this.miWTPull = new System.Windows.Forms.ToolStripMenuItem();
            this.miWTCommit = new System.Windows.Forms.ToolStripMenuItem();
            this.miWTPush = new System.Windows.Forms.ToolStripMenuItem();
            this.pbProgress = new System.Windows.Forms.ProgressBar();
            this.cmTreeView.SuspendLayout();
            this.cmRepositories.SuspendLayout();
            this.cmTortoiseGit.SuspendLayout();
            this.cmBranch.SuspendLayout();
            this.cmWorktree.SuspendLayout();
            this.SuspendLayout();
            // 
            // tvRepositories
            // 
            this.tvRepositories.ContextMenuStrip = this.cmTreeView;
            this.tvRepositories.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvRepositories.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tvRepositories.ImageIndex = 0;
            this.tvRepositories.ImageList = this.ilImages;
            this.tvRepositories.Location = new System.Drawing.Point(0, 0);
            this.tvRepositories.Name = "tvRepositories";
            this.tvRepositories.SelectedImageIndex = 0;
            this.tvRepositories.Size = new System.Drawing.Size(284, 353);
            this.tvRepositories.TabIndex = 0;
            this.tvRepositories.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvRepositories_Before);
            this.tvRepositories.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvRepositories_Before);
            this.tvRepositories.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.tvRepositories_AfterExpand);
            this.tvRepositories.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvRepositories_NodeMouseClick);
            this.tvRepositories.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvRepositories_NodeMouseDoubleClick);
            this.tvRepositories.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tvRepositories_MouseDown);
            // 
            // cmTreeView
            // 
            this.cmTreeView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miAddRepo2});
            this.cmTreeView.Name = "cmTreeView";
            this.cmTreeView.Size = new System.Drawing.Size(153, 26);
            // 
            // miAddRepo2
            // 
            this.miAddRepo2.Image = global::NppKate.Properties.Resources.database__plus;
            this.miAddRepo2.Name = "miAddRepo2";
            this.miAddRepo2.Size = new System.Drawing.Size(152, 22);
            this.miAddRepo2.Text = "Add repository";
            this.miAddRepo2.Click += new System.EventHandler(this.miAddRepo_Click);
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
            this.ilImages.Images.SetKeyName(7, "WORKTREE_FOLDER");
            this.ilImages.Images.SetKeyName(8, "WORKTREE_LEAF");
            this.ilImages.Images.SetKeyName(9, "WORKTREE_LOCK");
            this.ilImages.Images.SetKeyName(10, "WORKTREE_NOT_EXISTS");
            // 
            // cmRepositories
            // 
            this.cmRepositories.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miSetActive,
            this.miAddRepo,
            this.miRemoveRepo,
            this.tsGit,
            this.tortoiseGitToolStripMenuItem});
            this.cmRepositories.Name = "cmRepositories";
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
            // tsGit
            // 
            this.tsGit.Name = "tsGit";
            this.tsGit.Size = new System.Drawing.Size(170, 6);
            this.tsGit.Visible = false;
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
            this.findInLogMenuItem,
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
            this.cmTortoiseGit.OwnerItem = this.tortoiseGitToolStripMenuItem;
            this.cmTortoiseGit.Size = new System.Drawing.Size(197, 444);
            // 
            // findInLogMenuItem
            // 
            this.findInLogMenuItem.Image = global::NppKate.Properties.Resources.Search_32;
            this.findInLogMenuItem.Name = "findInLogMenuItem";
            this.findInLogMenuItem.ShortcutKeyDisplayString = "";
            this.findInLogMenuItem.Size = new System.Drawing.Size(196, 22);
            this.findInLogMenuItem.Text = "Quick search";
            this.findInLogMenuItem.Click += new System.EventHandler(this.findInLogMenuItem_Click);
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
            this.miSwitchTo,
            this.miToWorktree});
            this.cmBranch.Name = "cmBranch";
            this.cmBranch.Size = new System.Drawing.Size(138, 48);
            this.cmBranch.Opening += new System.ComponentModel.CancelEventHandler(this.cmBranch_Opening);
            // 
            // miSwitchTo
            // 
            this.miSwitchTo.Image = global::NppKate.Properties.Resources.checkout;
            this.miSwitchTo.Name = "miSwitchTo";
            this.miSwitchTo.Size = new System.Drawing.Size(137, 22);
            this.miSwitchTo.Text = "Switch to";
            this.miSwitchTo.Click += new System.EventHandler(this.miSwitchTo_Click);
            // 
            // miToWorktree
            // 
            this.miToWorktree.Image = global::NppKate.Properties.Resources.arrow_branch;
            this.miToWorktree.Name = "miToWorktree";
            this.miToWorktree.Size = new System.Drawing.Size(137, 22);
            this.miToWorktree.Text = "To worktree";
            this.miToWorktree.Click += new System.EventHandler(this.miToWorktree_Click);
            // 
            // cmNone
            // 
            this.cmNone.Name = "cmNone";
            this.cmNone.Size = new System.Drawing.Size(61, 4);
            // 
            // cmWorktree
            // 
            this.cmWorktree.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miPrune,
            this.miRefresh,
            this.smiWorktree,
            this.miLock,
            this.miUnlock,
            this.miRemove,
            this.smiGit,
            this.miWTPull,
            this.miWTCommit,
            this.miWTPush});
            this.cmWorktree.Name = "cmWorktree";
            this.cmWorktree.Size = new System.Drawing.Size(119, 192);
            this.cmWorktree.Opening += new System.ComponentModel.CancelEventHandler(this.cmWorktree_Opening);
            // 
            // miPrune
            // 
            this.miPrune.Image = global::NppKate.Properties.Resources.tree__minus;
            this.miPrune.Name = "miPrune";
            this.miPrune.Size = new System.Drawing.Size(118, 22);
            this.miPrune.Text = "Prune";
            this.miPrune.Click += new System.EventHandler(this.miPrune_Click);
            // 
            // miRefresh
            // 
            this.miRefresh.Image = global::NppKate.Properties.Resources.arrow_circle_315;
            this.miRefresh.Name = "miRefresh";
            this.miRefresh.Size = new System.Drawing.Size(118, 22);
            this.miRefresh.Text = "Refresh";
            this.miRefresh.Click += new System.EventHandler(this.miRefresh_Click);
            // 
            // smiWorktree
            // 
            this.smiWorktree.Name = "smiWorktree";
            this.smiWorktree.Size = new System.Drawing.Size(115, 6);
            this.smiWorktree.Visible = false;
            // 
            // miLock
            // 
            this.miLock.Image = global::NppKate.Properties.Resources._lock;
            this.miLock.Name = "miLock";
            this.miLock.Size = new System.Drawing.Size(118, 22);
            this.miLock.Text = "Lock";
            this.miLock.Click += new System.EventHandler(this.miLock_Click);
            // 
            // miUnlock
            // 
            this.miUnlock.Image = global::NppKate.Properties.Resources.lock_unlock;
            this.miUnlock.Name = "miUnlock";
            this.miUnlock.Size = new System.Drawing.Size(118, 22);
            this.miUnlock.Text = "Unlock";
            this.miUnlock.Click += new System.EventHandler(this.miUnlock_Click);
            // 
            // miRemove
            // 
            this.miRemove.Image = global::NppKate.Properties.Resources.tree__minus;
            this.miRemove.Name = "miRemove";
            this.miRemove.Size = new System.Drawing.Size(118, 22);
            this.miRemove.Text = "Remove";
            this.miRemove.Click += new System.EventHandler(this.miRemove_Click);
            // 
            // smiGit
            // 
            this.smiGit.Name = "smiGit";
            this.smiGit.Size = new System.Drawing.Size(115, 6);
            // 
            // miWTPull
            // 
            this.miWTPull.Image = global::NppKate.Properties.Resources.arrow_transition_270;
            this.miWTPull.Name = "miWTPull";
            this.miWTPull.Size = new System.Drawing.Size(118, 22);
            this.miWTPull.Text = "Pull";
            this.miWTPull.Click += new System.EventHandler(this.miWTPull_Click);
            // 
            // miWTCommit
            // 
            this.miWTCommit.Image = global::NppKate.Properties.Resources.arrow_curve_090_left;
            this.miWTCommit.Name = "miWTCommit";
            this.miWTCommit.Size = new System.Drawing.Size(118, 22);
            this.miWTCommit.Text = "Commit";
            this.miWTCommit.Click += new System.EventHandler(this.miWTCommit_Click);
            // 
            // miWTPush
            // 
            this.miWTPush.Image = global::NppKate.Properties.Resources.arrow_transition_090;
            this.miWTPush.Name = "miWTPush";
            this.miWTPush.Size = new System.Drawing.Size(118, 22);
            this.miWTPush.Text = "Push";
            this.miWTPush.Click += new System.EventHandler(this.miWTPush_Click);
            // 
            // pbProgress
            // 
            this.pbProgress.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pbProgress.Location = new System.Drawing.Point(0, 338);
            this.pbProgress.MarqueeAnimationSpeed = 200;
            this.pbProgress.Name = "pbProgress";
            this.pbProgress.Size = new System.Drawing.Size(284, 15);
            this.pbProgress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.pbProgress.TabIndex = 6;
            this.pbProgress.UseWaitCursor = true;
            // 
            // RepoBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 353);
            this.Controls.Add(this.pbProgress);
            this.Controls.Add(this.tvRepositories);
            this.Name = "RepoBrowser";
            this.Text = "Repositories";
            this.VisibleChanged += new System.EventHandler(this.RepoBrowser_VisibleChanged);
            this.cmTreeView.ResumeLayout(false);
            this.cmRepositories.ResumeLayout(false);
            this.cmTortoiseGit.ResumeLayout(false);
            this.cmBranch.ResumeLayout(false);
            this.cmWorktree.ResumeLayout(false);
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
        private System.Windows.Forms.ToolStripSeparator tsGit;
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
        private System.Windows.Forms.ToolStripMenuItem findInLogMenuItem;
        private System.Windows.Forms.ContextMenuStrip cmTreeView;
        private System.Windows.Forms.ToolStripMenuItem miAddRepo2;
        private System.Windows.Forms.ToolStripMenuItem miToWorktree;
        private System.Windows.Forms.ContextMenuStrip cmNone;
        private System.Windows.Forms.ContextMenuStrip cmWorktree;
        private System.Windows.Forms.ToolStripMenuItem miPrune;
        private System.Windows.Forms.ToolStripSeparator smiWorktree;
        private System.Windows.Forms.ToolStripMenuItem miLock;
        private System.Windows.Forms.ToolStripMenuItem miUnlock;
        private System.Windows.Forms.ToolStripMenuItem miRemove;
        private System.Windows.Forms.ToolStripMenuItem miRefresh;
        private System.Windows.Forms.ProgressBar pbProgress;
        private System.Windows.Forms.ToolStripSeparator smiGit;
        private System.Windows.Forms.ToolStripMenuItem miWTPull;
        private System.Windows.Forms.ToolStripMenuItem miWTCommit;
        private System.Windows.Forms.ToolStripMenuItem miWTPush;
    }
}