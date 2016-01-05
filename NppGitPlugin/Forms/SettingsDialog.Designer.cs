namespace NppGit.Forms
{
    partial class SettingsDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsDialog));
            this.chbTGToolbar = new System.Windows.Forms.CheckBox();
            this.chlButtons = new System.Windows.Forms.CheckedListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // chbTGToolbar
            // 
            this.chbTGToolbar.AutoSize = true;
            this.chbTGToolbar.Location = new System.Drawing.Point(12, 12);
            this.chbTGToolbar.Name = "chbTGToolbar";
            this.chbTGToolbar.Size = new System.Drawing.Size(142, 17);
            this.chbTGToolbar.TabIndex = 0;
            this.chbTGToolbar.Text = "Show TortoiseGit toolbar";
            this.chbTGToolbar.UseVisualStyleBackColor = true;
            this.chbTGToolbar.CheckedChanged += new System.EventHandler(this.chbTGToolbar_CheckedChanged);
            // 
            // chlButtons
            // 
            this.chlButtons.CheckOnClick = true;
            this.chlButtons.FormattingEnabled = true;
            this.chlButtons.Items.AddRange(new object[] {
            "Fetch",
            "Pull",
            "Commit",
            "Push",
            "Blame",
            "Log",
            "Stash save",
            "Stash pop",
            "Repo status",
            "Switch"});
            this.chlButtons.Location = new System.Drawing.Point(12, 35);
            this.chlButtons.MultiColumn = true;
            this.chlButtons.Name = "chlButtons";
            this.chlButtons.Size = new System.Drawing.Size(380, 64);
            this.chlButtons.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 102);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "*Only after restart NPP";
            // 
            // SettingsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(402, 144);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chlButtons);
            this.Controls.Add(this.chbTGToolbar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Settings";
            this.TopMost = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SettingsDialog_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chbTGToolbar;
        private System.Windows.Forms.CheckedListBox chlButtons;
        private System.Windows.Forms.Label label1;
    }
}