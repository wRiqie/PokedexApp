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

        public async Task<Pokemon> GetPokemon(decimal id)
            => _sqlite.GetPokemon(id);

        public ResultadoExecucaoEnum SavePokemonType(TypeDescription pokemonType)
        {
            try
            {
                if (_sqlite.Save(pokemonType))
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

        public ResultadoExecucaoEnum SavePokemonStat(PokemonStats pokemonStat)
        {
            try
            {
                if (_sqlite.Save(pokemonStat))
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

        public ResultadoExecucaoEnum SavePokemonAbility(PokemonAbility pokemonAbility)
        {
            try
            {
                if (_sqlite.Save(pokemonAbility))
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

        public async Task<List<TypeDescription>> GetPokemonTypes(decimal pokemonId)
            => _sqlite.GetPokemonTypes(pokemonId);

        public async Task<List<PokemonStats>> GetPokemonStats(decimal pokemonId)
            => _sqlite.GetPokemonStats(pokemonId);

        public async Task<List<PokemonAbility>> GetPokemonAbilities(decimal pokemonId)
            => _sqlite.GetPokemonAbilities(pokemonId);
    }
}
