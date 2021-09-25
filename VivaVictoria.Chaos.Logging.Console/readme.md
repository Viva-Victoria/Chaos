# Chaos.Logging.Console
Simple console logging package.  

### Install
Install via [nuget](https://www.nuget.org/packages/VivaVictoria.Chaos.Logging.Console/).

### Usage
```c#
var services = new ServiceCollection();
services.AddChaosConsoleLogger();
```
or provide configuration
```c#
var services = new ServiceCollection();
services.AddChaosConsoleLogger(logger => {
    logger.Enable(LogLevel.Debug);
    logger.Disable(LogLevel.Trace);
});
```