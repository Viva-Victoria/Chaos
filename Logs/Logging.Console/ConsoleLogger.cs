using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Extensions.Logging;
using VivaVictoria.Chaos.Logging.Interfaces;

namespace VivaVictoria.Chaos.Logging.Console
{
    public class ConsoleLogger : IChaosLogger
    {
        //lock on mutex for correct console output with await/async operations or multi-thread programs 
        private Mutex mutex = new Mutex();
        private Dictionary<LogLevel, bool> levels;

        /*
         * Enables all levels if enabledByDefault is true (default is true), disables otherwise
         */
        public ConsoleLogger(bool enabledByDefault = true)
        { 
            levels = new Dictionary<LogLevel, bool>
            {
                {LogLevel.Critical, enabledByDefault},
                {LogLevel.Debug, enabledByDefault},
                {LogLevel.Error, enabledByDefault},
                {LogLevel.Information, enabledByDefault},
                {LogLevel.None, false},
                {LogLevel.Trace, enabledByDefault},
                {LogLevel.Warning, enabledByDefault}
            };
        }

        /*
         * Enables only specified levels. All other disabled by default
         */
        public ConsoleLogger(params LogLevel[] levels) : this(false)
        {
            if (levels == null || levels.Length == 0)
            {
                return;
            }

            foreach (var level in levels)
            {
                Set(level, true);
            }
        }

        /*
         * Default constructor for Dependency Injection mechanism.
         */
        public ConsoleLogger() : this(true)
        { }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
            Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            mutex.WaitOne();
            
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
            
            mutex.ReleaseMutex();
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