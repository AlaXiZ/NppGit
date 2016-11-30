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

namespace NppKate.Common
{
    public class FormManager : IFormManager
    {        
        public T BuildForm<T>(int commandIndex, NppTbMsg dockParam, IntPtr iconHandle, IDockableManager dockableManager) where T : DockDialog, new()
        {
            var formInst = new T();
            var cmdId = Npp.NppInfo.Instance.SearchCmdIdByIndex(commandIndex);
            formInst.Init(dockableManager, cmdId);
            var nppTbData = new NppTbData
            {
                hClient = formInst.Handle,
                dlgID = cmdId,
                hIconTab = (uint)iconHandle,
                pszModuleName = Properties.Resources.PluginName,
                pszName = formInst.Text,
                uMask = dockParam
            };
            Npp.NppUtils.RegisterAsDockDialog(nppTbData);
            return formInst;
        }

        public bool ToogleVisibleDockableForm(IntPtr hwnd)
        {
            var isVisible = true;
            if (Npp.NppUtils.IsVisibleDockForm(hwnd))
            {
                Npp.NppUtils.HideDockForm(hwnd);
                isVisible = false;
            }
            else
                Npp.NppUtils.ShowDockForm(hwnd);
            return isVisible;
        }
    }
}
