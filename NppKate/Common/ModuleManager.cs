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
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using NppKate.AppSettings;

namespace NppKate.Common
{
    public class ModuleManager : IModuleManager, IDockableManager
    {
        private const int TOOLBAR_ICON_SIZE = 16;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private LinkedList<IModule> _modules;
        private Dictionary<int, DockForm> _forms;
        private Dictionary<int, DockDialogData> _dockDialog;
        private Dictionary<int, IntPtr> _hwnds;
        private Dictionary<string, List<CommandItem>> _cmdList;
        private Dictionary<uint, string> _menuCache;
        private bool _canEvent = false;
        private ulong _currentFormId = ulong.MaxValue;
        private ResourceManager _resManager = null;
        private LocalWindowsHook winHookProcRet;
        private LocalWindowsHook winHookProc;
        public event Action OnToolbarRegisterEvent;
        public event Action OnSystemInit;
        public event EventHandler<TabEventArgs> OnTabChangeEvent;
        public event EventHandler<CommandItemClickEventArgs> OnCommandItemClick;
        public event EventHandler<SettingsInitEventArgs> OnSettingsEvent;
        public event Action OnSettingsFinishEvent;

        public ModuleManager()
        {
            _modules = new LinkedList<IModule>();
            _forms = new Dictionary<int, DockForm>();
            _cmdList = new Dictionary<string, List<CommandItem>>();
            _menuCache = new Dictionary<uint, string>();
            _dockDialog = new Dictionary<int, DockDialogData>();
            _hwnds = new Dictionary<int, IntPtr>();
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
            _resManager = new ResourceManager();
            foreach (var m in _modules)
            {
                if (m.IsNeedRun)
                    m.Init(this);
            }

            RegisterCommandItem(new CommandItem{Name = "Sample context menu", Hint = "Sample context menu", Action = DoContextMenu});
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
            Interop.MSG msg = (Interop.MSG)Marshal.PtrToStructure(e.lParam, typeof (Interop.MSG));
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
            CWPRETSTRUCT msg = (CWPRETSTRUCT)Marshal.PtrToStructure(e.lParam, typeof (CWPRETSTRUCT));
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
            if (!_menuCache.ContainsKey(menuId))
            {
                foreach (var item in NppInfo.Instance.FuncItems.Items)
                {
                    if (item._cmdID == menuId)
                    {
                        menuName = item._itemName;
                        break;
                    }
                }

                _menuCache.Add(menuId, menuName);
            }
            else
            {
                menuName = _menuCache[menuId];
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

        public int RegisterCommandItem(CommandItem menuItem)
        {
            menuItem.Selected = false;
            if (!Settings.InnerSettings.IsSetDefaultShortcut)
                menuItem.ShortcutKey = null;
            logger.Debug("Register menu item: {0}, {1}, {2}", menuItem.Name == "-" ? "Separator" : menuItem.Name, menuItem.ShortcutKey == null ? "<null>" : menuItem.ShortcutKey.Value.ToString(), menuItem.Checked);
            var mth = new StackTrace().GetFrame(1).GetMethod();
            var className = mth.ReflectedType.Name;
            if (!_cmdList.ContainsKey(className))
            {
                _cmdList.Add(className, new List<CommandItem>());
            }

            _cmdList[className].Add(menuItem);
            return NppInfo.Instance.AddCommand(menuItem.Name, menuItem.Action, menuItem.ShortcutKey, menuItem.Checked);
        }

        public void RegisterDockForm(Type formClass, int cmdId, bool updateWithChangeContext, NppTbMsg uMask = NppTbMsg.DWS_PARAMSALL | NppTbMsg.DWS_DF_CONT_RIGHT, IntPtr? hBitmap = null)
        {
            logger.Debug("Reigister form: Class={0} CmdID = {1} UpdateWithChangeContext={2}", formClass.Name, cmdId, updateWithChangeContext);
            if (!_forms.ContainsKey(cmdId))
            {
                _forms.Add(cmdId, new DockForm{Type = formClass, Form = null, UpdateWithChangeContext = updateWithChangeContext, uMask = uMask, hBitmap = hBitmap ?? IntPtr.Zero});
                if (updateWithChangeContext)
                {
                    OnTabChangeEvent += (o, a) =>
                    {
                        if (_forms[cmdId].Form != null)
                        {
                            (_forms[cmdId].Form as FormDockable).ChangeContext();
                        }
                    }

                    ;
                }
            }
        }

        public bool ToogleFormState(int cmdId)
        {
            logger.Debug("Toogle form state: CmdID={0}", cmdId);
            IntPtr hwnd = IntPtr.Zero;
            bool isReg = false;
            if (_forms.ContainsKey(cmdId))
            {
                /* old logic */
                var form = _forms[cmdId];
                if (form.Form == null)
                {
                    isReg = true;
                    form.Form = Activator.CreateInstance(form.Type) as Form;
                    form.TabIcon = NppUtils.NppBitmapToIcon((form.Form as FormDockable).TabIcon);
                    NppTbData _nppTbData = new NppTbData();
                    _nppTbData.hClient = form.Form.Handle;
                    _nppTbData.pszName = (form.Form as FormDockable).Title;
                    _nppTbData.dlgID = NppInfo.Instance.SearchCmdIdByIndex(cmdId);
                    _nppTbData.uMask = form.uMask;
                    _nppTbData.hIconTab = form.hBitmap == IntPtr.Zero ? (uint)form.TabIcon.Handle : (uint)form.hBitmap;
                    _nppTbData.pszModuleName = Properties.Resources.PluginName;
                    NppUtils.RegisterAsDockDialog(_nppTbData);
                }

                hwnd = form.Form?.Handle ?? IntPtr.Zero;
            }
            // New logic
            else if (_dockDialog.ContainsKey(cmdId))
            {
                isReg = true;
                var data = _dockDialog[cmdId];
                Form wnd = Activator.CreateInstance(data.Class) as Form;
                if (wnd != null)
                {
                    NppTbData _nppTbData = new NppTbData{hClient = wnd.Handle, dlgID = NppInfo.Instance.SearchCmdIdByIndex(cmdId), hIconTab = (uint)NppUtils.NppBitmapToIcon(_resManager.LoadImage(data.IconResourceName, TOOLBAR_ICON_SIZE, TOOLBAR_ICON_SIZE))?.Handle, pszModuleName = Properties.Resources.PluginName, pszName = data.Title, uMask = data.uMask};
                    NppUtils.RegisterAsDockDialog(_nppTbData);
                    _dockDialog.Remove(cmdId);
                    _hwnds.Add(cmdId, wnd.Handle);
                    (wnd as IDockDialog).init(this, NppInfo.Instance.SearchCmdIdByIndex(cmdId));
                    hwnd = wnd.Handle;
                }
            // TODO: delete return
            //return false;
            }
            else if (_hwnds.ContainsKey(cmdId))
            {
                // TODO: delete return
                hwnd = _hwnds[cmdId];
            //
            //return false;
            }
            else
            {
                logger.Error("Form with command ID = {0} not found", cmdId);
                return false;
            }

            if (hwnd != IntPtr.Zero)
            {
                if (!isReg)
                {
                    if (Win32.IsWindowVisible(hwnd))
                    {
                        Win32.SendMessage(NppInfo.Instance.NppHandle, NppMsg.NPPM_DMMHIDE, 0, hwnd);
                    }
                    else
                    {
                        Win32.SendMessage(NppInfo.Instance.NppHandle, NppMsg.NPPM_DMMSHOW, 0, hwnd);
                    }
                }
            }
            else
            {
                logger.Error("Handle of window with command ID = {0} not found", cmdId);
                return false;
            }

            NppUtils.SetCheckedMenu(NppInfo.Instance.SearchCmdIdByIndex(cmdId), Win32.IsWindowVisible(hwnd));
            return Win32.IsWindowVisible(hwnd);
        }

        public void AddToolbarButton(int cmdId, string iconName)
        {
            toolbarIcons tbIcons = new toolbarIcons();
            tbIcons.hToolbarBmp = _resManager.LoadImage(iconName, TOOLBAR_ICON_SIZE, TOOLBAR_ICON_SIZE);
            IntPtr pTbIcons = Marshal.AllocHGlobal(Marshal.SizeOf(tbIcons));
            Marshal.StructureToPtr(tbIcons, pTbIcons, false);
            Win32.SendMessage(NppInfo.Instance.NppHandle, NppMsg.NPPM_ADDTOOLBARICON, NppInfo.Instance.SearchCmdIdByIndex(cmdId), pTbIcons);
            Marshal.FreeHGlobal(pTbIcons);
        }

        public void SetCheckedMenu(int cmdId, bool isChecked)
        {
            logger.Debug("Menu cmdId={0}, state={1}", cmdId, isChecked);
            Win32.SendMessage(NppInfo.Instance.NppHandle, NppMsg.NPPM_SETMENUITEMCHECK, NppInfo.Instance.SearchCmdIdByIndex(cmdId), isChecked ? 1 : 0);
        }

        public void RegisterDockForm(int indexId, DockDialogData dlgData)
        {
            _dockDialog.Add(indexId, dlgData);
        }

#region "Context menu"
        private static readonly string ItemTemplate = "<Item FolderName=\"{0}\" PluginEntryName=\"{1}\" PluginCommandItemName=\"{2}\" ItemNameAs=\"{3}\"/>";
        private static readonly string ItemSeparator = "<Item FolderName=\"{0}\" id = \"0\" />";
        private static readonly string ItemSeparator2 = "<Item id=\"0\" />";
        public ResourceManager ResourceManager
        {
            get
            {
                return _resManager;
            }
        }

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
            var dlg = new Forms.CommandList();
            dlg.Commands = _cmdList;
            if (dlg.Commands.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Нет доступных команд", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                /*
                for (int i = 0; i < PluginUtils._funcItems.Items.Count; i++)
                {
                    PluginUtils.AppendText(GetItemTemplate(PluginUtils._funcItems.Items[i]._itemName));
                    PluginUtils.NewLine();
                }
                */
                NppUtils.SetLang(LangType.L_XML);
            }
        }
#endregion
    }
}