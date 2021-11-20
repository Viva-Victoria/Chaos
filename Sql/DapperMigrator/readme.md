# Chaos.Dapper
IMigrator implementation for Data Mapper Dapper library. Supports all databases supported by Dapper. 

### Communication
This library provider `IDatabaseDriver` interface. This interface should be implemented to provide Dapper valid
`IDbConnection` and create/insert/select statements for migration metadata table.

### Install
Install via [nuget](https://www.nuget.org/packages/VivaVictoria.Chaos.DapperMigrator/).

### Usage
```c#
var services = new ServiceCollection();
services.AddChaosDapper<MyMetadata>();
```
It defines DapperMigrator and specified `MyMetadata` class for Chaos. Insteadof `MyMetadata` you should use your `IMetadata` class, supported by `IDatabaseDriver`.

### Extension
1. Create new IDatabaseDriver implementation. Your implementation can require specified Metadata implementation:
```c#
public class MyMetadata : IMetadata
{
    public string TableName { get; }
    public string IdColumnName { get; }
    public string IdColumnType { get; }
    public string StateColumnName { get; }
    public string StateColumnType { get; }
    public string VersionColumnName { get; }
    public string VersionColumnType { get; }
    public string DateColumnName { get; }
    public string DateColumnType { get; }
}

public class MyDriver : IDatabaseDriver<MyMetadata>
{
    /*
     * Use connection string and metadata model to create connection. You can store
     * in metadata runtime-dependent connection parameters and use it for providing 
     * correct connection. For example, Postgres uses Schema from metadata to specify
     * NpgsqlConnection SearchPath.
     */
    public IDbConnection Connect(string connectionString, MyMetadata metadata) =>
        new MyDbConnection(connectionString);
       
    /*
     * Not all RDBMS supports transactions or supports their functional not fully or in custom format.
     * For example, Clickhouse does not support transactions and always returns false from this method.
     */ 
    public bool IsTransactionSupported();
    
    /*
     * Create raw sql script for creating metadata table and other infrastructure.
     * For example, SqlServer and Postgres creates requested Schema before creating table.
     * Use provided metadata instance to determine table and column parameters like name, type or constraints
     */
    public string CreateStatement(MyMetadata metadata);
    
    /*
     * Returns raw dapper-style sql script for inserting metadata info into table.
     * You can use any params via @Param keys, but you need to define all required params
     * in InsertParameters method.
     * Default params dateTime, version and state. They are required and should be used always.
     */
    public string InsertStatement(MyMetadata metadata);
    
    /*
     * Returns params for InsertStatement script in any supported by Dapper format: POCOs, dynamic, etc
     */
    public object InsertParameters(DateTime dateTime, long version, MigrationState state);
    
    /*
     * Returns sql script for receiving latest metadata info. Should returns from DB two values:
     * Version as int64 number,
     * State as any Enum-convertible value, recommended int or int8/int16/int32/int64
     */
    public string SelectStatement(MyMetadata metadata);
}
```