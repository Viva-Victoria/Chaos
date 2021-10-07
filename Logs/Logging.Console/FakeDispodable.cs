using System;

namespace VivaVictoria.Chaos.Logging.Console
{
    internal class FakeDisposable : IDisposable
    {
        public static FakeDisposable Instance = new FakeDisposable();

        public void Dispose()
        {
            
        }
    }
}