using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using VivaVictoria.Chaos.Logging.Interfaces;

namespace VivaVictoria.Chaos.Logging
{
    /// <summary>
    /// CommandWrapper wraps provided IDbCommand instance and allows write command's command and params to log
    /// </summary>
    public class CommandWrapper : IDbCommand
    {
        private readonly ILogger<CommandWrapper> logger;
        private readonly IWrapperSettings settings;
        private readonly ICommandFormatter formatter;
        public readonly IDbCommand Original;

        public CommandWrapper(
            ILogger<CommandWrapper> logger, 
            IWrapperSettings settings, 
            ICommandFormatter formatter, 
            IDbCommand original)
        {
            this.logger = logger;
            this.settings = settings;
            this.formatter = formatter;
            Original = original;
        }
        
        /// <summary>
        /// Writes command and params to logger and then executes specified func
        /// </summary>
        /// <param name="func">Function to be executed</param>
        /// <typeparam name="T">Func return type</typeparam>
        /// <returns>Result produced by func</returns>
        private T ExecuteWithLog<T>(Func<T> func)
        {
            var sw = Stopwatch.StartNew();
            T result;
            
            var sql = Original.CommandText;
            var parameters = new Dictionary<string, object>();
            foreach (IDbDataParameter parameter in Original.Parameters)
            {
                parameters[parameter.ParameterName] = parameter.Value == null || parameter.Value is DBNull
                    ? null
                    : parameter.Value;
            }

            var log = formatter.Format(sql, parameters);
            logger.Log(settings.Level, log);
            
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
            return ExecuteWithLog(() => Original.ExecuteNonQuery());
        }

        public IDataReader ExecuteReader()
        {
            return ExecuteWithLog(() => Original.ExecuteReader());
        }

        public IDataReader ExecuteReader(CommandBehavior behavior)
        {
            return ExecuteWithLog(() => Original.ExecuteReader(behavior));
        }

        public object ExecuteScalar()
        {
            return ExecuteWithLog(() => Original.ExecuteScalar());
        }

        public IDataParameterCollection Parameters => Original.Parameters;
        public void Prepare() => Original.Prepare();
        public void Dispose() => Original.Dispose();
        public void Cancel() => Original.Cancel();
        public IDbDataParameter CreateParameter() => Original.CreateParameter();

        public string CommandText
        {
            get => Original.CommandText; 
            set => Original.CommandText = value;
        }

        public int CommandTimeout
        {
            get => Original.CommandTimeout; 
            set => Original.CommandTimeout = value;
        }

        public CommandType CommandType
        {
            get => Original.CommandType; 
            set => Original.CommandType = value;
        }

        public IDbConnection Connection
        {
            get => Original.Connection;
            set => Original.Connection = value;
        }

        public IDbTransaction Transaction
        {
            get => Original.Transaction; 
            set => Original.Transaction = value;
        }

        public UpdateRowSource UpdatedRowSource
        {
            get => Original.UpdatedRowSource; 
            set => Original.UpdatedRowSource = value;
        }
    }
}