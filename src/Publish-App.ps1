# -- Script Parameters

param (
    [string] $configuration = "Release",
    [string] $environment = "Production",
    [string] $framework = "netcoreapp2.2",
    [string] $runtime = "win-x64",
    [switch] $selfContained = $false,
    [switch] $cleanArtifactsBeforeBuild = $false,
    [switch] $showArtifactsAfterBuild = $false,
    [switch] $verbose = $false,
    [string] $outputFolder,
    [string[]] $excludeProject
)

# -- Globals

$configurations = @("Debug", "Release")
$environments = @("Development", "DevelopmentAuth", "Staging", "StagingQualibit", "Production")
$frameworks = @("netcoreapp2.1", "netcoreapp2.2", "netcoreapp3.1")
$runtimes = @("win-x64", "win7-x64", "win81-x64", "win10-x64", "linux-x64", "rhel-x64", "rhel.6-x64")
$projects = @("Api", "IdentityServer", "Web")
$nsghsm = "NSGHSM";
$csprojPrefix = "DA.WI.$($nsghsm)."

# $workingDir = Convert-Path .

# -- Check user parameters

if ($configurations -NotContains $configuration) {
    throw "invalid configuration: '$($configuration)' (valid: $($configurations -join ', '))"
}

if ($environments -NotContains $environment) {
    throw "invalid environment: '$($environment)' (valid: $($environments -join ', '))"
}

if ($frameworks -NotContains $framework) {
    throw "invalid framework: '$($framework)' (valid: $($frameworks -join ', '))"
}

if ($runtimes -NotContains $runtime) {
    throw "invalid runtime: '$($runtime)' (valid: $($runtimes -join ', '))"
}

if (-not($outputFolder)) {
    $outputFolder = "../artifacts/$($nsghsm)"
}

# ---
function Write-Params {
    Write-Host "--- SCRIPT PARAMETERS  -----------------------------------------"
    Write-Host "            configuration: " $configuration
    Write-Host "              environment: " $environment
    Write-Host "                framework: " $framework
    Write-Host "                  runtime: " $runtime
    Write-Host "            selfContained: " $selfContained
    Write-Host "cleanArtifactsBeforeBuild: " $cleanArtifactsBeforeBuild
    Write-Host "  showArtifactsAfterBuild: " $showArtifactsAfterBuild
    Write-Host "             outputFolder: " $outputFolder
    Write-Host "           excludeProject: " $excludeProject
    Write-Host "----------------------------------------------------------------"
}

