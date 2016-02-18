using NppGit.Modules.SnippetFeature;
using System;
using System.Windows.Forms;

namespace NppGit.Forms
{
    public partial class SnippetEdit : Form
    {
        public SnippetEdit()
        {
            InitializeComponent();
        }

        private void bOk_Click(object sender, EventArgs e)
        {
            if (!Modules.SnippetFeature.Snippet.CheckCorrectSnippet(tbSnippet.Text))
            {
                MessageBox.Show("Snippet is bad!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                DialogResult = DialogResult.None;
                return;
            }
            try
            {
                if (string.IsNullOrEmpty(_snippet))
                {
                    Modules.SnippetFeature.SnippetManager.Instance.AddSnippet(new Snippet(tbName.Text, tbSnippet.Text));
                }
                else
                {
                    Modules.SnippetFeature.SnippetManager.Instance.UpdateSnippet(_snippet, new Snippet(tbName.Text, tbSnippet.Text));
                }
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
            tbSnippet.Text = s.SnippetText.Replace("\n", "\r\n");
        }

        private string _snippet;
        public string SnippetText
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
