/*

using System.Management.Automation;
using System.Threading;
using System.IO;

namespace NppGit.Modules.PSSEFeatures
{
    public sealed class POSHExecutor
    {
        private static object syncRoot = new object();
        private static volatile POSHExecutor _instance;
        private POSHExecutor() { }

        public static POSHExecutor Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                            _instance = new POSHExecutor();
                    }
                }
                return _instance;
            }
        }

        public void RunScriptAsync(string scriptFile)
        {
            ThreadPool.QueueUserWorkItem(RunSrriptInThread, scriptFile);
        }

        private void RunSrriptInThread(object scriptFile)
        {
            if (File.Exists(scriptFile as string))
            {


                using (PowerShell posh = PowerShell.Create())
                {

                }
            }
        }
    }
}
*/