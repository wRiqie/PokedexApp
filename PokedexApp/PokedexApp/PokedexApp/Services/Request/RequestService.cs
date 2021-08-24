using Newtonsoft.Json;
using PokedexApp.Models;
using PokedexApp.Repositories.PokemonRepository;
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
        readonly IPokemonRepository _pokemonRepository;

        private List<Pokemon> _pokemons;
        public List<Pokemon> Pokemons
        {
            get { return _pokemons; }
            set { _pokemons = value; }
        }


        public RequestService(
            IPokemonRepository pokemonRepository,
            IPageDialogService pageDialogService)
        {
            _pageDialogService = pageDialogService;
            _pokemonRepository = pokemonRepository;
            httpClient = new HttpClient();
            Pokemons = new List<Pokemon>();
        }

        public async Task<List<Pokemon>> GetPokemons(decimal quantidade, decimal indice)
        {
            Pokemons.Clear();
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
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

                        // Add Pokemon photo
                        var responseImg = await httpClient.GetAsync(pokemon.Sprites.front_default);
                        byte[] PokePhoto = null;
                        if (responseImg.IsSuccessStatusCode)
                        {
                            PokePhoto = await responseImg.Content.ReadAsByteArrayAsync();
                        }
                        if (PokePhoto != null)
                        {
                            pokemon.TypeName = pokemon.Types[0].Type.Name; // TODO - Ajustar para pokemon receber todos os tipos
                            pokemon.Photo = PokePhoto;
                        }
                        // Save type
                        foreach (var type in pokemon.Types)
                        {
                            type.PokemonId = pokemon.Id;
                            type.TypeName = type.Type.Name;
                            _pokemonRepository.SavePokemonType(type);
                        }
                        foreach (var stat in pokemon.Stats)
                        {
                            stat.StatName = stat.Stat.Name;
                            stat.PokemonId = pokemon.Id;
                            _pokemonRepository.SavePokemonStat(stat);
                        }
                        foreach (var ability in pokemon.Abilities)
                        {
                            ability.AbilityName = ability.Ability.Name;
                            ability.PokemonId = pokemon.Id;
                            _pokemonRepository.SavePokemonAbility(ability);
                        }
                        Pokemons.Add(pokemon);
                    }
                }
            }
            return Pokemons;
        }
    }
}
