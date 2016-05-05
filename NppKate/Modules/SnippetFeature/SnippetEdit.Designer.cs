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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SnippetEdit));
            this.pTop = new System.Windows.Forms.Panel();
            this.chbIsShowInMenu = new System.Windows.Forms.CheckBox();
            this.lSnippet = new System.Windows.Forms.Label();
            this.lName = new System.Windows.Forms.Label();
            this.tbName = new System.Windows.Forms.TextBox();
            this.pMiddle = new System.Windows.Forms.Panel();
            this.tbSnippet = new System.Windows.Forms.TextBox();
            this.pBottom = new System.Windows.Forms.Panel();
            this.bCancel = new System.Windows.Forms.Button();
            this.bOk = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pTop.SuspendLayout();
            this.pMiddle.SuspendLayout();
            this.pBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pTop
            // 
            this.pTop.Controls.Add(this.label2);
            this.pTop.Controls.Add(this.label1);
            this.pTop.Controls.Add(this.comboBox2);
            this.pTop.Controls.Add(this.comboBox1);
            this.pTop.Controls.Add(this.chbIsShowInMenu);
            this.pTop.Controls.Add(this.lSnippet);
            this.pTop.Controls.Add(this.lName);
            this.pTop.Controls.Add(this.tbName);
            this.pTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pTop.Location = new System.Drawing.Point(0, 0);
            this.pTop.Name = "pTop";
            this.pTop.Size = new System.Drawing.Size(624, 93);
            this.pTop.TabIndex = 0;
            // 
            // chbIsShowInMenu
            // 
            this.chbIsShowInMenu.AutoSize = true;
            this.chbIsShowInMenu.Location = new System.Drawing.Point(6, 53);
            this.chbIsShowInMenu.Name = "chbIsShowInMenu";
            this.chbIsShowInMenu.Size = new System.Drawing.Size(93, 17);
            this.chbIsShowInMenu.TabIndex = 3;
            this.chbIsShowInMenu.Text = "Show in menu";
            this.chbIsShowInMenu.UseVisualStyleBackColor = true;
            // 
            // lSnippet
            // 
            this.lSnippet.AutoSize = true;
            this.lSnippet.Location = new System.Drawing.Point(3, 73);
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
            this.tbName.Size = new System.Drawing.Size(260, 20);
            this.tbName.TabIndex = 0;
            // 
            // pMiddle
            // 
            this.pMiddle.Controls.Add(this.tbSnippet);
            this.pMiddle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pMiddle.Location = new System.Drawing.Point(0, 93);
            this.pMiddle.Name = "pMiddle";
            this.pMiddle.Size = new System.Drawing.Size(624, 323);
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
            this.tbSnippet.Size = new System.Drawing.Size(624, 323);
            this.tbSnippet.TabIndex = 1;
            // 
            // pBottom
            // 
            this.pBottom.Controls.Add(this.bCancel);
            this.pBottom.Controls.Add(this.bOk);
            this.pBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pBottom.Location = new System.Drawing.Point(0, 416);
            this.pBottom.Name = "pBottom";
            this.pBottom.Size = new System.Drawing.Size(624, 26);
            this.pBottom.TabIndex = 2;
            // 
            // bCancel
            // 
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Dock = System.Windows.Forms.DockStyle.Right;
            this.bCancel.Image = global::NppKate.Properties.Resources.button_cancel_8865;
            this.bCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bCancel.Location = new System.Drawing.Point(474, 0);
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
            this.bOk.Location = new System.Drawing.Point(549, 0);
            this.bOk.Name = "bOk";
            this.bOk.Size = new System.Drawing.Size(75, 26);
            this.bOk.TabIndex = 3;
            this.bOk.Text = "OK";
            this.bOk.UseVisualStyleBackColor = true;
            this.bOk.Click += new System.EventHandler(this.bOk_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(272, 28);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(271, 21);
            this.comboBox1.TabIndex = 4;
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(549, 27);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(72, 21);
            this.comboBox2.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(269, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Category:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(546, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "File extention:";
            // 
            // SnippetEdit
            // 
            this.AcceptButton = this.bOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bCancel;
            this.ClientSize = new System.Drawing.Size(624, 442);
            this.Controls.Add(this.pMiddle);
            this.Controls.Add(this.pBottom);
            this.Controls.Add(this.pTop);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SnippetEdit";
            this.ShowInTaskbar = false;
            this.Text = "Snippet Editor";
            this.pTop.ResumeLayout(false);
            this.pTop.PerformLayout();
            this.pMiddle.ResumeLayout(false);
            this.pMiddle.PerformLayout();
            this.pBottom.ResumeLayout(false);
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
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.ComboBox comboBox1;
    }
}