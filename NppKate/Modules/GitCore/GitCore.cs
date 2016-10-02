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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using LibGit2Sharp;
using NLog;
using NppKate.Common;
using NppKate.Npp;

namespace NppKate.Modules.GitCore
{
    public class GitCore : IModule, IGitCore
    {
        private const string CErrorEventTemplate = "Error in event {0}";
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        #region IModule
        private IModuleManager _manager = null;
        private int _browserCmdId;
        private Repository _currRepo = null;
        private RepoBrowser _repoBrowser = null;

        public void Final()
        {
            _currRepo?.Dispose();
        }

        private void DoBrowser()
        {
            if (_repoBrowser == null)
            {
                var icon = _manager.ResourceManager.LoadToolbarIcon(Resources.ExternalResourceName.IDB_REPOSITORIES);
                _repoBrowser = _manager.FormManager.BuildForm<RepoBrowser>(_browserCmdId, NppTbMsg.DWS_PARAMSALL | NppTbMsg.DWS_DF_CONT_RIGHT, icon.Handle, (IDockableManager)_manager);
                Settings.Panels.RepoBrowserPanelVisible = true;
            }
            else
            {
                Settings.Panels.RepoBrowserPanelVisible = _manager.FormManager.ToogleVisibleDockableForm(_repoBrowser.Handle);
                _manager.CommandManager.SetCommandChekedState(_browserCmdId, Settings.Panels.RepoBrowserPanelVisible);
            }
        }

        public void Init(IModuleManager manager)
        {
            _manager = manager;

            manager.OnTabChangeEvent += ManagerOnTabChangeEvent;
            manager.OnSystemInit += ManagerOnSystemInit;
            manager.OnToolbarRegisterEvent += ManagerOnToolbarRegisterEvent;

            var selfName = GetType().Name;

            _browserCmdId = manager.CommandManager.RegisterCommand(selfName, Properties.Resources.CmdRepositoryBrowser, DoBrowser, Settings.Panels.RepoBrowserPanelVisible);

            manager.CommandManager.RegisterCommand(selfName, Properties.Resources.CmdQuickSearch, DoQuickSearch, false, new ShortcutKey(false, true, false, System.Windows.Forms.Keys.F));

            manager.CommandManager.RegisterSeparator(selfName);

            if (!Settings.CommonSettings.GetToolbarCommandState(selfName, Properties.Resources.CmdRepositoryBrowser))
                Settings.CommonSettings.SetToolbarCommandState(selfName, Properties.Resources.CmdRepositoryBrowser, true);
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

        private void DoQuickSearch()
        {
            var path = ActiveRepository?.Path;
            if (path == null) return;
            var searchDialog = new TortoiseLogSearch();
            searchDialog.Init((IDockableManager)_manager);
            searchDialog.RepositoryPath = path;
            searchDialog.SearchText = NppUtils.GetSelectedText();
            searchDialog.Show();
        }
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
                Ctor();
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
                Ctor();
                return _instance;
            }
        }

        private static void Ctor()
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
                          select e).ToDictionary(e => e.Attribute("Name")?.Value, (e) => new RepositoryLink(e.Value));
                foreach (var name in _repos.Keys.ToList())
                {
                    if (_repos[name].Name == null)
                    {
                        _repos.Remove(name);
                    }
                }
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

        public string CurrentBranch => _currRepo?.Head.CanonicalName ?? "";

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

            var oldName = FindRepoByPath(newPath);
            if (!string.IsNullOrWhiteSpace(oldName))
            {
                UpdateRepository(oldName, newRepo);
            }

            if (_currentRepo != null)
            {
                // По пути сравнить надежнее, чем по имени
                if (_currentRepo.Path.Equals(newRepo.Path, StringComparison.InvariantCultureIgnoreCase))
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

        private string FindRepoByPath(string path)
        {
            return _repos.Values.FirstOrDefault(r => r.Path == path)?.Name ?? string.Empty;
        }

        private void UpdateRepository(string name, RepositoryLink link)
        {
            if (InnerRemoveRepository(name))
                SaveRepo(link);
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
            _currRepo = null;
            RepositoryChangedEventArgs args;
            if (_currentRepo != null)
            {
                _currRepo = new Repository(_currentRepo.Path);
                Settings.GitCore.LastActiveRepository = _currentRepo.Name;
                args = new RepositoryChangedEventArgs(_currentRepo.Name);
            }
            else
            {
                args = new RepositoryChangedEventArgs(null);
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

        private void DoDocumentRepositiry(string repoName)
        {
            var args = new RepositoryChangedEventArgs(repoName);
            try
            {
                OnDocumentReposituryChanged?.Invoke(this, args);
            }
            catch (Exception ex)
            {
                LoggerUtil.Error(Logger, ex, CErrorEventTemplate, "OnDocumentReposituryChanged");
            }
        }

        private void DoRepoAdded(string repoName)
        {
            var args = new RepositoryChangedEventArgs(repoName);
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

        #endregion

        private void SaveRepo(RepositoryLink repoLink)
        {
            Logger.Trace($"Save repo Name={repoLink.Name}, Path={repoLink.Path}");
            _repos.Add(repoLink.Name, repoLink);
            DoRepoAdded(repoLink.Name);
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

        public static bool IsValidGitRepo(string path)
        {
            Logger.Trace($"IsValidGitRepo path={path}");
            var repoDir = GetRootDir(path);
            return !string.IsNullOrEmpty(repoDir) && Repository.IsValid(repoDir);
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
                Logger.Trace($"return {remote}");
                return remote;
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
    }
}
