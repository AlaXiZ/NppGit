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

namespace NppKate.Modules.GitRepositories.RepositoryExt
{
    public static class WorktreeOperation
    {
        const string PullCommand = "pull --rebase --autostash -v";
        const string CommitComand = "gui citool";
        const string PushCommand = "push -v";
        const string LogCommand = "gitk.exe";

        public static void Pull(this Worktree wt)
        {
            GitCommand(wt, PullCommand);
        }
        public static void Commit(this Worktree wt)
        {
            GitCommand(wt, CommitComand);
        }
        public static void Push(this Worktree wt)
        {
            GitCommand(wt, PushCommand);
        }

        public static void Log(this Worktree wt)
        {
            var shell = GitFeatures.GitShell.GetNppInst();
            if (shell == null) return;
            shell.GetType().GetField("_exe", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .SetValue(shell, LogCommand);

            shell.GetType().GetField("_executePath", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .SetValue(shell, System.IO.Path.Combine(shell.ShellExecutePath, "..", "cmd"));

            string errMsg = null;
            try
            {
                errMsg = shell.Execute(wt.Path, "");
            }
            finally
            {
                if (!string.IsNullOrWhiteSpace(errMsg))
                    Console.WriteLine(errMsg);
            }
        }

        private static void GitCommand(Worktree wt, string command)
        {
            var shell = GitFeatures.GitShell.GetNppInst();
            if (shell == null) return;

            string errMsg = null;
            try
            {
                errMsg = shell.Execute(wt.Path, command);
            }
            finally
            {
                if (!string.IsNullOrWhiteSpace(errMsg))
                  Console.WriteLine(errMsg);
            }
        }
    }
}
