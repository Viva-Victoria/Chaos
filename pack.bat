@echo off
set version=1.0.1-preview
set token=
dotnet pack VivaVictoria.Chaos.Logging/VivaVictoria.Chaos.Logging.csproj -o nuget -c Release /p:Version=%version%
dotnet pack VivaVictoria.Chaos.Logging.Console/VivaVictoria.Chaos.Logging.Console.csproj -o nuget -c Release /p:Version=%version%
dotnet pack VivaVictoria.Chaos/VivaVictoria.Chaos.csproj -o nuget -c Release /p:Version=%version%
dotnet pack VivaVictoria.Chaos.ClickHouse/VivaVictoria.Chaos.ClickHouse.csproj -o nuget -c Release /p:Version=%version%
dotnet pack VivaVictoria.Chaos.Dapper/VivaVictoria.Chaos.Dapper.csproj -o nuget -c Release /p:Version=%version%
dotnet pack VivaVictoria.Chaos.PostgreSQL/VivaVictoria.Chaos.PostgreSQL.csproj -o nuget -c Release /p:Version=%version%
dotnet pack VivaVictoria.Chaos.SqlServer/VivaVictoria.Chaos.SqlServer.csproj -o nuget -c Release /p:Version=%version%
dotnet pack VivaVictoria.Chaos.RawSql/VivaVictoria.Chaos.RawSql.csproj -o nuget -c Release /p:Version=%version%
dotnet pack VivaVictoria.Chaos.Reflection/VivaVictoria.Chaos.Reflection.csproj -o nuget -c Release /p:Version=%version%
pause("system")
dotnet nuget push nuget/VivaVictoria.Chaos.Logging.%version%.nupkg --api-key %token% --source https://api.nuget.org/v3/index.json
dotnet nuget push nuget/VivaVictoria.Chaos.Logging.Console.%version%.nupkg --api-key %token% --source https://api.nuget.org/v3/index.json
dotnet nuget push nuget/VivaVictoria.Chaos.%version%.nupkg --api-key %token% --source https://api.nuget.org/v3/index.json
dotnet nuget push nuget/VivaVictoria.Chaos.ClickHouse.%version%.nupkg --api-key %token% --source https://api.nuget.org/v3/index.json
dotnet nuget push nuget/VivaVictoria.Chaos.Dapper.%version%.nupkg --api-key %token% --source https://api.nuget.org/v3/index.json
dotnet nuget push nuget/VivaVictoria.Chaos.PostgreSQL.%version%.nupkg --api-key %token% --source https://api.nuget.org/v3/index.json
dotnet nuget push nuget/VivaVictoria.Chaos.SqlServer.%version%.nupkg --api-key %token% --source https://api.nuget.org/v3/index.json
dotnet nuget push nuget/VivaVictoria.Chaos.RawSql.%version%.nupkg --api-key %token% --source https://api.nuget.org/v3/index.json
dotnet nuget push nuget/VivaVictoria.Chaos.Reflection.%version%.nupkg --api-key %token% --source https://api.nuget.org/v3/index.json