function Resolve-NonExistentPath {
    <#
    .SYNOPSIS
        Calls Resolve-Path but works for files that don't exist.
    .REMARKS
        From http://devhawk.net/blog/2010/1/22/fixing-powershells-busted-resolve-path-cmdlet
    #>
    param (
    	[Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)]
	    [string]
        $FileName
    )

    $FileName = Resolve-Path $FileName -ErrorAction SilentlyContinue `
                                       -ErrorVariable _frperror
    if (-not($FileName)) {
        $FileName = $_frperror[0].TargetObject
    }

    return $FileName
}

function Remove-Artifacts {
    if (Test-Path -Path "$($outputFolder)") {
        $artifactsDirectory = Resolve-Path $outputFolder
        Write-Host "Removing " -ForegroundColor yellow -NoNewline
        Write-Host $artifactsDirectory -ForegroundColor Magenta -NoNewline
        Write-Host " ... " -ForegroundColor yellow -NoNewline
        Remove-Item -Recurse -Path $artifactsDirectory
        Write-Host "DONE" -ForegroundColor yellow
    }
}

function Write-ExcludedProject {
    param
    (
    	[Parameter(Mandatory = $true, Position = 0)]
	    [string]
        $projectName
    )

    $projectFullName = $csProjPrefix + $projectName

    Write-Host "Project " -ForegroundColor green -NoNewline
    Write-Host $projectFullName -ForegroundColor Magenta -NoNewline
    Write-Host " skipped" -ForegroundColor green
}

function Write-ProjectTitle {
    param
    (
    	[Parameter(Mandatory = $true, Position = 0)]
	    [string]
        $projectFullName,

    	[Parameter(Mandatory = $true, Position = 1)]
        [string]
        $configuration,

        [Parameter(Mandatory = $true, position = 2)]
        [string]
        $environment
    )

    Write-Host "Publishing project " -ForegroundColor green -NoNewline
    Write-Host $projectFullName -ForegroundColor Magenta -NoNewline
    Write-Host " with " -ForegroundColor green -NoNewline
    Write-Host $configuration -ForegroundColor Magenta -NoNewline
    Write-Host " configuration and " -ForegroundColor green -NoNewline
    Write-Host $environment -ForegroundColor Magenta -NoNewline
    Write-Host " environment" -ForegroundColor green
}

function Write-ProjectTrailer {
    param
    (
    	[Parameter(Mandatory = $true, Position = 0)]
	    [string]
        $projectFullName
    )

    Write-Host '--- ' -ForegroundColor green -NoNewline
    Write-Host $projectFullName -ForegroundColor Magenta -NoNewline
    Write-Host "  DONE" -ForegroundColor green
}

function Write-ProjectDetail {
    param
    (
    	[Parameter(Mandatory = $true, Position = 0)]
	    [string]
        $name,

    	[Parameter(Mandatory = $true, Position = 1)]
	    [string]
        $value
    )

    Write-Host "- $($name): " -ForegroundColor yellow -NoNewline
    Write-Host $value -ForegroundColor white
}

function Write-ProjectDetails {
    param
    (
    	[Parameter(Mandatory = $true, Position = 0)]
	    [string]
        $sourceSln,

    	[Parameter(Mandatory = $true, Position = 1)]
	    [string]
        $publishPath,

    	[Parameter(Mandatory = $true, Position = 2)]
        [string]
        $fullCommand
    )

    Write-ProjectDetail "source" $sourceSln
    Write-ProjectDetail "output" $publishPath
    Write-ProjectDetail "command" $fullCommand
}

function Publish-Project {
    param
    (
    	[Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)]
	    [string]
	    $projectName
    )

    $projectFullName = $csProjPrefix + $projectName

    Write-ProjectTitle $projectFullName $configuration $environment

    if (Test-Path -Path $projectFullName)
    {
        $projectFileName = (Join-Path $projectFullName "$($projectFullName).csproj") | Resolve-NonExistentPath
        $outputPath =  (Join-Path $outputFolder $projectName.ToLower()) | Resolve-NonExistentPath
        $dotnetCommandParts = @(
            "dotnet",
            "publish",
            $projectFileName,
            "--output", $outputPath,
            "--configuration", $configuration,
            "--runtime", $runtime,
            "--framework", $framework
        )

        if ($selfContained) {
            $dotnetCommandParts += "--self-contained"
        }

        $dotnetCommandParts += "*>&1"

        $dotnetCommand = $dotnetCommandParts -join ' '

        Write-ProjectDetails $projectFileName $outputPath $dotnetCommand

        Invoke-Expression -Command:$dotnetCommand
    }
    else
    {
        Write-Error "Cannot find project directory: $($projectFullName)"
    }

    Write-ProjectTrailer $projectFullName
}

function Show-Artifacts {
    param (
        [Parameter(Mandatory = $true)]
        [string]
        $outputDirectory
    )

    if (Test-Path -Path $outputDirectory) {

        $directoryToShow = Resolve-Path $outputDirectory

        if ($IsWindows) {
            Start-Process -FilePath "C:\Windows\explorer.exe" -ArgumentList $directoryToShow
        }
        elseif ($IsLinux) {
            Write-Host $directoryToShow
            Write-Error "Show-Artifacts NOT implemented for $($IsLinux)"
        }
        elseif ($IsMacOS) {
            Write-Host $directoryToShow
            Write-Error "Show-Artifacts NOT implemented for $($IsMacOS)"
        }

    } else {
        throw "Output directory $($outputDirectory) does not exist!"
    }
}

# entry point

Clear-Host

if ($verbose) {
    Write-Params
}

if ($cleanArtifactsBeforeBuild) {
    Remove-Artifacts
}

foreach ($projectName in $projects) {
    if ($excludeProject) {
        if ($excludeProject -NotContains $projectName) {
            Publish-Project $projectName
        } else {
            Write-ExcludedProject $projectName
        }
    } else {
        Publish-Project $projectName
    }
}

if ($showArtifactsAfterBuild) {
    Show-Artifacts $outputFolder
}
