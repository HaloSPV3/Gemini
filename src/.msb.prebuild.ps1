# Thanks to:
# torek, StackOverflow community | https://stackoverflow.com/questions/37531605/how-to-test-if-git-repository-is-shallow
# Fabian | https://invoke-thebrain.com/2018/12/comparing-version-numbers-powershell/

## WARNING! This script may fail if Origin is a private repository!
# If credentials are required to fetch from the remote repository,
# git processes spawned by this script may be unable to fetch successfully.

function prebuild {
  $isShallow = $false;

  [string]$gitDir = [System.IO.Path]::GetFullPath("$PSScriptRoot/.git");
  [string]$rootDir = Join-Path "$([System.IO.Path]::GetPathRoot($gitDir))" '.git';
  while ("$gitDir" -ne $rootDir) {
    [string]$shallow = "$gitDir/shallow";
    $isShallow = (Test-Path $shallow) -and ([System.IO.File]::Exists($shallow));
    if ($isShallow) { break; }
    $gitDir = [System.IO.Path]::GetFullPath("$gitDir/../../.git");
    Write-Debug $gitDir
  }

  # If the repository is shallow, then unshallow
  if ($isShallow -eq $true) {
    Write-Warning 'Repository is shallow. Fetching full history...'
    git fetch --unshallow
    Write-Verbose 'Repository un-shallowed.'
  }
}

$(prebuild $args)
