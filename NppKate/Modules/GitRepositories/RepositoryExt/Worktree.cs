// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
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

        public bool IsLocked { get; set; }
    }
}
