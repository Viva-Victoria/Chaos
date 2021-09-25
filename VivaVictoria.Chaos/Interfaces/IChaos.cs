using System;

namespace VivaVictoria.Chaos.Interfaces
{
    public interface IChaos
    {
        public Chaos Init(Func<bool> condition = null);
        public void Up();
        public void Down(long targetVersion);
    }
}