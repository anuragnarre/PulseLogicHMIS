$RepoRoot = Split-Path -Parent $MyInvocation.MyCommand.Path
$DotNetPath = Join-Path $RepoRoot ".dotnet"
if (Test-Path $DotNetPath) {
    $env:PATH = "$DotNetPath;" + $env:PATH
}
$WebProject = Join-Path $RepoRoot "Code\Websites\PulseLogicHMR"
Set-Location $WebProject
dotnet run
