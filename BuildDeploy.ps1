# goto folder project
cd .\NppGitPlugin

# Clear folder before build
#if((Test-Path -Path .\bin\Release)) {
#    Remove-Item .\bin\Release -Force -Recurse
#}
# build project
#Start-Process -FilePath "MSBuild" -ArgumentList ('NppGit.csproj /t:Clean;Compile;Build /property:Configuration=Release /property:Platform=AnyCPU /property:DevEnvDir="D:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE"') -Wait -RedirectStandardOutput ..\build.log
# goto 
cd .\bin\Release

# if build success
if((Test-Path -Path NppGit.dll)) {

    $cloud_folder = "d:\Clouds\YandexDisk\Apps\NppGit\"
    $version = (Get-Item NppGit.dll).VersionInfo.FileVersion
    $version = $version.Substring(0,$version.LastIndexOf('.'))

    $plugin = '.\plugins'
    $zip = 'release-' + $version + '.zip'

    $dll = $plugin + '\NppGit\'

    New-Item -ItemType Directory -Path $plugin -Force
    New-Item -ItemType Directory -Path $dll -Force

    Copy-Item -Path "NppGit.dll" $plugin -Force
    Copy-Item -Path ".\*" $dll -Include "LibGit2Sharp.*" -Force
    Rename-Item "NLog.config" "NLog.dll.nlog" -Force
    Copy-Item -Path ".\*" $dll -Include "NLog.*" -Force
    Rename-Item "NLog.dll.nlog" "NLog.config" -Force
    Copy-Item -Path ".\NativeBinaries" $dll -Force -Recurse

    Compress-Archive -DestinationPath $zip -Path $plugin -Force
 
    Copy-Item -Path $zip $cloud_folder

    Remove-Item $plugin -Force -Recurse
    Remove-Item $zip -Force
}

cd ..\..\..\