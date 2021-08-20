using PokedexApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PokedexApp.Services.Request
{
    public interface IRequestService
    {
        Task<List<Pokemon>> GetPokemons(decimal quantidade, decimal indice);
    }
}
