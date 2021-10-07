# VivaVictoria.Chaos.Sql

### Install
Install via [nuget](https://www.nuget.org/packages/VivaVictoria.Chaos.Sql/).

### About
Basic package for RDBMS-based Chaos.  
Package provides abstractions and enums for RDBMS-based IMigrator implementations like ClickHouse or DapperMigrator.

#### TransactionMode
TransactionMode defines how migration should be applied: inside transaction or not.
Applying complex migration inside transactions allows you to keep your DB in consistent state:
all changes will be rollback if error occurred.

#### ISqlSettings
Extensions of basic `ISettings` with required `ConnectionString` and `TransactionMode` fields.

#### Migration
Implementation of `IMigration` interface to provide more information about migration:
* Name (will be used in future, now is optional)
* TransactionMode
* Up - upgrade sql script
* Down - downgrade sql script

#### DependencyInjection
Provides simple wrapper for registering `IChaos<Migration>` in specified `IServicesCollection`. Just call `AddChaosCore` without generic parameter.