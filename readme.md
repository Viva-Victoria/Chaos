# Chaos
## One of most powerful and flexible version migration tool for .NET and .NET Core

### Supported platforms
- .NET Core 3.1
- .NET 5
- .NET 6

### Install
Chaos is divided into several small NuGet packages:  
| Package | Description | Github | NuGet |
| ------- |:-----------:|:------:|:-----:|
| `VivaVictoria.Chaos` | Base package | [source](https://github.com/Viva-Victoria/Chaos/tree/master) | [nuget](https://www.nuget.org/packages/VivaVictoria.Chaos/) |
| `VivaVictoria.Chaos.ClickHouse` | ClickHouse driver | [source](https://github.com/Viva-Victoria/Chaos/tree/master/Sql/ClickHouse) | [nuget](https://www.nuget.org/packages/VivaVictoria.Chaos.ClickHouse/) |
| `VivaVictoria.Chaos.DapperMigrator` | `IMigrator` implementations by `Dapper` | [source](https://github.com/Viva-Victoria/Chaos/tree/master/Sql/DapperMigrator) | [nuget](https://www.nuget.org/packages/VivaVictoria.Chaos.DapperMigrator/) |
| `VivaVictoria.Chaos.Logging` | Tools for logging SQL scripts | [source](https://github.com/Viva-Victoria/Chaos/tree/master/Sql/Logging) | [nuget](https://www.nuget.org/packages/VivaVictoria.Chaos.SqlLogging/) |
| `VivaVictoria.Chaos.Postgres` | `PostgreSQL` driver by Npgsql | [source](https://github.com/Viva-Victoria/Chaos/tree/master/Sql/Postgres) | [nuget](https://www.nuget.org/packages/VivaVictoria.Chaos.Postgres/) |
| `VivaVictoria.Chaos.RawSqlReader` | Raw sql-files migrations parser | [source](https://github.com/Viva-Victoria/Chaos/tree/master/Sql/RawSqlReader) | [nuget](https://www.nuget.org/packages/VivaVictoria.Chaos.RawSqlReader/) |
| `VivaVictoria.Chaos.ReflectionSqlReader` | Migration-class parser | [source](https://github.com/Viva-Victoria/Chaos/tree/master/Sql/ReflectionSqlReader) | [nuget](https://www.nuget.org/packages/VivaVictoria.Chaos.ReflectionSqlReader/) |
| `VivaVictoria.Chaos.ResxReader` | `.resx` files parser | [source](https://github.com/Viva-Victoria/Chaos/tree/master/Sql/ResxReader) | [nuget](https://www.nuget.org/packages/VivaVictoria.Chaos.ResxReader/) |
| `VivaVictoria.Chaos.Sql` | Base package for SQL-based packages | [source](https://github.com/Viva-Victoria/Chaos/tree/master/Sql/Sql) | [nuget](https://www.nuget.org/packages/VivaVictoria.Chaos.Sql/) |
| `VivaVictoria.Chaos.SqlServer` | `SQL Server` driver | [source](https://github.com/Viva-Victoria/Chaos/tree/master/Sql/SqlServer) | [nuget](https://www.nuget.org/packages/VivaVictoria.Chaos.SqlServer/) |
All packages always have same version and can be upgraded together.

### Sample
You can find several different samples in [`Samples`](https://github.com/Viva-Victoria/Chaos/tree/master/Samples) directory. Simple example projects describes how you can use Chaos.

## Contributing
1. If you creates new project, please follow the following structure:
* Extensions - all static classes with extension methods
* Interfaces - for public project interfaces
* Models - for all models
* Enums - for all enums
* Service implementations and readme.md should be placed in project root
2. Please, throw Exceptions only if you receive fatal error and need to abort migration process.
3. Support readme actuality and create samples for new projects.
