using System;
using System.IO;
using System.Reflection;

namespace NppGitPlugin
{
    public static class AssemblyLoader
    {
        public static void Init()
        {
            AppDomain.CurrentDomain.AssemblyResolve += AssemblyResolve;
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
            }
            catch { }
            return null;
        }
    }
}
