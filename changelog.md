# Changelog

### 1.2.0
21.11.2021, first production-ready release
- logging wrappers reworked and moved to SQL-like projects
- new readme
- new samples

### 1.1.1
17.10.2021
- new packages
- drop down custom logging, reworked to use `Microsoft.Extensions.Logging`
- new SQL scripts logging
- more samples
- event broadcasting from Chaos
- more logs
- introducing `States` - mechanism for saving more metadata about migration
- `.resx` files reader
- `Serilog` integration
- bug fixes

### 1.1.0
7.10.2021, new repository structure
- added samples
- started dividing RDBMS-dependent functional from base Chaos tool
- readme
- licenses
- reworked interfaces 

### 1.0.1-preview
25.09.2021, minor bug fixes

### 1.0.0-preview
25.09.2021, Development started, released first unstable version.
- ClickHouse driver
- Dapper
- custom Logging
- PostgreSQL driver
- SQL Server driver
- Raw sql files parser
- Reflection migration classes parser