using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NppGit.Modules
{
    public class SqlIde : IModule
    {
        private IModuleManager _manager;

        public bool IsNeedRun
        {
            get { return Settings.Modules.SQLIDE; }
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
