using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VivaVictoria.Chaos.Enums;
using VivaVictoria.Chaos.Interfaces;
using VivaVictoria.Chaos.Models;

namespace VivaVictoria.Chaos.RawSql
{
    public class RawSqlMigrationReader : IMigrationReader
    {
        private string path;

        public RawSqlMigrationReader(string path)
        {
            this.path = path;
        }

        private long ReadVersion(string word)
        {
            int startIndex = 0;
            while (word[startIndex] == '0')
            {
                startIndex++;
            }

            return long.Parse(word[startIndex..]);
        }
        
        public List<Migration> Read()
        {
            var preloaded = new Dictionary<long, Migration>();
            
            foreach (var filePath in Directory.GetFiles(path))
            {
                var index = filePath.LastIndexOfAny(new []{ '/', '\\' }); 
                
                var parts = filePath[(index + 1)..].Split(".");
                var version = ReadVersion(parts[0]);
                var name = parts[1];

                var up = false;
                if (parts.Length == 4)
                {
                    up = parts[2].Equals("up");
                }

                var script = File.ReadAllText(filePath);
                Migration info;
                if (!preloaded.TryGetValue(version, out info))
                {
                    info = new Migration(version, name, TransactionMode.Default, "", "");
                    preloaded.Add(version, info);
                }

                if (up)
                {
                    info.UpScript = script;
                }
                else
                {
                    info.DownScript = script;
                }
            }

            var invalidMigration = preloaded.Values.FirstOrDefault(i => i.UpScript == "" || i.DownScript == "");
            if (invalidMigration != null)
            {
                throw new ArgumentException($"Migration {invalidMigration.Version} is invalid: empty Up or Down script provided");
            }
            
            return preloaded.Values.ToList();
        }
    }
}