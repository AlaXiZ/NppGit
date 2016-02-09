using NLog;
using NppGit.Utils;

namespace NppGit.Modules
{
    delegate void InsertSnippet(string snippetName);

    public class Snippets : IModule
    {
        private static event InsertSnippet InsertSnippetEvent;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        IModuleManager _manager;
        int _snipManagerId = 0;

        public bool IsNeedRun
        {
            get { return Settings.Modules.Snippets; }
        }

        public void Final()
        {
        }

        public void Init(IModuleManager manager)
        {
            _manager = manager;
            // Load snippets ---------------------------------------------------
            foreach(var i in SnippetManager.Instance.Snippets)
            {
                _snipManagerId = _manager.RegisterMenuItem(new MenuItem
                {
                    Name = i.Value.Name,
                    Hint = i.Value.Name,
                    Action = () => { logger.Debug("Snippet clicked"); }
                });
            }
            // -----------------------------------------------------------------
            _snipManagerId = _manager.RegisterMenuItem(new MenuItem
            {
                Name = "Snippet manager",
                Hint = "Snippet manager",
                Action = DoSnippetsManager,
                Checked = Settings.Panels.SnippetsPanelVisible
            });

            _manager.RegisterDockForm(typeof(Forms.SnippetsManagerForm), _snipManagerId, false);

            // -----------------------------------------------------------------
            _manager.RegisterMenuItem(new MenuItem
            {
                Name = "-",
                Hint = "-",
                Action = null
            });

            _manager.OnToolbarRegisterEvent += ToolbarRegister;
            _manager.OnMenuItemClick += ManagerOnMenuItemClick;
            _manager.OnSystemInit += ManagerOnSystemInit;
            InsertSnippetEvent += SnippetsOnInsertSnippetEvent;
        }

        private void ManagerOnSystemInit()
        {
            if (Settings.Panels.SnippetsPanelVisible)
            {
                DoSnippetsManager();
            }
        }

        private void ToolbarRegister()
        {
            _manager.AddToolbarButton(_snipManagerId, Properties.Resources.snippets_bar);
        }

        private void ManagerOnMenuItemClick(MenuItemClickEventArgs args)
        {
            SnippetsOnInsertSnippetEvent(args.MenuName);
        }

        private void SnippetsOnInsertSnippetEvent(string snippetName)
        {
            // Insert 
            if (SnippetManager.Instance.Contains(snippetName)) 
            {
                logger.Debug("Insert snippet {0}", snippetName);
                Snippet snip = SnippetManager.Instance[snippetName];
                var outLines = snip.Assemble(PluginUtils.GetSelectedText());
                PluginUtils.ReplaceSelectedText(outLines);
            }
        }

        // ---------------------------------------------------------------------
        private void DoSnippetsManager()
        {
            Settings.Panels.SnippetsPanelVisible = _manager.ToogleFormState(_snipManagerId);
        }
        // ---------------------------------------------------------------------
        public static void SetSnippet(string snippet)
        {
            if (InsertSnippetEvent != null)
            {
                InsertSnippetEvent(snippet);
            }
        }
    }
}
