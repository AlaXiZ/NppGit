using NppGit.Common;

namespace NppGit.Modules.PSSEFeatures
{
    public class POSHFeatures : IModule
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
