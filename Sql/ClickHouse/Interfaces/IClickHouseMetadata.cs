using System;
using System.Collections.Generic;
using System.Data;
using ClickHouse.Ado;
using VivaVictoria.Chaos.Enums;

namespace VivaVictoria.Chaos.ClickHouse.Interfaces
{
    public interface IClickHouseMetadata
    {
        public string Engine { get; }
        public string TableName { get; }

        public string IdColumnName { get; }
        public string IdColumnType { get; }
        public string IdColumnParams { get; }

        public string StateColumnName { get; }
        public string StateColumnType { get; }
        public string StateColumnParams { get; }

        public string VersionColumnName { get; }
        public string VersionColumnType { get; }
        public string VersionColumnParams { get; }

        public string DateColumnName { get; }
        public string DateColumnType { get; }
        public string DateColumnParams { get; }

        public string CreateStatement { get; }

        public IEnumerable<ClickHouseParameter> InsertParameters(DateTime dateTime, long version, MigrationState state);

        public string InsertStatement { get; }

        public string SelectStatement { get; }
    }
}