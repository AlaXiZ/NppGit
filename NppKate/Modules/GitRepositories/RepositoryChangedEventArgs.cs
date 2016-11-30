// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;

namespace NppKate.Modules.GitCore
{
    public class RepositoryChangedEventArgs : EventArgs
    {
        public string RepositoryName { get; protected set; }

        public RepositoryChangedEventArgs(string repositoryName)
        {
            RepositoryName = repositoryName;
        }
    }
}