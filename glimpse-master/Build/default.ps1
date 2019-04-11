Include ".\helpers.ps1"

properties {
  	$cleanMessage = 'Executed Clean!'
	$testMessage = 'Executed Test!'
  
	$solutionDirectory = (Get-Item $solutionFile).DirectoryName
	$outputDirectory= "$solutionDirectory\.build"
	$temporaryOutputDirectory = "$outputDirectory\temp"

	# Find Test Projects
	$publishedMSTestTestsDirectory = "$temporaryOutputDirectory\_PublishedMSTestTests"
	$publishedNUnitTestsDirectory = "$temporaryOutputDirectory\_PublishedNUnitTests"

	$testResultsDirectory = "$outputDirectory\TestResults"

	#Place Test Results in this directory
	$MSTestTestResultsDirectory = "$testResultsDirectory\MSTest"
	$NUnitTestResultsDirectory = "$testResultsDirectory\NUnit"

	$testCoverageDirectory = "$outputDirectory\TestCoverage"
	$testCoverageReportPath = "$testCoverageDirectory\OpenCover.xml"
	$testCoverageFilter = "+[*]* -[xunit.*]* -[*.NUnitTests]* -[*.Tests]* -[*.xUnitTests]*"
	$testCoverageExcludeByAttribute = "*.ExcludeFromCodeCoverage*"
	#$testCoverageExcludeByFile = "*\*Designer.cs;*\*.g.cs;*\*.g.i.cs"

	$buildConfiguration = "Debug"
	$buildPlatform = "Any CPU"

	$packagesPath = "$solutionDirectory\packages"
	
	#Using vsTest Runner
	$vsTestExe = (Get-ChildItem ("C:\Program Files (x86)\Microsoft Visual Studio*\Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe")).FullName | Sort-Object $_ | select -last 1
	
	#Using Nunit Runner
	$NUnitExe = (Find-PackagePath $packagesPath "NUnit.ConsoleRunner" ) + "\tools\nunit3-console.exe"

	#Find OpenCoverExe
	$openCoverExe = (Find-PackagePath $packagesPath "OpenCover") + "\Tools\OpenCover.Console.exe"

	#Report Generator
	$reportGeneratorExe = (Find-PackagePath $packagesPath "ReportGenerator") + "\Tools\ReportGenerator.exe"
}

FormatTaskName "`r`n`r`n-------- Executing {0} Task --------"

task default -depends Test

