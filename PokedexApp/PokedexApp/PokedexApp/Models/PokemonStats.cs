using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace PokedexApp.Models
{
    public class PokemonStats
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public decimal Base_stat { get; set; }
        public string StatName { get; set; }
        public decimal PokemonId { get; set; }
        public double ProgressValue { get; set; }

        [Ignore]
        public StatDescription Stat { get; set; }
    }
}
