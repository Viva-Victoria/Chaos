# VivaVictoria.Chaos
### Universal version migration tool for .NET

### Status
version 1.1.1 - _**unstable**_

### Supported platforms:
- .NET Core 3.1
- .NET 5+

### Structure 
Chaos is simple database-independent migration tool. Independence from the DBMS and the connection method is 
achieved due to the following mechanism:  
base tool class is `Chaos`, it manages process of migration and requires next components:
* `ILogger` for logging actions and SQL statements,
* `IMigrator` for receiving / saving metadata and applying migration scripts,
* `IMigrationReader` for reading list of migrations stored in `MigrationInfo` model.

In turn, some `IMigrator` or `IMigrationReader` implementations can be dependent on `ISettings` instance. `ISettings` provides 
`ConnectionString` and default `MigrationMode`.  
`MigrationMode` is enum describing the migration mode:
* `Default` - default mode from `ISettings`
* `One` - run each migration and save metadata in one migration,
* `None` - do not use migrations.  
**Note:** `MigrationMode` is just recommendation and will be used only if `IMigrator` and `DBMS` supports specified mode.

### Sample
Simple example with PostgreSQL, ConsoleLogger and Reflection reader:
```c#
var services = new ServiceCollection()

//register settings
services.AddTransient<ISettings, Settings>();
//register PostgreSQL or other RDBMS driver
services.RegisterChaosPostgres();
            
//add Reflection support with project Assembly
services.RegisterChaosReflection(typeof(Program).Assembly);
```
Now you can request IChaos service and run migrations.
```c#
//build chaos via DI container            
using (var scope = services.BuildServiceProvider())
{
    var chaos = scope.GetChaos();
    chaos.Init().Migrate();
}
```

### Data Race on replicated services
##### Problem
Let's imagine a situation in which our service is implemented in a Docker container with replication.  
In most cases, one of the replicas will start before the others and perform migrations. However, 
there may be cases when replicas are started simultaneously, or the migration process is too long and 
several replicas have time to start the migration. In this case, all replicas will receive an error 
due to the race condition.

##### Usual solution
Usual solution is running migration as a part of CI/CD pipeline - your CI/CD tool runs migration and then, 
if database migrated successfully, containers will be updated. 

##### Chaos solution
Chaos provides you possibility to skip migrations if some condition is not met:
```c#        
using (var scope = services.BuildServiceProvider())
{
    var chaos = scope.GetChaos();
    chaos
        .Init(() => !string.IsNullOrEmpty(System.Environment.GetEnvironmentVariable("REPLICA_MASTER")))
        .Migrate();
}
```
Now, define environment variable REPLICA_MASTER only on your master replica container. All other replicas will be skipped.  
**Note:** it is not ideal solution, because some of your replicas can be available before database will be migrated and
some requests to database may be failed. But if you use blue-green deploy with 'usual' solution you can receive same errors
when blue container is still exists and green container runs migrations. Blue container can fail on database requests. 

### Total nuget packages list
`VivaVictoria.Chaos`  
`VivaVictoria.Chaos.ClickHouse`  
`VivaVictoria.Chaos.DapperMigrator`  
`VivaVictoria.Chaos.Logging`  
`VivaVictoria.Chaos.Postgres`  
`VivaVictoria.Chaos.RawSqlReader`  
`VivaVictoria.Chaos.ReflectionSqlReader`  
`VivaVictoria.Chaos.ResxReader`
`VivaVictoria.Chaos.Sql`  
`VivaVictoria.Chaos.SqlServer`  

## Contributing
1. If you creates new project, please follow the following structure:
* Extensions - all static classes with extension methods
* Interfaces - for public project interfaces
* Models - for all models
* Enums - for all enums
* Service implementations and readme.md should be placed in project root
2. Please, throw Exceptions only if you receive fatal error and need to abort migration process.
3. Support readme actuality and create samples for new projects.
