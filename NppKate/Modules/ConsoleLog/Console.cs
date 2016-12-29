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

using System;
using NppKate.Common;
using NppKate.Core;

namespace NppKate.Modules.ConsoleLog
{
    [Module("Action log", 0, "{5D3520F9-7E9C-40C7-8968-87D12D8ED53E}", "1.0.0")]
    public class Console : IModule
    {
        private IModuleManager _manager;
        private int _consoleId;
        private LogForm _consoleForm = null;

        public void Finalization()
        {
        }

        public void Initialization()
        {
            var selfName = GetType().Name;
            _consoleId = _manager.CommandManager.RegisterCommand(selfName, Properties.Resources.CmdConsoleLog, DoConsoleLog, Settings.Panels.ConsoleLogPanelVisible);
            _manager.OnSystemInit += ManagerOnSystemInit;
            _manager.OnToolbarRegisterEvent += ManagerOnToolbarRegisterEvent;

            if (!Settings.CommonSettings.GetToolbarCommandState(selfName, Properties.Resources.CmdConsoleLog))
                Settings.CommonSettings.SetToolbarCommandState(selfName, Properties.Resources.CmdConsoleLog, true);
        }

        private void ManagerOnToolbarRegisterEvent()
        {
            _manager.AddToolbarButton(_consoleId, Resources.ExternalResourceName.IDB_CONSOLE);
        }

        private void ManagerOnSystemInit()
        {
            if (Settings.Panels.ConsoleLogPanelVisible)
                DoConsoleLog();
        }

        private void DoConsoleLog()
        {
            if (_consoleForm == null)
            {
                var icon = _manager.ResourceManager.LoadToolbarIcon(Resources.ExternalResourceName.IDB_CONSOLE);
                _consoleForm = _manager.FormManager.BuildForm<LogForm>(_consoleId, NppTbMsg.DWS_PARAMSALL | NppTbMsg.DWS_DF_CONT_BOTTOM, icon.Handle, (IDockableManager)_manager);
                Settings.Panels.ConsoleLogPanelVisible = true;
            }
            else
            {
                Settings.Panels.ConsoleLogPanelVisible = _manager.FormManager.ToogleVisibleDockableForm(_consoleForm.Handle);
                _manager.CommandManager.SetCommandChekedState(_consoleId, Settings.Panels.ConsoleLogPanelVisible);
            }
        }

        public void Context(IModuleManager manager)
        {
            _manager = manager;
        }

        public void Registration()
        {
        }
    }
}
