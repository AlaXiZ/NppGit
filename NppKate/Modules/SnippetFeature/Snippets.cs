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

using NLog;
using NppKate.Common;
using NppKate.Npp;

namespace NppKate.Modules.SnippetFeature
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
                if (i.Value.IsShowInMenu)
                {
                    _manager.RegisteCommandItem(new CommandItem
                    {
                        Name = i.Value.Name,
                        Hint = i.Value.Name,
                        Action = () => { logger.Debug("Snippet clicked"); }
                    });
                }
            }
            // -----------------------------------------------------------------
            _snipManagerId = _manager.RegisteCommandItem(new CommandItem
            {
                Name = "Snippet manager",
                Hint = "Snippet manager",
                Action = DoSnippetsManager,
                Checked = Settings.Panels.SnippetsPanelVisible
            });

            _manager.RegisterDockForm(typeof(Forms.SnippetsManagerForm), _snipManagerId, false);

            // -----------------------------------------------------------------
            _manager.RegisteCommandItem(new CommandItem
            {
                Name = "-",
                Hint = "-",
                Action = null
            });

            _manager.OnToolbarRegisterEvent += ToolbarRegister;
            _manager.OnCommandItemClick += ManagerOnMenuItemClick;
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

        private void ManagerOnMenuItemClick(object sender, CommandItemClickEventArgs args)
        {
            SnippetsOnInsertSnippetEvent(args.CommandName);
        }

        private void SnippetsOnInsertSnippetEvent(string snippetName)
        {
            // Insert 
            if (SnippetManager.Instance.Contains(snippetName)) 
            {
                logger.Debug("Insert snippet {0}", snippetName);
                Snippet snip = SnippetManager.Instance[snippetName];
                var outLines = snip.Assemble(NppUtils.GetSelectedText());
                NppUtils.ReplaceSelectedText(outLines);
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
