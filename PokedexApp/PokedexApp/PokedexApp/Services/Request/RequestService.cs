using Newtonsoft.Json;
using PokedexApp.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PokedexApp.Services.Request
{
    public class RequestService : IRequestService
    {
        readonly HttpClient httpClient;

        private List<Pokemon> _pokemons;
        public List<Pokemon> Pokemons
        {
            get { return _pokemons; }
            set { _pokemons = value; }
        }


        public RequestService()
        {
            httpClient = new HttpClient();
            Pokemons = new List<Pokemon>();
        }

        public async Task<List<Pokemon>> GetPokemons(decimal quantidade, decimal indice)
        {
            quantidade++;
            for (decimal i = indice; i < quantidade; i++)
            {
                Uri uri = new Uri($"https://pokeapi.co/api/v2/pokemon/{i}/");
                HttpResponseMessage response = await httpClient.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    var pokemon = JsonConvert.DeserializeObject<Pokemon>(content);
                    Pokemons.Add(pokemon);
                }
            }
            
            return Pokemons;
        }
    }
}
