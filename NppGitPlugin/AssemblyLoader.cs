using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.IO;
using System.Reflection;

namespace NppGit
{
    public static class AssemblyLoader
    {
        public static void Init()
        {
            AppDomain.CurrentDomain.AssemblyResolve += AssemblyResolve;
            InitLog();
        }

        private static void InitLog()
        {
            var console = new DebuggerTarget
            {
                Layout = "[${longdate}][${level:uppercase=true}][${processid}] ${message}"
            };
            SimpleConfigurator.ConfigureForTargetLogging(console, LogLevel.Trace);

            LogManager.GetCurrentClassLogger().Debug("Logger initialized");            
        }

        private static Assembly AssemblyResolve(object sender, ResolveEventArgs args)
        {
            var pluginDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            try
            {
                if (args.Name.StartsWith("LibGit2Sharp"))
                {
                    return Assembly.LoadFrom(Path.Combine(pluginDir, @"NppGit\LibGit2Sharp.dll"));
                }
                else if (args.Name.StartsWith("NLog"))
                {
                    return Assembly.LoadFrom(Path.Combine(pluginDir, @"NppGit\NLog.dll"));
                }
            }
            catch { }
            return null;
        }
    }
}
