using PokedexApp.Enums;
using PokedexApp.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PokedexApp.Repositories.PokemonRepository
{
    public interface IPokemonRepository
    {
        ResultadoExecucaoEnum SaveAllPokemon(IEnumerable pokemons);

        Task<List<Pokemon>> GetAllPokemons();
        Task<Pokemon> GetPokemon(decimal id);
        ResultadoExecucaoEnum SavePokemonType(TypeDescription pokemonType);
        ResultadoExecucaoEnum SavePokemonStat(PokemonStats pokemonStat);
        ResultadoExecucaoEnum SavePokemonAbility(PokemonAbility pokemonAbility);
        Task<List<TypeDescription>> GetPokemonTypes(decimal pokemonId);
        Task<List<PokemonStats>> GetPokemonStats(decimal pokemonId);
        Task<List<PokemonAbility>> GetPokemonAbilities(decimal pokemonId);
    }
}
