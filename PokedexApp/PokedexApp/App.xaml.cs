using Plugin.Settings;
using Plugin.Settings.Abstractions;
using PokedexApp.Extenders;
using PokedexApp.Popups;
using PokedexApp.ThemeResources;
using PokedexApp.ViewModels;
using PokedexApp.Views;
using Prism;
using Prism.DryIoc;
using Prism.Ioc;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static PokedexApp.ThemeResources.ThemeManager;

namespace PokedexApp
{
    public partial class App : PrismApplication
    {
        private static ISettings AppSettings =>
            CrossSettings.Current;

        public App()
            : this(null)
        {

        }

        public App(IPlatformInitializer initializer)
            : this(initializer, true)
        {

        }

        public App(IPlatformInitializer initializer, bool setFormsDependencyResolver)
            : base(initializer, setFormsDependencyResolver)
        {

        }

        protected override async void OnInitialized()
        {
            InitializeComponent();
            //var theme = (Themes)CrossSettings.Current.GetValueOrDefault("SelectedTheme", (int)Themes.MeowTwo);
            
            await NavigationService.NavigateAsync("NavigationPage/HomePage");
            //ThemeManager.ChangeTheme(theme);
            ThemeManager.LoadTheme();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.ResolveServices();
            containerRegistry.ResolveRepository();
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<HomePage, HomePageViewModel>();
        }
    }
}
