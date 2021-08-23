using PokedexApp.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace PokedexApp.Services.SQLite
{
    public interface ISQLite
    {
        bool Save(object obj);
        bool SaveAll(IEnumerable list);
        List<Pokemon> GetAllPokemons();
        Pokemon GetPokemon(decimal id);
        List<TypeDescription> GetPokemonTypes(decimal PokemonId);
        List<PokemonStats> GetPokemonStats(decimal PokemonId);
        List<PokemonAbility> GetPokemonAbilities(decimal PokemonId);
    }
}
