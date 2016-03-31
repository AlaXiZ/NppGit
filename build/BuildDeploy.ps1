param([Int32]$build = 0, [Int32]$deploy = 0, [Int32]$clearAfterDeploy = 1, [string]$addVersion = "")
# goto folder project
# cd .\NppGit

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
	$net = "net4_0"
    $version = (Get-Item NppGit.dll).VersionInfo.FileVersion
    $version = $version.Substring(0,$version.LastIndexOf('.')) + $addVersion

    $plugin = '.\plugins'
    $zip = 'NppGit-release-' + $version + '-' + $net + '.zip'
    $release = '.\' + $version + '-' + $net

    $dll = $plugin + '\NppGit\'
	
	if ($build -eq 1) {
        New-Item -ItemType Directory -Path $plugin -Force
        New-Item -ItemType Directory -Path $dll -Force
        
        Copy-Item -Path "NppGit.dll" $plugin -Force
        Copy-Item -Path ".\*" $dll -Include "*.dll" -Exclude "NppGit.*" -Force
        Copy-Item -Path ".\*" $dll -Include "*.exe" -Exclude "NppGit.*" -Force
        #Rename-Item "NLog.config" "NLog.dll.nlog" -Force
        #Copy-Item -Path ".\*" $dll -Include "NLog.dll" -Exclude "*.config" -Force
        #Rename-Item "NLog.dll.nlog" "NLog.config" -Force
	    $exclude = @('*.pdb')
	    $src = ".\NativeBinaries"
        $src_i = Get-Item $src
        $dll = Join-Path $dll NativeBinaries
        #Copy-Item -Path ".\NativeBinaries" $dll -Exclude ".\*\*\git2-e0902fb.pdb" -Force -Recurse
	    Get-ChildItem $src -Recurse -Exclude $exclude | Copy-Item -Destination {Join-Path $dll $_.FullName.Substring($src_i.FullName.Length)}
	}
	
	if ($deploy -eq 1) {
		Compress-Archive -DestinationPath $zip -Path $plugin -Force
	
		Copy-Item -Path $zip $cloud_folder
		Copy-Item $plugin $release -Force -Recurse
		Copy-Item -Path $release $cloud_folder -Force -Recurse
		
		if ($clearAfterDeploy -eq 1) {
			Remove-Item $release -Force -Recurse
			Remove-Item $plugin -Force -Recurse
			Remove-Item $zip -Force
		}
	}
}

cd ..\..\