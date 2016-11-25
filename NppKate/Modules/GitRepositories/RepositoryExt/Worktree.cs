using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NppKate.Modules.GitRepositories.RepositoryExt
{
    public class Worktree
    {
        public string Path { get; set; }

        public string Branch { get; set; }

        public string HeadSha { get; set; }
    }
}
