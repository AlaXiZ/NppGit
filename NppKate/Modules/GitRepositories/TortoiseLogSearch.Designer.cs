namespace NppKate.Modules.GitCore
{
    partial class TortoiseLogSearch
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
            this.tbFindString = new System.Windows.Forms.TextBox();
            this.chklstFindType = new System.Windows.Forms.CheckedListBox();
            this.bSearch = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tbFindString
            // 
            this.tbFindString.ImeMode = System.Windows.Forms.ImeMode.On;
            this.tbFindString.Location = new System.Drawing.Point(12, 12);
            this.tbFindString.Name = "tbFindString";
            this.tbFindString.Size = new System.Drawing.Size(369, 20);
            this.tbFindString.TabIndex = 0;
            // 
            // chklstFindType
            // 
            this.chklstFindType.BackColor = System.Drawing.SystemColors.Control;
            this.chklstFindType.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.chklstFindType.CheckOnClick = true;
            this.chklstFindType.Items.AddRange(new object[] {
            "By message",
            "By path",
            "By author",
            "By revision (SHA)",
            "By bug ID",
            "By subject",
            "By ref name",
            "By e-mail",
            "By notes"});
            this.chklstFindType.Location = new System.Drawing.Point(12, 38);
            this.chklstFindType.MultiColumn = true;
            this.chklstFindType.Name = "chklstFindType";
            this.chklstFindType.Size = new System.Drawing.Size(369, 45);
            this.chklstFindType.TabIndex = 1;
            // 
            // bSearch
            // 
            this.bSearch.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.bSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bSearch.Image = global::NppKate.Properties.Resources.magnifier_left;
            this.bSearch.Location = new System.Drawing.Point(387, 10);
            this.bSearch.Name = "bSearch";
            this.bSearch.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.bSearch.Size = new System.Drawing.Size(32, 23);
            this.bSearch.TabIndex = 1;
            this.bSearch.UseVisualStyleBackColor = true;
            this.bSearch.Click += new System.EventHandler(this.bSearch_Click);
            // 
            // TortoiseLogSearch
            // 
            this.AcceptButton = this.bSearch;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(428, 91);
            this.Controls.Add(this.bSearch);
            this.Controls.Add(this.chklstFindType);
            this.Controls.Add(this.tbFindString);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "TortoiseLogSearch";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Search";
            this.Deactivate += new System.EventHandler(this.TortoiseLogSearch_Deactivate);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.TortoiseLogSearch_FormClosed);
            this.Shown += new System.EventHandler(this.TortoiseLogSearch_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbFindString;
        private System.Windows.Forms.CheckedListBox chklstFindType;
        private System.Windows.Forms.Button bSearch;
    }
}