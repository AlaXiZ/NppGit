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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using NppKate.Common;
using System.Linq;

namespace NppKate.Forms
{
    public partial class SnippetsManagerForm : DockDialog, FormDockable
    {
        private bool _isGrouping = true;
        private bool _isHiding = false;

        public SnippetsManagerForm()
        {
            InitializeComponent();
            _isGrouping = Settings.Snippets.IsGroupByCategory;
            _isHiding = Settings.Snippets.IsHideByExtention;
        }

        public Bitmap TabIcon
        {
            get { return Properties.Resources.snippets; }
        }

        public string Title
        {
            get { return "Snippets manager"; }
        }

        public void ChangeContext()
        {

        }

        private void contextMenuSnippets_Opening(object sender, CancelEventArgs e)
        {
            var isSelected = false; // lbSnippets.SelectedIndex != -1;
            miEdit.Enabled = miDelete.Enabled = isSelected;
        }

        private void miAdd_Click(object sender, EventArgs e)
        {
            var dlg = new SnippetEdit();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                reloadSnippets();
            }
        }

        private void reloadSnippets()
        {
            if (_isGrouping)
            {
                var categories = SnippetManager.Instance.GetCategories();
                var allSnippets = SnippetManager.Instance.Snippets;
                categories.Sort();
                foreach (var cat in categories)
                {
                    var catItem = tvSnippets.Nodes.Add(cat);
                    var snips = allSnippets.Values.Where(s => s.Category == cat).OrderBy(s => s.Name);
                    foreach (var s in snips)
                    {
                        catItem.Nodes.Add(s.Name);
                    }
                }
            } else
            {
                foreach (var s in SnippetManager.Instance.Snippets.Values.OrderBy(s => s.Name))
                {
                    tvSnippets.Nodes.Add(s.Name);
                }
            }
        }

        private void SnippetsManagerForm_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                reloadSnippets();
            }
        }

        private void miDelete_Click(object sender, EventArgs e)
        {
            //var selectedSnippet = lbSnippets.SelectedItem as string;
            //if (MessageBox.Show(string.Format("Delete snippet \"{0}\"?", selectedSnippet), "Warning", 
            //    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            //{
            //    SnippetManager.Instance.RemoveSnippet(selectedSnippet);
            //    lbSnippets.Items.RemoveAt(lbSnippets.SelectedIndex);
            //}
        }

        private void miEdit_Click(object sender, EventArgs e)
        {
            //var dlg = new SnippetEdit();
            //dlg.SnippetText = lbSnippets.SelectedItem as string;
            //if (dlg.ShowDialog() == DialogResult.OK)
            //{
            //    reloadSnippets();
            //}
        }

        private void lbSnippets_DoubleClick(object sender, EventArgs e)
        {
            //if (lbSnippets.SelectedIndex != -1)
            //{
            //    InsertSnippet(lbSnippets.SelectedItem as string);
            //}
        }

        private void miInsert_Click(object sender, EventArgs e)
        {
            //if (lbSnippets.SelectedIndex != -1)
            //{
            //    InsertSnippet(lbSnippets.SelectedItem as string);
            //}
        }

        private void InsertSnippet(string snippet)
        {
            Snippets.SetSnippet(snippet);
        }
    }
}
