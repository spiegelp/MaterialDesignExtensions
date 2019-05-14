using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace MaterialDesignExtensions.Themes
{
    /// <summary>
    /// Helper class for themes based on MaterialDesignThemes.Wpf.PaletteHelper.
    /// </summary>
    public class PaletteHelper : MaterialDesignThemes.Wpf.PaletteHelper
    {
        /// <summary>
        /// Creates a new <see cref="PaletteHelper" />.
        /// </summary>
        public PaletteHelper() : base() { }

        /// <summary>
        /// Replaces the current theme with the light or dark theme. The implementation is based on MaterialDesignThemes.Wpf.PaletteHelper.SetLightDark(bool isDark).
        /// </summary>
        /// <param name="isDark"></param>
        public override void SetLightDark(bool isDark)
        {
            // this method is a copy of MaterialDesignThemes.Wpf.PaletteHelper.SetLightDark(bool isDark) with changed resource names/strings

            // MaterialDesignExtensions
            ResourceDictionary oldThemeResourceDictionary = Application.Current.Resources.MergedDictionaries
                .Where(resourceDictionary => resourceDictionary != null && resourceDictionary.Source != null)
                .SingleOrDefault(resourceDictionary => Regex.Match(resourceDictionary.Source.OriginalString, @"(\/MaterialDesignExtensions;component\/Themes\/MaterialDesign)((Light)|(Dark))Theme\.").Success);

            if (oldThemeResourceDictionary == null)
            {
                throw new ApplicationException($"Unable to find light or dark theme in application resources.");
            }

            string newThemeSource = $"pack://application:,,,/MaterialDesignExtensions;component/Themes/MaterialDesign{(isDark ? "Dark" : "Light")}Theme.xaml";
            ResourceDictionary newThemeResourceDictionary = new ResourceDictionary() { Source = new Uri(newThemeSource) };

            Application.Current.Resources.MergedDictionaries.Remove(oldThemeResourceDictionary);
            Application.Current.Resources.MergedDictionaries.Add(newThemeResourceDictionary);

            // MahApps
            ResourceDictionary oldMahAppsResourceDictionary = Application.Current.Resources.MergedDictionaries
                .Where(resourceDictionary => resourceDictionary != null && resourceDictionary.Source != null)
                .SingleOrDefault(resourceDictionary => Regex.Match(resourceDictionary.Source.OriginalString, @"(\/MahApps.Metro;component\/Styles\/Accents\/)((BaseLight)|(BaseDark))").Success);

            if (oldMahAppsResourceDictionary != null)
            {
                newThemeSource = $"pack://application:,,,/MahApps.Metro;component/Styles/Accents/{(isDark ? "BaseDark" : "BaseLight")}.xaml";
                var newMahAppsResourceDictionary = new ResourceDictionary { Source = new Uri(newThemeSource) };

                Application.Current.Resources.MergedDictionaries.Remove(oldMahAppsResourceDictionary);
                Application.Current.Resources.MergedDictionaries.Add(newMahAppsResourceDictionary);
            }
        }

        /// <summary>
        /// Switches the theme to the respective other theme (light or dark).
        /// </summary>
        public void SwitchTheme()
        {
            SetLightDark(!IsLightTheme());
        }

        /// <summary>
        /// Is the light theme applied?
        /// </summary>
        /// <returns></returns>
        public bool IsLightTheme()
        {
            ResourceDictionary themeResourceDictionary = Application.Current.Resources.MergedDictionaries
                .Where(resourceDictionary => resourceDictionary != null && resourceDictionary.Source != null)
                .SingleOrDefault(resourceDictionary => Regex.Match(resourceDictionary.Source.OriginalString, @"(\/MaterialDesignExtensions;component\/Themes\/MaterialDesign)((Light)|(Dark))Theme\.").Success);

            if (themeResourceDictionary == null)
            {
                throw new ApplicationException("Unable to find light or dark theme in application resources.");
            }

            return themeResourceDictionary.Source.OriginalString.Contains("MaterialDesignLightTheme");
        }

        /// <summary>
        /// Is the dark theme applied?
        /// </summary>
        /// <returns></returns>
        public bool IsDarkTheme()
        {
            return !IsLightTheme();
        }
    }
}
