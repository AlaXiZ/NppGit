using System;
using System.IO;
using System.Windows.Forms;

namespace NppGitPlugin
{
    public static class PluginCommands
    {
        private static readonly string ItemTemplate = "<Item FolderName=\"{0}\" PluginEntryName=\"{1}\" PluginCommandItemName=\"{2}\" />";
        private static readonly string ItemSeparator = "<Item FolderName=\"{0}\" id = \"0\" />";

        private static string GetItemTemplate(string itemName)
        {
            if (itemName == "-")
                return string.Format(ItemSeparator, Plugin.PluginName);
            else
                return string.Format(ItemTemplate, Plugin.PluginName, Plugin.PluginName, itemName);
        }

        public static void ContextMenu()
        {
            PluginUtils.NewFile();
            PluginUtils.AppendText("\t\t<!--Sample menu -->");
            PluginUtils.NewLine();
            for (int i = 0; i < PluginUtils._funcItems.Items.Count; i++)
            {
                PluginUtils.AppendText(GetItemTemplate(PluginUtils._funcItems.Items[i]._itemName));
                PluginUtils.NewLine();
            }
            PluginUtils.SetLang(LangType.L_XML);
        }

        public static void Branch()
        {
            var repoDir = PluginUtils.GetRootDir(PluginUtils.CurrentFileDir);
            try
            {
                if (LibGit2Sharp.Repository.IsValid(repoDir))
                {
                    using (var repo = new LibGit2Sharp.Repository(repoDir))
                    {
                        foreach (var br in repo.Branches)
                        {
                            if (br.IsCurrentRepositoryHead)
                            {
                                MessageBox.Show(br.Name);
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                //
            }

        }
    }
}
