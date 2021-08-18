using System;
using System.Collections.Generic;
using System.Text;

namespace PokedexApp.Models
{
    public class Pokemon
    {
        public decimal? Id { get; set; }
        public string Name { get; set; }
        public PokemonSprite Sprites { get; set; }
        public List<Types> Types { get; set; }
    }
}
