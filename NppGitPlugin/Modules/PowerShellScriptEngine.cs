using System;

namespace NppGit.Modules
{
    public class PowerShellScriptEngine : IModule
    {
        private IModuleManager _manager;

        public bool IsNeedRun
        {
            get { return Settings.Modules.PSSE; }
        }

        public void Final()
        {
        }

        public void Init(IModuleManager manager)
        {
            _manager = manager;
        }
    }
}
