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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NppKate.Core
{
    public class ModuleLoader
    {
        private Type[] _types;
        private string _modulePath;
        public ModuleLoader(string modulePath)
        {
            _modulePath = modulePath;
        }

        public uint LoadModules()
        {
            LoadAssemblies();
            FindTypes();
            return (uint?)_types?.Length ?? 0;
        }

        private void FindTypes()
        {
            var comparer = new TypeComparer();
            var moduleIntf = typeof(IModule);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                        .SelectMany(s => s.GetTypes())
                        .Where(p => moduleIntf.IsAssignableFrom(p))
                        .Where(p => p.IsClass)                    // Нужны только классы
                        .OrderBy(t => t, comparer);
            _types = types.ToArray();
        }

        private void LoadAssemblies()
        {
            if (!Directory.Exists(_modulePath))
                return;

            var dInfo = new DirectoryInfo(_modulePath);
            foreach(var f in dInfo.GetFileSystemInfos("Kate.*.dll"))
            {
                AssemblyLoader.LoadAssembly(f.FullName);
            }
        }

        public Type[] Modules => _types;
    }

    class TypeComparer : IComparer<Type>
    {
        public int Compare(Type x, Type y)
        {
            var xOrder = (x.GetCustomAttributes(typeof(Module), false).FirstOrDefault() as Module).Order;
            var yOrder = (y.GetCustomAttributes(typeof(Module), false).FirstOrDefault() as Module).Order;

            return xOrder.CompareTo(yOrder);
        }
    }
}
