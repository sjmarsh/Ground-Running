properties { 

  $framework = '4.0'
  
  $base = @{}
  $base.dir = Resolve-Path ..	 
	
  $source = @{}
  $source.dir = "$($base.dir)\src"

  $build = @{}
  $build.dir = "$($base.dir)\build"
  
  $solution = @{}
  $solution.name = "<<SolutionName>>"	
  $solution.file = "$($source.dir)\$($solution.name).sln"  
  
  $version = @{}
  $version.major = 1
  $version.minor = 0
  $version.patch = 0		
  $version.build_number = if($env:BUILD_NUMBER) { $env:BUILD_NUMBER } else { "0" }		
  $version.branch_name = if($env:BRANCH_NAME) { $env:BRANCH_NAME } else { "" }		
  $version.full = "$($version.major).$($version.minor).$($version.patch).$($version.build_number)"
  if (($version.branch_name -gt 0) -and ("master" -ne $version.branch_name)) {
      $version.full = "$($version.full)-$($version.branch_name)"
  }
  
  $release = @{}
  $release.dir = "$($source.dir)\<<SolutionName>>\bin"
  
  $octopus = @{}
  $octopus.createReleaseForProject = "<<OctopusProjectName>>"
}