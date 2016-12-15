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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using NLog;
using NppKate.Common;
using NppKate.Modules.SnippetFeature;

namespace NppKate.Forms
{
    public partial class SnippetsManagerForm : DockDialog, FormDockable
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        private const string SNIPPET_INDEX = "SNIPPET";
        private const string CATEGORY_INDEX = "CATEGORY";
        private const string CATEGORY_OPEN_INDEX = "CATEGORY_OPEN";
        private const string UNDEFINED_EXT = "?";
        private const string ALL_EXT = "*";
        private bool _isGrouping;
        private bool _isLoaded = false;
        private bool _isNewTree = true;
        private List<TreeNode> _needShow = new List<TreeNode>();
        private List<TreeNode> _needHide = new List<TreeNode>();
        private ISnippetManager _snippetManager;

        private string _currentExt = UNDEFINED_EXT;

        public SnippetsManagerForm()
        {
            InitializeComponent();
            _isGrouping = Settings.Snippets.IsGroupByCategory;
        }

        public Bitmap TabIcon => Properties.Resources.snippets;

        public string Title => "Snippets manager";

        public void ChangeContext()
        {
            if (Visible && Settings.Snippets.IsHideByExtention)
            {
                var newExt = Npp.NppUtils.CurrentFileExt;
                newExt = newExt == "" ? ALL_EXT : newExt;
                if (newExt != _currentExt)
                {
                    _currentExt = newExt;
                    ReloadSnippets();
                }
            }
        }

        protected override void OnSwitchIn()
        {
            base.OnSwitchIn();
            if (!_isLoaded || _currentExt == UNDEFINED_EXT)
            {
                ReloadSnippets();
                _isLoaded = true;
            }
        }

        protected override void OnSwitchOut()
        {
            base.OnSwitchOut();
            _currentExt = UNDEFINED_EXT;
        }

        protected override void AfterInit()
        {
            _snippetManager = _manager.ModuleManager.GetService(typeof(ISnippetManager)) as ISnippetManager;
        }

        private void contextMenuSnippets_Opening(object sender, CancelEventArgs e)
        {
            var node = tvSnippets.SelectedNode;
            miInsert.Enabled = miEdit.Enabled = miDelete.Enabled = node?.ImageKey == SNIPPET_INDEX;
            miExtract.Enabled = Npp.NppUtils.HasSelected;
        }

        private void miAdd_Click(object sender, EventArgs e)
        {
            var dlg = new SnippetEdit();
            dlg.Init(_manager, 0);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                SaveSnippet(_snippetManager.FindByName(dlg.SnippetName));
            }
        }

        private void ReloadSnippets()
        {
            if (_currentExt == UNDEFINED_EXT)
                _currentExt = Npp.NppUtils.CurrentFileExt;

            if (_isGrouping != Settings.Snippets.IsGroupByCategory || Settings.Snippets.IsHideByExtention)
            {
                _isGrouping = Settings.Snippets.IsGroupByCategory;
                tvSnippets.BeginUpdate();
                try
                {
                    _isNewTree = true;
                    tvSnippets.Nodes.Clear();
                }
                finally
                {
                    tvSnippets.EndUpdate();
                }
            }
            var snippetList = _snippetManager.GetAllSnippets();
            var filtered = Settings.Snippets.IsHideByExtention && _currentExt != ALL_EXT ? snippetList.Where(s => s.FileExt.Contains(_currentExt) || s.FileExt.Contains(ALL_EXT)) : snippetList;
            var allSnippets = _isGrouping ? filtered.OrderBy(s => s.Category).ThenBy(s => s.Name) : filtered.OrderBy(s => s.Name);

            tvSnippets.BeginUpdate();
            try
            {
                foreach (var s in allSnippets)
                {
                    SaveSnippet(s);
                }
            }
            finally
            {
                tvSnippets.EndUpdate();
            }
            // ≈сли нет узлов, то повесим меню на дерево
            tvSnippets.ContextMenuStrip = tvSnippets.Nodes.Count != 0 ? null : contextMenuSnippets;
            if (_isNewTree && Settings.Snippets.IsExpanAfterCreate)
            {
                tvSnippets.ExpandAll();
                _isNewTree = false;
            }
        }

        private void miDelete_Click(object sender, EventArgs e)
        {
            var selectedSnippet = tvSnippets.SelectedNode?.Tag as Snippet;
            if (selectedSnippet == null) return;
            if (MessageBox.Show($"Delete snippet \"{selectedSnippet.Name}\"?", "Warning",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                RemoveSnippet(selectedSnippet);
                _snippetManager.Remove(selectedSnippet.Name);
            }
        }

        private void miEdit_Click(object sender, EventArgs e)
        {
            var selectedSnippet = tvSnippets.SelectedNode?.Tag as Snippet;
            if (selectedSnippet == null) return;
            var dlg = new SnippetEdit();
            dlg.Init(_manager, 0);
            dlg.SnippetName = selectedSnippet.Name;
            dlg.Action = SnippetEditAction.Update;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (dlg.SnippetName == selectedSnippet.Name)
                {
                    SaveSnippet(_snippetManager.FindByName(selectedSnippet.Name));
                }
                else
                {
                    RemoveSnippet(selectedSnippet);
                    SaveSnippet(_snippetManager.FindByName(dlg.SnippetName));
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
                Tag = linkedObject,
                ContextMenuStrip = contextMenuSnippets
            };
            parent?.Nodes.Add(node);
            return node;
        }

        private void SaveSnippet(Snippet snippet)
        {
            if (_isGrouping)
            {
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
            node.Expand();
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
                InsertSnippet((e.Node.Tag as Snippet)?.Name);
            }
        }

        private void miRefresh_Click(object sender, EventArgs e)
        {
            ReloadSnippets();
        }

        private void tvSnippets_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (tvSnippets.SelectedNode != e.Node)
            {
                tvSnippets.SelectedNode = e.Node;
            }
        }

        private void miExtract_Click(object sender, EventArgs e)
        {
            var dlg = new SnippetEdit();
            dlg.Init(_manager, 0);
            dlg.SnippetText = Npp.NppUtils.GetSelectedText();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                SaveSnippet(_snippetManager.FindByName(dlg.SnippetName));
            }
        }
    }
}
