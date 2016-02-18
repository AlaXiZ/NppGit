using NppGit.Modules.SnippetFeature;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace NppGit.Forms
{
    public partial class SnippetsManagerForm : Form, FormDockable
    {
        public SnippetsManagerForm()
        {
            InitializeComponent();
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
            var isSelected = lbSnippets.SelectedIndex != -1;
            miInsert.Enabled = miEdit.Enabled = miDelete.Enabled = isSelected;
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
            lbSnippets.BeginUpdate();
            try
            {
                lbSnippets.Items.Clear();
                foreach(var s in SnippetManager.Instance.Snippets)
                {
                    lbSnippets.Items.Add(s.Value.Name);
                }
            }
            finally
            {
                lbSnippets.EndUpdate();
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
            var selectedSnippet = lbSnippets.SelectedItem as string;
            if (MessageBox.Show(string.Format("Delete snippet \"{0}\"?", selectedSnippet), "Warning", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                SnippetManager.Instance.RemoveSnippet(selectedSnippet);
                lbSnippets.Items.RemoveAt(lbSnippets.SelectedIndex);
            }
        }

        private void miEdit_Click(object sender, EventArgs e)
        {
            var dlg = new SnippetEdit();
            dlg.SnippetText = lbSnippets.SelectedItem as string;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                reloadSnippets();
            }
        }

        private void lbSnippets_DoubleClick(object sender, EventArgs e)
        {
            if (lbSnippets.SelectedIndex != -1)
            {
                InsertSnippet(lbSnippets.SelectedItem as string);
            }
        }

        private void miInsert_Click(object sender, EventArgs e)
        {
            if (lbSnippets.SelectedIndex != -1)
            {
                InsertSnippet(lbSnippets.SelectedItem as string);
            }
        }

        private void InsertSnippet(string snippet)
        {
            Snippets.SetSnippet(snippet);
        }
    }
}
