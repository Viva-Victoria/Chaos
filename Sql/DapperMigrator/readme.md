# Chaos.Dapper
IMigrator implementation for Data Mapper Dapper library. Supports all databases supported by Dapper. 

### Communication
This library provider `IDatabaseDriver` interface. This interface should be implemented to provide Dapper valid
`IDbConnection` and create/insert/select statements for migration metadata table.

### Install
Install via [nuget](https://www.nuget.org/packages/VivaVictoria.Chaos.Dapper/).

### Usage
```c#
var services = new ServiceCollection();
services.AddChaosDapper<MyMetadata>();
```
It defines DapperMigrator and specified `MyMetadata` class for Chaos. Insteadof `MyMetadata` you should use your `IMetadata` class, supported by `IDatabaseDriver`.