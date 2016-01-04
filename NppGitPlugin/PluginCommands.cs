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

        public static int ShowBranchID { get; set; }

        public static void ShowBranch()
        {
            Settings.Instance.FuncSet.ShowBranch = !Settings.Instance.FuncSet.ShowBranch;
            Win32.SendMessage(PluginUtils.nppData._nppHandle, NppMsg.NPPM_SETMENUITEMCHECK, PluginUtils._funcItems.Items[ShowBranchID]._cmdID, Settings.Instance.FuncSet.ShowBranch ? 1 : 0);
            DoShowBranch(Settings.Instance.FuncSet.ShowBranch);
        }

        public static void DoShowBranch(bool isShow = true)
        {
            var repoDir = PluginUtils.GetRootDir(PluginUtils.CurrentFileDir);
            if (!string.IsNullOrEmpty(repoDir) && LibGit2Sharp.Repository.IsValid(repoDir))
            {
                using (var repo = new LibGit2Sharp.Repository(repoDir))
                {
                    var branch = " [Branch: " + repo.Head.Name + "]";
                    if (isShow)
                    {
                        PluginUtils.WindowTitle += branch;
                    }
                    else
                    {
                        var title = PluginUtils.WindowTitle;
                        PluginUtils.WindowTitle = title.Replace(branch, "");
                    }
                }
            }
        }
    }

}
