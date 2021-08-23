using PokedexApp.Models;
using SQLite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PokedexApp.Services.SQLite
{
    public class Database : ISQLite
    {
        private readonly string _databasePath;
        private SQLiteConnection _conexao;
        private static object _locker = new object();
        public bool DatabaseExist => File.Exists(_databasePath);

        public Database()
        {
            try
            {
                _databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Pokedex.db3");
                if(_databasePath != null)
                {
                    _conexao = new SQLiteConnection(_databasePath);
                    _conexao.CreateTable<Pokemon>();
                    _conexao.CreateTable<PokemonStats>();
                    _conexao.CreateTable<TypeDescription>();
                    _conexao.CreateTable<PokemonAbility>();
                }
            }
            catch (Exception ex)
            {
            }
        }

        #region [ Generics ]
        public bool Save(object obj)
        {
            try
            {
                if (_conexao.Update(obj) == 0)
                    _conexao.Insert(obj);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool SaveAll(IEnumerable list)
        {
            try
            {
                lock (_locker)
                {
                    _conexao.RunInTransaction(() =>
                    {
                        foreach (var item in list)
                        {
                            if (_conexao.Update(item) == 0)
                                _conexao.Insert(item);
                        }
                    });
                    _conexao.Commit();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion [ Generics ]

        #region [ Pokemons ]
        public List<Pokemon> GetAllPokemons()
        {
            var sql = new StringBuilder();
            sql.AppendLine("Select Id,");
            sql.AppendLine("       Name,");
            sql.AppendLine("       Photo,");
            sql.AppendLine("       TypeName");
            sql.AppendLine("From Pokemon");

            lock (_locker)
            {
                return _conexao.Query<Pokemon>(sql.ToString());
            }
        }

        public Pokemon GetPokemon(decimal id)
        {
            var sql = new StringBuilder();
            sql.AppendLine("Select Id,");
            sql.AppendLine("       Name,");
            sql.AppendLine("       Photo,");
            sql.AppendLine("       Weight,");
            sql.AppendLine("       Height,");
            sql.AppendLine("       TypeName");
            sql.AppendLine("From Pokemon");
            sql.AppendLine($"Where Id = '{id}'");

            lock (_locker)
            {
                return _conexao.Query<Pokemon>(sql.ToString()).FirstOrDefault();
            }
        }
        #endregion [ Pokemons ]

        #region [ Types ]
        public List<TypeDescription> GetPokemonTypes(decimal PokemonId)
        {
            try
            {
                var sql = new StringBuilder();
                sql.AppendLine("Select Name");
                sql.AppendLine("From TypeDescription ");
                sql.AppendLine($"Where PokemonId = '{PokemonId}'");

                lock (_locker)
                {
                    return _conexao.Query<TypeDescription>(sql.ToString());
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion [ Types ]

        #region [ Stats ]
        public List<PokemonStats> GetPokemonStats(decimal PokemonId)
        {
            try
            {
                var sql = new StringBuilder();
                sql.AppendLine("Select Base_stat,");
                sql.AppendLine("       StatName");
                sql.AppendLine("  From PokemonStats");
                sql.AppendLine($"Where PokemonId = '{PokemonId}'");

                lock (_locker)
                {
                    return _conexao.Query<PokemonStats>(sql.ToString());
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion [ Stats ]

        #region [ Abilities ]
        public List<PokemonAbility> GetPokemonAbilities(decimal PokemonId)
        {
            try
            {
                var sql = new StringBuilder();
                sql.AppendLine("Select AbilityName");
                sql.AppendLine("From PokemonAbility");
                sql.AppendLine($"Where PokemonId = '{PokemonId}'");

                lock (_locker)
                {
                    return _conexao.Query<PokemonAbility>(sql.ToString());
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion [ Abilities ]
    }
}
