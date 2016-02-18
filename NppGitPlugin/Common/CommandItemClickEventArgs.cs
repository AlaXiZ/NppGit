using System;

namespace NppGit.Common
{
    public class CommandItemClickEventArgs : EventArgs
    {
        public string CommandName { get; protected set; }

        public CommandItemClickEventArgs(string commandName)
        {
            CommandName = commandName;
        }
    }
}
