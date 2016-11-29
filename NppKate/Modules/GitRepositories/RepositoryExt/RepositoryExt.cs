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

using System.Collections.Generic;
using LibGit2Sharp;

namespace NppKate.Modules.GitRepositories.RepositoryExt
{
    public static class RepositoryExt
    {
        const string WorktreeList = "worktree list --porcelain"; //--git-dir=\"{0}\" --work-tree=\"{1}\" 
        public static Worktree[] GetWorktrees(this Repository repo)
        {
            var shell = GitFeatures.GitShell.GetNppInst();
            var inf = repo.Info;
            var gitDir = inf.Path.Substring(0, inf.Path.Length - 1);
            var workDir = inf.WorkingDirectory.Substring(0, inf.WorkingDirectory.Length - 1);
            var result = new List<Worktree>();
            var wt = shell.Execute(workDir, string.Format(WorktreeList, gitDir, workDir)).Split(new char[]{ '\r', '\n'}, System.StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < wt.Length; i += 3)
            {
                result.Add(new Worktree(workDir)
                {
                    Path = wt[i].Substring(wt[i].IndexOf(' ') + 1),
                    HeadSha = wt[i + 1].Substring(wt[i + 1].IndexOf(' ') + 1),
                    Branch = wt[i + 2].Substring(wt[i + 2].IndexOf(' ') + 1).Replace("refs/heads/", "")
                });
            }
            return result.ToArray();
        }
    }
}
