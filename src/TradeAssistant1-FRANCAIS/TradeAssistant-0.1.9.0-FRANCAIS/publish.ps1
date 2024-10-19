if (-not (Test-Path env:ECO_MODS_DIR)) { 
  Write-Output "ECO_MODS_DIR env variable not set!"
  return
}
if (-not ((Get-Item $env:ECO_MODS_DIR) -is [System.IO.DirectoryInfo])) {
  Write-Output "ECO_MODS_DIR ($env:ECO_MODS_DIR) is not a directory"
  return
}

dotnet publish -c Release -o ./temp -v q
Move-Item -Path ./temp/TradeAssistant.dll -Destination $env:ECO_MODS_DIR -Force
Remove-Item -Path ./temp -Recurse
Write-Output "Moved latest build of TradeAssistant.dll to $env:ECO_MODS_DIR"