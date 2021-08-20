using PokedexApp.Enums;
using PokedexApp.Models;
using PokedexApp.Services.SQLite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PokedexApp.Repositories.PokemonRepository
{
    public class PokemonRepository : IPokemonRepository
    {
        readonly ISQLite _sqlite;
        public PokemonRepository(
            ISQLite sqlite)
        {
            _sqlite = sqlite;
        }

        public async Task<List<Pokemon>> GetAllPokemons()
            => _sqlite.GetAllPokemons();

        public ResultadoExecucaoEnum SaveAllPokemon(IEnumerable pokemons)
        {
            try
            {
                if (_sqlite.SaveAll(pokemons))
                {
                    return ResultadoExecucaoEnum.sucesso;
                }
                else
                {
                    return ResultadoExecucaoEnum.erro;
                }
            }
            catch (Exception ex)
            {
                return ResultadoExecucaoEnum.erro;
            }
        }
    }
}
