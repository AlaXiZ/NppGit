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
using NppKate.Common;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Diagnostics;
using LibGit2Sharp;
using NppKate.Npp;
using NLog;

namespace NppKate.Modules.GitCore
{
    public class GitCore : IModule, IGitCore
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        #region IModule
        private IModuleManager _manager = null;
        private int _browserCmdId;
        private Repository _currRepo = null;

        public void Final()
        {
            _currRepo?.Dispose();
        }

        private void DoBrowser()
        {
            Settings.Panels.RepoBrowserPanelVisible = _manager.ToogleFormState(_browserCmdId);
        }

        public void Init(IModuleManager manager)
        {
            _manager = manager;

            manager.OnTabChangeEvent += ManagerOnTabChangeEvent;
            manager.OnSystemInit += ManagerOnSystemInit;
            manager.OnToolbarRegisterEvent += ManagerOnToolbarRegisterEvent;

            _browserCmdId = manager.RegisterCommandItem(new CommandItem
            {
                Name = "Repository browser",
                Hint = "Repository browser",
                Action = DoBrowser,
                Checked = Settings.Panels.RepoBrowserPanelVisible
            });

            manager.RegisterDockForm(_browserCmdId, new DockDialogData
            {
                Class = typeof(RepoBrowser),
                IconResourceName = Resources.ExternalResourceName.IDB_REPOSITORIES,
                Title = "Repository browser",
                uMask = NppTbMsg.DWS_PARAMSALL | NppTbMsg.DWS_DF_CONT_LEFT
            });
            //manager.RegisterDockForm(typeof(RepoBrowser), _browserCmdId, false);

            manager.RegisterCommandItem(new CommandItem
            {
                Name = "-",
                Hint = "-",
                Action = null
            });
        }

        private void ManagerOnToolbarRegisterEvent()
        {
            _manager.AddToolbarButton(_browserCmdId, Resources.ExternalResourceName.IDB_REPOSITORIES);
        }

        private void ManagerOnSystemInit()
        {
            if (Settings.Panels.RepoBrowserPanelVisible)
            {
                DoBrowser();
            }
        }

        private void ManagerOnTabChangeEvent(object sender, TabEventArgs e)
        {
            if (SwitchByPath(NppUtils.CurrentFilePath))
                DoActiveRepository();
            DoDocumentRepositiry(DocumentRepository?.Name);
        }

        public bool IsNeedRun => true;
        #endregion

        private readonly XDocument _doc;
        private readonly string _filename;

        #region Singletone
        private static GitCore _instance;
        private static readonly object ObjLock = new object();
        public static IGitCore Instance
        {
            get
            {
                ctor();
                return _instance;
            }
        }

        public static IModule Module
        {
            get
            {
                var mth = new StackTrace().GetFrame(1).GetMethod();
                var type = mth.ReflectedType;
                if (type != typeof(Main))
                {
                    throw new FieldAccessException("Property Module using only in Plugin class");
                }
                ctor();
                return _instance;
            }
        }

        private static void ctor()
        {
            if (_instance != null) return;
            lock (ObjLock)
                if (_instance == null)
                    _instance = new GitCore();
        }

        #endregion

