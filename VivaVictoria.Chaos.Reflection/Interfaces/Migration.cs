namespace VivaVictoria.Chaos.Reflection.Interfaces
{
    public interface IMigration
    {
        public string Up();
        public string Down();
    }
}