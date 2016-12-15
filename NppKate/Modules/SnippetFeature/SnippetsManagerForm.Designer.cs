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

namespace NppKate.Forms
{
    partial class SnippetsManagerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SnippetsManagerForm));
            this.contextMenuSnippets = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.miInsert = new System.Windows.Forms.ToolStripMenuItem();
            this.miExtract = new System.Windows.Forms.ToolStripMenuItem();
            this.miAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.miEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.miDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tvSnippets = new System.Windows.Forms.TreeView();
            this.ilImages = new System.Windows.Forms.ImageList(this.components);
            this.contextMenuSnippets.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuSnippets
            // 
            this.contextMenuSnippets.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miRefresh,
            this.miInsert,
            this.miExtract,
            this.miAdd,
            this.miEdit,
            this.miDelete});
            this.contextMenuSnippets.Name = "contextMenuSnippets";
            this.contextMenuSnippets.Size = new System.Drawing.Size(114, 136);
            this.contextMenuSnippets.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuSnippets_Opening);
            // 
            // miRefresh
            // 
            this.miRefresh.Image = global::NppKate.Properties.Resources.arrow_circle_315;
            this.miRefresh.Name = "miRefresh";
            this.miRefresh.Size = new System.Drawing.Size(113, 22);
            this.miRefresh.Text = "Refresh";
            this.miRefresh.Click += new System.EventHandler(this.miRefresh_Click);
            // 
            // miInsert
            // 
            this.miInsert.Image = global::NppKate.Properties.Resources.tick_button;
            this.miInsert.Name = "miInsert";
            this.miInsert.Size = new System.Drawing.Size(113, 22);
            this.miInsert.Text = "Insert";
            this.miInsert.Click += new System.EventHandler(this.miInsert_Click);
            // 
            // miExtract
            // 
            this.miExtract.Image = global::NppKate.Properties.Resources.arrow_curve;
            this.miExtract.Name = "miExtract";
            this.miExtract.Size = new System.Drawing.Size(113, 22);
            this.miExtract.Text = "Extract";
            this.miExtract.Click += new System.EventHandler(this.miExtract_Click);
            // 
            // miAdd
            // 
            this.miAdd.Image = global::NppKate.Properties.Resources.plus_button;
            this.miAdd.Name = "miAdd";
            this.miAdd.Size = new System.Drawing.Size(113, 22);
            this.miAdd.Text = "Add";
            this.miAdd.Click += new System.EventHandler(this.miAdd_Click);
            // 
            // miEdit
            // 
            this.miEdit.Image = global::NppKate.Properties.Resources.pencil_button;
            this.miEdit.Name = "miEdit";
            this.miEdit.Size = new System.Drawing.Size(113, 22);
            this.miEdit.Text = "Edit";
            this.miEdit.Click += new System.EventHandler(this.miEdit_Click);
            // 
            // miDelete
            // 
            this.miDelete.Image = global::NppKate.Properties.Resources.minus_button;
            this.miDelete.Name = "miDelete";
            this.miDelete.Size = new System.Drawing.Size(113, 22);
            this.miDelete.Text = "Delete";
            this.miDelete.Click += new System.EventHandler(this.miDelete_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tvSnippets);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(405, 373);
            this.panel1.TabIndex = 3;
            // 
            // tvSnippets
            // 
            this.tvSnippets.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvSnippets.ImageIndex = 0;
            this.tvSnippets.ImageList = this.ilImages;
            this.tvSnippets.ItemHeight = 18;
            this.tvSnippets.Location = new System.Drawing.Point(0, 0);
            this.tvSnippets.Name = "tvSnippets";
            this.tvSnippets.SelectedImageIndex = 0;
            this.tvSnippets.Size = new System.Drawing.Size(405, 373);
            this.tvSnippets.TabIndex = 0;
            this.tvSnippets.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.tvSnippets_AfterCollapse);
            this.tvSnippets.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.tvSnippets_AfterExpand);
            this.tvSnippets.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvSnippets_NodeMouseClick);
            this.tvSnippets.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvSnippets_NodeMouseDoubleClick);
            // 
            // ilImages
            // 
            this.ilImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilImages.ImageStream")));
            this.ilImages.TransparentColor = System.Drawing.Color.Transparent;
            this.ilImages.Images.SetKeyName(0, "SNIPPET");
            this.ilImages.Images.SetKeyName(1, "CATEGORY");
            this.ilImages.Images.SetKeyName(2, "CATEGORY_OPEN");
            // 
            // SnippetsManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(405, 373);
            this.Controls.Add(this.panel1);
            this.Name = "SnippetsManagerForm";
            this.Text = "Snippets";
            this.contextMenuSnippets.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ContextMenuStrip contextMenuSnippets;
        private System.Windows.Forms.ToolStripMenuItem miInsert;
        private System.Windows.Forms.ToolStripMenuItem miAdd;
        private System.Windows.Forms.ToolStripMenuItem miEdit;
        private System.Windows.Forms.ToolStripMenuItem miDelete;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TreeView tvSnippets;
        private System.Windows.Forms.ImageList ilImages;
        private System.Windows.Forms.ToolStripMenuItem miRefresh;
        private System.Windows.Forms.ToolStripMenuItem miExtract;
    }
}