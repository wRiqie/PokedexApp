using PokedexApp.ThemeResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PokedexApp.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ThemePopup : Popup
    {

        public ThemePopup()
        {
            InitializeComponent();
            BindingContext = this;
            var theme = ThemeManager.CurrentTheme();
            if(theme == ThemeManager.Themes.Pikachu)
            {
                bulbaBackground.IsVisible = false;
                meowBackground.IsVisible = false;
                piBackground.IsVisible = true;
                bulbasaur.IsVisible = false;
                mewtwo.IsVisible = false;
                pikachu.IsVisible = true;
            }
            else if(theme == ThemeManager.Themes.MeowTwo)
            {
                bulbaBackground.IsVisible = false;
                meowBackground.IsVisible = true;
                piBackground.IsVisible = false;
                bulbasaur.IsVisible = false;
                mewtwo.IsVisible = true;
                pikachu.IsVisible = false;
            }
            else
            {
                bulbaBackground.IsVisible = true;
                meowBackground.IsVisible = false;
                piBackground.IsVisible = false;
                bulbasaur.IsVisible = true;
                mewtwo.IsVisible = false;
                pikachu.IsVisible = false;
            }
        }

        private void BulbasaurTheme(object sender, EventArgs e)
        {
            bulbaBackground.IsVisible = true;
            meowBackground.IsVisible = false;
            piBackground.IsVisible = false;
            bulbasaur.IsVisible = true;
            mewtwo.IsVisible = false;
            pikachu.IsVisible = false;
            ThemeManager.ChangeTheme(ThemeManager.Themes.Bulbasaur);
        }

        private void MewTwoTheme(object sender, EventArgs e)
        {
            bulbaBackground.IsVisible = false;
            meowBackground.IsVisible = true;
            piBackground.IsVisible = false;
            bulbasaur.IsVisible = false;
            mewtwo.IsVisible = true;
            pikachu.IsVisible = false;
            ThemeManager.ChangeTheme(ThemeManager.Themes.MeowTwo);
        }

        private void PikachuTheme(object sender, EventArgs e)
        {
            bulbaBackground.IsVisible = false;
            meowBackground.IsVisible = false;
            piBackground.IsVisible = true;
            bulbasaur.IsVisible = false;
            mewtwo.IsVisible = false;
            pikachu.IsVisible = true;
            ThemeManager.ChangeTheme(ThemeManager.Themes.Pikachu);
        }
    }
}