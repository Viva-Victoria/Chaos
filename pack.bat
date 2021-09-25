@echo off
set version=1.0.0-preview
dotnet pack VivaVictoria.Chaos.Logging/VivaVictoria.Chaos.Logging.csproj -o nuget -c Release /p:Version=%version%
dotnet pack VivaVictoria.Chaos.Logging.Console/VivaVictoria.Chaos.Logging.Console.csproj -o nuget -c Release /p:Version=%version%
dotnet pack VivaVictoria.Chaos/VivaVictoria.Chaos.csproj -o nuget -c Release /p:Version=%version%
dotnet pack VivaVictoria.Chaos.ClickHouse/VivaVictoria.Chaos.ClickHouse.csproj -o nuget -c Release /p:Version=%version%
dotnet pack VivaVictoria.Chaos.Dapper/VivaVictoria.Chaos.Dapper.csproj -o nuget -c Release /p:Version=%version%
dotnet pack VivaVictoria.Chaos.PostgreSQL/VivaVictoria.Chaos.PostgreSQL.csproj -o nuget -c Release /p:Version=%version%
dotnet pack VivaVictoria.Chaos.SqlServer/VivaVictoria.Chaos.SqlServer.csproj -o nuget -c Release /p:Version=%version%
dotnet pack VivaVictoria.Chaos.RawSql/VivaVictoria.Chaos.RawSql.csproj -o nuget -c Release /p:Version=%version%
dotnet pack VivaVictoria.Chaos.Reflection/VivaVictoria.Chaos.Reflection.csproj -o nuget -c Release /p:Version=%version%