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
using System.Collections.Generic;
using System.Linq;

namespace NppKate.Common
{
    public class CommandManager : ICommandManager
    {
        const int DefaultCapacity = 5;
        private volatile int HiddenIndex = -1;
        private volatile bool _LastItemIsSeparator = true;

        private Dictionary<string, List<CommandMenuItem>> _commandIndexes;

        public CommandManager()
        {
            _commandIndexes = new Dictionary<string, List<CommandMenuItem>>(DefaultCapacity);
        }

        public Dictionary<string, List<CommandMenuItem>> GetCommands()
        {
            return _commandIndexes;
        }

        public List<CommandMenuItem> GetCommandsByModule(string module)
        {
            if (_commandIndexes.ContainsKey(module))
            {
                return _commandIndexes[module];
            }
            return new List<CommandMenuItem>(0);
        }

        public int GetIdByIndex(int commandIndex)
        {
            return Npp.NppInfo.Instance.SearchCmdIdByIndex(commandIndex);
        }

        public string GetNameByIndex(string module, int commandIndex)
        {
            return _commandIndexes[module].Where(cm => cm.CommandIndex == commandIndex).Select(cm => cm.Name).FirstOrDefault() ?? "";
        }

        public int RegisterCommand(string module, string name, Action commandHandler = null, bool isCheckedWithStart = false, ShortcutKey? shortcut = null)
        {
            if (_LastItemIsSeparator && "-".Equals(name))
                return 0;

            if (!_commandIndexes.ContainsKey(module))
            {
                _commandIndexes.Add(module, new List<CommandMenuItem>(DefaultCapacity));
            }
            if (!Settings.CommonSettings.IsSetDefaultShortcut)
                shortcut = null;
            int cmdIndex = 0;
            if (Settings.CommonSettings.GetCommandState(module, name))
                cmdIndex = Npp.NppInfo.Instance.AddCommand(name, commandHandler, shortcut, isCheckedWithStart);
            else
                cmdIndex = HiddenIndex--;

            _commandIndexes[module].Add(new CommandMenuItem
            {
                Name = name,
                CommandIndex = cmdIndex
            });
            _LastItemIsSeparator = "-".Equals(name);
            return cmdIndex;
        }

        public void RegisterSeparator(string module)
        {
            RegisterCommand(module, "-");
        }

        public void SetCommandChekedState(int commandIndex, bool isChecked)
        {
            var commandId = Npp.NppInfo.Instance.SearchCmdIdByIndex(commandIndex);
            Npp.NppUtils.SetCheckedMenu(commandId, isChecked);
        }
    }
}
