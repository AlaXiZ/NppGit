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
    partial class SnippetEdit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SnippetEdit));
            this.pTop = new System.Windows.Forms.Panel();
            this.btnSnippet = new System.Windows.Forms.Button();
            this.btnFileName = new System.Windows.Forms.Button();
            this.btnUserName = new System.Windows.Forms.Button();
            this.btnDate = new System.Windows.Forms.Button();
            this.lShortName = new System.Windows.Forms.Label();
            this.tbShortName = new System.Windows.Forms.TextBox();
            this.lExt = new System.Windows.Forms.Label();
            this.lCategory = new System.Windows.Forms.Label();
            this.cbExtention = new System.Windows.Forms.ComboBox();
            this.cbCategory = new System.Windows.Forms.ComboBox();
            this.chbIsShowInMenu = new System.Windows.Forms.CheckBox();
            this.lSnippet = new System.Windows.Forms.Label();
            this.lName = new System.Windows.Forms.Label();
            this.tbName = new System.Windows.Forms.TextBox();
            this.pMiddle = new System.Windows.Forms.Panel();
            this.tbSnippet = new System.Windows.Forms.TextBox();
            this.pBottom = new System.Windows.Forms.Panel();
            this.lblHelp = new System.Windows.Forms.Label();
            this.bCancel = new System.Windows.Forms.Button();
            this.bOk = new System.Windows.Forms.Button();
            this.cmsSnippets = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.pTop.SuspendLayout();
            this.pMiddle.SuspendLayout();
            this.pBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pTop
            // 
            this.pTop.Controls.Add(this.btnSnippet);
            this.pTop.Controls.Add(this.btnFileName);
            this.pTop.Controls.Add(this.btnUserName);
            this.pTop.Controls.Add(this.btnDate);
            this.pTop.Controls.Add(this.lShortName);
            this.pTop.Controls.Add(this.tbShortName);
            this.pTop.Controls.Add(this.lExt);
            this.pTop.Controls.Add(this.lCategory);
            this.pTop.Controls.Add(this.cbExtention);
            this.pTop.Controls.Add(this.cbCategory);
            this.pTop.Controls.Add(this.chbIsShowInMenu);
            this.pTop.Controls.Add(this.lSnippet);
            this.pTop.Controls.Add(this.lName);
            this.pTop.Controls.Add(this.tbName);
            this.pTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pTop.Location = new System.Drawing.Point(0, 0);
            this.pTop.Name = "pTop";
            this.pTop.Size = new System.Drawing.Size(784, 81);
            this.pTop.TabIndex = 0;
            // 
            // btnSnippet
            // 
            this.btnSnippet.Location = new System.Drawing.Point(486, 54);
            this.btnSnippet.Name = "btnSnippet";
            this.btnSnippet.Size = new System.Drawing.Size(80, 23);
            this.btnSnippet.TabIndex = 13;
            this.btnSnippet.Text = "$(SNIPPET)";
            this.btnSnippet.UseVisualStyleBackColor = true;
            this.btnSnippet.Click += new System.EventHandler(this.btnSnippet_Click);
            // 
            // btnFileName
            // 
            this.btnFileName.Location = new System.Drawing.Point(387, 54);
            this.btnFileName.Name = "btnFileName";
            this.btnFileName.Size = new System.Drawing.Size(93, 23);
            this.btnFileName.TabIndex = 12;
            this.btnFileName.Text = "$(FILENAME)";
            this.btnFileName.UseVisualStyleBackColor = true;
            this.btnFileName.Click += new System.EventHandler(this.AutoParamInsert);
            // 
            // btnUserName
            // 
            this.btnUserName.Location = new System.Drawing.Point(291, 54);
            this.btnUserName.Name = "btnUserName";
            this.btnUserName.Size = new System.Drawing.Size(90, 23);
            this.btnUserName.TabIndex = 11;
            this.btnUserName.Text = "$(USERNAME)";
            this.btnUserName.UseVisualStyleBackColor = true;
            this.btnUserName.Click += new System.EventHandler(this.AutoParamInsert);
            // 
            // btnDate
            // 
            this.btnDate.Location = new System.Drawing.Point(228, 55);
            this.btnDate.Name = "btnDate";
            this.btnDate.Size = new System.Drawing.Size(57, 23);
            this.btnDate.TabIndex = 10;
            this.btnDate.Text = "$(DATE)";
            this.btnDate.UseVisualStyleBackColor = true;
            this.btnDate.Click += new System.EventHandler(this.AutoParamInsert);
            // 
            // lShortName
            // 
            this.lShortName.AutoSize = true;
            this.lShortName.Location = new System.Drawing.Point(225, 9);
            this.lShortName.Name = "lShortName";
            this.lShortName.Size = new System.Drawing.Size(101, 13);
            this.lShortName.TabIndex = 9;
            this.lShortName.Text = "Snippet short name:";
            // 
            // tbShortName
            // 
            this.tbShortName.Location = new System.Drawing.Point(228, 28);
            this.tbShortName.Name = "tbShortName";
            this.tbShortName.Size = new System.Drawing.Size(179, 20);
            this.tbShortName.TabIndex = 1;
            this.tbShortName.Validating += new System.ComponentModel.CancelEventHandler(this.NameValidating);
            // 
            // lExt
            // 
            this.lExt.AutoSize = true;
            this.lExt.Location = new System.Drawing.Point(569, 9);
            this.lExt.Name = "lExt";
            this.lExt.Size = new System.Drawing.Size(72, 13);
            this.lExt.TabIndex = 7;
            this.lExt.Text = "File extention:";
            // 
            // lCategory
            // 
            this.lCategory.AutoSize = true;
            this.lCategory.Location = new System.Drawing.Point(410, 9);
            this.lCategory.Name = "lCategory";
            this.lCategory.Size = new System.Drawing.Size(52, 13);
            this.lCategory.TabIndex = 6;
            this.lCategory.Text = "Category:";
            // 
            // cbExtention
            // 
            this.cbExtention.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbExtention.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.cbExtention.FormattingEnabled = true;
            this.cbExtention.Location = new System.Drawing.Point(572, 27);
            this.cbExtention.Name = "cbExtention";
            this.cbExtention.Size = new System.Drawing.Size(72, 21);
            this.cbExtention.TabIndex = 3;
            // 
            // cbCategory
            // 
            this.cbCategory.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbCategory.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.cbCategory.FormattingEnabled = true;
            this.cbCategory.Location = new System.Drawing.Point(413, 28);
            this.cbCategory.Name = "cbCategory";
            this.cbCategory.Size = new System.Drawing.Size(153, 21);
            this.cbCategory.TabIndex = 2;
            // 
            // chbIsShowInMenu
            // 
            this.chbIsShowInMenu.AutoSize = true;
            this.chbIsShowInMenu.Location = new System.Drawing.Point(650, 29);
            this.chbIsShowInMenu.Name = "chbIsShowInMenu";
            this.chbIsShowInMenu.Size = new System.Drawing.Size(93, 17);
            this.chbIsShowInMenu.TabIndex = 4;
            this.chbIsShowInMenu.Text = "Show in menu";
            this.chbIsShowInMenu.UseVisualStyleBackColor = true;
            // 
            // lSnippet
            // 
            this.lSnippet.AutoSize = true;
            this.lSnippet.Location = new System.Drawing.Point(3, 64);
            this.lSnippet.Name = "lSnippet";
            this.lSnippet.Size = new System.Drawing.Size(46, 13);
            this.lSnippet.TabIndex = 2;
            this.lSnippet.Text = "Snippet:";
            // 
            // lName
            // 
            this.lName.AutoSize = true;
            this.lName.Location = new System.Drawing.Point(3, 9);
            this.lName.Name = "lName";
            this.lName.Size = new System.Drawing.Size(75, 13);
            this.lName.TabIndex = 1;
            this.lName.Text = "Snippet name:";
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(6, 28);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(216, 20);
            this.tbName.TabIndex = 0;
            this.tbName.Validating += new System.ComponentModel.CancelEventHandler(this.NameValidating);
            // 
            // pMiddle
            // 
            this.pMiddle.Controls.Add(this.tbSnippet);
            this.pMiddle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pMiddle.Location = new System.Drawing.Point(0, 81);
            this.pMiddle.Name = "pMiddle";
            this.pMiddle.Size = new System.Drawing.Size(784, 455);
            this.pMiddle.TabIndex = 1;
            // 
            // tbSnippet
            // 
            this.tbSnippet.AcceptsReturn = true;
            this.tbSnippet.AcceptsTab = true;
            this.tbSnippet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbSnippet.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbSnippet.Location = new System.Drawing.Point(0, 0);
            this.tbSnippet.Multiline = true;
            this.tbSnippet.Name = "tbSnippet";
            this.tbSnippet.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbSnippet.Size = new System.Drawing.Size(784, 455);
            this.tbSnippet.TabIndex = 1;
            // 
            // pBottom
            // 
            this.pBottom.Controls.Add(this.lblHelp);
            this.pBottom.Controls.Add(this.bCancel);
            this.pBottom.Controls.Add(this.bOk);
            this.pBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pBottom.Location = new System.Drawing.Point(0, 536);
            this.pBottom.Name = "pBottom";
            this.pBottom.Size = new System.Drawing.Size(784, 26);
            this.pBottom.TabIndex = 2;
            // 
            // lblHelp
            // 
            this.lblHelp.AutoSize = true;
            this.lblHelp.Location = new System.Drawing.Point(3, 7);
            this.lblHelp.Name = "lblHelp";
            this.lblHelp.Size = new System.Drawing.Size(257, 13);
            this.lblHelp.TabIndex = 5;
            this.lblHelp.Text = "If you need characters \"{\" or \"}\" then use \"{{\" or \"}}\"";
            // 
            // bCancel
            // 
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Dock = System.Windows.Forms.DockStyle.Right;
            this.bCancel.Image = global::NppKate.Properties.Resources.button_cancel_8865;
            this.bCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bCancel.Location = new System.Drawing.Point(634, 0);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 26);
            this.bCancel.TabIndex = 4;
            this.bCancel.Text = "Cancel";
            this.bCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.bCancel.UseVisualStyleBackColor = true;
            // 
            // bOk
            // 
            this.bOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.bOk.Dock = System.Windows.Forms.DockStyle.Right;
            this.bOk.Image = ((System.Drawing.Image)(resources.GetObject("bOk.Image")));
            this.bOk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bOk.Location = new System.Drawing.Point(709, 0);
            this.bOk.Name = "bOk";
            this.bOk.Size = new System.Drawing.Size(75, 26);
            this.bOk.TabIndex = 3;
            this.bOk.Text = "OK";
            this.bOk.UseVisualStyleBackColor = true;
            this.bOk.Click += new System.EventHandler(this.bOk_Click);
            // 
            // cmsSnippets
            // 
            this.cmsSnippets.Name = "cmsSnippets";
            this.cmsSnippets.Size = new System.Drawing.Size(61, 4);
            this.cmsSnippets.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.cmsSnippets_ItemClicked);
            // 
            // SnippetEdit
            // 
            this.AcceptButton = this.bOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bCancel;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.pMiddle);
            this.Controls.Add(this.pBottom);
            this.Controls.Add(this.pTop);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(740, 300);
            this.Name = "SnippetEdit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Snippet Editor";
            this.VisibleChanged += new System.EventHandler(this.SnippetEdit_VisibleChanged);
            this.pTop.ResumeLayout(false);
            this.pTop.PerformLayout();
            this.pMiddle.ResumeLayout(false);
            this.pMiddle.PerformLayout();
            this.pBottom.ResumeLayout(false);
            this.pBottom.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pTop;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.Panel pMiddle;
        private System.Windows.Forms.TextBox tbSnippet;
        private System.Windows.Forms.Panel pBottom;
        private System.Windows.Forms.Label lSnippet;
        private System.Windows.Forms.Label lName;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.Button bOk;
        private System.Windows.Forms.CheckBox chbIsShowInMenu;
        private System.Windows.Forms.Label lExt;
        private System.Windows.Forms.Label lCategory;
        private System.Windows.Forms.ComboBox cbExtention;
        private System.Windows.Forms.ComboBox cbCategory;
        private System.Windows.Forms.Label lShortName;
        private System.Windows.Forms.TextBox tbShortName;
        private System.Windows.Forms.Label lblHelp;
        private System.Windows.Forms.Button btnFileName;
        private System.Windows.Forms.Button btnUserName;
        private System.Windows.Forms.Button btnDate;
        private System.Windows.Forms.Button btnSnippet;
        private System.Windows.Forms.ContextMenuStrip cmsSnippets;
    }
}