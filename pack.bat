@echo off
set version=1.1.0
set token=123
set projects=Logs/Logging/Logging.csproj Logs/Logging.Console/Logging.Console.csproj VivaVictoria.Chaos/VivaVictoria.Chaos.csproj Sql/Sql/Sql.csproj Sql/ClickHouse/ClickHouse.csproj Sql/DapperMigrator/DapperMigrator.csproj Sql/Postgres/Postgres.csproj Sql/SqlServer/SqlServer.csproj Sql/RawSqlReader/RawSqlReader.csproj Sql/ReflectionReader/ReflectionReader.csproj
for %%a in (%projects%) do (
    dotnet pack %%a -o nuget -c Release /p:Version=%version%
)
pause("system")

set names=VivaVictoria.Chaos.Logging VivaVictoria.Chaos.Logging.Console VivaVictoria.Chaos VivaVictoria.Chaos.Sql VivaVictoria.Chaos.ClickHouse VivaVictoria.Chaos.DapperMigrator VivaVictoria.Chaos.Postgres VivaVictoria.Chaos.SqlServer VivaVictoria.Chaos.RawSqlReader VivaVictoria.Chaos.ReflectionSqlReader
for %%a in (%names%) do (
    dotnet nuget push nuget/%%a.%version%.nupkg --api-key %token% --source https://api.nuget.org/v3/index.json
)