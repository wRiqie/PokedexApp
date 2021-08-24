using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace PokedexApp.Models
{
    public class Types
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public decimal PokemonId { get; set; }
        public string TypeName { get; set; }
        [Ignore]
        public TypeDescription Type { get; set; }
    }
}
