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

using NppKate.Modules.SnippetFeature;
using System;
using System.Windows.Forms;

namespace NppKate.Forms
{
    public partial class SnippetEdit : Form
    {
        public SnippetEdit()
        {
            InitializeComponent();
            cbCategory.Items.AddRange(SnippetManager.Instance.GetCategories().ToArray());
            cbExtention.Items.AddRange(SnippetManager.Instance.GetExt().ToArray());
        }

        private void bOk_Click(object sender, EventArgs e)
        {
            if (!Snippet.CheckCorrectSnippet(tbSnippet.Text))
            {
                MessageBox.Show("Snippet is bad!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                DialogResult = DialogResult.None;
                return;
            }
            try
            {
                var newSnippet = new Snippet(tbName.Text, tbSnippet.Text, chbIsShowInMenu.Checked, cbCategory.Text, cbExtention.Text, tbShortName.Text);
                if (string.IsNullOrEmpty(_snippet))
                {
                    SnippetManager.Instance.AddSnippet(newSnippet);
                }
                else
                {
                    SnippetManager.Instance.UpdateSnippet(_snippet, newSnippet);
                }
                _snippet = newSnippet.Name;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                DialogResult = DialogResult.None;
            }
        }

        private void LoadSnippet()
        {
            var s = SnippetManager.Instance.Snippets[_snippet];
            tbName.Text = s.Name;
            tbShortName.Text = s.ShortName;
            tbSnippet.Text = s.SnippetText.Replace("\n", "\r\n");
            chbIsShowInMenu.Checked = s.IsShowInMenu;
            cbCategory.Text = s.Category;
            cbExtention.Text = s.FileExt;
        }

        private string _snippet;
        public string SnippetName
        {
            get { return _snippet; }
            set
            {
                _snippet = value;
                LoadSnippet();
            }
        }
    }
}
