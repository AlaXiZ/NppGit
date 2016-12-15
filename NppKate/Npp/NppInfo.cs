// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
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

namespace NppKate.Npp
{
    public class NppInfo : IDisposable
    {
        private static object locker = new object();
        private volatile static NppInfo _instance;

        private NppData _nppData;
        private FuncItems _funcItems;
        private int _index = 0;

        public static NppInfo Instance
        {
            get
            {
                if (_instance == null)
                    lock (locker)
                        if (_instance == null)
                            _instance = new NppInfo();
                return _instance;
            }
        }
        private NppInfo()
        {
            _funcItems = new FuncItems();
        }

        public NppData NppData
        {
            get { return _nppData; }
            set { _nppData = value; }
        }

        public FuncItems FuncItems
        {
            get { return _funcItems; }
        }

        public IntPtr NppHandle { get { return _nppData._nppHandle; } }

        public int SearchCmdIdByIndex(int index)
        {
            if (index >= 0 && index < _funcItems.Items.Count)
            {
                return _funcItems.Items[index]._cmdID;
            }
            else return -1;
        }

        public int AddCommand(string commandName, Action functionPointer, ShortcutKey? shortcut = null, bool checkOnInit = false)
        {
            FuncItem funcItem = new FuncItem();
            funcItem._cmdID = _index++;
            funcItem._itemName = commandName;
            if (functionPointer != null)
                funcItem._pFunc = functionPointer;
            if ((shortcut?._key ?? 0) != 0)
                funcItem._pShKey = (ShortcutKey)shortcut;
            funcItem._init2Check = checkOnInit;
            _funcItems.Add(funcItem);
            return (_index - 1);
        }

        public void Dispose()
        {
            if (_funcItems != null)
                _funcItems.Dispose();
        }
    }
}
