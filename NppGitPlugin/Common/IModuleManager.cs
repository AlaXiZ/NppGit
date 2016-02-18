using System;
using System.Drawing;

namespace NppGit.Common
{
    public interface IModuleManager
    {
        event Action OnToolbarRegisterEvent;
        event Action OnTitleChangingEvent;
        event Action OnSystemInit;
        event EventHandler<TabEventArgs> OnTabChangeEvent;
        event EventHandler<CommandItemClickEventArgs> OnCommandItemClick;

        int RegisteCommandItem(CommandItem menuItem);
        void RegisterDockForm(Type formClass, int cmdId, bool updateWithChangeContext);
        void AddToolbarButton(int cmdId, Bitmap icon);
        bool ToogleFormState(int cmdId);
        void SetCheckedMenu(int cmdId, bool isChecked);
    }
}
