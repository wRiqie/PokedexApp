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
    }
}
