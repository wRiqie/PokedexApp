using Plugin.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PokedexApp.ThemeResources
{
    public class ThemeManager
    {
        /// <summary>
        /// Defines the supported themes for the sample app
        /// </summary>
        public enum Themes
        {
            Bulbasaur,
            MeowTwo,
            Pikachu
            
        }

        string name;

        /// <summary>
        /// Changes the theme of the app.
        /// Add additional switch cases for more themes you add to the app.
        /// This also updates the local key storage value for the selected theme.
        /// </summary>
        /// <param name="theme"></param>
        public static void ChangeTheme(Themes theme)
        {
            var mergedDictionaries = Application.Current.Resources.MergedDictionaries;
            if (mergedDictionaries != null)
            {
                mergedDictionaries.Clear();

                //Update local key value with the new theme you select.
                CrossSettings.Current.AddOrUpdateValue("SelectedTheme", (int)theme);

                switch (theme)
                {
                    case Themes.Bulbasaur:
                        {
                            mergedDictionaries.Add(new BulbasaurTheme());
                            break;
                        }
                    case Themes.MeowTwo:
                        {
                            mergedDictionaries.Add(new MewTwoTheme());
                            break;
                        }
                    case Themes.Pikachu:
                        {
                            mergedDictionaries.Add(new PikachuTheme());
                            break;
                        }
                    default:
                        mergedDictionaries.Add(new MewTwoTheme());
                        break;
                }
            }
        }

        /// <summary>
        /// Reads current theme id from the local storage and loads it.
        /// </summary>
        public static void LoadTheme()
        {
            Themes currentTheme = CurrentTheme();
            ChangeTheme(currentTheme);
        }

        /// <summary>
        /// Gives current/last selected theme from the local storage.
        /// </summary>
        /// <returns></returns>
        public static Themes CurrentTheme()
        {
            return (Themes)CrossSettings.Current.GetValueOrDefault("SelectedTheme", (int)Themes.MeowTwo);
        }
    }
}
