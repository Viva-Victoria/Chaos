using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using VivaVictoria.Chaos.Interfaces;

namespace VivaVictoria.Chaos.Extensions
{
    public static class DependencyInjection
    {
        public static TChild GetService<TBase, TChild>(this IEnumerable<TBase> all, bool strictType = true) where TChild : TBase
        {
            var found = strictType
                ? all.FirstOrDefault(t => t.GetType() == typeof(TChild))
                : all.FirstOrDefault(t => t.GetType() == typeof(TChild) || t.GetType().IsSubclassOf(typeof(TChild))); 
            return found == null ? default : (TChild) found;
        }
        
        public static IServiceCollection AddChaosCore(this IServiceCollection services)
        {
            return services.AddSingleton<IChaos, Chaos>();
        }
    }
}