        #region GitCore
        private GitCore()
        {
            var fileName = Path.Combine(NppUtils.ConfigDir, Properties.Resources.PluginName, Properties.Resources.RepositoriesXml);
            if (File.Exists(fileName))
            {
                _doc = XDocument.Load(fileName);
                _repos = (from e in _doc.Descendants("Repository")
                             select e).ToDictionary(e => e.Attribute("Name").Value, (e) => new RepositoryLink(e.Value));
                if (_repos.ContainsKey(Settings.GitCore.LastActiveRepository))
                {
                    _currentRepo = _repos[Settings.GitCore.LastActiveRepository];
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

            SwitchByName(Settings.GitCore.LastActiveRepository);
        }

        #endregion

        #region IGitCore
        private RepositoryLink _currentRepo = null;
        private readonly Dictionary<string, RepositoryLink> _repos = new Dictionary<string, RepositoryLink>();

        public event Common.EventHandler<RepositoryChangedEventArgs> OnActiveRepositoryChanged;
        public event Common.EventHandler<RepositoryChangedEventArgs> OnDocumentReposituryChanged;
        public event Common.EventHandler<RepositoryChangedEventArgs> OnRepositoryAdded;
        public event Common.EventHandler<RepositoryChangedEventArgs> OnRepositoryRemoved;

        public RepositoryLink ActiveRepository => _currentRepo;

        public List<RepositoryLink> Repositories => _repos.Values.ToList();

        public string CurrentBranch => _currRepo?.Head.Name ?? "";

        public RepositoryLink DocumentRepository
        {
            get
            {
                var path = GetRootDir(NppUtils.CurrentFileDir);
                if (path == null) return null;
                var name = GetRepoName(path);
                return _repos.ContainsKey(name) ? _repos[name] : null;
            }
        }

        public bool SwitchByPath(string path)
        {
            var newPath = GetRootDir(path);
            if (string.IsNullOrWhiteSpace(newPath) || !Repository.IsValid(newPath))
            {
                return false;
            }
            var newRepo = new RepositoryLink(newPath);
            if (_currentRepo != null)
            {
                if (_currentRepo.Name.Equals(newRepo.Name, StringComparison.InvariantCultureIgnoreCase))
                {
                    return false;
                }
            }
            if (SwitchByName(newRepo.Name))
            {
                return true;
            }
            _currentRepo = newRepo;
            SaveRepo(newRepo);
            DoActiveRepository();
            return true;
        }

        public bool SwitchByName(string name)
        {
            if (!_repos.ContainsKey(name)) return false;
            _currentRepo = _repos[name];
            DoActiveRepository();
            return true;
        }

        private void DoActiveRepository()
        {
            _currRepo?.Dispose();
            _currRepo = new Repository(_currentRepo.Path);

            Settings.GitCore.LastActiveRepository = _currentRepo.Name;
            var args = new RepositoryChangedEventArgs(_currentRepo.Name);
            OnActiveRepositoryChanged?.Invoke(this, args);
        }

        private void DoDocumentRepositiry(string repoName)
        {
            var args = new RepositoryChangedEventArgs(repoName);
            OnDocumentReposituryChanged?.Invoke(this, args);
        }

        private void DoRepoAdded(string repoName)
        {
            var args = new RepositoryChangedEventArgs(repoName);
            OnRepositoryAdded?.Invoke(this, args);
        }

        private void DoRepoRemoved(string repoName)
        {
            var args = new RepositoryChangedEventArgs(repoName);
            OnRepositoryRemoved?.Invoke(this, args);
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

        #endregion

        private void SaveRepo(RepositoryLink repoLink) 
        {
            _logger.Trace($"Save repo Name={repoLink.Name}, Path={repoLink.Path}");
            _repos.Add(repoLink.Name, repoLink);
            DoRepoAdded(repoLink.Name);
            var root = _doc.Root;
            var element = new XElement("Repository", repoLink.Path, new XAttribute("Name", repoLink.Name));
            root?.Add(element);
            try
            {
                _doc.Save(_filename);
            } catch (Exception ex)
            {
                _logger.Error(ex);
            }

        }

        public static bool IsValidGitRepo(string path)
        {
            _logger.Trace($"IsValidGitRepo path={path}");
            var repoDir = GetRootDir(path);
            return !string.IsNullOrEmpty(repoDir) && Repository.IsValid(repoDir);
        }

        public static string GetRootDir(string path)
        {
            _logger.Trace($"GetRootDir path={path}");
            var search = Path.Combine(path, ".git");
            if (Directory.Exists(search) || File.Exists(search))
            {
                return path;
            }
            if (!string.IsNullOrEmpty(path) && Directory.GetParent(path) != null && Path.IsPathRooted(path))
            {
                return GetRootDir(Directory.GetParent(path).FullName);
            }
            return null;
        }

        public static string GetRepoName(string repoDir)
        {
            _logger.Trace($"GetRepoName path={repoDir}");
            var remote = "";
            using (var repo = new Repository(repoDir))
            {
                if (repo.Network.Remotes.Any())
                {
                    var remoteUrl = repo.Network.Remotes.First().Url;
                    if (!string.IsNullOrEmpty(remoteUrl))
                    {
                        remote = remoteUrl.Substring(remoteUrl.LastIndexOf('/') + 1, remoteUrl.Length - remoteUrl.LastIndexOf('/') - 1).Replace(".git", "");
                    }
                }
                else
                {
                    remote = new DirectoryInfo(repoDir).Name;
                }
                _logger.Trace($"return {remote}");
                return remote;
            }
        }

        public RepositoryLink GetRepositoryByName(string repoName)
        {
            return _repos[repoName];
        }
    }
}
