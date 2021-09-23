namespace VivaVictoria.Chaos.Interfaces
{
    public interface IChaos
    {
        public Chaos Init();
        public void Up();
        public void Down(long targetVersion);
    }
}