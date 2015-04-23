Include settings.ps1
Include includes\commonfunctions.ps1
Include includes\assemblyinfo.ps1
Include includes\devdeploy.ps1
Include includes\octopus.ps1
Include includes\nuget.ps1

properties {
  # Do not put any code in here. This method is invoked before all others
  # and will not have access to any of your shared properties.
}

Task Clean {
  Exec { msbuild "$($solution.file)" /t:Clean /p:Configuration=Release /verbosity:quiet /nologo }
}

Task Build-Solution -depends Clean, Version-AssemblyInfo {
  Exec { msbuild "$($solution.file)" /t:Build /p:Configuration=Release /verbosity:quiet /nologo }
}

Task Prepare -depends Clean

Task Build -depends Bootstrap-NuGetPackages, Build-Solution

Task Install -depends Build, Run-DevPreDeploy, Run-DevPostDeploy

Task Ci -depends Prepare, Bootstrap-NuGetPackages, Build-Solution Create-NuGetPackage, Publish-NugetPackage, Create-OctopusRelease

Task default -depends Prepare, Build