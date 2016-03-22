namespace NppGit.Modules.GitCore
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
            this.tvRepositories = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // tvRepositories
            // 
            this.tvRepositories.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvRepositories.Location = new System.Drawing.Point(0, 0);
            this.tvRepositories.Name = "tvRepositories";
            this.tvRepositories.Size = new System.Drawing.Size(284, 353);
            this.tvRepositories.TabIndex = 0;
            this.tvRepositories.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvRepositories_NodeMouseDoubleClick);
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
    }
}