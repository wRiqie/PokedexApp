using Newtonsoft.Json;
using PokedexApp.Models;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace PokedexApp.Services.Request
{
    public class RequestService : IRequestService
    {
        readonly HttpClient httpClient;
        readonly IPageDialogService _pageDialogService;

        private List<Pokemon> _pokemons;
        public List<Pokemon> Pokemons
        {
            get { return _pokemons; }
            set { _pokemons = value; }
        }


        public RequestService(
            IPageDialogService pageDialogService)
        {
            httpClient = new HttpClient();
            _pageDialogService = pageDialogService;
            Pokemons = new List<Pokemon>();
        }

        public async Task<List<Pokemon>> GetPokemons(decimal quantidade, decimal indice)
        {
            Pokemons.Clear();
            if(Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await _pageDialogService.DisplayAlertAsync("Alerta", "É necessário estar conectado a internet", "OK");
                return null;
            }
            else
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
                        var responseImg = await httpClient.GetAsync(pokemon.Sprites.front_default);
                        byte[] PokePhoto = null;
                        if (responseImg.IsSuccessStatusCode)
                        {
                            PokePhoto = await responseImg.Content.ReadAsByteArrayAsync();
                        }
                        if(PokePhoto != null)
                        {
                            pokemon.TypeName = pokemon.Types[0].Type.Name;
                            pokemon.Photo = PokePhoto;
                        }
                        Pokemons.Add(pokemon);
                    }
                }
            }
            return Pokemons;
        }
    }
}
