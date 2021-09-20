$originalPath = $(Get-Location).Path

Set-Location $PSScriptRoot\..\bin
Get-ChildItem -Path "*.exe.config" -Recurse | Remove-Item

Set-Location $originalPath
