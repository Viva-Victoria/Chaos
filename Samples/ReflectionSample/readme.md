# Example of using VivaVictoria.Chaos.RawSql
### About
This is WebApplication .NET Core 3.1 project with Chaos migrations tool integration.

### Project structure
`Startup.cs` - default netcore app Startup class.  
`Settings.cs` - implementation of ISettings interface for providing to Migrator required project-dependent properties like `ConnectionString` and default `TransactionMode`.  
`Program.cs` - program entrypoint with Chaos setting up.
`Migrations/Migration_001_CreateTableTest.cs` - migration class, provides Up and Down scripts.

### Dependencies
- `Chaos.Logging.Console` (can be replaced with any other `ILogger`)
- `Chaos.PostgreSQL` (can be replaced with any other RDBMS driver)
- `Chaos.Reflection` for parsing migrations from project assembly in runtime
