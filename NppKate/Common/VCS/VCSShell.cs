/*
Copyright (c) 2016, Schadin Alexey (schadin@gmail.com)
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


using System.Threading.Tasks;

namespace NppKate.Common.VCS
{
    public abstract class VCSShell : IVCSShell
    {
        protected string _executePath;
        protected string _exe;

        public VCSShell(string shellExecutePath)
        {
            _executePath = shellExecutePath;
        }

        public string ShellExecutePath
        {
            get { return _executePath; }
        }

        public bool IsWaitShell
        {
            get; set;
        }

        public virtual void ExecuteCommand(VCSCommand command)
        {
            Execute(command.Path, command.CommandString);
        }

        public virtual void Execute(string workingDirectory, string arguments)
        {
            var task = new Task(() =>
            {
                var pi = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = System.IO.Path.Combine(_executePath, _exe),
                    WorkingDirectory = workingDirectory,
                    Arguments = arguments,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true
                };

                var process = new System.Diagnostics.Process();
                process.StartInfo = pi;
                process.OutputDataReceived += (o, e) => System.Console.Write(e.Data);

                process.Start();
                process.BeginOutputReadLine();
                process.WaitForExit();
                process.CancelOutputRead();
            });

            task.Start();
            if (IsWaitShell)
                task.Wait();
        }
    }
}