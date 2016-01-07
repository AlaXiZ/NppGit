using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace NppGit
{
    public struct MenuItem
    {
        public string Name;
        public string Hint;
        public ShortcutKey ShortcutKey;
        public Action Action;
        public bool Checked;
    }

    struct DockForm
    {
        public Type Type;
        public Form Form;
        public bool UpdateWithChangeContext;
        public Icon TabIcon;
    }

    public class ModuleManager : IModuleManager
    {
        private LinkedList<IModule> _modules;
        private Dictionary<int, DockForm> _forms;

        public ModuleManager()
        {
            _modules = new LinkedList<IModule>();
            _forms = new Dictionary<int, DockForm>();
        }

        public void Final()
        {
            foreach (var m in _modules)
                m.Final();
        }

        public void Init()
        {
            foreach (var m in _modules)
                m.Init(this);
        }

        public void ToolBarInit()
        {
            foreach (var m in _modules)
                m.ToolBarInit();
        }

        public void ChangeContext()
        {
            foreach (var f in _forms)
                if (f.Value.Form != null && f.Value.UpdateWithChangeContext)
                    (f.Value.Form as FormDockable).ChangeContext();
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
            return PluginUtils.SetCommand(menuItem.Name, menuItem.Action, menuItem.ShortcutKey, menuItem.Checked);
        }

        public void RegisterDockForm(Type formClass, int cmdId, bool updateWithChangeContext)
        {
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
                throw new Exception(string.Format("Form with command ID = {0} not found", cmdId));
            }
        }

        public void AddToolbarButton(int cmdId, Bitmap icon)
        {
            toolbarIcons tbIcons = new toolbarIcons();
            tbIcons.hToolbarBmp = icon.GetHbitmap();
            IntPtr pTbIcons = Marshal.AllocHGlobal(Marshal.SizeOf(tbIcons));
            Marshal.StructureToPtr(tbIcons, pTbIcons, false);
            Win32.SendMessage(PluginUtils.NppHandle, NppMsg.NPPM_ADDTOOLBARICON, PluginUtils._funcItems.Items[cmdId]._cmdID, pTbIcons);
            Marshal.FreeHGlobal(pTbIcons);
        }

        public void SetCheckedMenu(int cmdId, bool isChecked)
        {
            Win32.SendMessage(PluginUtils.NppHandle, NppMsg.NPPM_SETMENUITEMCHECK, cmdId, isChecked ? 1 : 0);
        }
    }
}
