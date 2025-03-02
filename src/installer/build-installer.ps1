# build-installer.ps1
# Run from repository root directory

$WiXPath = "C:\Program Files (x86)\WiX Toolset v3.11\bin"

Write-Host "Building Installer..."

& "$WiXPath\candle.exe" ".\installer\installer.wxs" -o ".\installer\installer.wixobj"
if ($LASTEXITCODE -ne 0) {
    Write-Error "Error running candle.exe"
    exit $LASTEXITCODE
}

& "$WiXPath\light.exe" ".\installer\installer.wixobj" -o ".\installer\MaliciousAIAgentInstaller.msi"
if ($LASTEXITCODE -ne 0) {
    Write-Error "Error running light.exe"
    exit $LASTEXITCODE
}

Write-Host "Installer successfully built at installer\MaliciousAIAgentInstaller.msi"