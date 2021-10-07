# Chaos.Postgres
IDatabaseDriver implementation for Npgsql. 

### Install
Install via [nuget](https://www.nuget.org/packages/VivaVictoria.Chaos.Postgres/).

### Usage
```c#
var services = new ServiceCollection();
services.AddChaosPostgres();
```
It adds `PostgresMetadata`, `PostgresDriver` and `Chaos.Dapper` to `ServiceCollection`. 
If you need to specify custom `PostgresMetadata` you need to use other method:
```c#
var services = new ServiceCollection();
services.AddChaosPostgres(_ => new MyPostgresMetadata());
```