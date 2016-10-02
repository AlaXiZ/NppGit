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
using System;
using NppKate.Npp;

namespace NppKate.Modules
{
    public class Git : IModule
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private IModuleManager _manager;

        public bool IsNeedRun
        {
            get { return Settings.Modules.Git; }
        }
        
        public void Final()
        {
        }

        public void Init(IModuleManager manager)
        {
            _manager = manager;

            var selfName = GetType().Name;

            _manager.CommandManager.RegisterCommand(selfName, "File in other branch", OpenFileInOtherBranch);
            _manager.CommandManager.RegisterSeparator(selfName);
        }

        private void OpenFileInOtherBranch()
        {
            var repoDir = NppUtils.GetRootDir(NppUtils.CurrentFileDir);
            try
            {
                using (var repo = new Repository(repoDir))
                {
                    var dlg = new Forms.BranchList();
                    dlg.RepoDirectory = repoDir;
                    // Replace "\" to "/" and delete first "/"
                    var fileInRepo = NppUtils.CurrentFilePath.Replace(repoDir, "").Replace("\\", "/").Remove(0, 1);

                    if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        Branch branch = null;
                        foreach (var b in repo.Branches)
                        {
                            if (b.CanonicalName == dlg.SelectedItem)
                            {
                                branch = b;
                                break;
                            }
                        }
                        if (branch != null)
                        {
                            var commit = repo.Lookup<Commit>(branch.Tip.Id);
                            var blob = (Blob)commit[fileInRepo].Target;
                            var fileName = commit.Sha.Substring(1, Settings.Functions.SHACount) + "_" + NppUtils.CurrentFileName;
                            var contentStream = blob.GetContentStream();
                            var outFile = GitHelper.SaveStreamToFile(contentStream, fileName);
                            if (outFile != null)
                            {
                                var curFile = NppUtils.CurrentFilePath;
                                NppUtils.OpenFile(outFile);
                                if (Settings.Functions.OpenFileInOtherView)
                                {
                                    NppUtils.MoveFileToOtherView();
                                    NppUtils.CurrentFilePath = curFile;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                logger.Debug(e, "Directory {0} isn't git repository!", repoDir);
            }
        }

    }
}
