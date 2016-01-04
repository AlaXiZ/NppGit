echo off

set npp="d:\npp\plugins"
set npp_git="%npp%\NppGit"
rem set build="%1%"
set build="NppGitPlugin\bin\Debug"

md "%npp_git%"

rem CSScriptIntellisense.dll cannot be copied from build events as it would copy the assembly before DllExport is performed
rem so it needs to be done manually.
echo "NLog copy"
copy "%build%\NLog.dll" "%npp_git%\NLog.dll"
copy "%build%\NLog.xml" "%npp_git%\NLog.xml"
copy "%build%\NLog.xsd" "%npp_git%\NLog.xsd"
copy "%build%\NLog.config" "%npp_git%\NLog.dll.nlog"

echo "LibGit2Sharp copy"
copy "%build%\LibGit2Sharp.dll" "%npp_git%\LibGit2Sharp.dll"
copy "%build%\LibGit2Sharp.xml" "%npp_git%\LibGit2Sharp.xml"
xcopy "%build%\NativeBinaries" "%npp_git%\NativeBinaries" /s /y /d

echo "Plugin copy"
rem need to keep it last so copy errors (if any) are visible
copy "%build%\NppGit.dll" "%npp%\NppGit.dll"
copy "%build%\NppGit.pdb" "%npp%\NppGit.pdb"
copy "%build%\NppGit.lib" "%npp%\NppGit.lib"
copy "%build%\NppGit.exp" "%npp%\NppGit.exp"

pause