# VivaVictoria.Chaos
### Universal version migration tool for .NET

### Status
Chaos is very simple library, but now **_is not stable_**.

### Supported platforms:
- .NET Core 3.1
- .NET 5+

### Install
Visit nuget.org page for more information about available packages.

### Usage with dependency injection
Simple example with PostgreSQL, ConsoleLogger and Reflection reader:
```c#
var services = new ServiceCollection()

//register settings
services.AddTransient<ISettings, Settings>();
//register PostgreSQL or other RDBMS driver
services.RegisterChaosPostgres();
//register ConsoleLogger or other ILogger
services.RegisterChaosConsoleLogger();
            
//add Reflection support with project Assembly
services.RegisterChaosReflection(typeof(Program).Assembly);

//build chaos via DI container            
var chaos = new ChaosBuilder()
    .Resolve(services.BuildServiceProvider())
    .Build();
```

### Usage with manual building
```c#
var chaos = new ChaosBuilder()
    .Use(new Settings())
    .Use(new PostgresMetadata())
    .Use(new DapperMigrator(new PostgresProvider()))
    .Use(new ConsoleLogger())
    .Use(new ReflectMigrationReader(typeof(Program).Assembly))
    .Build();
```

### Recommendations
Chaos supports manual building, but we recommends to use dependency injection. 
All Chaos subprojects provides `DependencyInjection` class with extension for IServiceCollection. Those extensions hides all project's required types.  
For example, `RegisterChaosPostgres` hides from user next call stack:
```c#
AddTransient<IMetadata, PostgresMetadata>()
AddTransient<IConnectionProvider, PostgresProvider>()
AddTransient<IMigrator, DapperMigrator>();
```
Yes, it 

### Architecture [for contributors]
Base project `VivaVictoria.Chaos` provides `Chaos` migration tool and `ChaosBuilder` class for creating `Chaos` instance via Builder template.
Also this project describes abstractions of metadata table, migration reader and migrator settings.  
All implementations and logging placed in different projects:
- `Logging` - logging abstractions and IDbConnection wrappers for logging SQL queries;
- `Logging.Console` - ConsoleLogger for logging data into System.Console;
- `Dapper` - Migrator impl based on Dapper library, used for drivers;
- `ClickHouse` - driver for ClickHouse, uses ClickHouse.Ado;
- `PostgreSQL` - driver for PostgreSQL, uses Npgsql;
- `SqlServer` - driver for MS Sql Server, uses SqlClient;
- `RawSql` - MigrationReader impl for reading migrations from SQL-scripts, placed in project;
- `Reflection` - MigrationReader impl for reading migrations from runtime via Reflect API.