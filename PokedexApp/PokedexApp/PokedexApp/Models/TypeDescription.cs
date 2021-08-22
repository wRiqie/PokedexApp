using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace PokedexApp.Models
{
    public class TypeDescription
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal PokemonId { get; set; }
    }
}
