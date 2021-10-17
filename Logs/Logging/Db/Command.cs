using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace VivaVictoria.Chaos.Logging.Db
{
    /*
     * Simple IDbCommand wrapper for logging Sql scripts to ILogger.
     * Requires real IDbCommand implementation and logger.
     */
    public class Command : IDbCommand
    {
        private readonly ILogger<ConnectionProvider> logger;
        private readonly IDbCommand command;

        public Command(ILogger<ConnectionProvider> logger, IDbCommand command)
        {
            this.logger = logger;
            this.command = command;
        }

        private T ExecuteWithLog<T>(Func<T> func)
        {
            var sw = Stopwatch.StartNew();
            T result;
            
            var sql = command.CommandText;
            foreach (IDbDataParameter parameter in command.Parameters)
            {
                string value = parameter.Value == null || parameter.Value is DBNull
                    ? "<null>"
                    : parameter.Value.ToString();

                sql = sql.Replace($"@{parameter.ParameterName}", value);
            }
                
            logger.LogDebug($"Executing\n{sql}");
            
            try
            {
                result = func();
            }
            finally
            {
                logger.LogDebug($"Executed. Time elapsed {sw.ElapsedMilliseconds}ms");
            }

            return result;
        }

        public int ExecuteNonQuery()
        {
            return ExecuteWithLog(() => command.ExecuteNonQuery());
        }

        public IDataReader ExecuteReader()
        {
            return ExecuteWithLog(() => command.ExecuteReader());
        }

        public IDataReader ExecuteReader(CommandBehavior behavior)
        {
            return ExecuteWithLog(() => command.ExecuteReader(behavior));
        }

        public object ExecuteScalar()
        {
            return ExecuteWithLog(() => command.ExecuteScalar());
        }

        public IDataParameterCollection Parameters => command.Parameters;
        public void Prepare() => command.Prepare();
        public void Dispose() => command.Dispose();
        public void Cancel() => command.Cancel();
        public IDbDataParameter CreateParameter() => command.CreateParameter();

        public string CommandText
        {
            get => command.CommandText; 
            set => command.CommandText = value;
        }

        public int CommandTimeout
        {
            get => command.CommandTimeout; 
            set => command.CommandTimeout = value;
        }

        public CommandType CommandType
        {
            get => command.CommandType; 
            set => command.CommandType = value;
        }

        public IDbConnection Connection
        {
            get => command.Connection;
            set => command.Connection = value;
        }

        public IDbTransaction Transaction
        {
            get => command.Transaction; 
            set => command.Transaction = value;
        }

        public UpdateRowSource UpdatedRowSource
        {
            get => command.UpdatedRowSource; 
            set => command.UpdatedRowSource = value;
        }
    }
}