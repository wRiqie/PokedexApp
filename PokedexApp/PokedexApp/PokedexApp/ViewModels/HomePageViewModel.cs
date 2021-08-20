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

        private bool _moreEnable;
        public bool MoreEnable
        {
            get { return _moreEnable; }
            set { SetProperty(ref _moreEnable, value); }
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

        public ICommand MostrarMaisCommand { get; set; }
        public ICommand BulbaCommand { get; set; }
        public ICommand MeowCommand { get; set; }
        public ICommand PikachuCommand { get; set; }
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
            MoreEnable = true;
            Pokemons = new ObservableCollection<Pokemon>();
            Indice = 1;
            IndiceFinal = 50;

            MostrarMaisCommand = new DelegateCommand(async () => await MostrarMaisCommandExecute());
            BulbaCommand = new DelegateCommand(async () => await BulbaCommandExecute());
            MeowCommand = new DelegateCommand(async () => await MeowCommandExecute());
            PikachuCommand = new DelegateCommand(async () => await PikachuCommandExecute());
        }

        private async Task PikachuCommandExecute()
        {
            ThemeManager.ChangeTheme(Themes.Pikachu);
        }

        private async Task MeowCommandExecute()
        {
            ThemeManager.ChangeTheme(Themes.MeowTwo);
        }

        private async Task BulbaCommandExecute()
        {
            ThemeManager.ChangeTheme(ThemeManager.Themes.Bulbasaur);
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            IsLoading = true;
            MoreEnable = false;
            if (Pokemons.Count == 0)
            {
                await RecuperarPokemons();
            }
            IsLoading = false;
            MoreEnable = true;
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
                IsLoading = true;
                MoreEnable = false;
                Indice = Pokemons.Count + 1;
                IndiceFinal = Pokemons.Count + 50;
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
                        MoreEnable = true;
                        return true;
                    }
                    else 
                    {
                        await _pageDialogService.DisplayAlertAsync("Alerta", "Ocorreu um erro", "OK");
                        IsLoading = false;
                        MoreEnable = true;
                        return false;
                    }
                }
                IsLoading = false;
                MoreEnable = true;
                await _pageDialogService.DisplayAlertAsync("Alerta", "Não foi possível trazer mais pokemons!", "OK");
                return true;
            }
            catch (Exception ex)
            {
                IsLoading = false;
                MoreEnable = true;
                await _pageDialogService.DisplayAlertAsync("Alerta", "Ocorreu um erro, tente novamente", "OK");
                return false;
            }
        }
    }
}
