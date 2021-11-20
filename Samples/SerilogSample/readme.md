# Example of using structured logging by Serilog with VivaVictoria.Chaos
### About
This is WebApplication .NET Core 3.1 project with Chaos migrations tool integration and Serilog logging tool.

### Project structure
`Startup.cs` - default netcore app Startup class.  
`Settings.cs` - implementation of ISettings interface for providing to Migrator required project-dependent properties like `ConnectionString` and default `TransactionMode`.  
`Program.cs` - program entrypoint with Chaos setting up.
`Migrations` - directory for sql migrations scripts. See `Chaos.RawSql` for more info about script naming rules.

### Project file RawSqlSample.csproj
```xml
<ItemGroup>
    <Content Include="Migrations\*.sql">
        <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </Content>
</ItemGroup>
```

### Main differences from other examples
This example uses custom logger - Serilog with structured (JsonCompact) format. For better and simpler logging integration
Chaos integrates into HostBuilder with other application services like logging or controllers.  
But Chaos still running migrations before `Host.Run()` called at `Program.Call()`.

### Dependencies
- `Serilog.AspNetCore` Serilog structured logger
- `Chaos.Logging.Console` (can be replaced with any other `ILogger`)
- `Chaos.PostgreSQL` (can be replaced with any other RDBMS driver)
- `Chaos.RawSql` for parsing migrations from sql scripts
