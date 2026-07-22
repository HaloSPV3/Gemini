#!/usr/bin/env pwsh
$ErrorActionPreference = 'Stop'

# -------- Configuration --------
$Channel = '11.0'
$Quality = 'GA'
# Uncomment the workloads your project needs:
# $Workloads = @('maui', 'wasm-tools')
# --------------------------------

$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Definition
$installDir = Join-Path $scriptDir '.dotnet'
$DOTNET_INSTALL_DIR = "$env:LocalAppData\Microsoft\dotnet"
$isUnixLike = $PSVersionTable.Platform -eq 'Unix';
if ($isUnixLike) { $DOTNET_INSTALL_DIR = "$env:HOME/.dotnet" }

[System.Environment]::SetEnvironmentVariable('DOTNET_INSTALL_DIR', "$DOTNET_INSTALL_DIR", [System.EnvironmentVariableTarget]::User);

if (-not (Test-Path "$DOTNET_INSTALL_DIR")) {
  New-Item -ItemType Directory "$DOTNET_INSTALL_DIR"
  $ItemType = if ($PSVersionTable.Platform -eq 'Unix')
  { $ItemType = SymbolicLink }
  else { $ItemType = Junction }

  New-Item -ItemType $ItemType -Path "$installDir" -Target "$DOTNET_INSTALL_DIR"
}

Write-Host "Installing .NET $Channel ($Quality) SDK to $installDir ..."

$installerPath = Join-Path $scriptDir 'dotnet-install.ps1'
Invoke-WebRequest 'https://dot.net/v1/dotnet-install.ps1' -OutFile $installerPath
& $installerPath -Channel $Channel -Quality $Quality -InstallDir $installDir
Remove-Item $installerPath -ErrorAction SilentlyContinue

# Auto-detect the installed SDK version
$sdkVersion = & (Join-Path $installDir 'dotnet.exe') --version

# Create global.json with the installed version
@"
{
  "sdk": {
    "version": "$sdkVersion",
    "allowPrerelease": false,
    "rollForward": "latestFeature",
    "paths": [".dotnet", "`$host`$"],
    "errorMessage": "Required .NET SDK not found. Run ./install-dotnet.sh (macOS/Linux) or .\\install-dotnet.ps1 (Windows) to install it locally."
  }
}
"@ | Set-Content -Path (Join-Path $scriptDir 'global.json') -Encoding UTF8

# Ensure .dotnet is in .gitignore
$gitignorePath = Join-Path $scriptDir '.gitignore'
if (!(Test-Path $gitignorePath) -or !(Select-String -Path $gitignorePath -Pattern '^\s*\.dotnet\s*$' -Quiet)) {
  Add-Content -Path $gitignorePath -Value '.dotnet'
}

# Install workloads if configured
if ($Workloads) {
  Write-Host "Installing workloads: $($Workloads -join ', ')"
  & (Join-Path $installDir 'dotnet.exe') workload install @Workloads
}

Write-Host ''
Write-Host "Done! SDK $sdkVersion installed to $installDir"
Write-Host "Run 'dotnet --version' to verify."