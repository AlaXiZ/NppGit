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

using NLog;
using NLog.Config;
using NLog.Targets;
using NLog.Targets.Wrappers;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
#if !DEBUG
using NppKate.Npp;
#endif

namespace NppKate.Common
{
    public static class AssemblyLoader
    {
        [DllImport("shlwapi.dll")]
        private static extern bool PathIsNetworkPath(string pszPath);

        private static Object asyncWrapper = null;


        public static void Init()
        {
            AppDomain.CurrentDomain.AssemblyResolve += AssemblyResolve;
            ReConfigLog();
        }

        public static void ReConfigLog()
        {
            var layout = "[${level:uppercase=true}][${logger}][${longdate}][${processid}] ${message}";
            if (asyncWrapper == null)
            {
#if DEBUG
                var logTarget = new DebuggerTarget
                {
                    Layout = layout
                };
#else
            var logTarget = new FileTarget
            {
                Layout = layout,
                AutoFlush = true,
                DeleteOldFileOnStartup = true,
                CreateDirs = true,
                FileName = Path.Combine(NppUtils.ConfigDir, Properties.Resources.PluginName, Properties.Resources.PluginName + ".log")
            };
#endif
                asyncWrapper = new AsyncTargetWrapper
                {
                    WrappedTarget = logTarget,
                    QueueLimit = 5000,
                    OverflowAction = AsyncTargetWrapperOverflowAction.Discard,
                    Name = "NppKate.AsyncTarget",
                    BatchSize = 10
                };
            }
            try
            {
                LogManager.Configuration.RemoveTarget("NppKate.AsyncTarget");
            }
            catch { }

            SimpleConfigurator.ConfigureForTargetLogging((AsyncTargetWrapper)asyncWrapper, LogLevel.FromString(Settings.CommonSettings.LogLevel));
            LogManager.GetCurrentClassLogger().Info("Logger initialized");
        }

        public static void StopLogging()
        {
            LogManager.DisableLogging();
        }

        private static Assembly AssemblyResolve(object sender, ResolveEventArgs args)
        {
            var pluginDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            try
            {
                if (args.Name.StartsWith("LibGit2Sharp"))
                {
                    return LoadAssembly(Path.Combine(pluginDir, Properties.Resources.PluginName, "LibGit2Sharp.dll"));
                }
                else if (args.Name.StartsWith("NLog"))
                {
                    return LoadAssembly(Path.Combine(pluginDir, Properties.Resources.PluginName, "NLog.dll"));
                }
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message + "\r\n" + e.StackTrace, "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return null;
        }

        
        public static Assembly LoadAssembly(string path)
        {
            try
            {
                if (PathIsNetworkPath(path))
                {
                    return Assembly.UnsafeLoadFrom(path);
                }
                else
                {
                    return Assembly.LoadFrom(path);
                }
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message + "\r\n" + e.StackTrace, "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return null;
            }
        }
    }
}
