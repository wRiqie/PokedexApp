using PokedexApp.Models;
using PokedexApp.Repositories.PokemonRepository;
using PokedexApp.ThemeResources;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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

        private ObservableCollection<Types> _types;
        public ObservableCollection<Types> Types
        {
            get { return _types; }
            set { SetProperty(ref _types, value); }
        }

        private ObservableCollection<PokemonStats> _stats;
        public ObservableCollection<PokemonStats> Stats
        {
            get { return _stats; }
            set { SetProperty(ref _stats, value); }
        }

        private ObservableCollection<PokemonAbility> _abilities;
        public ObservableCollection<PokemonAbility> Abilities
        {
            get { return _abilities; }
            set { SetProperty(ref _abilities, value); }
        }

        private string _idFormatado;
        public string IdFormatado
        {
            get { return _idFormatado; }
            set { SetProperty(ref _idFormatado, value); }
        }

        private bool _overviewVisible;
        public bool OverviewVisible
        {
            get { return _overviewVisible; }
            set 
            { 
                SetProperty(ref _overviewVisible, value);
            }
        }

        private bool _statsVisible;
        public bool StatsVisible
        {
            get { return _statsVisible; }
            set { SetProperty(ref _statsVisible, value); }
        }

        private bool _abilityVisible;
        public bool AbilityVisible
        {
            get { return _abilityVisible; }
            set { SetProperty(ref _abilityVisible, value); }
        }

        public ICommand OverviewCommand { get; set; }
        public ICommand StatsCommand { get; set; }
        public ICommand AbilitiesCommand { get; set; }
        public PokemonDetailsPageViewModel(
            IPokemonRepository pokemonRepository,
            INavigationService navigationService)
            : base(navigationService)
        {
            _pokemonRepository = pokemonRepository;

            OverviewCommand = new DelegateCommand(async () => await OverviewCommandExecute());
            StatsCommand = new DelegateCommand(async () => await StatsCommandExecute());
            AbilitiesCommand = new DelegateCommand(async () => await AbilitiesCommandExecute());
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
            OverviewVisible = true;
            ThemeManager.LoadTheme();
        }

        private async Task GetPokeInfo(decimal id)
        {
            PokemonInfo = await _pokemonRepository.GetPokemon(id);
            Stats = new ObservableCollection<PokemonStats>(await _pokemonRepository.GetPokemonStats(id));
            Abilities = new ObservableCollection<PokemonAbility>(await _pokemonRepository.GetPokemonAbilities(id));
            Types = new ObservableCollection<Types>(await _pokemonRepository.GetPokemonTypes(id));
        }

        private async Task AbilitiesCommandExecute()
        {
            OverviewVisible = false;
            StatsVisible = false;
            AbilityVisible = true;
        }

        private async Task StatsCommandExecute()
        {
            OverviewVisible = false;
            StatsVisible = true;
            AbilityVisible = false;
        }

        private async Task OverviewCommandExecute()
        {
            OverviewVisible = true;
            StatsVisible = false;
            AbilityVisible = false;
        }
    }
}
