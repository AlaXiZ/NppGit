using System;

namespace NppKate.Modules.TortoiseGitFeatures
{
    [Flags]
    public enum TortoiseGitFindType : uint
    {
        ByNothing = 0x0000,
        ByMessages = 0x0001,
        ByPath = 0x0002,
        ByAuthors = 0x0004,
        ByRevisions = 0x0008,
        ByBugID = 0x0020,
        BySubject = 0x0040,
        ByRefName = 0x0080,
        ByEmail = 0x0100,
        ByNotes = 0x0200,
        ByEverything = 0xffff
    }
}
