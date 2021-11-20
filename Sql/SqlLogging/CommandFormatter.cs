using System;
using System.Collections.Generic;
using VivaVictoria.Chaos.Logging.Interfaces;

namespace VivaVictoria.Chaos.Logging
{
    /// <summary>
    /// Formats specified IDbCommand's command and parameters by special format.
    /// Available aliases:
    /// <list type="table">
    ///     <item>
    ///         <term><code>{timestamp}</code></term>
    ///         <description>Current UTC DateTime in "yyyy-MM-ddTHH:mm:ss" format</description>
    ///     </item>
    ///     <item>
    ///         <term><code>{command}</code></term>
    ///         <description>Raw IDbCommand's commandText</description>
    ///     </item>
    ///     <item>
    ///         <term><code>{script}</code></term>
    ///         <description>SQL script - commandText formatted with parameters</description>
    ///     </item>
    /// </list>
    /// </summary>
    public class CommandFormatter : ICommandFormatter
    {
        private delegate string Formatter(string sql, Dictionary<string, object> parameters);
            
        private readonly IWrapperSettings settings;
        private readonly Dictionary<string, Formatter> formatters;

        public CommandFormatter(IWrapperSettings settings)
        {
            this.settings = settings;
            this.formatters = new Dictionary<string, Formatter>
            {
                { "{timestamp}", (sql, parameters) => DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss") },
                { "{command}", (sql, parameters) => sql },
                { "{script}", MakeScript }
            };
        }

        private string MakeScript(string sql, Dictionary<string, object> parameters)
        {
            if (!settings.FormatParameters)
            {
                return sql;
            }
            
            foreach (var (name, value) in parameters)
            {
                sql = sql.Replace($"@{name}", value?.ToString() ?? settings.Null);
            }

            return sql;
        }

        public string Format(string sql, Dictionary<string, object> parameters)
        {
            var result = settings.Format;
            
            foreach (var (key, func) in formatters)
            {
                if (settings.Format.Contains(key))
                {
                    result = result.Replace(key, func(sql, parameters));
                }
            }

            return result;
        }
    }
}