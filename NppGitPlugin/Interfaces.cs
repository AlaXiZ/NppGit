using System;
using System.Drawing;

namespace NppGit
{
    public class MessageEventArgs : EventArgs
    {
        public MessageEventArgs(uint message, int lParam, int wParam)
        {
            Message = message;
            LParam = lParam;
            WParam = wParam;
        }

        public uint Message { get; set; }
        public int LParam { get; set; }
        public int WParam { get; set; }
    }

    public class TabEventArgs : EventArgs
    {
        public uint TabId { get; protected set; }
        public TabEventArgs(uint tabId)
        {
            TabId = tabId;
        }
    }

    public delegate void WndProc(MessageEventArgs args);
    public delegate void TabChange(TabEventArgs args);

    public interface IModuleManager
    {
        event Action OnToolbarRegisterEvent;
        event TabChange OnTabChangeEvent;

        int RegisterMenuItem(MenuItem menuItem);
        void RegisterDockForm(Type formClass, int cmdId, bool updateWithChangeContext);
        void AddToolbarButton(int cmdId, Bitmap icon);
        bool ToogleFormState(int cmdId);
        void SetCheckedMenu(int cmdId, bool isChecked);
    }

    public interface IModule
    {
        void Init(IModuleManager manager);
        void Final();
    }
}
