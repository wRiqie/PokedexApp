using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace PokedexApp.Models
{
    public class Pokemon
    {
        [PrimaryKey]
        public decimal Id { get; set; }
        public string Name { get; set; }
        public byte[] Photo { get; set; }
        public string TypeName { get; set; }
        [Ignore]
        public PokemonSprite Sprites { get; set; }
        [Ignore]
        public List<Types> Types { get; set; }
        [Ignore]
        public List<PokemonAbility> Abilities { get; set; }
        [Ignore]
        public List<PokemonStats> Stats { get; set; }
        public decimal Weight { get; set; }
        public decimal Height { get; set; }
    }
}
