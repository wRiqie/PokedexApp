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

        private Pokemon _pokemon;
        public Pokemon Pokemon
        {
            get { return _pokemon; }
            set { SetProperty(ref _pokemon, value); }
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
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            var id = parameters["Id"].ToString();
            _pokemon = new Pokemon();

            var idNumber = Convert.ToInt32(id);
            _pokemon.Id = idNumber;
            IdFormatado = string.Format("{0:000}", idNumber);
            await GetPokeInfo(_pokemon.Id);
            ThemeManager.LoadTheme();
        }

        private async Task GetPokeInfo(decimal id)
        {
            Pokemon = await _pokemonRepository.GetPokemon(id);
        }
    }
}
