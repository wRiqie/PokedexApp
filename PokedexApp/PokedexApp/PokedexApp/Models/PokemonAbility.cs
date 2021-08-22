using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace PokedexApp.Models
{
    public class PokemonAbility
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [Ignore]
        public AbilityDescription Ability { get; set; }
        public string AbilityName { get; set; }
        public decimal PokemonId { get; set; }
    }
}
