using System;

namespace NppGit.Interop
{
    public class WinEventHookArgs: EventArgs
    {
        public WinEvent EventType { get; protected set; }
        public IntPtr Hwnd { get; protected set; }
        public int IdObject { get; protected set; }
        public int IdChild { get; protected set; }
        public uint EventThread { get; protected set; }
        public uint EventTime { get; protected set; }

        public WinEventHookArgs(WinEvent eventType, IntPtr hwnd, int idObject, int idChild, uint eventThread, uint eventTime)
        {
            EventType = eventType;
            Hwnd = hwnd;
            IdObject = idObject;
            IdChild = idChild;
            EventThread = eventThread;
            EventTime = eventTime;
        }
    }
    public class LocalWinEventHook
    {
        WinEvent _eventMin = WinEvent.EVENT_MIN;
        WinEvent _eventMax = WinEvent.EVENT_MAX;
        IntPtr _hookHandle = IntPtr.Zero;
        WinEventDelegate _hookProc = null;

        public delegate void WinEventHookHandler(WinEventHookArgs e);

        public event WinEventHookHandler WinEventHook;

        internal void HookProc(IntPtr hWinEventHook, WinEvent eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            if (WinEventHook != null)
            {
                WinEventHook(new WinEventHookArgs(eventType, hwnd, idObject, idChild, dwEventThread, dwmsEventTime));
            }
        }

        public LocalWinEventHook(WinEvent eventMin, WinEvent eventMax)
        {
            _eventMin = eventMin;
            _eventMax = eventMax;
            _hookProc = new WinEventDelegate(HookProc);
        }

        public LocalWinEventHook(WinEvent eventMin, WinEvent eventMax, WinEventDelegate hookProc)
        {
            _eventMin = eventMin;
            _eventMax = eventMax;
            _hookProc = hookProc;
        }

        public void Attach()
        {
            if (_hookHandle == IntPtr.Zero)
            {
                _hookHandle = Win32.SetWinEventHook(
                    _eventMin,
                    _eventMax,
                    IntPtr.Zero,
                    _hookProc,
                    0,
#pragma warning disable CS0618 // Тип или член устарел
                    (uint)AppDomain.GetCurrentThreadId(),
#pragma warning restore CS0618 // Тип или член устарел
                    (uint)WinEventContext.WINEVENT_INCONTEXT | (uint)WinEventContext.WINEVENT_SKIPOWNPROCESS
                    );
            }
        }

        public void Detach()
        {
            if (_hookHandle != IntPtr.Zero)
            {
                Win32.UnhookWinEvent(_hookHandle);
            }
        }

        public bool IsAttached
        {
            get { return _hookHandle != IntPtr.Zero; }
        }
    }
}
