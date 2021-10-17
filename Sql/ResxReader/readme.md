# Chaos.ResxReader

### Install
Install via [nuget](https://www.nuget.org/packages/VivaVictoria.Chaos.ResxReader/).

### Summary
ResxReader allows you to read `Migration` items for `Chaos.Sql` from ResX file designers.

### Script and properties naming
Script files can be named in any format, but ResX properties should be named in special format `Migration_{version}_{name}`.    
`version` should be integer number, but can starts with `0`.  
`name` can be any string, can contains special symbols, `.`, `_`.  
Valid property name: `Migration_0001_create_test_table`.   
Invalid name: `A1.create_test_table`.
**NOTE:** ResxReader is fully case insensitive.

### Script format
You can use any SQL-like format depending on your RDBMS, but you must annotate scripts in file with specified format:
```sql
@chaos+up
/* up script, required */
create table test;

@chaos+down
/* down script, required */
drop table test;

@chaos+transaction one
/* transaction parameter - one, none or default. Optional */
```
**NOTE:** the order of scripts and parameters does not matter. It is possible to define down script, then transaction param,
then up script or any other combination. 

### Usage
Use `AddChaosResxReader` extension method. This is vararg method for registering ResxReader with specified type or several types.
```c#
var services = new ServiceCollection();
services.AddChaosResxReader(typeof(FirstResX), typeof(SecondResX));
```