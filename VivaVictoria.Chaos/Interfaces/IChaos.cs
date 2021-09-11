namespace VivaVictoria.Chaos.Interfaces
{
    public interface IChaos
    {
        public void Up();
        public void Down(long targetVersion);
    }
}