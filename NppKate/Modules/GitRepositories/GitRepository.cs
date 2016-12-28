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

using LibGit2Sharp;
using NLog;
using NppKate.Common;
using NppKate.Modules.GitCore;
using NppKate.Modules.GitRepositories.RepositoryExt;
using NppKate.Npp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace NppKate.Modules.GitRepositories
{
    public class GitRepository : IGitRepository, IDisposable
    {
        private const string CErrorEventTemplate = "Error in event {0}";
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly Dictionary<string, RepositoryLink> _repos = new Dictionary<string, RepositoryLink>();
        private RepositoryLink _currentRepo = null;
        private Repository _currRepo = null;
        private readonly XDocument _doc;
        private readonly string _filename;

        public GitRepository(string fileName)
        {
            if (File.Exists(fileName))
            {
                _doc = XDocument.Load(fileName);
                _repos = (from e in _doc.Descendants("Repository")
                          select e).ToDictionary(e => e.Attribute("Name")?.Value, (e) => new RepositoryLink(e.Value, e.Attribute("Name")?.Value));
                foreach (var v in _repos.Values.ToList())
                {
                    if (!IsValidGitRepo(v.Path))
                    {
                        _repos.Remove(v.Name);
                    }
                }
                if (_repos.ContainsKey(Settings.GitCore.LastActiveRepository))
                {
                    _currentRepo = _repos[Settings.GitCore.LastActiveRepository];
                    _currentRepo.SetActiveWorktree(Settings.GitCore.LastActiveWorktree);
                }
            }
            else
            {
                if (!Directory.Exists(Path.GetDirectoryName(fileName)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(fileName));
                }
                _doc = new XDocument();
                _doc.Add(new XElement("Repositories"));
                _doc.Save(fileName);
            }
            _filename = fileName;
        }

        public event Common.EventHandler<RepositoryChangedEventArgs> OnActiveRepositoryChanged;
        public event Common.EventHandler<RepositoryChangedEventArgs> OnDocumentReposituryChanged;
        public event Common.EventHandler<RepositoryChangedEventArgs> OnRepositoryAdded;
        public event Common.EventHandler<RepositoryChangedEventArgs> OnRepositoryRemoved;

        public RepositoryLink ActiveRepository => _currentRepo;
        public RepositoryLink DocumentRepository
        {
            get
            {
                var path = GetRootDir(NppUtils.CurrentFileDir);
                if (string.IsNullOrEmpty(path)) return null;
                Worktree newWt = null;

                if (Worktree.IsWorktreePath(path))
                {
                    newWt = new Worktree(wtDir: path);
                    path = Worktree.GetMainRepositoryPath(path);
                }

                var name = FindRepoByPath(path);
                var repo = _repos.ContainsKey(name) ? _repos[name] : null;
                if (repo != null)
                    repo.ActiveWorktree = newWt;
                return repo;
            }
        }

        public List<RepositoryLink> Repositories => _repos.Values.ToList();

        public string CurrentBranch => _currRepo?.Head.FriendlyName ?? "";

        public bool SwitchByPath(string path)
        {
            var result = InnerSwitchByPath(path);
            DoDocumentRepositiry(DocumentRepository?.Name, DocumentRepository?.ActiveWorktree?.Branch);
            return result;
        }

        private bool InnerSwitchByPath(string path)
        {
            var newPath = GetRootDir(path);
            Worktree newWt = null;

            if (Worktree.IsWorktreePath(newPath))
            {
                newWt = new Worktree(wtDir: newPath);
                newPath = Worktree.GetMainRepositoryPath(newPath);
            }

            if (string.IsNullOrWhiteSpace(newPath) || !Repository.IsValid(newPath))
            {
                return false;
            }

            var newRepo = new RepositoryLink(newPath);
            newRepo.ActiveWorktree = newWt;

            var oldName = FindRepoByPath(newPath);
            if (!string.IsNullOrWhiteSpace(oldName) && _repos[oldName].Path == newPath) // Репозиторий нашли по пути
            {
                if (oldName != newRepo.Name && !_repos.ContainsKey(newRepo.Name)) // но имена не совпадают
                    UpdateRepository(oldName, newRepo);
                else
                    newRepo.Name = oldName;
            }
            else // не нашли по пути
            {
                var i = 1;
                while (true)
                {
                    if (_repos.ContainsKey(newRepo.Name))
                    {
                        newRepo.Name += $"_{i}";
                        i++;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            if (_currentRepo != null)
            {
                // По пути сравнить надежнее, чем по имени
                if (_currentRepo.Path.Equals(newRepo.Path, StringComparison.InvariantCultureIgnoreCase) && (
                    _currentRepo.ActiveWorktree?.Path.Equals(newWt?.Path, StringComparison.InvariantCultureIgnoreCase) ?? false))
                {
                    return false;
                }
            }
            var switchName = newWt == null ? newRepo.Name : $"{newRepo.Name}|{newWt.Branch}";
            if (SwitchByName(switchName))
            {
                return true;
            }
            _currentRepo = newRepo;
            SaveRepo(newRepo);
            DoActiveRepository();
            return true;
        }

        private string FindRepoByPath(string path)
        {
            return _repos.Values.FirstOrDefault(r => r.Path.Equals(path, StringComparison.InvariantCultureIgnoreCase))?.Name ?? string.Empty;
        }

        private void UpdateRepository(string name, RepositoryLink link)
        {
            if (InnerRemoveRepository(name))
                SaveRepo(link);
        }

        public bool SwitchByName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return false;
            string repoName = null, wtName = null;
            if (name.Contains("|"))
            {
                var arr = name.Split('|');
                repoName = arr[0];
                wtName = arr[1];
            }
            else
            {
                repoName = name;
            }

            if (!_repos.ContainsKey(repoName))
                return false;
            _currentRepo = _repos[repoName];
            _currentRepo.SetActiveWorktree(wtName);
            DoActiveRepository();
            return true;
        }

        private void DoActiveRepository()
        {
            _currRepo?.Dispose();
            _currRepo = null;
            RepositoryChangedEventArgs args;
            if (_currentRepo != null)
            {
                _currRepo = new Repository(_currentRepo.Path);
                Settings.GitCore.LastActiveRepository = _currentRepo.Name;
                Settings.GitCore.LastActiveWorktree = _currentRepo.ActiveWorktree?.Branch ?? "";
                args = new RepositoryChangedEventArgs(_currentRepo.Name, _currentRepo.ActiveWorktree?.Branch);
            }
            else
            {
                args = new RepositoryChangedEventArgs(null, null);
            }
            try
            {
                OnActiveRepositoryChanged?.Invoke(this, args);
            }
            catch (Exception ex)
            {
                LoggerUtil.Error(Logger, ex, CErrorEventTemplate, "OnActiveRepositoryChanged");
            }
        }

        private void DoDocumentRepositiry(string repoName, string wtName = null)
        {
            var args = new RepositoryChangedEventArgs(repoName, wtName);
            try
            {
                OnDocumentReposituryChanged?.Invoke(this, args);
            }
            catch (Exception ex)
            {
                LoggerUtil.Error(Logger, ex, CErrorEventTemplate, "OnDocumentReposituryChanged");
            }
        }

        private void DoRepoAdded(string repoName, string wtName = null)
        {
            var args = new RepositoryChangedEventArgs(repoName, wtName);
            try
            {
                OnRepositoryAdded?.Invoke(this, args);
            }
            catch (Exception ex)
            {
                LoggerUtil.Error(Logger, ex, CErrorEventTemplate, "OnDocumentReposituryChanged");
            }
        }

        private void DoRepoRemoved(string repoName)
        {
            var args = new RepositoryChangedEventArgs(repoName);
            try
            {
                OnRepositoryRemoved?.Invoke(this, args);
            }
            catch (Exception ex)
            {
                LoggerUtil.Error(Logger, ex, CErrorEventTemplate, "OnDocumentReposituryChanged");
            }
        }

        public FileStatus GetFileStatus(string filePath)
        {
            var repoPath = GetRootDir(filePath);
            if (repoPath != _currentRepo?.Path)
            {
                return FileStatus.Nonexistent;
            }
            return _currRepo?.RetrieveStatus(filePath) ?? FileStatus.Nonexistent;
        }

        private void SaveRepo(RepositoryLink repoLink)
        {
            Logger.Trace($"Save repo Name={repoLink.Name}, Path={repoLink.Path}");
            _repos.Add(repoLink.Name, repoLink);
            DoRepoAdded(repoLink.Name, repoLink.ActiveWorktree?.Branch);
            var root = _doc.Root;
            var element = new XElement("Repository", repoLink.Path, new XAttribute("Name", repoLink.Name));
            root?.Add(element);
            try
            {
                _doc.Save(_filename);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        public static bool IsValidGitRepo(string path, bool detectWorktree = false)
        {
            Logger.Trace($"IsValidGitRepo path={path}");
            var repoDir = GetRootDir(path);
            return !string.IsNullOrEmpty(repoDir) && Directory.Exists(path) && (Repository.IsValid(repoDir) || detectWorktree && Worktree.IsWorktreePath(path));
        }

        public static string GetRootDir(string path)
        {
            while (true)
            {
                Logger.Trace($"GetRootDir path={path}");
                var search = Path.Combine(path, ".git");
                if (Directory.Exists(search) || File.Exists(search))
                {
                    return path;
                }
                if (string.IsNullOrEmpty(path) || Directory.GetParent(path) == null || !Path.IsPathRooted(path))
                    return null;
                path = Directory.GetParent(path).FullName;
            }
        }



        public static string GetRepoName(string repoDir)
        {
            if (!Directory.Exists(repoDir)) return null;

            Logger.Trace($"GetRepoName path={repoDir}");
            var repoName = "";
            using (var repo = new Repository(repoDir))
            {
                // Если есть несколько ссылок на репозиторий, то не можем точно определить
                // какое имя у репозитория должно быть, значит берем имя по папке
                if (repo.Network.Remotes.Any() && repo.Network.Remotes.Count() == 1)
                {
                    var remote = repo.Network.Remotes.FirstOrDefault();
                    var remoteUrl = remote?.Url;
                    if (!string.IsNullOrEmpty(remoteUrl))
                    {
                        repoName = remoteUrl.Substring(remoteUrl.LastIndexOf('/') + 1, remoteUrl.Length - remoteUrl.LastIndexOf('/') - 1).Replace(".git", "");
                    }
                }

                if (string.IsNullOrEmpty(repoName))
                {
                    repoName = new DirectoryInfo(repoDir).Name;
                }
                Logger.Trace($"return {repoName}");
                return repoName;
            }
        }

        public RepositoryLink GetRepositoryByName(string name)
        {
            return _repos.ContainsKey(name) ? _repos[name] : null;
        }

        private bool InnerRemoveRepository(string name)
        {
            XElement root, element;
            if ((root = _doc.Root) != null && (element = root.Descendants("Repository").Where(e => e.Attribute("Name").Value == name).FirstOrDefault()) != null)
            {
                _repos.Remove(name);
                element.Remove();
                if (_currentRepo?.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase) ?? false)
                {
                    _currentRepo = null;
                    DoActiveRepository();
                }

                DoRepoRemoved(name);
                try
                {
                    _doc.Save(_filename);
                    return true;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                }
            }
            return false;

        }

        public void RemoveRepository(string name)
        {
            InnerRemoveRepository(name);
        }

        public void Dispose()
        {
            if (_currRepo != null)
            {
                _currRepo.Dispose();
                _currRepo = null;
            }
        }
    }
}
