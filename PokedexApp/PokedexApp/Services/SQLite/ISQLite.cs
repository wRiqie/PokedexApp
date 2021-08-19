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
    }
}
