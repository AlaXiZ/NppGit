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
    public interface IModuleManager
    {
        event Action OnToolbarRegisterEvent;
        event Action OnSystemInit;
        event EventHandler<TabEventArgs> OnTabChangeEvent;
        event EventHandler<CommandItemClickEventArgs> OnCommandItemClick;

        int RegisterCommandItem(CommandItem menuItem);
        void RegisterDockForm(Type formClass, int cmdId, bool updateWithChangeContext, NppTbMsg uMask = NppTbMsg.DWS_PARAMSALL | NppTbMsg.DWS_DF_CONT_RIGHT, IntPtr? hBitmap = null);
        void RegisterDockForm(int indexId, DockDialogData dlgData);
        void AddToolbarButton(int cmdId, string iconName);
        bool ToogleFormState(int cmdId);
        void SetCheckedMenu(int cmdId, bool isChecked);
        object GetService(Type interfaceType);
        void RegisterService(Type interfaceType, object instance);
        bool ServiceExists(Type interfaceName);

        ResourceManager ResourceManager { get; }
    }
}
