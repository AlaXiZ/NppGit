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