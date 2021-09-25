# Chaos.SqlServer
IDatabaseDriver implementation for SqlClient. 

### Install
Install via [nuget](https://www.nuget.org/packages/VivaVictoria.Chaos.SqlServer/).

### Usage
```c#
var services = new ServiceCollection();
services.AddChaosSqlServer();
```
It adds `SqlServerMetadata`, `SqlServerDriver` and `Chaos.Dapper` to `ServiceCollection`. 
If you need to specify custom `SqlServerMetadata` you need to use other method:
```c#
var services = new ServiceCollection();
services.AddChaosSqlServer(_ => new MySqlServerMetadata());
```