task Init `
  -description "Initialises the build by removing previous artifacts and creating output directories" `
  -requiredVariables outputDirectory, temporaryOutputDirectory `
{
	Assert ("Debug", "Release" -contains $buildConfiguration) `
		   "Invalid build configuration '$buildConfiguration'. Valid values are 'Debug' or 'Release'"

	Assert ("x86", "x64", "Any CPU" -contains $buildPlatform) `
		   "Invalid build platform '$buildPlatform'. Valid values are 'x86', 'x64' or 'Any CPU'"

	# Check that all tools are available
	Write-Host "Checking that all required tools are available"

	Assert (Test-Path $vsTestExe) "VSTest Console could not be found"
	Assert (Test-Path $NUnitExe) "NUnit Console could not be found"
	Assert (Test-Path $openCoverExe) "OpenCover Console could not be found"
	Assert (Test-Path $reportGeneratorExe) "ReportGenerator Console could not be found"


	# Remove previous build results
	if (Test-Path $outputDirectory) 
	{
		Write-Host "Removing output directory located at $outputDirectory"
		Remove-Item $outputDirectory -Force -Recurse
	}
	
	Write-Host "Creating output directory located at $outputDirectory"
	New-Item $outputDirectory -ItemType Directory | Out-Null

	Write-Host "Creating temporary directory located at $temporaryOutputDirectory"
	New-Item $temporaryOutputDirectory -ItemType Directory | Out-Null
}

task Compile `
	-depends Init `
	-description "Compile the code" `
	-requiredVariables solutionFile, buildConfiguration, buildPlatform, temporaryOutputDirectory `
{ 
  	Write-Host "Building solution $solutionFile" 
	Exec {
		msbuild $SolutionFile "/p:Configuration=$buildConfiguration;Platform=$buildPlatform;OutDir=$temporaryOutputDirectory"
	}
}

#NUnit Test Task
task TestNUnit `
	-depends Compile `
	-description "Run NUnit tests" `
	-precondition { return Test-Path $publishedNUnitTestsDirectory } `
	-requiredVariable publishedNUnitTestsDirectory, NUnitTestResultsDirectory `
{
	$testAssemblies = Prepare-Tests -testRunnerName "NUnit" `
									-publishedTestsDirectory $publishedNUnitTestsDirectory `
									-testResultsDirectory $NUnitTestResultsDirectory `
									-testCoverageDirectory $testCoverageDirectory

	$targetArgs = "$testAssemblies /xml:`"`"$NUnitTestResultsDirectory\NUnit.xml`"`" /nologo /noshadow"

	# Run OpenCover, which in turn will run NUnit	
	Run-Tests -openCoverExe $openCoverExe `
			  -targetExe $nunitExe `
			  -targetArgs $targetArgs `
			  -coveragePath $testCoverageReportPath `
			  -filter $testCoverageFilter `
			  -excludebyattribute:$testCoverageExcludeByAttribute
			  #-excludebyfile: $testCoverageExcludeByFile
}


#MSBuild Test Task depends on the compile task and will run the MSTest tests
task TestMSTest `
	-depends Compile `
	-description "Run MSTest tests" `
	-precondition { return Test-Path $publishedMSTestTestsDirectory } `
	-requiredVariable publishedMSTestTestsDirectory, MSTestTestResultsDirectory `
{
	$testAssemblies = Prepare-Tests -testRunnerName "MSTest" `
									-publishedTestsDirectory $publishedMSTestTestsDirectory `
									-testResultsDirectory $MSTestTestResultsDirectory `
									-testCoverageDirectory $testCoverageDirectory

	$targetArgs = "$testAssemblies /Logger:trx"

	# vstest console doesn't have any option to change the output directory
	# so we need to change the working directory
	Push-Location $MSTestTestResultsDirectory
	
	# Run OpenCover, which in turn will run VSTest	
	Run-Tests -openCoverExe $openCoverExe `
			  -targetExe $vsTestExe `
			  -targetArgs $targetArgs `
			  -coveragePath $testCoverageReportPath `
			  -filter $testCoverageFilter `
			  -excludebyattribute:$testCoverageExcludeByAttribute
			  #-excludebyfile: $testCoverageExcludeByFile

	Pop-Location

	# move the .trx file back to $MSTestTestResultsDirectory
	Move-Item -path $MSTestTestResultsDirectory\TestResults\*.trx -destination $MSTestTestResultsDirectory\MSTest.trx

	Remove-Item $MSTestTestResultsDirectory\TestResults
}

task Test `
	-depends Compile, TestNUnit, TestMSTest `
	-description "Run unit tests and test coverage" `
	-requiredVariables testCoverageDirectory, testCoverageReportPath `
{
	# parse OpenCover results to extract summary
	if (Test-Path $testCoverageReportPath)
	{
		Write-Host "Parsing OpenCover results"

		# Load the coverage report as XML
		$coverage = [xml](Get-Content -Path $testCoverageReportPath)

		$coverageSummary = $coverage.CoverageSession.Summary

		# Write class coverage
		Write-Host "##teamcity[buildStatisticValue key='CodeCoverageAbsCCovered' value='$($coverageSummary.visitedClasses)']"
		Write-Host "##teamcity[buildStatisticValue key='CodeCoverageAbsCTotal' value='$($coverageSummary.numClasses)']"
		Write-Host ("##teamcity[buildStatisticValue key='CodeCoverageC' value='{0:N2}']" -f (($coverageSummary.visitedClasses / $coverageSummary.numClasses)*100))

		# Report method coverage
		Write-Host "##teamcity[buildStatisticValue key='CodeCoverageAbsMCovered' value='$($coverageSummary.visitedMethods)']"
		Write-Host "##teamcity[buildStatisticValue key='CodeCoverageAbsMTotal' value='$($coverageSummary.numMethods)']"
		Write-Host ("##teamcity[buildStatisticValue key='CodeCoverageM' value='{0:N2}']" -f (($coverageSummary.visitedMethods / $coverageSummary.numMethods)*100))
		
		# Report branch coverage
		Write-Host "##teamcity[buildStatisticValue key='CodeCoverageAbsBCovered' value='$($coverageSummary.visitedBranchPoints)']"
		Write-Host "##teamcity[buildStatisticValue key='CodeCoverageAbsBTotal' value='$($coverageSummary.numBranchPoints)']"
		Write-Host "##teamcity[buildStatisticValue key='CodeCoverageB' value='$($coverageSummary.branchCoverage)']"

		# Report statement coverage
		Write-Host "##teamcity[buildStatisticValue key='CodeCoverageAbsSCovered' value='$($coverageSummary.visitedSequencePoints)']"
		Write-Host "##teamcity[buildStatisticValue key='CodeCoverageAbsSTotal' value='$($coverageSummary.numSequencePoints)']"
		Write-Host "##teamcity[buildStatisticValue key='CodeCoverageS' value='$($coverageSummary.sequenceCoverage)']"

		# Generate HTML test coverage report
		Write-Host "`r`nGenerating HTML test coverage report"
		Exec { &$reportGeneratorExe $testCoverageReportPath $testCoverageDirectory }
	}
	else
	{
		Write-Host "No coverage file found at: $testCoverageReportPath"
	}
}


task Clean -description "Remove temporary files" 
{ 
  	Write-Host $cleanMessage
}