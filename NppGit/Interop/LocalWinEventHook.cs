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
