using VivaVictoria.Chaos.ReflectionSqlReader.Attributes;
using VivaVictoria.Chaos.ReflectionSqlReader.Interfaces;

namespace ReflectionSample.Migrations
{
    [Migration(Version = 1, Name = "create table test")]
    public class Migration_001_CreateTableTest : IReflectMigration {
        public string Up()
        {
            return "create table test(id int primary key)";
        }

        public string Down()
        {
            return "drop table test";
        }
    }
}