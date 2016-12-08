// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NppKate.Common;

namespace NppKate.Modules.GitRepositories.RepositoryExt
{
    public class Worktree
    {
        public string Path { get; set; }

        public string Branch { get; set; }

        public string HeadSha { get; set; }

        public bool IsLocked { get; set; }

        public Worktree() { }

        public Worktree(string gitdir)
        {
            var @ref = GitHelper.ReadOneLineFromFile(System.IO.Path.Combine(gitdir, "HEAD"));
            var Branch = @ref.Replace("ref: refs/heads/", "");
            var HeadSha = GitHelper.ReadOneLineFromFile(System.IO.Path.Combine(gitdir, "ORIG_HEAD"));
            var Path = GitHelper.ReadOneLineFromFile(System.IO.Path.Combine(gitdir, "gitdir"))?.Replace("/.git", "");
            var IsLocked = new FileInfo(System.IO.Path.Combine(gitdir, "locked")).Exists;
        }

        public static bool IsWorktreePath(string path)
        {
            return File.Exists(System.IO.Path.Combine(path, ".git"));
        }
        public static string GetMainRepositoryPath(string path)
        {
            var mainRepoPath = new StringBuilder(270);
            var git = System.IO.Path.Combine(path, ".git");
            if (File.Exists(git))
            {
                mainRepoPath.Append(GitHelper.ReadOneLineFromFile(git));
                mainRepoPath.Replace("gitdir: ", "").Replace(".git", "|");
                int pos;
                for (pos = 0; pos < mainRepoPath.Length; pos++)
                {
                    if (mainRepoPath[pos] == '|')
                        break;
                }
                if (pos > 0 && pos < mainRepoPath.Length)
                    mainRepoPath.Remove(pos - 1, mainRepoPath.Length - pos + 1);

            }
            return new DirectoryInfo(mainRepoPath.ToString()).FullName;
        }



    }
}
