﻿using System;
using System.Data;

namespace VivaVictoria.Chaos.Dapper.Interfaces
{
    public interface IDatabaseDriver<in TMetadata> where TMetadata : IMetadata
    {
        public IDbConnection Connect(string connectionString);
        public bool IsTransactionSupported();
        public string CreateStatement(TMetadata metadata);
        public string InsertStatement(TMetadata metadata);
        public string SelectStatement(TMetadata metadata);
        public object InsertParameters(DateTime dateTime, long version);
    }
}