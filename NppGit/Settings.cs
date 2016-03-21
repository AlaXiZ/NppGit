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

using NppGit.Common;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace NppGit
{
    public static class Settings
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private static Dictionary<string, Dictionary<string, object>> _cache = new Dictionary<string, Dictionary<string, object>>();
        private static IniFile _file = new IniFile(Path.Combine(PluginUtils.ConfigDir, Properties.Resources.PluginName + ".ini"));
        
        #region "Get/Set"
        // Загрузка/сохранение происходит по имени класса и свойства
        // Имена получаются через стек и рефлексию
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void Set<T>(T value)
        {
            var mth = new StackTrace().GetFrame(1).GetMethod();
            var className = mth.ReflectedType.Name;
            var propName = mth.Name.Replace("set_", "");
            try
            {
                logger.Debug("Save: Section={0}, Key={1}, Value={2}", className, propName, value);
            }
            finally { }
            _file.SetValue(className, propName, value);
            if (!_cache.ContainsKey(className))
            {
                _cache.Add(className, new Dictionary<string, object>());
            }
            if (!_cache[className].ContainsKey(propName))
            {
                _cache[className].Add(propName, null);
            }
            _cache[className][propName] = value;
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static T Get<T>(T defaultValue)
        {
            var mth = new StackTrace().GetFrame(1).GetMethod();
            var className = mth.ReflectedType.Name;
            var propName = mth.Name.Replace("get_", "");
            T value;
            if (_cache.ContainsKey(className) && _cache[className].ContainsKey(propName))
            {
                value = (T)_cache[className][propName];
            }
            else
            {
                value = _file.GetValue(className, propName, defaultValue);
                try
                {
                    logger.Debug("Load: Section={0}, Key={1}, Value={2}", className, propName, value);
                }
                finally { }
                if (!_cache.ContainsKey(className))
                {
                    _cache.Add(className, new Dictionary<string, object>());
                }
                if (!_cache[className].ContainsKey(propName))
                {
                    _cache[className].Add(propName, null);
                }
                _cache[className][propName] = value;
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
            public static bool ActiveRepoInTitle
            {
                [MethodImpl(MethodImplOptions.NoInlining)]
                get { return Get(false); }
                [MethodImpl(MethodImplOptions.NoInlining)]
                set { Set(value); }
            }
        }
        public static class Modules
        {
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
            public static bool PSSE
            {
                [MethodImpl(MethodImplOptions.NoInlining)]
                get { return Get(true); }
                [MethodImpl(MethodImplOptions.NoInlining)]
                set { Set(value); }
            }
        }

        public static class Functions
        {
            public static bool ShowBranch
            {
                [MethodImpl(MethodImplOptions.NoInlining)]
                get { return Get(false); }
                [MethodImpl(MethodImplOptions.NoInlining)]
                set { Set(value); }
            }

            public static bool ShowRepoName
            {
                [MethodImpl(MethodImplOptions.NoInlining)]
                get { return Get(false); }
                [MethodImpl(MethodImplOptions.NoInlining)]
                set { Set(value); }
            }

            public static bool ShowStatusFile
            {
                [MethodImpl(MethodImplOptions.NoInlining)]
                get { return Get(false); }
                [MethodImpl(MethodImplOptions.NoInlining)]
                set { Set(value); }
            }

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
            public static bool StatusPanelVisible
            {
                [MethodImpl(MethodImplOptions.NoInlining)]
                get { return Get(false); }
                [MethodImpl(MethodImplOptions.NoInlining)]
                set { Set(value); }
            }
            public static bool FileStatusPanelVisible
            {
                [MethodImpl(MethodImplOptions.NoInlining)]
                get { return Get(false); }
                [MethodImpl(MethodImplOptions.NoInlining)]
                set { Set(value); }
            }
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
            public static bool ShowToolbar
            {
                [MethodImpl(MethodImplOptions.NoInlining)]
                get { return Get(false); }
                [MethodImpl(MethodImplOptions.NoInlining)]
                set { Set(value); }
            }
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
        }

        public static class InnerSettings
        {
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
                    AssemblyLoader.ReConfigLog();
                }
            }
        }
        #endregion
    }
}
