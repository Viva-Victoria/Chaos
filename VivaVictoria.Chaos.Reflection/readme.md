# VivaVictoria.Chaos.Reflection

### Migration class
```c#
namespace Migrations
{
    [Migration(Version = 1, Name = "create table test", TransactionMode = TransactionMode.One)]
    public class Migration_001_CreateTableTest : IMigration {
        public string Up()
        {
            return "create table test(id int primary key)";
        }

        public string Down()
        {
            return "drop table test";
        }
    }
}
```

### Migration Attribute
Next code marks class as Migration class:
```c#
[Migration(Version = 1, Name = "create table test", TransactionMode = TransactionMode.One)]
```
Version and Name fields are required, TransactionMode is optional. If TransactionMode does not set or set to Default - Migrator will uses TransactionMode from ISettings.

### Usage with DI
Just use extension method:
```c#
var services = new ServiceCollection();
services.AddChaosReflection("Migrations/");
```
or add components manually:
```c#
collection.AddTransient<IMigrationReader, ReflectMigrationReader>(p => 
    new ReflectMigrationReader(typeof(Program).Assembly));
```

### Usage with Builder
```c#
var chaos = new ChaosBuilder()
    .Use(new ReflectMigrationReader(typeof(Program).Assembly))
    .Build();
```