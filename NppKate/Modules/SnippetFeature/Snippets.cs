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
using System.IO;
using System.Linq;

namespace NppKate.Modules.SnippetFeature
{
    internal delegate void InsertSnippet(string snippetName);

    public class Snippets : IModule
    {
        #region Commands Name
        const string ExpandSnippet = "Expand snippet";
        const string SnippetManager = "Snippet manager";
        #endregion
        private static event InsertSnippet InsertSnippetEvent;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private IModuleManager _manager;
        private int _snipManagerId;
        private Forms.SnippetsManagerForm _managerForm;
        private ISnippetManager _snippetManager;

        public bool IsNeedRun => Settings.Modules.Snippets;

        public void Final()
        {
        }

        public void Init(IModuleManager manager)
        {
            _manager = manager;

            _snippetManager = new SnippetManager(Path.Combine(NppUtils.ConfigDir, Properties.Resources.PluginName, Properties.Resources.SnippetsXml));

            _manager.RegisterService(typeof(ISnippetManager), _snippetManager);
            _manager.RegisterService(typeof(ISnippetValidator), new SnippetValidator());

            var selfName = GetType().Name;
            // Load snippets ---------------------------------------------------
            foreach (var i in _snippetManager.GetAllSnippets())
            {
                if (i.IsVisible)
                {
                    _manager.CommandManager.RegisterCommand(selfName, i.Name, () => { Logger.Debug("Snippet clicked"); });
                }
            }
            // -----------------------------------------------------------------
            _manager.CommandManager.RegisterCommand(selfName, ExpandSnippet, DoExpandSnippet, false, new ShortcutKey("Ctrl+Shift+X"));
            _snipManagerId = _manager.CommandManager.RegisterCommand(selfName, SnippetManager, DoSnippetsManager, Settings.Panels.SnippetsPanelVisible);

            _manager.CommandManager.RegisterSeparator(selfName);
            _manager.OnToolbarRegisterEvent += ToolbarRegister;
            _manager.OnCommandItemClick += ManagerOnMenuItemClick;
            _manager.OnSystemInit += ManagerOnSystemInit;
            InsertSnippetEvent += SnippetsOnInsertSnippetEvent;

            if (!Settings.CommonSettings.GetToolbarCommandState(selfName, SnippetManager))
                Settings.CommonSettings.SetToolbarCommandState(selfName, SnippetManager, true);
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
            _manager.AddToolbarButton(_snipManagerId, Resources.ExternalResourceName.IDB_SNIPPETS);
        }

        private void ManagerOnMenuItemClick(object sender, CommandItemClickEventArgs args)
        {
            SnippetsOnInsertSnippetEvent(args.CommandName);
        }

        private void SnippetsOnInsertSnippetEvent(string snippetName)
        {
            // Insert 
            var snippet = _snippetManager.FindByName(snippetName);
            if (snippet == Snippet.Null) return;
            Logger.Debug($"Insert snippet {snippetName}");
            var builder = GetTextBuilder();
            var outLines = builder.BuildText(snippet, NppUtils.GetSelectedText());
            NppUtils.ReplaceSelectedText(outLines);
        }
        // ---------------------------------------------------------------------
        private void DoSnippetsManager()
        {

            if (_managerForm == null)
            {
                var icon = _manager.ResourceManager.LoadToolbarIcon(Resources.ExternalResourceName.IDB_SNIPPETS);
                _managerForm = _manager.FormManager.BuildForm<Forms.SnippetsManagerForm>(_snipManagerId, NppTbMsg.DWS_PARAMSALL | NppTbMsg.DWS_DF_CONT_RIGHT, icon.Handle, (IDockableManager)_manager);
                Settings.Panels.SnippetsPanelVisible = true;
            }
            else
            {
                Settings.Panels.SnippetsPanelVisible = _manager.FormManager.ToogleVisibleDockableForm(_managerForm.Handle);
                _manager.CommandManager.SetCommandChekedState(_snipManagerId, Settings.Panels.SnippetsPanelVisible);
            }
        }

        private void DoExpandSnippet()
        {
            var inputParams = NppUtils.GetSelectedText().Trim().Split(' ');
            if (inputParams.Length == 0) return;
            var snippet = _snippetManager.FindByBothName(inputParams[0]);
            if (snippet == Snippet.Null) return;
            var param = inputParams.Length > 1 ? inputParams.Where(s => s != inputParams[0]).Aggregate((s1, s2) => s1 + " " + s2) : "";
            var builder = GetTextBuilder();
            var outLines = builder.BuildText(snippet, param);
            NppUtils.ReplaceSelectedText(outLines);
        }

        private ISnippetTextBuilder GetTextBuilder()
        {
            return new SnippetTextBuilder(_snippetManager, Settings.Snippets.InsertEmpty, Settings.Snippets.MaxLevel);
        }
        // ---------------------------------------------------------------------
        public static void SetSnippet(string snippet)
        {
            InsertSnippetEvent?.Invoke(snippet);
        }
    }
}
