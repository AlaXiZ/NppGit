using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NppGitPlugin
{
    public static class PluginCommands
    {
        private static readonly string ItemTemplate = "<Item FolderName=\"{0}\" PluginEntryName=\"{1}\" PluginCommandItemName=\"{2}\" />";

        private static string GetItemTemplate(string itemName)
        {
            return string.Format(ItemTemplate, Main.PluginName, Main.PluginName, itemName);
        }

        public static void ContextMenu()
        {
            Plugin.NewFile();
            Plugin.AppendText("\t\t<!--Sample menu -->");
            Plugin.NewLine();
            for (int i = 0; i < Plugin._funcItems.Items.Count; i++)
            {
                Plugin.AppendText(GetItemTemplate(Plugin._funcItems.Items[i]._itemName));
                Plugin.NewLine();
            }
            Plugin.SetLang(LangType.L_XML);
        }
    }
}
