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

using NppKate.Common;
using NppKate.Core;
using System;

namespace NppKate.Modules.GitFeatures
{
    [Module("Git shell", 1000, "{C2FA64C4-5B09-4407-9C8C-3A8DC6F46529}", "1.0.0")]
    public class GitBash : IModule
    {
        private IModuleManager _manager;
        private GitShell _gitShell;
        private static readonly string _gitBinPath = System.IO.Path.Combine("Git", "cmd");
        
        public void Context(IModuleManager manager)
        {
            _manager = manager;
        }

        public void Registration()
        {
            if (SearchGit())
            {
                _gitShell = new GitShell(Settings.GitBash.BinPath);

                _manager.RegisterService(typeof(IGitShell), _gitShell);
            }
            else
            {
                Console.WriteLine("Need configure git path: NppKate-Settings-Git");
            }
        }

        public void Initialization()
        {
        }

        public void Finalization() { }

        private static bool ExistsGit(string programPath)
        {
            return System.IO.Directory.Exists(System.IO.Path.Combine(programPath, _gitBinPath));
        }

        private static bool SearchGit()
        {
            var gitPath = Settings.GitBash.BinPath;
            // Path not set and first run
            if (string.IsNullOrEmpty(gitPath) && Settings.GitBash.IsFirstRun)
            {
                var env_path = Environment.GetEnvironmentVariable("PATH");
                if (env_path.Contains(_gitBinPath))
                {
                    var idx = env_path.IndexOf(_gitBinPath);
                    var stopIdx = env_path.IndexOf(';', idx);
                    if (stopIdx == -1)
                        stopIdx = env_path.Length;
                    var pathTmp = env_path.Substring(0, stopIdx);
                    idx = pathTmp.LastIndexOf(';');
                    gitPath = pathTmp.Substring(idx + 1);
                }
                // If OS x64, then search in "Program Files (x86)"
                if (string.IsNullOrEmpty(gitPath) && (8 == IntPtr.Size || !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432"))))
                {
                    var path = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
                    if (ExistsGit(path))
                    {
                        gitPath = System.IO.Path.Combine(path, _gitBinPath);
                    }
                }
                // if not found or OS x32, then search in "Program Files"
                if (string.IsNullOrEmpty(gitPath))
                {
                    var environmentVariable = Environment.GetEnvironmentVariable("ProgramFiles");
                    if (environmentVariable != null)
                    {
                        // But npp is 32bit process,
                        // then Environment.GetEnvironmentVariable("ProgramFiles") return "Program Files (x86)"
                        var path = environmentVariable.Replace(" (x86)", "");
                        if (ExistsGit(path))
                        {
                            gitPath = System.IO.Path.Combine(path, _gitBinPath);
                        }
                    }
                }
                // If found then save path in setting
                if (!string.IsNullOrEmpty(gitPath))
                    Settings.GitBash.BinPath = gitPath;
                Settings.GitBash.IsFirstRun = false;
            }
            
            return !string.IsNullOrEmpty(gitPath) && System.IO.Directory.Exists(gitPath);
        }
    }
}
