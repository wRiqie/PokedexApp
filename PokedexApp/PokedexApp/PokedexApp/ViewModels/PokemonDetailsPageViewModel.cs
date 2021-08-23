using PokedexApp.Models;
using PokedexApp.Repositories.PokemonRepository;
using PokedexApp.ThemeResources;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace PokedexApp.ViewModels
{
    public class PokemonDetailsPageViewModel : ViewModelBase
    {
        readonly IPokemonRepository _pokemonRepository;

        private Pokemon _pokemonInfo;
        public Pokemon PokemonInfo
        {
            get { return _pokemonInfo; }
            set { SetProperty(ref _pokemonInfo, value); }
        }

        private List<TypeDescription> _types;
        public List<TypeDescription> Types
        {
            get { return _types; }
            set { SetProperty(ref _types, value); }
        }

        private string _idFormatado;
        public string IdFormatado
        {
            get { return _idFormatado; }
            set { SetProperty(ref _idFormatado, value); }
        }

        private byte[] _foto;
        public byte[] Foto
        {
            get { return _foto; }
            set { SetProperty(ref _foto, value); }
        }


        public PokemonDetailsPageViewModel(
            IPokemonRepository pokemonRepository,
            INavigationService navigationService)
            : base(navigationService)
        {
            _pokemonRepository = pokemonRepository;
            Types = new List<TypeDescription>();
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            var id = parameters["Id"].ToString();
            _pokemonInfo = new Pokemon();

            var idNumber = Convert.ToInt32(id);
            _pokemonInfo.Id = idNumber;
            IdFormatado = string.Format("{0:000}", idNumber);
            await GetPokeInfo(_pokemonInfo.Id);
            ThemeManager.LoadTheme();
        }

        private async Task GetPokeInfo(decimal id)
        {
            PokemonInfo = await _pokemonRepository.GetPokemon(id);
            PokemonInfo.Stats = await _pokemonRepository.GetPokemonStats(id);
            PokemonInfo.Abilities = await _pokemonRepository.GetPokemonAbilities(id);
            Types = await _pokemonRepository.GetPokemonTypes(id);
        }
    }
}
