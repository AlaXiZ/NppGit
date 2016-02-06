using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NppGit.Forms
{
    public partial class CommandList : Form
    {
        private Dictionary<string, List<MenuItem>> _cmdList;
        public Dictionary<string, List<MenuItem>> Commands
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
