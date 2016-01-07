using System;
using System.Drawing;

namespace NppGit
{
    public interface IModuleManager
    {
        int RegisterMenuItem(MenuItem menuItem);
        void RegisterDockForm(Type formClass, int cmdId, bool updateWithChangeContext);
        void AddToolbarButton(int cmdId, Bitmap icon);
        bool ToogleFormState(int cmdId);
        void SetCheckedMenu(int cmdId, bool isChecked);
    }

    public interface IModule
    {
        void Init(IModuleManager manager);
        void ToolBarInit();
        void ChangeContext();
        void Final();
    }
}
