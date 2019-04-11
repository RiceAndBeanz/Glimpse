﻿function Find-PackagePath


function Prepare-Tests
{
	[CmdletBinding()]

	$projects = Get-ChildItem $publishedTestsDirectory

	if ($projects.Count -eq 1) 
	{
		Write-Host "1 $testRunnerName project has been found:"
	}
	else 
	{
		Write-Host $projects.Count " $testRunnerName projects have been found:"
	}
	
	Write-Host ($projects | Select $_.Name )

	# Create the test results directory if needed

	# Create the test coverage directory if needed
	if (!(Test-Path $testCoverageDirectory))
	{
		Write-Host "Creating test coverage directory located at $testCoverageDirectory"
		mkdir $testCoverageDirectory | Out-Null
	}

	# Get the list of test DLLs

	$testAssemblies = [string]::Join(" ", $testAssembliesPaths)

	return $testAssemblies
}

function Run-Tests
{
	[CmdletBinding()]
	param(
		[Parameter(Position=0,Mandatory=1)]$openCoverExe,
		[Parameter(Position=1,Mandatory=1)]$targetExe,
		[Parameter(Position=2,Mandatory=1)]$targetArgs,
		[Parameter(Position=3,Mandatory=1)]$coveragePath,
		[Parameter(Position=4,Mandatory=1)]$filter,
		[Parameter(Position=5,Mandatory=1)]$excludeByAttribute
		#[Parameter(Position=6,Mandatory=1)]$excludeByFile
	)

	Write-Host "Running tests"

	Exec { &$openCoverExe -target:$targetExe `
						  -targetargs:$targetArgs `
						  -output:$coveragePath `
						  -register:user `
						  -filter:$filter `
						  -excludebyattribute:$excludeByAttribute `
						  -excludebyfile `
						  -skipautoprops `
						  -mergebyhash `
						  -mergeoutput `
						  -hideskipped:All `
						  -returntargetcode}
}