# Chaos.Logging
Wrappers for logging SQL commands and parameters executed via IDbCommand.  

### Install
Install via [nuget](https://www.nuget.org/packages/VivaVictoria.Chaos.Sql.Logging/).

### Usage
At first, add wrappers to `IServicesCollection`. At this step you can configure wrappers with `WrapperBuilder`:
```c#
services.AddChaosConnectionLogging(builder => builder.
    Format("{timestamp} {script}").
    Level(LogLevel.Trace).
    Null("empty").
    FormatParameters(false));
```
When you receive `IConnectionProvider` instance from `DI`, just invoke `Wrap(connection)` method with
original `IDbConnection` received from your RDBMS driver like Npgsql or SqlServer.  
**Note:** `ConnectionWrapper` can't provide your driver-specific methods available in your `IDbConnection` instance. 
If you need to use it, you can receive original connection via `ConnectionWrapper.Original` readonly property. 