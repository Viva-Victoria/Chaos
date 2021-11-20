using System.Data;
using Microsoft.Extensions.Logging;
using VivaVictoria.Chaos.Logging.Interfaces;
using VivaVictoria.Chaos.Logging.Models;

namespace VivaVictoria.Chaos.Logging
{
    /// <summary>
    /// IDbConnection wrapper for producing CommandWrapper instead of origin IDbCommand
    /// </summary>
    public class ConnectionWrapper : IDbConnection
    {
        private readonly ICommandProvider commandProvider;
        public readonly IDbConnection Original;

        public ConnectionWrapper(ICommandProvider commandProvider, IDbConnection original)
        {
            this.commandProvider = commandProvider;
            Original = original;
        }

        public IDbCommand CreateCommand() => commandProvider.Wrap(Original.CreateCommand());

        public void Open() => Original.Open();

        public void Close() => Original.Close();

        public void Dispose() => Original.Dispose();

        public int ConnectionTimeout => Original.ConnectionTimeout;
        
        public string Database => Original.Database;
        
        public ConnectionState State => Original.State;

        public IDbTransaction BeginTransaction() => Original.BeginTransaction();

        public IDbTransaction BeginTransaction(IsolationLevel il) => Original.BeginTransaction(il);

        public void ChangeDatabase(string databaseName) => Original.ChangeDatabase(databaseName);

        public string ConnectionString
        {
            get => Original.ConnectionString; 
            set => Original.ConnectionString = value;
        }
    }
}