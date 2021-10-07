using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using VivaVictoria.Chaos.Interfaces;

namespace VivaVictoria.Chaos.Extensions
{
    public static class DependencyInjection
    {
        public static TChild GetService<TBase, TChild>(this IEnumerable<TBase> all, bool strictType = true) 
            where TChild : TBase
        {
            var childType = typeof(TChild);
            
            var found = strictType
                ? all.FirstOrDefault(t => t.GetType() == childType)
                : all.FirstOrDefault(t =>
                {
                    var type = t.GetType();
                    return type == childType || type.IsSubclassOf(childType) || childType.IsAssignableFrom(type);
                }); 
            return found == null ? default : (TChild) found;
        }

        public static TChild RequireService<TBase, TChild>(this IEnumerable<TBase> all, bool strictType = true)
            where TChild : TBase
        {
            var result = all.GetService<TBase, TChild>(strictType);
            if (result == null)
            {
                throw new InvalidOperationException($"Required service {typeof(TChild)} not present");
            }
            
            return result;
        }
        
        public static IServiceCollection AddChaosCore<TMigration>(this IServiceCollection services)
            where TMigration : IMigration
        {
            return services.AddSingleton<IChaos<TMigration>, Chaos<TMigration>>();
        }

        public static IChaos<TMigration> GetChaos<TMigration>(this IServiceProvider provider)
            where TMigration : IMigration
        {
            return provider.GetRequiredService<IChaos<TMigration>>();
        }
    }
}