using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using VivaVictoria.Chaos.Interfaces;
using VivaVictoria.Chaos.Sql.Enums;
using VivaVictoria.Chaos.Sql.Models;

namespace VivaVictoria.Chaos.ResxReader
{
    public class ResxReader : IMigrationReader<Migration>
    {
        private static readonly Regex UpRegex = new Regex(".*@chaos\\+up.+", RegexOptions.IgnoreCase);
        private static readonly Regex DownRegex = new Regex(".*@chaos\\+down.+", RegexOptions.IgnoreCase);
        private static readonly Regex TransactionRegex = new Regex(".*@chaos\\+transaction[ \t=]+(default|one|none).+", RegexOptions.IgnoreCase);
        private static readonly Regex PropertyNameRegex = new Regex("Migration_(\\d+)_(\\w+)", RegexOptions.IgnoreCase);

        private Dictionary<string, string> scripts;

        public ResxReader(BindingFlags bindingFlags, params Type[] types)
        {
            scripts = types
                .SelectMany(t => t.GetProperties(bindingFlags))
                .Where(p => PropertyNameRegex.IsMatch(p.Name) 
                    && p.PropertyType == typeof(string) 
                    && !string.IsNullOrEmpty((string)p.GetValue(null)))
                .ToDictionary(p => p.Name, p => (string) p.GetValue(null));
        }

        public ResxReader(params Type[] types) : this(BindingFlags.Static | BindingFlags.NonPublic, types)
        { }

        public List<Migration> Read()
        {
            return scripts
                .Select(pair => ParseScript(pair.Key, pair.Value))
                .ToList();
        }
        
        private Migration ParseScript(string name, string script)
        {
            var upMatches = UpRegex.Matches(script);
            var downMatches = DownRegex.Matches(script);

            if (upMatches.Count != 1 || downMatches.Count != 1)
            {
                throw new FormatException($"Can not parse next script\n{script}");
            }

            var upMatch = upMatches.First();
            var downMatch = downMatches.First();

            var e = upMatch.Index + upMatch.Length;
            var upScript = script[e..downMatch.Index];
            var downScript = script[(downMatch.Index + downMatch.Length)..];

            if (upMatch.Index > downMatch.Index)
            {
                (upScript, downScript) = (downScript, upScript);
            }

            var transactionMode = TransactionMode.Default;
            var transactionMatch = TransactionRegex.Match(script);
            if (transactionMatch.Success)
            {
                transactionMode = Enum.Parse<TransactionMode>(transactionMatch.Groups[1].Value, true);
            }

            var nameMatch = PropertyNameRegex.Match(name);

            return new Migration
            {
                TransactionMode = transactionMode,
                Name = nameMatch.Groups[2].Value,
                Version = long.Parse(nameMatch.Groups[1].Value.Trim('0', '_', ' ')),
                Up = upScript.Trim('\r', '\n'),
                Down = downScript.Trim('\r', '\n')
            };
        }
    }
}