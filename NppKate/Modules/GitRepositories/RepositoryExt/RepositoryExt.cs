// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
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

using System;
using System.Collections.Generic;
using LibGit2Sharp;

namespace NppKate.Modules.GitRepositories.RepositoryExt
{
    public enum LockType
    {
        IndexLock,
        HeadLock
    }
    public static class RepositoryExt
    {
        const string WorktreeAdd = "worktree add \"{0}\" {1}";
        const string WorktreePrune = "worktree prune";
        const string WorktreeLock = "worktree lock \"{0}\"";
        const string WorktreeUnlock = "worktree unlock \"{0}\"";

        public static void AddWorktree(this Repository repo, Branch branch, string worktreePath = null)
        {
            Branch local = null;

            if (branch.IsRemote)
            {
                if (branch.IsTracking)
                {
                    local = branch.TrackedBranch;
                }
                else
                {
                    var localName = branch.Name.Replace(branch.Remote.Name + "/", "");
                    try
                    {
                        local = repo.CreateBranch(localName, branch.Tip);
                    }
                    catch {}
                }
            }
            else
                local = branch;

            if (local == null) return;

            var repositoryPath = repo.Info.WorkingDirectory.Substring(0, repo.Info.WorkingDirectory.Length - 1);

            if (string.IsNullOrEmpty(worktreePath))
            {
                var parent = new System.IO.DirectoryInfo(repositoryPath).Name;

                worktreePath = System.IO.Path.Combine("..", parent + "-" + string.Join("_", local.Name.Split(System.IO.Path.GetInvalidFileNameChars(), StringSplitOptions.RemoveEmptyEntries)));
            }

            var shell = GitFeatures.GitShell.GetNppInst();
            if (shell == null) return;

            try
            {
                var errMsg = shell.Execute(repositoryPath, string.Format(WorktreeAdd, worktreePath, local.Name));

                if (!string.IsNullOrEmpty(errMsg?.Trim()) && errMsg.Contains("fatal"))
                {
                    throw new Exception(errMsg);
                }
            }
            finally
            {
            }
        }

        public static void LockWorktree(this Repository repo, string worktreePath)
        {
            var shell = GitFeatures.GitShell.GetNppInst();
            if (shell == null) return;
            var repositoryPath = repo.Info.WorkingDirectory.Substring(0, repo.Info.WorkingDirectory.Length - 1);

            var errMsg = shell.Execute(repositoryPath, string.Format(WorktreeLock, worktreePath));

            if (!string.IsNullOrEmpty(errMsg?.Trim()) && errMsg.Contains("fatal"))
            {
                throw new Exception(errMsg);
            }
        }

        public static void UnlockWorktree(this Repository repo, string worktreePath)
        {
            var shell = GitFeatures.GitShell.GetNppInst();
            if (shell == null) return;
            var repositoryPath = repo.Info.WorkingDirectory.Substring(0, repo.Info.WorkingDirectory.Length - 1);

            var errMsg = shell.Execute(repositoryPath, string.Format(WorktreeUnlock, worktreePath));

            if (!string.IsNullOrEmpty(errMsg?.Trim()) && errMsg.Contains("fatal"))
            {
                throw new Exception(errMsg);
            }
        }

        public static void RemoveWorktree(this Repository repo, string worktreePath)
        {
            System.IO.Directory.Delete(worktreePath, true);
            repo.PruneWorktree();
        }
        public static void PruneWorktree(this Repository repo)
        {
            var shell = GitFeatures.GitShell.GetNppInst();
            if (shell == null) return;
            var repositoryPath = repo.Info.WorkingDirectory.Substring(0, repo.Info.WorkingDirectory.Length - 1);

            var errMsg = shell.Execute(repositoryPath, WorktreePrune);

            if (!string.IsNullOrEmpty(errMsg?.Trim()) && errMsg.Contains("fatal"))
            {
                throw new Exception(errMsg);
            }
        }
    }
}
