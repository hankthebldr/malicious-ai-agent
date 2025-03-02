# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:6.0-windowsservercore-ltsc2022 AS build

WORKDIR /app
COPY . ./
RUN dotnet restore
RUN dotnet publish WindowsAgent.csproj -c Release -o out

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/runtime:6.0-windowsservercore-ltsc2022
WORKDIR /app
COPY --from=build /app/out ./
ENTRYPOINT ["WindowsAgent.exe"]