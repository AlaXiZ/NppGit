// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
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

using System.IO;
using NLog;
using NppKate.Common;
using NppKate.Npp;
using NppKate.Core;
using NppKate.Modules.GitRepositories;

namespace NppKate.Modules.GitCore
{
    [Module("Git browser", 3000, "{BF107EE3-C876-49B8-BDC8-D45947E37F28}", "1.0.0")]
    public class GitBrowser : IModule
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        #region IModule
        private IModuleManager _manager = null;
        private int _browserCmdId;
        private RepoBrowser _repoBrowser = null;
        private static IGitRepository _gitRepository;

        public static IGitRepository GitRepository => _gitRepository;

        public void Final()
        {
        }

        private void DoBrowser()
        {
            if (_repoBrowser == null)
            {
                var icon = _manager.ResourceManager.LoadToolbarIcon(Resources.ExternalResourceName.IDB_REPOSITORIES);
                _repoBrowser = _manager.FormManager.BuildForm<RepoBrowser>(_browserCmdId, NppTbMsg.DWS_PARAMSALL | NppTbMsg.DWS_DF_CONT_RIGHT, icon.Handle, (IDockableManager)_manager);
                Settings.Panels.RepoBrowserPanelVisible = true;
            }
            else
            {
                Settings.Panels.RepoBrowserPanelVisible = _manager.FormManager.ToogleVisibleDockableForm(_repoBrowser.Handle);
                _manager.CommandManager.SetCommandChekedState(_browserCmdId, Settings.Panels.RepoBrowserPanelVisible);
            }
        }

        public void Init(IModuleManager manager)
        {

            var fileName = Path.Combine(NppUtils.ConfigDir, Properties.Resources.PluginName, Properties.Resources.RepositoriesXml);
            _gitRepository = new GitRepository(fileName);
            _gitRepository.SwitchByName(Settings.GitCore.LastActiveRepository);

            _manager = manager;

            manager.RegisterService(typeof(IGitRepository), _gitRepository);

            manager.OnTabChangeEvent += ManagerOnTabChangeEvent;
            manager.OnSystemInit += ManagerOnSystemInit;
            manager.OnToolbarRegisterEvent += ManagerOnToolbarRegisterEvent;

            var selfName = GetType().Name;

            _browserCmdId = manager.CommandManager.RegisterCommand(selfName, Properties.Resources.CmdRepositoryBrowser, DoBrowser, Settings.Panels.RepoBrowserPanelVisible);

            manager.CommandManager.RegisterCommand(selfName, Properties.Resources.CmdQuickSearch, DoQuickSearch, false, new ShortcutKey(false, true, false, System.Windows.Forms.Keys.F));

            manager.CommandManager.RegisterSeparator(selfName);

            if (!Settings.CommonSettings.GetToolbarCommandState(selfName, Properties.Resources.CmdRepositoryBrowser))
                Settings.CommonSettings.SetToolbarCommandState(selfName, Properties.Resources.CmdRepositoryBrowser, true);
        }

        private void ManagerOnToolbarRegisterEvent()
        {
            _manager.AddToolbarButton(_browserCmdId, Resources.ExternalResourceName.IDB_REPOSITORIES);
        }

        private void ManagerOnSystemInit()
        {
            if (Settings.Panels.RepoBrowserPanelVisible)
            {
                DoBrowser();
            }
        }

        private void ManagerOnTabChangeEvent(object sender, TabEventArgs e)
        {
            _gitRepository.SwitchByPath(NppUtils.CurrentFilePath);
            /* if (SwitchByPath(NppUtils.CurrentFilePath))
             *   DoActiveRepository();
             * DoDocumentRepositiry(DocumentRepository?.Name, DocumentRepository?.ActiveWorktree?.Branch);
             */
        }

        public bool IsNeedRun => true;

        private void DoQuickSearch()
        {
            var path = _gitRepository.ActiveRepository?.Path;
            if (path == null) return;
            var searchDialog = new TortoiseLogSearch();
            searchDialog.Init((IDockableManager)_manager, 0);
            searchDialog.RepositoryPath = path;
            searchDialog.SearchText = NppUtils.GetSelectedText();
            searchDialog.Show();
        }
        #endregion        
        
    }
}
