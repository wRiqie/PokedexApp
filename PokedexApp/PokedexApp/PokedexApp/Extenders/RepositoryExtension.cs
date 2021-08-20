using PokedexApp.Repositories.PokemonRepository;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Text;

namespace PokedexApp.Extenders
{
    public static class RepositoryExtension
    {
        internal static void ResolveRepository(this IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IPokemonRepository, PokemonRepository>();
        }
    }
}
