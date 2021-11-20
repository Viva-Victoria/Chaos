using Microsoft.Extensions.Logging;
using VivaVictoria.Chaos.Logging.Interfaces;
using VivaVictoria.Chaos.Logging.Models;

namespace VivaVictoria.Chaos.Logging
{
    /// <summary>
    /// Builds <see cref="Interfaces.IWrapperSettings"/> via Builder pattern
    /// </summary>
    public class WrapperBuilder
    {
        private string format;
        private bool formatParameteres;
        private string nullValue;
        private LogLevel level;

        public WrapperBuilder()
        {
            format = "{script}";
            formatParameteres = true;
            nullValue = "<null>";
            level = LogLevel.Debug;
        }

        /// <summary>
        /// Specifies log format, see more in <see cref="CommandFormatter"/>, default is <code>{script}</code>
        /// </summary>
        /// <param name="format">format string, supports several aliases like <code>{script}</code></param>
        /// <returns></returns>
        public WrapperBuilder Format(string format)
        {
            if (!string.IsNullOrEmpty(format))
            {
                this.format = format;
            }
            
            return this;
        }

        /// <summary>
        /// Specifies value for NULL and DbNull values, default is <code>&#60;null&#62;</code>
        /// </summary>
        /// <param name="nullValue">value for <code>null</code> and <see cref="System.DBNull"/> values</param>
        /// <returns>WrapperBuilder for next configure calls</returns>
        public WrapperBuilder Null(string nullValue)
        {
            this.nullValue = nullValue;
            return this;
        }

        /// <summary>
        /// Specifies LogLevel for logging, default is <code>Debug</code>
        /// </summary>
        /// <param name="level">log level</param>
        /// <returns></returns>
        public WrapperBuilder Level(LogLevel level)
        {
            this.level = level;
            return this;
        }

        /// <summary>
        /// Produces IWrapperSettings built from specified or default params
        /// </summary>
        /// <returns>IWrapperSettings instance</returns>
        public IWrapperSettings Build()
        {
            return new WrapperSettings
            {
                Format = format,
                FormatParameters = formatParameteres,
                Null = nullValue,
                Level = level
            };
        }
    }
}