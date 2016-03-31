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
using System.Linq;
using System.Windows.Forms;

namespace NppKate.Forms
{
    public partial class CommandList : Form
    {
        private Dictionary<string, List<Common.CommandItem>> _cmdList;
        public Dictionary<string, List<Common.CommandItem>> Commands
        {
            get { return _cmdList; }
            set
            {
                _cmdList = value;
                _cmdList.Remove("ModuleManager");
                RefreshCommands();
            }
        }

        private void RefreshCommands()
        {
            tvCommands.BeginUpdate();
            tvCommands.Nodes.Clear();
            foreach(var k in _cmdList.Keys)
            {
                var item = tvCommands.Nodes.Add(k);
                item.Checked = true;
                item.Name = k;
                foreach (var i in _cmdList[k])
                {
                    if (i.Name != "-")
                    {
                        var subitem = item.Nodes.Add(i.Hint);
                        subitem.Name = i.Name;
                        subitem.Checked = true;
                    }
                }
                if (item.Nodes.Count == 0)
                {
                    tvCommands.Nodes.Remove(item);
                }
            }
            tvCommands.EndUpdate();
            tvCommands.ExpandAll();
        }

        public CommandList()
        {
            InitializeComponent();
        }

        private void bOk_Click(object sender, EventArgs e)
        {
            foreach (var k in _cmdList.Keys)
            {
                var clazz = tvCommands.Nodes.Find(k, true).FirstOrDefault();
                if (clazz != null)
                {
                    for (var i = 0; i < _cmdList[k].Count - 1; i++)
                    {
                        if (_cmdList[k][i].Name != "-")
                        {
                            var item = tvCommands.Nodes.Find(_cmdList[k][i].Name, true).FirstOrDefault();
                            _cmdList[k][i].Selected = item?.Checked ?? false;
                        }
                    }
                }
            }
            DialogResult = DialogResult.OK;
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void tvCommands_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Action == TreeViewAction.Unknown)
            {
                return;
            }
            if (e.Node.Nodes.Count > 0)
            {
                foreach (TreeNode n in e.Node.Nodes)
                {
                    n.Checked = e.Node.Checked;
                }
            }
            if (e.Node.Parent != null && e.Node.Checked)
            {
                e.Node.Parent.Checked = true;
            }
        }
    }
}
