param(
	[Parameter(Position=0,Mandatory=0)]
	[string]$target
)
$poshBuildVersion = "4.1.0.229"
$includeHome = "$env:ChocolateyInstall\lib\Nib.Common.PoshBuild.$poshBuildVersion\tools"
$psakeVersion = "4.3.1.0"

Write-Host "target: $target"

[System.Threading.Thread]::CurrentThread.CurrentCulture = '';
[System.Threading.Thread]::CurrentThread.CurrentUICulture = '';

& $env:ChocolateyInstall/chocolateyinstall/chocolatey.ps1 install Nib.Common.PoshBuild -version $poshBuildVersion
Import-Module $env:ChocolateyInstall\lib\psake.$psakeVersion\tools\psake.psm1; 
if (Test-Path "./build/packages.config") {
	& $env:ChocolateyInstall/chocolateyinstall/chocolatey.ps1 install ./build/packages.config
}
if(test-path .\build\includes) {
	rm .\build\includes -recurse | Out-Null	
}
mkdir .\build\includes | Out-Null
cp $includeHome\*.* .\build\includes | Out-Null

$msBuild12Path = Join-Path ${Env:ProgramFiles(x86)} "MSBuild\12.0\Bin"
if(Test-Path $msBuild12Path) {
	Write-Host "Using MS Build 12 / Visual Studio 2013 installed"
	$framework = "4.5.1"
}

$psake.use_exit_on_error = $true; 
Invoke-psake .\build\build.ps1 -framework $framework $target;
if ($psake.build_success -eq $false) { exit 1 } else { exit 0 };