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
using NLog;
using NLog.Targets.Wrappers;
using NLog.Targets;
using NLog.Config;
#if !DEBUG
using NppKate.Npp;
using System.IO;
#endif

namespace NppKate.Common
{
    public static class LoggerUtil
    {
        private static AsyncTargetWrapper asyncWrapper = null;
        public static void Configurate()
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
                    QueueLimit = 500,
                    TimeToSleepBetweenBatches = 100,
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

            SimpleConfigurator.ConfigureForTargetLogging(asyncWrapper, LogLevel.FromString(Settings.CommonSettings.LogLevel));
            LogManager.GetCurrentClassLogger().Info("Logger initialized");
        }

        public static void StopLogging()
        {
            LogManager.DisableLogging();
        }

        private static void ErrorEx(Logger logger, Exception ex)
        {
            logger.Error("Exception\r\nMessage: {0}\r\nSource: {1}\r\nStacktrace: {2}\r\n Has inner exception: {3}",
                ex.Message, ex.Source, ex.StackTrace, ex.InnerException != null);
            if (ex.InnerException != null)
                ErrorEx(logger, ex.InnerException);
        }
        public static void Error(Logger logger, Exception ex, string format, params object[] args)
        {
            if (args != null)
                logger.Error(format, args);
            else
                logger.Error(format);
            ErrorEx(logger, ex);
        }

        private static void ConsoleErrorEx(Exception ex)
        {
             System.Diagnostics.Debug.WriteLine("Exception\r\nMessage: {0}\r\nSource: {1}\r\nStacktrace: {2}\r\n",
                ex.Message, ex.Source, ex.StackTrace);
            System.Diagnostics.Debug.WriteLineIf(ex.InnerException != null, "");
            if (ex.InnerException != null)
                ConsoleErrorEx(ex.InnerException);
        }
        public static void ConsoleError(Exception ex, string format, params object[] args)
        {
            if (args != null)
                System.Diagnostics.Debug.WriteLine(format, args);
            ConsoleErrorEx(ex);
        }
    }
}
