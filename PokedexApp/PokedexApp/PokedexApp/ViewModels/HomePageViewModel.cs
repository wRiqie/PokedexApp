using PokedexApp.Models;
using PokedexApp.Services.Request;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
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


        public HomePageViewModel(
            IRequestService requestService,
            IPageDialogService pageDialogService,
            INavigationService navigationService) 
            : base(navigationService)
        {
            _requestService = requestService;
            _pageDialogService = pageDialogService;
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if(Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                var poke = await _requestService.GetPokemons(5, 1);
                Pokemons = new ObservableCollection<Pokemon>(poke);
            }
            else
            {
                await _pageDialogService.DisplayAlertAsync("Alerta", "Você precisa estar conectado a internet", "OK");
            }
            
        }
    }
}
