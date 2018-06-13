[CmdletBinding()]
Param(
    #[switch]$CustomParam,
    [Parameter(Position=0,Mandatory=$false,ValueFromRemainingArguments=$true)]
    [string[]]$BuildArguments
)

Write-Output "Windows PowerShell $($Host.Version)"

Set-StrictMode -Version 2.0; $ErrorActionPreference = "Stop"; $ConfirmPreference = "None"; trap { $host.SetShouldExit(1) }
$PSScriptRoot = Split-Path $MyInvocation.MyCommand.Path -Parent

###########################################################################
# CONFIGURATION
###########################################################################

$SolutionDirectory = "$PSScriptRoot\."
$BuildProjectFile = "$PSScriptRoot\.\build\.build.csproj"
$BuildExeFile = "$PSScriptRoot\.\build\bin\debug\.build.exe"
$TempDirectory = "$PSScriptRoot\.\.tmp"

$NuGetVersion = "latest"
$NuGetUrl = "https://dist.nuget.org/win-x86-commandline/$NuGetVersion/nuget.exe"

###########################################################################
# EXECUTION
###########################################################################

function ExecSafe([scriptblock] $cmd) {
    & $cmd
    if ($LASTEXITCODE) { exit $LASTEXITCODE }
}

$env:NUGET_EXE = "$TempDirectory\nuget.exe"
if (!(Test-Path $env:NUGET_EXE)) {
    md -force $TempDirectory > $null
    (New-Object System.Net.WebClient).DownloadFile($NuGetUrl, $env:NUGET_EXE)
}
elseif ($NuGetVersion -eq "latest") {
    ExecSafe { & $env:NUGET_EXE update -Self }
}
Write-Output $(& $env:NUGET_EXE help | select -First 1)

ExecSafe { & $env:NUGET_EXE install Nuke.MSBuildLocator -ExcludeVersion -OutputDirectory $TempDirectory -SolutionDirectory $SolutionDirectory }
$MSBuildFile = & "$TempDirectory\Nuke.MSBuildLocator\tools\Nuke.MSBuildLocator.exe"

ExecSafe { & $env:NUGET_EXE restore $BuildProjectFile -SolutionDirectory $SolutionDirectory }
ExecSafe { & $MSBuildFile $BuildProjectFile }
ExecSafe { & $BuildExeFile $BuildArguments }
