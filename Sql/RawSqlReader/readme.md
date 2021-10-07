# Chaos.RawSql

### Script naming
Script files should be named with special format: `{version}.{name}.{up or down}.sql`.  
`version` should be integer number, but can starts with `0`.  
`name` can be any string, but it should not contain `.` symbol.  
Valid name: `0001.create_test_table.up.sql`.  
Invalid name: `A1.create_test_table.up.sql`.  
**NOTE:** every `up` file should have `down` companion and vice versa.

### Install
Install via [nuget](https://www.nuget.org/packages/VivaVictoria.Chaos.RawSql/).

### .csproj file
All scripts should be available at runtime, so you need to copy scripts directory to output on every build:
```xml
<ItemGroup>
    <Content Include="Migrations\*.sql">
        <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </Content>
</ItemGroup>
```

### Usage
```c#
var services = new ServiceCollection();
services.AddChaosRawSql("Migrations/");
```