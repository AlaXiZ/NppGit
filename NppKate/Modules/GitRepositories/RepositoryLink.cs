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


using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using LibGit2Sharp;
using NppKate.Modules.GitRepositories.RepositoryExt;

namespace NppKate.Modules.GitCore
{
    public class RepositoryLink
    {
        public string Name
        {
            get; set;
        }

        public string Path
        {
            get; protected set;
        }

        public RepositoryLink(string path, string name = null)
        {
            Path = path;
            Name = name ?? GitRepository.GetRepoName(path);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            return obj != null && obj is RepositoryLink && (obj as RepositoryLink).Path.Equals(Path);
        }

        public override int GetHashCode()
        {
            return Path.GetHashCode();
        }

        public Branch Head
        {
            get
            {
                using (var repo = new Repository(Path))
                {
                    return repo.Head;
                }
            }
        }
        
        public Branch[] LocalBranches
        {
            get
            {
                using (var repo = new Repository(Path))
                {
                    return repo.Branches.Where(b => !b.IsRemote).ToArray();
                }
            }
        }
        public Branch[] RemoteBranches
        {
            get
            {
                using (var repo = new Repository(Path))
                {
                    return repo.Branches.Where(b => b.IsRemote).ToArray();
                }
            }
        }
        public Branch[] UntractedBranches
        {
            get
            {
                using (var repo = new Repository(Path))
                {
                    return repo.Branches.Where(b => !b.IsRemote && !b.IsTracking).ToArray();
                }
            }
        }

        private static string ReadOneLineFromFile(string path)
        {
            using (var read = new StreamReader(path, Encoding.UTF8))
            {
                return read.ReadLine();
            }
        }

        public Worktree[] Worktrees
        {
            get
            {
                var wtFolder = System.IO.Path.Combine(Path, ".git", "worktrees");
                var di = new DirectoryInfo(wtFolder);
                var result = new List<Worktree>();
                if (di.Exists)
                    foreach (var d in di.GetDirectories())
                    {
                        var @ref = ReadOneLineFromFile(System.IO.Path.Combine(d.FullName, "HEAD"));
                        var branch = @ref.Replace("ref: refs/heads/", "");
                        var head = ReadOneLineFromFile(System.IO.Path.Combine(d.FullName, "ORIG_HEAD"));
                        var path = ReadOneLineFromFile(System.IO.Path.Combine(d.FullName, "gitdir"))?.Replace("/.git", "");
                        var fi = new FileInfo(System.IO.Path.Combine(d.FullName, "locked"));
                        result.Add(new Worktree
                        {
                            HeadSha = head,
                            Path = path,
                            IsLocked = fi.Exists,
                            Branch = branch
                        });
                    }
                return result.ToArray();
            }
        }
    }
}
