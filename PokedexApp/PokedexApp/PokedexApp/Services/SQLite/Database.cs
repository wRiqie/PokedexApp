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
            sql.AppendLine("       TypeName");
            sql.AppendLine("From Pokemon");
            sql.AppendLine($"Where Id = '{id}'");

            lock (_locker)
            {
                return _conexao.Query<Pokemon>(sql.ToString()).FirstOrDefault();
            }
        }
        #endregion [ Pokemons ]

    }
}
