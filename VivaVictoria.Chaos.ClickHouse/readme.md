# Chaos.ClickHouse
IMigrator implementation for open-source column-oriented [Yandex ClickHouse DBMS](https://clickhouse.com/). 

### Communication
This library uses `ClickHouse.Ado` package for communication with ClickHouse server via native protocol. 

### Install
Install via [nuget](https://www.nuget.org/packages/VivaVictoria.Chaos.ClickHouse/).

### Usage
```c#
var services = new ServiceCollection();
services.AddChaosClickHouse();
```
It defines default `ClickHouseMetadata`, if you want to register custom metadata class, use next method
```c#
var services = new ServiceCollection();
services.AddChaosClickHouse(_ => new MyCustomClickHouseMetadata());
```