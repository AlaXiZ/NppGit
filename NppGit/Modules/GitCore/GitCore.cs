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
using NppGit.Common;
using System.Collections.Generic;
using System.IO;

namespace NppGit.Modules.GitCore
{
    public class GitCore : IModule, IGitCore
    {
        #region IModule
        IModuleManager _manager = null;

        public void Final() { }

        public void Init(IModuleManager manager)
        {
            _manager = manager;
        }

        public bool IsNeedRun
        {
            get { return true; }
        }
        #endregion

        private static GitCore _instance;
        private static object _objLock = new object();
        
        public static IGitCore Instance
        {
            get
            {
                if (_instance == null)
                    lock(_objLock)
                        if (_instance == null)
                            _instance = new GitCore();
                
                return _instance;
            }
        }

        private GitCore()
        {
            // TODO: load repo list
        }

        #region IGitCore
        private string _currentRepoName = null;
        private string _currentRepoPath = null;
        private Dictionary<string, Repository> _repos = new Dictionary<string, Repository>();

        public event Action OnActiveRepositoryChanged;
        
        public Repository ActiveRepository
        {
            get
            {
                if (_currentRepoName != null && _repos.ContainsKey(_currentRepoName))
                    return _repos[_currentRepoName];
                else
                    return null;                
            }
        }

        public void UpdateCurrentPath(string path)
        {
            string newPath = GetRootDir(path);
            if (string.IsNullOrWhiteSpace(newPath))
            {
                return;
            }
            // TODO: Check newPath
            // if exists in repo-list, switch to branch
            // else add to list and switch to...
        }
        #endregion

        private static string GetRootDir(string path)
        {
            var search = Path.Combine(path, ".git");
            if (Directory.Exists(search) || File.Exists(search))
            {
                return path;
            }
            else
            {
                if (!string.IsNullOrEmpty(path) && Directory.GetParent(path) != null)
                {
                    return GetRootDir(Directory.GetParent(path).FullName);
                }
                else {
                    return null;
                }
            }
        }
    }
}
