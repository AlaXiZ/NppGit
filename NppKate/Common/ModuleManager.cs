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
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NppKate.Common
{
    public class CommandItem
    {
        public string Name { get; set; }
        public string Hint { get; set; }
        public ShortcutKey? ShortcutKey { get; set; }
        public Action Action { get; set; }
        public bool Checked { get; set; }
        public bool Selected { get; set; }
    }

    class DockForm
    {
        public Type Type { get; set; }
        public Form Form { get; set; }
        public bool UpdateWithChangeContext { get; set; }
        public Icon TabIcon { get; set; }
        public NppTbMsg uMask { get; set; }

        public DockForm()
        {
            uMask = NppTbMsg.DWS_PARAMSALL | NppTbMsg.DWS_DF_CONT_RIGHT;
        }
    }

    public class ModuleManager : IModuleManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private LinkedList<IModule> _modules;
        private Dictionary<int, DockForm> _forms;
        private Dictionary<string, List<CommandItem>> _cmdList;
        private Dictionary<uint, string> _menuCache;
        private bool _canEvent = false;
        private ulong _currentFormId = ulong.MaxValue;

        private LocalWindowsHook winHookProcRet;
        private LocalWindowsHook winHookProc;

        public event Action OnToolbarRegisterEvent;
        public event Action OnTitleChangingEvent;
        public event Action OnSystemInit;
        public event EventHandler<TabEventArgs> OnTabChangeEvent;
        public event EventHandler<CommandItemClickEventArgs> OnCommandItemClick;
        public event EventHandler<TitleChangedEventArgs> OnTitleChangedEvent;

        public ModuleManager()
        {
            _modules = new LinkedList<IModule>();
            _forms = new Dictionary<int, DockForm>();
            _cmdList = new Dictionary<string, List<CommandItem>>();
            _menuCache = new Dictionary<uint, string>();
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
            if (OnSystemInit != null)
            {
                OnSystemInit();
            }
        }

        private void DoTabChangeEvent(uint idFormChanged)
        {
            if (!_canEvent) return;

            if (OnTabChangeEvent != null)
            {
                OnTabChangeEvent(this, new TabEventArgs(idFormChanged));
            }
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
            foreach (var m in _modules)
            {
                if (m.IsNeedRun)
                    m.Init(this);
            }

            RegisteCommandItem(new CommandItem
            {
                Name = "Sample context menu",
                Hint = "Sample context menu",
                Action = DoContextMenu
            });

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
            MSG msg = (MSG)Marshal.PtrToStructure(e.lParam, typeof(MSG));
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
            if (msg.message == (uint)WinMsg.WM_SETTEXT && (uint)msg.wParam != (uint)WinMsg.WM_SETTEXT)
            {
                DoTitleChangedEvent();
                //DoTitleChangingEvent();
            }
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
                foreach (var item in PluginUtils._funcItems.Items)
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

        private void DoTitleChangingEvent()
        {
            if (!_canEvent) return;

            if (OnTitleChangingEvent != null)
            {
                OnTitleChangingEvent();
            }
        }

        private string _ending = null;

        private void DoTitleChangedEvent()
        {
            if (!_canEvent) return;

            if (OnTitleChangedEvent != null)
            {
                Task task = new Task(() =>
                {
                    var title = PluginUtils.WindowTitle;
                    if (string.IsNullOrEmpty(title))
                        return;
                    var args = new TitleChangedEventArgs();
                    if (!_canEvent) return;
                    OnTitleChangedEvent(this, args);
                    //
                    // Заголовок может заканчиваться на Notepad++ или на [Administrator]
                    // но всегда есть разделительный дефис между имененем файла и Notepad++
                    if (string.IsNullOrEmpty(_ending))
                    {
                        // Ищем последний дефис
                        var pos = title.LastIndexOf(" - ") + 3;
                        // Получаем окончание для заголовка
                        _ending = title.Substring(pos, title.Length - pos);
                    }
                    // Вдруг на пришел заголовок с нашими дописками,
                    // сначала их порежем
                    title = title.Substring(0, title.LastIndexOf(_ending) + _ending.Length) + args.GetTitle();
                    PluginUtils.WindowTitle = title;
                });
                task.Start();
            }
        }

        public void ToolBarInit()
        {
            if (OnToolbarRegisterEvent != null)
            {
                OnToolbarRegisterEvent();
            }
        }

        public void AddModule(IModule item)
        {
            if (!_modules.Contains(item))
            {
                _modules.AddLast(item);
            }
        }

        public int RegisteCommandItem(CommandItem menuItem)
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

            return PluginUtils.SetCommand(menuItem.Name, menuItem.Action, (menuItem.ShortcutKey ?? new ShortcutKey()), menuItem.Checked);
        }

        public void RegisterDockForm(Type formClass, int cmdId, bool updateWithChangeContext, NppTbMsg uMask = NppTbMsg.DWS_PARAMSALL | NppTbMsg.DWS_DF_CONT_RIGHT)
        {
            logger.Debug("Reigister form: Class={0} CmdID = {1} UpdateWithChangeContext={2}", formClass.Name, cmdId, updateWithChangeContext);
            if (!_forms.ContainsKey(cmdId))
            {
                _forms.Add(cmdId, new DockForm
                {
                    Type = formClass,
                    Form = null,
                    UpdateWithChangeContext = updateWithChangeContext,
                    uMask = uMask
                });
                if (updateWithChangeContext)
                {
                    OnTabChangeEvent += (o, a) =>
                    {
                        if (_forms[cmdId].Form != null)
                        {
                            (_forms[cmdId].Form as FormDockable).ChangeContext();
                        }
                    };
                }
            }
        }

        public bool ToogleFormState(int cmdId)
        {
            logger.Debug("Toogle form state: CmdID={0}", cmdId);
            if (_forms.ContainsKey(cmdId))
            {
                var form = _forms[cmdId];
                if (form.Form == null)
                {
                    form.Form = Activator.CreateInstance(form.Type) as Form;
                    form.TabIcon = PluginUtils.NppBitmapToIcon((form.Form as FormDockable).TabIcon);

                    NppTbData _nppTbData = new NppTbData();
                    _nppTbData.hClient = form.Form.Handle;
                    _nppTbData.pszName = (form.Form as FormDockable).Title;
                    _nppTbData.dlgID = cmdId;
                    _nppTbData.uMask = form.uMask;
                    _nppTbData.hIconTab = (uint)form.TabIcon.Handle;
                    _nppTbData.pszModuleName = Properties.Resources.PluginName;
                    IntPtr _ptrNppTbData = Marshal.AllocHGlobal(Marshal.SizeOf(_nppTbData));
                    Marshal.StructureToPtr(_nppTbData, _ptrNppTbData, false);

                    Win32.SendMessage(PluginUtils.NppHandle, NppMsg.NPPM_DMMREGASDCKDLG, 0, _ptrNppTbData);
                }
                else
                {
                    if (form.Form.Visible)
                    {
                        Win32.SendMessage(PluginUtils.NppHandle, NppMsg.NPPM_DMMHIDE, 0, form.Form.Handle);
                    }
                    else
                    {
                        Win32.SendMessage(PluginUtils.NppHandle, NppMsg.NPPM_DMMSHOW, 0, form.Form.Handle);
                    }
                }
                PluginUtils.SetCheckedMenu(PluginUtils.GetCmdId(cmdId), form.Form.Visible);
                return form.Form.Visible;
            }
            else
            {
                logger.Error("Form with command ID = {0} not found", cmdId);
                return false;
            }
        }

        public void AddToolbarButton(int cmdId, Bitmap icon)
        {
            toolbarIcons tbIcons = new toolbarIcons();
            tbIcons.hToolbarBmp = icon.GetHbitmap();
            IntPtr pTbIcons = Marshal.AllocHGlobal(Marshal.SizeOf(tbIcons));
            Marshal.StructureToPtr(tbIcons, pTbIcons, false);
            Win32.SendMessage(PluginUtils.NppHandle, NppMsg.NPPM_ADDTOOLBARICON, PluginUtils.GetCmdId(cmdId), pTbIcons);
            Marshal.FreeHGlobal(pTbIcons);
        }

        public void SetCheckedMenu(int cmdId, bool isChecked)
        {
            logger.Debug("Menu cmdId={0}, state={1}", cmdId, isChecked);
            Win32.SendMessage(PluginUtils.NppHandle, NppMsg.NPPM_SETMENUITEMCHECK, PluginUtils.GetCmdId(cmdId), isChecked ? 1 : 0);
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
            var dlg = new Forms.CommandList();
            dlg.Commands = _cmdList;
            if (dlg.Commands.Count == 0)
            {
                MessageBox.Show("Нет доступных команд", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                PluginUtils.NewFile();
                PluginUtils.AppendText("\t\t<!--Sample menu -->");
                PluginUtils.NewLine();

                var countItem = 1;
                foreach (var folder in dlg.Commands.Keys)
                {
                    if (countItem > 0)
                    {
                        PluginUtils.AppendText(GetItemTemplate());
                        PluginUtils.NewLine();
                        countItem = 0;
                    }
                    foreach (var command in dlg.Commands[folder])
                    {
                        if (command.Hint != "-" && command.Selected)
                        {
                            PluginUtils.AppendText(GetItemTemplate(folder, command.Name, command.Hint));
                            PluginUtils.NewLine();
                            countItem++;
                        }
                    }
                }
                PluginUtils.AppendText(GetItemTemplate());
                PluginUtils.NewLine();
                /*
                for (int i = 0; i < PluginUtils._funcItems.Items.Count; i++)
                {
                    PluginUtils.AppendText(GetItemTemplate(PluginUtils._funcItems.Items[i]._itemName));
                    PluginUtils.NewLine();
                }
                */
                PluginUtils.SetLang(LangType.L_XML);
            }
        }

        public void ManualTitleUpdate()
        {
            DoTitleChangedEvent();
        }
        #endregion

    }
}
