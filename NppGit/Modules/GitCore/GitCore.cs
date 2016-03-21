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
using System.Linq;
using System.Xml.Linq;
using System.Diagnostics;

namespace NppGit.Modules.GitCore
{
    public class GitCore : IModule, IGitCore
    {
        #region IModule
        private IModuleManager _manager = null;
        private int _browserCmdId;
        private int _showInTitleCmdId;

        public void Final() { }

        private void DoBrowser()
        {
            Settings.Panels.RepoBrowserPanelVisible = _manager.ToogleFormState(_browserCmdId);
        }

        private void DoShowInTitle()
        {
            Settings.GitCore.ActiveRepoInTitle = !Settings.GitCore.ActiveRepoInTitle;
            _manager.SetCheckedMenu(_showInTitleCmdId, Settings.GitCore.ActiveRepoInTitle);
            _manager.ManualTitleUpdate();
        }

        public void Init(IModuleManager manager)
        {
            _manager = manager;

            manager.OnTabChangeEvent += ManagerOnTabChangeEvent;
            manager.OnSystemInit += ManagerOnSystemInit;
            manager.OnTitleChangedEvent += ManagerOnTitleChangedEvent;

            _browserCmdId = manager.RegisteCommandItem(new CommandItem
            {
                Name = "Repository browser",
                Hint = "Repository browser",
                Action = DoBrowser,
                Checked = Settings.Panels.RepoBrowserPanelVisible
            });

            _showInTitleCmdId = manager.RegisteCommandItem(new CommandItem
            {
                Name = "Active repo in title",
                Hint = "Active repo in title",
                Action = DoShowInTitle,
                Checked = Settings.GitCore.ActiveRepoInTitle
            });

            manager.RegisterDockForm(typeof(RepoBrowser), _browserCmdId, false);

            manager.RegisteCommandItem(new CommandItem
            {
                Name = "-",
                Hint = "-",
                Action = null
            });
        }

        private void ManagerOnTitleChangedEvent(object sender, TitleChangedEventArgs e)
        {
            if (_currentRepo != null && Settings.GitCore.ActiveRepoInTitle)
            {
                e.AddTitleItem("Active repo: " + _currentRepo.Name + ":" + _currentRepo.Branch);
            }
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
            SwitchByPath(PluginUtils.CurrentFilePath);
        }

        public bool IsNeedRun
        {
            get { return true; }
        }
        #endregion

        private XDocument _doc;
        private string _filename;

        #region Singletone
        private static GitCore _instance;
        private static object _objLock = new object();
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
                if (!type.Equals(typeof(Plugin)))
                {
                    throw new FieldAccessException("Property Module using only in Plugin class");
                }
                ctor();
                return _instance;
            }
        }

        private static void ctor()
        {
            if (_instance == null)
                lock (_objLock)
                    if (_instance == null)
                        _instance = new GitCore();
        }
        #endregion

        #region GitCore
        private GitCore()
        {
            string fileName = Path.Combine(PluginUtils.ConfigDir, Properties.Resources.PluginName, Properties.Resources.RepositoriesXml);
            if (File.Exists(fileName))
            {
                _doc = XDocument.Load(fileName);
                _repos = (from e in _doc.Descendants("Repository")
                             select e).ToDictionary(e => e.Attribute("Name").Value, (e) => { return new RepositoryLink(e.Value); });
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
        }

        #endregion

        #region IGitCore
        private RepositoryLink _currentRepo = null;
        private Dictionary<string, RepositoryLink> _repos = new Dictionary<string, RepositoryLink>();

        public event Action OnActiveRepositoryChanged;
        
        public RepositoryLink ActiveRepository
        {
            get { return _currentRepo; }
        }

        public List<RepositoryLink> Repositories
        {
            get { return _repos.Values.ToList(); }
        }

        public bool SwitchByPath(string path)
        {
            string newPath = GetRootDir(path);
            if (string.IsNullOrWhiteSpace(newPath) || !LibGit2Sharp.Repository.IsValid(newPath))
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
            if (_repos.ContainsKey(name))
            {
                _currentRepo = _repos[name];
                DoActiveRepository();
                return true;
            }
            else
            {
                return false;
            }
        }

        private void DoActiveRepository()
        {
            Settings.GitCore.LastActiveRepository = _currentRepo.Name;
            if (OnActiveRepositoryChanged != null)
            {
                OnActiveRepositoryChanged();
                _manager.ManualTitleUpdate();
            }
        }
        #endregion

        private void SaveRepo(RepositoryLink repoLink) 
        {
            _repos.Add(repoLink.Name, repoLink);
            var root = _doc.Root;
            var element = new XElement("Repository", repoLink.Path, new XAttribute("Name", repoLink.Name));
            root.Add(element);
            _doc.Save(_filename);
        }

        private static string GetRootDir(string path)
        {
            var search = Path.Combine(path, ".git");
            if (Directory.Exists(search) || File.Exists(search))
            {
                return path;
            }
            else
            {
                if (!string.IsNullOrEmpty(path) && Directory.GetParent(path) != null && Path.IsPathRooted(path))
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
