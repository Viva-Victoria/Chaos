using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using VivaVictoria.Chaos.Logging.Interfaces;

namespace VivaVictoria.Chaos.Logging.Console
{
    public class ConsoleLogger : IChaosLogger
    {
        private Dictionary<LogLevel, bool> levels;

        public ConsoleLogger()
        { 
            levels = new Dictionary<LogLevel, bool>
            {
                {LogLevel.Critical, true},
                {LogLevel.Debug, true},
                {LogLevel.Error, true},
                {LogLevel.Information, true},
                {LogLevel.None, false},
                {LogLevel.Trace, true},
                {LogLevel.Warning, true}
            };
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
            Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }
            
            var description = formatter(state, exception);
            Write(ConsoleColor.DarkGray, $"{DateTime.Now:yyyy-MM-dd hh:mm:ss.fff}");
            Write(ConsoleColor.Black, " [");

            var color = ConsoleColor.Black;
            switch (logLevel)
            {
                case LogLevel.Critical:
                case LogLevel.Error:
                    color = ConsoleColor.Red;
                    break;
                case LogLevel.Warning:
                    color = ConsoleColor.Yellow;
                    break;
                case LogLevel.Trace:
                case LogLevel.Information:
                case LogLevel.Debug:
                    color = ConsoleColor.Green;
                    break;
            }
            
            Write(color, $"{logLevel.ToString().ToUpper()}");
            Write(ConsoleColor.Black, $"] {description} \n");
        }

        private void Write(ConsoleColor color, string message)
        {
            System.Console.ForegroundColor = color;
            System.Console.Write(message);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return levels[logLevel];
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return FakeDisposable.Instance;
        }

        public void Set(LogLevel level, bool enabled)
        {
            levels[level] = enabled;
        }
    }
}