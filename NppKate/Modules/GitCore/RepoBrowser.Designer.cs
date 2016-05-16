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
            this.tvRepositories.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvRepositories_NodeMouseDoubleClick);
            // 
            // ilImages
            // 
            this.ilImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilImages.ImageStream")));
            this.ilImages.TransparentColor = System.Drawing.Color.Transparent;
            this.ilImages.Images.SetKeyName(0, "REPO");
            this.ilImages.Images.SetKeyName(1, "BRANCH");
            this.ilImages.Images.SetKeyName(2, "EMPTY");
            this.ilImages.Images.SetKeyName(3, "REMOTE_BRANCH");
            this.ilImages.Images.SetKeyName(4, "BRANCH_FOLDER");
            this.ilImages.Images.SetKeyName(5, "CURRENT_BRANCH");
            // 
            // RepoBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 353);
            this.Controls.Add(this.tvRepositories);
            this.Name = "RepoBrowser";
            this.Text = "RepoBrowser";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView tvRepositories;
        private System.Windows.Forms.ImageList ilImages;
    }
}