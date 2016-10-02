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

using NppKate.Common;
using NppKate.Modules.SnippetFeature;
using System;
using System.Linq;
using System.Windows.Forms;

namespace NppKate.Forms
{
    public enum SnippetEditAction { Insert, Update };

    public partial class SnippetEdit : SimpleDialog
    {
        ISnippetManager _snippetManager;
        ISnippetValidator _snippetValidator;
        Snippet _oldSnippet = null;

        public SnippetEdit()
        {
            InitializeComponent();
            Action = SnippetEditAction.Insert;
        }

        protected override void AfterInit()
        {
            base.AfterInit();
            _snippetManager = _manager.ModuleManager.GetService(typeof(ISnippetManager)) as ISnippetManager;
            _snippetValidator = _manager.ModuleManager.GetService(typeof(ISnippetValidator)) as ISnippetValidator;
            LoadCategoriesAndExtentions();
        }

        private void LoadCategoriesAndExtentions()
        {
            var allSnippets = _snippetManager.GetAllSnippets();
            if (allSnippets == null || allSnippets.Count == 0) return;
            var categories = allSnippets.Select(s => s.Category).Distinct().ToArray();
            cbCategory.AutoCompleteCustomSource.AddRange(categories);
            cbCategory.Items.AddRange(categories);
        }

        private void bOk_Click(object sender, EventArgs e)
        {
            var snippet = new Snippet(tbName.Text, tbShortName.Text, tbSnippet.Text.Replace("\r\n", "\n"), chbIsShowInMenu.Checked, cbCategory.Text, tbFileExt.Text);
            try
            {
                _snippetValidator.SnippetIsValid(snippet);
                _snippetManager.AddOrUpdate(snippet, _oldSnippet?.Name);
                _snippet = snippet.Name;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Snippet is bad!\r\n" + ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                DialogResult = DialogResult.None;
            }
        }

        private void LoadSnippet()
        {
            _oldSnippet = _snippetManager.FindByName(_snippet);
            tbName.Text = _oldSnippet.Name;
            tbShortName.Text = _oldSnippet.ShortName;
            tbSnippet.Text = _oldSnippet.Text.Replace("\n", "\r\n");
            chbIsShowInMenu.Checked = _oldSnippet.IsVisible;
            cbCategory.Text = _oldSnippet.Category;
            tbFileExt.Text = _oldSnippet.FileExt;
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

        public SnippetEditAction Action { get; set; }

        private void NameValidating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var name = (sender as TextBox).Text;
            if (Action == SnippetEditAction.Insert ||
                (_oldSnippet != null && _oldSnippet.Name != name && _oldSnippet.ShortName != name))
            {
                e.Cancel = _snippetManager.Contains(name);
            }

            if (e.Cancel)
            {
                (sender as TextBox).BackColor = System.Drawing.Color.IndianRed;
            }
            else
            {
                (sender as TextBox).BackColor = System.Drawing.Color.FromKnownColor(System.Drawing.KnownColor.Window);
            }
        }

        private void AutoParamInsert(object sender, EventArgs e)
        {
            var text = (sender as Button).Text;
            InsertTextInSnippet(text);
        }

        private void InsertTextInSnippet(string text)
        {
            var textPos = tbSnippet.SelectionStart;
            tbSnippet.Text = tbSnippet.Text.Insert(textPos, text);
            tbSnippet.SelectionStart = textPos + text.Length;
            tbSnippet.Focus();
        }

        private void SnippetEdit_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                cmsSnippets.Items.Clear();
                _snippetManager.GetAllSnippets()
                    .Where(s => s.Name != _oldSnippet?.Name)
                    .Select(s => s.Name)
                    .ToList()
                    .ForEach((s) =>
                    {
                        cmsSnippets.Items.Add(s);
                    });
            }
        }

        private void cmsSnippets_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            var text = "$(SNIPPET:" + e.ClickedItem.Text + ")";
            InsertTextInSnippet(text);
        }

        private void btnSnippet_Click(object sender, EventArgs e)
        {
            cmsSnippets.Show(MousePosition);
        }
    }
}
