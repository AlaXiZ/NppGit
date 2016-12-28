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
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using NppKate.Common;
using NppKate.Npp;

namespace NppKate
{
    public static class Settings
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private static Dictionary<string, Dictionary<string, object>> _cache = new Dictionary<string, Dictionary<string, object>>();
        private static IniFile _file = new IniFile(Path.Combine(NppUtils.ConfigDir, Properties.Resources.PluginName + ".ini"));

        #region "Get/Set"
        // Загрузка/сохранение происходит по имени класса и свойства
        // Имена получаются через стек и рефлексию
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void Set<T>(T value, string section = null, string key = null)
        {
            if (string.IsNullOrEmpty(section) || string.IsNullOrEmpty(key))
            {
                var mth = new StackTrace().GetFrame(1).GetMethod();
                section = mth.ReflectedType.Name;
                key = mth.Name.Replace("set_", "");
            }
            try
            {
                logger.Debug("Save: Section={0}, Key={1}, Value={2}", section, key, value);
            }
            finally { }
            _file.SetValue(section, key, value);
            if (!_cache.ContainsKey(section))
            {
                _cache.Add(section, new Dictionary<string, object>());
            }
            if (!_cache[section].ContainsKey(key))
            {
                _cache[section].Add(key, null);
            }
            _cache[section][key] = value;
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static T Get<T>(T defaultValue, string section = null, string key = null)
        {
            if (string.IsNullOrEmpty(section) || string.IsNullOrEmpty(key))
            {
                var mth = new StackTrace().GetFrame(1).GetMethod();
                if (string.IsNullOrEmpty(section))
                    section = mth.ReflectedType.Name;
                if (string.IsNullOrEmpty(key))
                    key = mth.Name.Replace("get_", "");
            }
            T value;
            if (_cache.ContainsKey(section) && _cache[section].ContainsKey(key))
            {
                value = (T)_cache[section][key];
            }
            else
            {
                value = _file.GetValue(section, key, defaultValue);
                try
                {
                    logger.Debug("Load: Section={0}, Key={1}, Value={2}", section, key, value);
                }
                finally { }
                if (!_cache.ContainsKey(section))
                {
                    _cache.Add(section, new Dictionary<string, object>());
                }
                if (!_cache[section].ContainsKey(key))
                {
                    _cache[section].Add(key, null);
                }
                _cache[section][key] = value;
            }

            return value;
        }
        #endregion

        #region "Settings classes"
        public static class GitCore
        {
            public static string LastActiveRepository
            {
                [MethodImpl(MethodImplOptions.NoInlining)]
                get { return Get(""); }
                [MethodImpl(MethodImplOptions.NoInlining)]
                set { Set(value); }
            }

            public static string LastActiveWorktree
            {
                [MethodImpl(MethodImplOptions.NoInlining)]
                get { return Get(""); }
                [MethodImpl(MethodImplOptions.NoInlining)]
                set { Set(value); }
            }

            public static bool AutoExpand
            {
                [MethodImpl(MethodImplOptions.NoInlining)]
                get { return Get(false); }
                [MethodImpl(MethodImplOptions.NoInlining)]
                set { Set(value); }
            }

            public static int SearchFlags
            {
                [MethodImpl(MethodImplOptions.NoInlining)]
                get { return Get(0xffff); }
                [MethodImpl(MethodImplOptions.NoInlining)]
                set { Set(value); }
            }
        }
        public static class Modules
        {
            private const string IniSection = "Modules";
            public static bool TortoiseGit
            {
                [MethodImpl(MethodImplOptions.NoInlining)]
                get { return Get(true); }
                [MethodImpl(MethodImplOptions.NoInlining)]
                set { Set(value); }
            }
            public static bool Git
            {
                [MethodImpl(MethodImplOptions.NoInlining)]
                get { return Get(true); }
                [MethodImpl(MethodImplOptions.NoInlining)]
                set { Set(value); }
            }
            public static bool SQLIDE
            {
                [MethodImpl(MethodImplOptions.NoInlining)]
                get { return Get(true); }
                [MethodImpl(MethodImplOptions.NoInlining)]
                set { Set(value); }
            }
            public static bool Snippets
            {
                [MethodImpl(MethodImplOptions.NoInlining)]
                get { return Get(true); }
                [MethodImpl(MethodImplOptions.NoInlining)]
                set { Set(value); }
            }

            public static bool LogConsole
            {
                [MethodImpl(MethodImplOptions.NoInlining)]
                get { return Get(true); }
                [MethodImpl(MethodImplOptions.NoInlining)]
                set { Set(value); }
            }

            [MethodImpl(MethodImplOptions.NoInlining)]
            public static bool NeedCurrentModuleLoad()
            {
                var mth = new StackTrace().GetFrame(1).GetMethod();
                var cls = mth.ReflectedType.Name;
                return GetModuleState(cls);
            }

            public static bool GetModuleState(string module)
            {
                return Get(true, IniSection, module);
            }

            public static void SetModuleState(string module, bool value)
            {
                Set(value, IniSection, module);
            }
        }

        public static class Functions
        {
            public static byte SHACount
            {
                [MethodImpl(MethodImplOptions.NoInlining)]
                get { return Get((byte)6); }
                [MethodImpl(MethodImplOptions.NoInlining)]
                set { Set(value); }
            }

            public static bool OpenFileInOtherView
            {
                [MethodImpl(MethodImplOptions.NoInlining)]
                get { return Get(false); }
                [MethodImpl(MethodImplOptions.NoInlining)]
                set { Set(value); }
            }
        }

        public static class Panels
        {
            public static bool SnippetsPanelVisible
            {
                [MethodImpl(MethodImplOptions.NoInlining)]
                get { return Get(false); }
                [MethodImpl(MethodImplOptions.NoInlining)]
                set { Set(value); }
            }
            public static bool RepoBrowserPanelVisible
            {
                [MethodImpl(MethodImplOptions.NoInlining)]
                get { return Get(false); }
                [MethodImpl(MethodImplOptions.NoInlining)]
                set { Set(value); }
            }
            public static bool ConsoleLogPanelVisible
            {
                [MethodImpl(MethodImplOptions.NoInlining)]
                get { return Get(false); }
                [MethodImpl(MethodImplOptions.NoInlining)]
                set { Set(value); }
            }

        }

        public static class TortoiseGitProc
        {
            public static string Path
            {
                [MethodImpl(MethodImplOptions.NoInlining)]
                get { return Get(""); }
                [MethodImpl(MethodImplOptions.NoInlining)]
                set { Set(value); }
            }

            [Obsolete("Deprecated", true)]
            public static bool ShowToolbar
            {
                [MethodImpl(MethodImplOptions.NoInlining)]
                get { return Get(false); }
                [MethodImpl(MethodImplOptions.NoInlining)]
                set { Set(value); }
            }

            [Obsolete("Deprecated", true)]
            public static uint ButtonMask
            {
                [MethodImpl(MethodImplOptions.NoInlining)]
                get { return Get(0u); }
                [MethodImpl(MethodImplOptions.NoInlining)]
                set { Set(value); }
            }
            public static bool IsFirstSearch
            {
                [MethodImpl(MethodImplOptions.NoInlining)]
                get { return Get(true); }
                [MethodImpl(MethodImplOptions.NoInlining)]
                set { Set(value); }
            }

            [MethodImpl(MethodImplOptions.NoInlining)]
            [Obsolete("Deprecated", true)]
            public static bool GetButtonVisible(string command)
            {
                return Get(false, key: command);
            }

            [MethodImpl(MethodImplOptions.NoInlining)]
            [Obsolete("Deprecated", true)]
            public static void SetButtonVisible(string command, bool value)
            {
                Set(value, key: command);
            }

        }

        public static class CommonSettings
        {
            const string ToolbarCommandSuffix = "_ToolbarCommand";
            public static bool IsSetDefaultShortcut
            {
                [MethodImpl(MethodImplOptions.NoInlining)]
                get { return Get(false); }
                [MethodImpl(MethodImplOptions.NoInlining)]
                set { Set(value); }
            }
            public static string LogLevel
            {
                [MethodImpl(MethodImplOptions.NoInlining)]
                get { return Get("Off"); }
                [MethodImpl(MethodImplOptions.NoInlining)]
                set
                {
                    Set(value);
                    LoggerUtil.Configurate();
                }
            }

            public static bool GetCommandState(string module, string command)
            {
                return Get(true, module, command);
            }

            public static void SetCommandState(string module, string command, bool value)
            {
                Set(value, module, command);
            }

            public static bool GetToolbarCommandState(string module, string command)
            {
                return Get(false, module, command + ToolbarCommandSuffix);
            }

            public static void SetToolbarCommandState(string module, string command, bool value)
            {
                Set(value, module, command + ToolbarCommandSuffix);
            }
        }

        public static class Snippets
        {
            public static bool IsGroupByCategory
            {
                [MethodImpl(MethodImplOptions.NoInlining)]
                get { return Get(true); }
                [MethodImpl(MethodImplOptions.NoInlining)]
                set { Set(value); }
            }

            public static bool IsHideByExtention
            {
                [MethodImpl(MethodImplOptions.NoInlining)]
                get { return Get(false); }
                [MethodImpl(MethodImplOptions.NoInlining)]
                set { Set(value); }
            }

            public static bool IsExpanAfterCreate
            {
                [MethodImpl(MethodImplOptions.NoInlining)]
                get { return Get(true); }
                [MethodImpl(MethodImplOptions.NoInlining)]
                set { Set(value); }
            }

            public static bool InsertEmpty
            {
                [MethodImpl(MethodImplOptions.NoInlining)]
                get { return Get(true); }
                [MethodImpl(MethodImplOptions.NoInlining)]
                set { Set(value); }
            }

            public static ushort MaxLevel
            {
                [MethodImpl(MethodImplOptions.NoInlining)]
                get { return Get<ushort>(5); }
                [MethodImpl(MethodImplOptions.NoInlining)]
                set { Set(value); }
            }
        }
        #endregion

        #region "Git Bash"
        public static class GitBash
        {
            public static string BinPath
            {
                [MethodImpl(MethodImplOptions.NoInlining)]
                get { return Get(""); }
                [MethodImpl(MethodImplOptions.NoInlining)]
                set { Set(value); }
            }

            public static bool IsFirstRun
            {
                [MethodImpl(MethodImplOptions.NoInlining)]
                get { return Get(true); }
                [MethodImpl(MethodImplOptions.NoInlining)]
                set { Set(value); }
            }
        }
        #endregion
    }
}
