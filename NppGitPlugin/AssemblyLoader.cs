using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace NppGit
{
    public static class AssemblyLoader
    {
        [DllImport("shlwapi.dll")]
        private static extern bool PathIsNetworkPath(string pszPath);

        private static bool isTrustedInit = false;

        public static void Init()
        {
            AppDomain.CurrentDomain.AssemblyResolve += AssemblyResolve;
            ReConfigLog();
        }

        public static void ReConfigLog()
        {
            var layout = "[${level:uppercase=true}][${logger}][${longdate}][${processid}] ${message}";
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
                FileName = Path.Combine(PluginUtils.ConfigDir, Properties.Resources.PluginName, Properties.Resources.PluginName + ".log")
            };
#endif
            SimpleConfigurator.ConfigureForTargetLogging(logTarget, LogLevel.FromString(Settings.InnerSettings.LogLevel));
            LogManager.GetCurrentClassLogger().Info("Logger initialized");
        }

        private static Assembly AssemblyResolve(object sender, ResolveEventArgs args)
        {
            var pluginDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            try
            {
                if (args.Name.StartsWith("LibGit2Sharp"))
                {
                    return LoadAssembly(Path.Combine(pluginDir, @"NppGit\LibGit2Sharp.dll"));
                }
                else if (args.Name.StartsWith("NLog"))
                {
                    return LoadAssembly(Path.Combine(pluginDir, @"NppGit\NLog.dll"));
                }
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message + "\r\n" + e.StackTrace, "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return null;
        }

        
        private static Assembly LoadAssembly(string path)
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
