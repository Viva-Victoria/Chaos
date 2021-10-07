# VivaVictoria.Chaos.Reflection

### Install
Install via [nuget](https://www.nuget.org/packages/VivaVictoria.Chaos.RawSql/).

### Migration class
```c#
using VivaVictoria.Chaos.ReflectionSqlReader.Attributes;
using VivaVictoria.Chaos.ReflectionSqlReader.Interfaces;

namespace Migrations
{
    [Migration(Version = 1, Name = "create table test", TransactionMode = TransactionMode.One)]
    public class Migration_001_CreateTableTest : IReflectMigration {
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

### Usage
Just use extension method:
```c#
var services = new ServiceCollection();
services.AddChaosReflection(typeof(Program).Assembly);
```