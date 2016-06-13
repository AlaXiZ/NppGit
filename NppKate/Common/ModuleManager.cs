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
using NppKate.Forms;
using NppKate.Interop;
using NppKate.Npp;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace NppKate.Common
{
    public class ModuleManager : IModuleManager, IDockableManager
    {
        private const int TOOLBAR_ICON_SIZE = 16;

        private static Logger logger = LogManager.GetCurrentClassLogger();

        private LinkedList<IModule> _modules;
        private bool _canEvent = false;
        private ulong _currentFormId = ulong.MaxValue;
        private LocalWindowsHook winHookProcRet;
        private LocalWindowsHook winHookProc;
        private Dictionary<Type, object> _services;
        private ResourceManager _resourceManager = null;
        private ICommandManager _commandManager = null;
        private IFormManager _formManager = null;

        public ResourceManager ResourceManager => _resourceManager;

        public ICommandManager CommandManager => _commandManager;

        public IFormManager FormManager => _formManager;

        IModuleManager IDockableManager.ModuleManager => this;

        public event Action OnToolbarRegisterEvent;
        public event Action OnSystemInit;
        public event EventHandler<TabEventArgs> OnTabChangeEvent;
        public event EventHandler<CommandItemClickEventArgs> OnCommandItemClick;

        public ModuleManager(ICommandManager commandManager, IFormManager formManager)
        {
            _modules = new LinkedList<IModule>();
            _services = new Dictionary<Type, object>();
            //
            _commandManager = commandManager;
            _formManager = formManager;
            //

        }

        public void MessageProc(SCNotification sn)
        {
            if (!_canEvent)
                return;
            switch (sn.nmhdr.code)
            {
                case (uint)NppMsg.NPPN_FILEOPENED:
                case (uint)NppMsg.NPPN_FILESAVED:
                    {
                        _currentFormId = 0;
                        DoTabChangeEvent(sn.nmhdr.idFrom);
                        break;
                    }

                case (uint)NppMsg.NPPN_BUFFERACTIVATED:
                    {
                        if (sn.nmhdr.idFrom != _currentFormId)
                        {
                            _currentFormId = sn.nmhdr.idFrom;
                            DoTabChangeEvent(sn.nmhdr.idFrom);
                        }
                        break;
                    }

                case (uint)NppMsg.NPPN_READY:
                    {
                        DoSystemInit();
                        break;
                    }

                case (uint)NppMsg.NPPN_SHUTDOWN:
                    {
                        logger.Debug("Notepad++ is shutdown");
                        break;
                    }
            }
        }

        private void DoSystemInit()
        {
            OnSystemInit?.Invoke();
        }

        private void DoTabChangeEvent(uint idFormChanged)
        {
            if (!_canEvent)
                return;
            OnTabChangeEvent?.Invoke(this, new TabEventArgs(idFormChanged));
        }

        public void Final()
        {
            _canEvent = false;
            if (winHookProcRet != null && winHookProcRet.IsInstalled)
                winHookProcRet.Uninstall();
            if (winHookProc != null && winHookProc.IsInstalled)
                winHookProc.Uninstall();
            foreach (var m in _modules)
                if (m.IsNeedRun)
                    m.Final();
        }

        public void Init()
        {
            _resourceManager = new ResourceManager();
            foreach (var m in _modules)
            {
                if (m.IsNeedRun)
                    m.Init(this);
            }

            //RegisterCommandItem(new CommandItem { Name = "Sample context menu", Hint = "Sample context menu", Action = DoContextMenu });

            winHookProcRet = new LocalWindowsHook(HookType.WH_CALLWNDPROCRET);
            winHookProcRet.HookInvoked += WinHookHookInvoked;
            winHookProcRet.Install();

            winHookProc = new LocalWindowsHook(HookType.WH_GETMESSAGE);
            winHookProc.HookInvoked += WinHookHookInvoked_GetMsg;
            winHookProc.Install();

            _canEvent = true;
        }

        private void WinHookHookInvoked_GetMsg(object sender, HookEventArgs e)
        {
            if (!_canEvent)
                return;
            MSG msg = (Interop.MSG)Marshal.PtrToStructure(e.lParam, typeof(Interop.MSG));
            if (msg.message == (uint)WinMsg.WM_COMMAND)
            {
                // Item click
                if ((uint)msg.lParam == 0 && (uint)msg.wParam >> 16 == 0)
                {
                    DoMenuItem((uint)msg.wParam & ushort.MaxValue);
                }
            }
        }

        private void WinHookHookInvoked(object sender, HookEventArgs e)
        {
            if (!_canEvent)
                return;
            CWPRETSTRUCT msg = (CWPRETSTRUCT)Marshal.PtrToStructure(e.lParam, typeof(CWPRETSTRUCT));
            if (msg.message == (uint)WinMsg.WM_COMMAND)
            {
                // Shortcut
                if ((uint)msg.lParam == 0 && (uint)msg.wParam >> 16 == 1)
                {
                    DoMenuItem((uint)msg.wParam & ushort.MaxValue);
                }
            }
        }

        private void DoMenuItem(uint menuId)
        {
            string menuName = "";

            foreach (var item in NppInfo.Instance.FuncItems.Items)
            {
                if (item._cmdID == menuId)
                {
                    menuName = item._itemName;
                    break;
                }
            }

            logger.Debug("MenuID = {0}, MenuName = {1}", menuId, menuName);
            if (!string.IsNullOrEmpty(menuName) && OnCommandItemClick != null)
            {
                OnCommandItemClick(this, new CommandItemClickEventArgs(menuName));
            }
        }

        public void ToolBarInit()
        {
            OnToolbarRegisterEvent?.Invoke();
        }

        public void AddModule(IModule item)
        {
            if (!_modules.Contains(item))
            {
                _modules.AddLast(item);
            }
        }
   
        public void AddToolbarButton(int cmdId, string iconName)
        {
            toolbarIcons tbIcons = new toolbarIcons();
            tbIcons.hToolbarBmp = _resourceManager.LoadImage(iconName, TOOLBAR_ICON_SIZE, TOOLBAR_ICON_SIZE);
            IntPtr pTbIcons = Marshal.AllocHGlobal(Marshal.SizeOf(tbIcons));
            Marshal.StructureToPtr(tbIcons, pTbIcons, false);
            Win32.SendMessage(NppInfo.Instance.NppHandle, NppMsg.NPPM_ADDTOOLBARICON, NppInfo.Instance.SearchCmdIdByIndex(cmdId), pTbIcons);
            Marshal.FreeHGlobal(pTbIcons);
        }
           
        public object GetService(Type interfaceType)
        {
            return _services[interfaceType];
        }

        public void RegisterService(Type interfaceType, object instance)
        {
            if (_services.ContainsKey(interfaceType)) return;
            _services.Add(interfaceType, instance);
        }

        public bool ServiceExists(Type interfaceType)
        {
            return _services.ContainsKey(interfaceType);
        }

        #region "Context menu"
        private static readonly string ItemTemplate = "<Item FolderName=\"{0}\" PluginEntryName=\"{1}\" PluginCommandItemName=\"{2}\" ItemNameAs=\"{3}\"/>";
        private static readonly string ItemSeparator = "<Item FolderName=\"{0}\" id = \"0\" />";
        private static readonly string ItemSeparator2 = "<Item id=\"0\" />";

        private static string GetItemTemplate(string folder = "", string itemName = "---", string itemNameAs = "---")
        {
            if (itemName == "---" && itemNameAs == "---")
                return ItemSeparator2;
            else if (itemName == "-")
                return string.Format(ItemSeparator, folder);
            else
                return string.Format(ItemTemplate, folder, Properties.Resources.PluginName, itemName, itemNameAs);
        }

        public void DoContextMenu()
        {
            /*
            var dlg = new Forms.CommandList();
            dlg.Commands = _cmdList;
            if (dlg.Commands.Count == 0)
            {
                MessageBox.Show("Нет доступных команд", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                NppUtils.NewFile();
                NppUtils.AppendText("\t\t<!--Sample menu -->");
                NppUtils.NewLine();
                var countItem = 1;
                foreach (var folder in dlg.Commands.Keys)
                {
                    if (countItem > 0)
                    {
                        NppUtils.AppendText(GetItemTemplate());
                        NppUtils.NewLine();
                        countItem = 0;
                    }

                    foreach (var command in dlg.Commands[folder])
                    {
                        if (command.Hint != "-" && command.Selected)
                        {
                            NppUtils.AppendText(GetItemTemplate(folder, command.Name, command.Hint));
                            NppUtils.NewLine();
                            countItem++;
                        }
                    }
                }

                NppUtils.AppendText(GetItemTemplate());
                NppUtils.NewLine();
                NppUtils.SetLang(LangType.L_XML);
            }
            */
        }
        #endregion

    }
}