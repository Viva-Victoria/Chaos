# VivaVictoria.Chaos

### Structure 
Chaos is complex database-independent migration tool. Independence from the DBMS and the connection method is 
achieved due to the following mechanism:  
base tool class is `Chaos`, it manages process of migration and requires next components:
* `IMigrationReader` for reading list of migrations stored in `IMigration` model,
* `IMigrator` for receiving / saving metadata and applying migration scripts.

In turn, some `IMigrator` or `IMigrationReader` implementations can be dependent on `ISettings` instance. `ISettings` provides 
`ConnectionString` and default `MigrationMode`.  
`MigrationMode` is enum describing the migration mode:
* `Default` - default mode from `ISettings`
* `One` - run each migration and save metadata in one migration,
* `None` - do not use migrations.  
**Note:** `MigrationMode` is just recommendation and will be used only if `IMigrator` and `RDBMS` supports specified mode.

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
