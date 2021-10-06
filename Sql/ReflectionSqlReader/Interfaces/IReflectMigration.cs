namespace VivaVictoria.Chaos.ReflectionSqlReader.Interfaces
{
    public interface IReflectMigration
    {
        public string Up();
        public string Down();
    }
}