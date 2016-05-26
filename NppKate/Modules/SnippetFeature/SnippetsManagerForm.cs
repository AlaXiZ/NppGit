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
        private const string SNIPPET_INDEX = "SNIPPET";
        private const string CATEGORY_INDEX = "CATEGORY";
        private const string CATEGORY_OPEN_INDEX = "CATEGORY_OPEN";
        private bool _isGrouping = true;
        private bool _isHiding = false;
        private string _currentExt = "*";

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
            var node = tvSnippets.SelectedNode;
            miInsert.Enabled = miEdit.Enabled = miDelete.Enabled = node?.ImageKey == SNIPPET_INDEX;
        }

        private void miAdd_Click(object sender, EventArgs e)
        {
            var dlg = new SnippetEdit();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                SaveSnippet(SnippetManager.Instance[dlg.SnippetName]);
            }
        }

        private void reloadSnippets()
        {
            if (_isGrouping != Settings.Snippets.IsGroupByCategory)
            {
                _isGrouping = Settings.Snippets.IsGroupByCategory;
                tvSnippets.BeginUpdate();
                tvSnippets.Nodes.Clear();
                tvSnippets.EndUpdate();
            }
            IOrderedEnumerable<Snippet> allSnippets;
            if (_isGrouping)
            {
                allSnippets = SnippetManager.Instance.Snippets.Values.OrderBy(s => s.Category).OrderBy(s => s.Name);
            }
            else
            {
                allSnippets = SnippetManager.Instance.Snippets.Values.OrderBy(s => s.Name);
            }
            tvSnippets.BeginUpdate();
            foreach(var s in allSnippets)
            {
                SaveSnippet(s);
            }
            tvSnippets.EndUpdate();
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
            var selectedSnippet = tvSnippets.SelectedNode?.Tag as Snippet;
            if (selectedSnippet == null) return;
            if (MessageBox.Show(string.Format("Delete snippet \"{0}\"?", selectedSnippet.Name), "Warning", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                RemoveSnippet(selectedSnippet);
                SnippetManager.Instance.RemoveSnippet(selectedSnippet.Name);
            }
        }

        private void miEdit_Click(object sender, EventArgs e)
        {
            var selectedSnippet = tvSnippets.SelectedNode?.Tag as Snippet;
            if (selectedSnippet == null) return;
            var dlg = new SnippetEdit();
            dlg.SnippetName = selectedSnippet.Name;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (dlg.SnippetName == selectedSnippet.Name)
                {
                    SaveSnippet(SnippetManager.Instance[selectedSnippet.Name]);
                } else
                {
                    RemoveSnippet(selectedSnippet);
                    SaveSnippet(SnippetManager.Instance[dlg.SnippetName]);
                }
            }
        }

        private void miInsert_Click(object sender, EventArgs e)
        {
            var selectedSnippet = tvSnippets.SelectedNode?.Tag as Snippet;
            if (selectedSnippet == null) return;
            InsertSnippet(selectedSnippet.Name);
        }

        private void InsertSnippet(string snippet)
        {
            Snippets.SetSnippet(snippet);
        }

        private TreeNode CreateNode(string name, string index, TreeNode parent = null, object linkedObject = null)
        {
            var node = new TreeNode
            {
                Name = name,
                Text = name,
                ImageKey = index,
                SelectedImageKey = index,
                Tag = linkedObject
            };
            node.ContextMenuStrip = contextMenuSnippets;
            parent?.Nodes.Add(node);
            return node;
        }

        private void SaveSnippet(Snippet snippet)
        {
            if (_isGrouping)
            {
                // »щем, вдруг есть сниппет с таким именем, но в другой категории
                var oldItem = tvSnippets.Nodes.Find(snippet.Name, true)?.FirstOrDefault();
                if (oldItem != null && oldItem.Parent.Name != snippet.Category)
                {
                    var cat = oldItem.Parent;
                    oldItem.Remove();
                    if (cat.Nodes.Count == 0)
                    {
                        cat.Remove();
                    }
                }
                // »щем узел категорий или создаем новый
                var catItem = tvSnippets.Nodes.Find(snippet.Category, false)?.FirstOrDefault() ?? CreateCategory(snippet.Category);
                if (!catItem.Nodes.ContainsKey(snippet.Name))
                    CreateNode(snippet.Name, SNIPPET_INDEX, catItem, snippet);
            }
            else
            {
                if (!tvSnippets.Nodes.ContainsKey(snippet.Name))
                    tvSnippets.Nodes.Add(CreateNode(snippet.Name, SNIPPET_INDEX, null, snippet));
            }
        }

        private void RemoveSnippet(Snippet snippet)
        {
            tvSnippets.BeginUpdate();
            try
            {
                tvSnippets.Nodes.Find(snippet.Name, true)?.FirstOrDefault()?.Remove();
            }
            finally
            {
                tvSnippets.EndUpdate();
            }
        }

        private TreeNode CreateCategory(string category)
        {
            var node = CreateNode(category, CATEGORY_INDEX);
            tvSnippets.Nodes.Add(node);
            return node;
        }

        private void tvSnippets_AfterExpand(object sender, TreeViewEventArgs e)
        {
            if (e.Node.ImageKey == CATEGORY_INDEX)
            {
                e.Node.ImageKey = CATEGORY_OPEN_INDEX;
                e.Node.SelectedImageKey = CATEGORY_OPEN_INDEX;
            }
        }

        private void tvSnippets_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            if (e.Node.ImageKey == CATEGORY_OPEN_INDEX)
            {
                e.Node.ImageKey = CATEGORY_INDEX;
                e.Node.SelectedImageKey = CATEGORY_INDEX;
            }
        }

        private void tvSnippets_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.ImageKey == SNIPPET_INDEX)
            {
                InsertSnippet((e.Node.Tag as Snippet).Name);
            }
        }
    }
}
