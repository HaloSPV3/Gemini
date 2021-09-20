# Thanks to:
# torek, StackOverflow community | https://stackoverflow.com/questions/37531605/how-to-test-if-git-repository-is-shallow
# Fabian | https://invoke-thebrain.com/2018/12/comparing-version-numbers-powershell/

## WARNING! This script may fail if Origin is a private repository!
# If credentials are required to fetch from the remote repository,
# git processes spawned by this script may be unable to fetch successfully.

function prebuild
{
    $minVer_isShallow = [version]('2.15.0.0');
    $minVer_unshallow = "2.1.4.0";
    $gitBinVer = "";
    $isShallow = $true;

    # 0. Announce
    Write-Host  "0. GitVersion cannot determine the next version in shallow reposistories.`n",
                "We will determine if the current repository needs to be un-shallowed.`n`n",
                "Note: Git is required to 'unshallow' the repository so GitVersion can work.`n",
                "Checking if Git is available...`n"

    # 1. Ensure Git is available
    try
    {
        Write-Host "1. Git was found.`n",
        "It is $(git --version) at...`n",
        "$($(Get-Command -Name git).path)`n"

        $gitBinVer = $([version]('{2}.{3}.{4}.{6}' -f $(git --version).split(' ').split('.')))
    }
    catch
    {
        Write-Error "Git is not installed or it is not in PATH!"
        Write-Host "`n"
    }

    # 2. Check if the repository is shallow
    Write-Host "2. Checking if repository is shallow...`n"
    if ($gitBinVer -gt $minVer_isShallow) # GitVersion >= 2.15.0.0
    {
        $isShallow = git rev-parse --is-shallow-repository
    }
    else
    {
        Write-Debug "Git Version less than 2.15.0.0"
        $isShallow = Test-Path (Join-Path $GitStatus.GitDir shallow)
    }

    # 3. If the repository is shallow, then unshallow
    if ($isShallow -eq $true)
    {
        Write-Host "3. Repository is shallow. Fetching full history..."
        if ($gitBinVer -lt $minVer_unshallow) # GitVersion < 2.1.4.0 (exact version unknown)
        {
            git fetch --depth=0;
        }
        else {
            git fetch --unshallow
        }
        Write-Host "Fetch Completed. Proceeding to Build...`n"
    }
    else
    {
        Write-Host "3. Repository is complete. Proceeding to Build...`n"
    }
}

$(prebuild)
