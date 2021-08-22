using Acr.UserDialogs;
using PokedexApp.Enums;
using PokedexApp.Models;
using PokedexApp.Repositories.PokemonRepository;
using PokedexApp.Services.Request;
using PokedexApp.ThemeResources;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using static PokedexApp.ThemeResources.ThemeManager;

namespace PokedexApp.ViewModels
{
    public class HomePageViewModel : ViewModelBase
    {
        readonly IRequestService _requestService;
        readonly IPageDialogService _pageDialogService;
        readonly IPokemonRepository _pokemonRepository;

        private ObservableCollection<Pokemon> _pokemons;
        public ObservableCollection<Pokemon> Pokemons
        {
            get { return _pokemons; }
            set { SetProperty(ref _pokemons, value); }
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set { SetProperty(ref _isLoading, value); }
        }

        private int _indice;
        public int Indice
        {
            get { return _indice; }
            set { SetProperty(ref _indice, value); }
        }

        private int _indiceFinal;
        public int IndiceFinal
        {
            get { return _indiceFinal; }
            set { SetProperty(ref _indiceFinal, value); }
        }

        private int _count;
        public int Count
        {
            get { return _count; }
            set { SetProperty(ref _count, value); }
        }

        public ICommand MostrarMaisCommand { get; set; }
        public ICommand DetalhesCommand { get; set; }
        public ICommand IrParaPesquisaCommand { get; set; }
        public HomePageViewModel(
            IRequestService requestService,
            IPageDialogService pageDialogService,
            IPokemonRepository pokemonRepository,
            INavigationService navigationService)
            : base(navigationService)
        {
            _requestService = requestService;
            _pageDialogService = pageDialogService;
            _pokemonRepository = pokemonRepository;
            IsLoading = false;
            Pokemons = new ObservableCollection<Pokemon>();
            Indice = 1;
            IndiceFinal = 15;
            Count = 0;

            MostrarMaisCommand = new DelegateCommand(async () => await MostrarMaisCommandExecute());
            DetalhesCommand = new DelegateCommand<Pokemon>(async (pokemon) => await DetalhesCommandExecute(pokemon));
            IrParaPesquisaCommand = new DelegateCommand(async () => await IrParaPesquisaCommandExecute());
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            IsLoading = true;
            if (Pokemons.Count == 0)
            {
                await RecuperarPokemons();
            }
            IsLoading = false;
        }

        private async Task DetalhesCommandExecute(Pokemon pokemon)
        {
            await NavigationService.NavigateAsync($"PokemonDetailsPage?Id={pokemon.Id}");
        }

        private async Task RecuperarPokemons()
        {
            try
            {
                var pokemons = await _pokemonRepository.GetAllPokemons();
                if(pokemons != null && pokemons.Count > 0)
                {
                    Pokemons = new ObservableCollection<Pokemon>(pokemons);
                }
                else
                {
                    var pokes = await _requestService.GetPokemons(IndiceFinal, Indice);
                    if (pokes != null)
                    {
                        _pokemonRepository.SaveAllPokemon(pokes);
                        Pokemons = new ObservableCollection<Pokemon>(pokes);
                    }
                    else
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
            }
            catch (Exception ex)
            {

            }
        }

        private async Task<bool> MostrarMaisCommandExecute()
        {
            try
            {
                if(Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    if(Pokemons.Count > 0)
                    {
                        if(Count == 0)
                        {
                            Count++;
                            IsLoading = true;
                            Indice = Pokemons.Count + 1;
                            IndiceFinal = Pokemons.Count + 10;
                            var pokemons = await _requestService.GetPokemons(IndiceFinal, Indice);
                            if (pokemons != null && pokemons.Count > 0)
                            {
                                var res = _pokemonRepository.SaveAllPokemon(pokemons);
                                if (res == ResultadoExecucaoEnum.sucesso)
                                {
                                    foreach (var pokemon in pokemons.Where(x => x.Id > Pokemons.Count))
                                    {
                                        Pokemons.Add(pokemon);
                                    }
                                    IsLoading = false;
                                    await Task.Delay(1000);
                                    Count = 0;
                                    return true;
                                }
                                else
                                {
                                    await _pageDialogService.DisplayAlertAsync("Alerta", "Ocorreu um erro", "OK");
                                    IsLoading = false;
                                    Count = 0;
                                    return false;
                                }
                            }
                            IsLoading = false;
                            await _pageDialogService.DisplayAlertAsync("Alerta", "Não foi possível trazer mais pokemons!", "OK");
                            Count = 0;
                            return true;
                        }
                    }
                }
                else
                {
                    Count = 0;
                }
                return false;
            }
            catch (Exception ex)
            {
                IsLoading = false;
                Count = 0;
                await _pageDialogService.DisplayAlertAsync("Alerta", "Ocorreu um erro, tente novamente", "OK");
                return false;
            }
        }
        private async Task IrParaPesquisaCommandExecute()
        {
            await NavigationService.NavigateAsync("SearchPage");
        }
    }
}
