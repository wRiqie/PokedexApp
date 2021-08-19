using Acr.UserDialogs;
using PokedexApp.Models;
using PokedexApp.Services.Request;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;

namespace PokedexApp.ViewModels
{
    public class HomePageViewModel : ViewModelBase
    {
        readonly IRequestService _requestService;
        readonly IPageDialogService _pageDialogService;

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

        public ICommand MostrarMaisCommand { get; set; }
        public HomePageViewModel(
            IRequestService requestService,
            IPageDialogService pageDialogService,
            INavigationService navigationService)
            : base(navigationService)
        {
            _requestService = requestService;
            _pageDialogService = pageDialogService;
            IsLoading = false;

            MostrarMaisCommand = new DelegateCommand(async () => await MostrarMaisCommandExecute());
        }
        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            IsLoading = true;
            _indice = 1;
            _indiceFinal = 5;
            var poke = await _requestService.GetPokemons(IndiceFinal, Indice);
            if(poke != null && poke.Count > 0)
            {
                Pokemons = new ObservableCollection<Pokemon>(poke);
            }
            IsLoading = false;
        }

        private async Task<bool> MostrarMaisCommandExecute()
        {
            try
            {
                IsLoading = true;
                Indice = Indice + 5;
                IndiceFinal = IndiceFinal + 5;
                var pokemons = await _requestService.GetPokemons(IndiceFinal, Indice);
                if (pokemons != null && pokemons.Count > 0)
                {
                    foreach (var pokemon in pokemons)
                    {
                        Pokemons.Add(pokemon);
                    }
                    IsLoading = false;
                    return true;
                }
                IsLoading = false;
                await _pageDialogService.DisplayAlertAsync("Alerta", "Não foi possível trazer mais pokemons!", "OK");
                return true;
            }
            catch (Exception ex)
            {
                IsLoading = false;
                Indice = Indice - 5;
                IndiceFinal = IndiceFinal - 5;
                await _pageDialogService.DisplayAlertAsync("Alerta", "Ocorreu um erro, tente novamente", "OK");
                return false;
            }
        }
    }
}
