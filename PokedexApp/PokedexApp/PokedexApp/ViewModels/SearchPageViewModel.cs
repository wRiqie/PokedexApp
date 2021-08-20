using Acr.UserDialogs;
using PokedexApp.Models;
using PokedexApp.Repositories.PokemonRepository;
using PokedexApp.ThemeResources;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PokedexApp.ViewModels
{
    public class SearchPageViewModel : ViewModelBase
    {
        readonly IPokemonRepository _pokemonRepository;

        private bool _detalhado;
        public bool Detalhado
        {
            get { return _detalhado; }
            set { SetProperty(ref _detalhado, value); }
        }

        private string _palavraChave;
        public string PalavraChave
        {
            get { return _palavraChave; }
            set { SetProperty(ref _palavraChave, value); }
        }

        private ObservableCollection<Pokemon> _pokemonsFiltrados;
        public ObservableCollection<Pokemon> PokemonsFiltrados
        {
            get { return _pokemonsFiltrados; }
            set { SetProperty(ref _pokemonsFiltrados, value); }
        }

        public ICommand PesquisarCommand { get; private set; }
        public ICommand DetalhesCommand { get; set; }
        public SearchPageViewModel(
            IPokemonRepository pokemonRepository,
            INavigationService navigationService)
            : base(navigationService)
        {
            _pokemonRepository = pokemonRepository;
            PokemonsFiltrados = new ObservableCollection<Pokemon>();
            PesquisarCommand = new DelegateCommand(async () => await PesquisarCommandExecute());
            DetalhesCommand = new DelegateCommand<Pokemon>(async (pokemon) => await DetalhesCommandExecute(pokemon));
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            ThemeManager.LoadTheme();
        }

        private async Task PesquisarCommandExecute()
        {
            try
            {
                var pokemons = await _pokemonRepository.GetAllPokemons();
                var pokemonsFiltro = pokemons.Where(x => x.Name.ToLower().Contains(PalavraChave.ToLower()) 
                || x.TypeName.ToLower().Contains(PalavraChave.ToLower())).ToList();
                PokemonsFiltrados = new ObservableCollection<Pokemon>(pokemonsFiltro);
            }
            catch (Exception ex)
            {
                var cfg = new ToastConfig("Vish, não foi possível encontrar pokemons...")
                {
                    MessageTextColor = System.Drawing.Color.FromArgb(145, 145, 145),
                    BackgroundColor = System.Drawing.Color.FromArgb(50, 50, 50),
                    Position = ToastPosition.Bottom
                };
                UserDialogs.Instance.Toast(cfg);
            }
        }

        private async Task DetalhesCommandExecute(Pokemon pokemon)
        {
            await NavigationService.NavigateAsync($"PokemonDetailsPage?Id={pokemon.Id}");
        }
    }
}
