using NLog;
using NppGit.Interop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace NppGit
{
    public struct MenuItem
    {
        public string Name;
        public string Hint;
        public ShortcutKey? ShortcutKey;
        public Action Action;
        public bool Checked;
    }

    class DockForm
    {
        public Type Type { get; set; }
        public Form Form { get; set; }
        public bool UpdateWithChangeContext { get; set; }
        public Icon TabIcon { get; set; }
    }

    public class ModuleManager : IModuleManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private LinkedList<IModule> _modules;
        private Dictionary<int, DockForm> _forms;
        private Dictionary<string, List<MenuItem>> _cmdList;

        private LocalWindowsHook winHookProcRet;
        
        public event Action OnToolbarRegisterEvent;
        public event TabChange OnTabChangeEvent;
        public event Action OnTitleChangingEvent;

        public ModuleManager()
        {
            _modules = new LinkedList<IModule>();
            _forms = new Dictionary<int, DockForm>();
            _cmdList = new Dictionary<string, List<MenuItem>>();
        }

        public void MessageProc(SCNotification sn)
        {  
            switch(sn.nmhdr.code)
            {
                case (uint)NppMsg.NPPN_BUFFERACTIVATED:
                case (uint)NppMsg.NPPN_FILEOPENED:
                    {
                        if (OnTabChangeEvent != null)
                        {
                            OnTabChangeEvent(new TabEventArgs(sn.nmhdr.idFrom));
                        }
                        break;
                    }
            }
            //logger.Debug("sn.nmhdr.code={0}", sn.nmhdr.code);
        }

        public void Final()
        {
            if (winHookProcRet != null && winHookProcRet.IsInstalled)
                winHookProcRet.Uninstall();

            foreach (var m in _modules)
                m.Final();
        }

        public void Init()
        {
            foreach (var m in _modules)
            {
                m.Init(this);
                this.RegisterMenuItem(new MenuItem
                {
                    Name = "-",
                    Hint = "-",
                    Action = null
                });
            }
            RegisterMenuItem(new MenuItem {
                Name = "Sample context menu",
                Hint = "Sample context menu",
                Action = ContextMenu
            });

            winHookProcRet = new LocalWindowsHook(HookType.WH_CALLWNDPROCRET);
            winHookProcRet.HookInvoked += WinHookHookInvoked;
            winHookProcRet.Install();
        }

        private void WinHookHookInvoked(object sender, HookEventArgs e)
        {
            CWPRETSTRUCT msg = (CWPRETSTRUCT)Marshal.PtrToStructure(e.lParam, typeof(CWPRETSTRUCT));
            if (msg.message == (uint)WinMsg.WM_SETTEXT && (uint)msg.wParam != (uint)WinMsg.WM_SETTEXT)
            {
                if (OnTitleChangingEvent != null)
                {
                    OnTitleChangingEvent();
                }
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

        public int RegisterMenuItem(MenuItem menuItem)
        {
            if (!Settings.InnerSettings.IsSetDefaultShortcut)
                menuItem.ShortcutKey = null;

            logger.Debug("Register menu item: {0}, {1}, {2}", menuItem.Name == "-" ? "Separator" : menuItem.Name, menuItem.ShortcutKey == null ? "<null>" : menuItem.ShortcutKey.Value.ToString(), menuItem.Checked);
            var mth = new StackTrace().GetFrame(1).GetMethod();
            var className = mth.ReflectedType.Name;
            if (!_cmdList.ContainsKey(className))
            {
                _cmdList.Add(className, new List<MenuItem>());
            }
            _cmdList[className].Add(menuItem);

            return PluginUtils.SetCommand(menuItem.Name, menuItem.Action, (menuItem.ShortcutKey ?? new ShortcutKey()), menuItem.Checked);
        }

        public void RegisterDockForm(Type formClass, int cmdId, bool updateWithChangeContext)
        {
            logger.Debug("Reigister form: Class={0} CmdID = {1} UpdateWithChangeContext={2}", formClass.Name, cmdId, updateWithChangeContext);
            if (!_forms.ContainsKey(cmdId))
            {
                _forms.Add(cmdId, new DockForm
                                        {
                                            Type = formClass,
                                            Form = null,
                                            UpdateWithChangeContext = updateWithChangeContext
                                        });
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
                    _nppTbData.uMask = NppTbMsg.DWS_DF_CONT_RIGHT | NppTbMsg.DWS_ICONTAB | NppTbMsg.DWS_ICONBAR;
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
                Win32.SendMessage(PluginUtils.NppHandle, NppMsg.NPPM_SETMENUITEMCHECK, cmdId, form.Form.Visible ? 1 : 0);
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

        public void ContextMenu()
        {
            PluginUtils.NewFile();
            PluginUtils.AppendText("\t\t<!--Sample menu -->");
            PluginUtils.NewLine();

            var countItem = 1;
            foreach (var folder in _cmdList.Keys)
            {
                if (countItem > 0)
                {
                    PluginUtils.AppendText(GetItemTemplate());
                    PluginUtils.NewLine();
                    countItem = 0;
                }
                foreach (var command in _cmdList[folder])
                {
                    if (command.Hint != "-")
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
        #endregion

    }
}
