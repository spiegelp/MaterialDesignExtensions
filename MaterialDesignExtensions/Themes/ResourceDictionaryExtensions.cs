using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

using MaterialDesignColors;
using MaterialDesignColors.ColorManipulation;
using MaterialDesignThemes.Wpf;

namespace MaterialDesignExtensions.Themes
{
    public static class ResourceDictionaryExtensions
    {
        private static readonly string MaterialDesignPrefix = "MaterialDesign";

        private static readonly Guid CurrentThemeKey = Guid.NewGuid();
        private static readonly Guid ThemeManagerKey = Guid.NewGuid();

        static ResourceDictionaryExtensions()
        {
            Type type = typeof(MaterialDesignThemes.Wpf.ResourceDictionaryExtensions);

            CurrentThemeKey = (Guid)type.GetProperty(nameof(CurrentThemeKey)).GetValue(null);
            ThemeManagerKey = (Guid)type.GetProperty(nameof(ThemeManagerKey)).GetValue(null);
        }

        /// <summary>
        /// Writes the specified theme into the resource dictionary.
        /// The implementation is based on MaterialDesignThemes.Wpf.ResourceDictionaryExtensions.SetTheme(this ResourceDictionary resourceDictionary, ITheme theme).
        /// </summary>
        /// <param name="resourceDictionary"></param>
        /// <param name="theme"></param>
        public static void SetExtendedTheme(this ResourceDictionary resourceDictionary, IExtendedTheme theme)
        {
            if (resourceDictionary == null)
            {
                throw new ArgumentNullException(nameof(resourceDictionary));
            }

            SetSolidColorBrush(resourceDictionary, "PrimaryHueLightBrush", theme.PrimaryLight.Color);
            SetSolidColorBrush(resourceDictionary, "PrimaryHueLightForegroundBrush", theme.PrimaryLight.ForegroundColor ?? theme.PrimaryLight.Color.ContrastingForegroundColor());
            SetSolidColorBrush(resourceDictionary, "PrimaryHueMidBrush", theme.PrimaryMid.Color);
            SetSolidColorBrush(resourceDictionary, "PrimaryHueMidForegroundBrush", theme.PrimaryMid.ForegroundColor ?? theme.PrimaryMid.Color.ContrastingForegroundColor());
            SetSolidColorBrush(resourceDictionary, "PrimaryHueDarkBrush", theme.PrimaryDark.Color);
            SetSolidColorBrush(resourceDictionary, "PrimaryHueDarkForegroundBrush", theme.PrimaryDark.ForegroundColor ?? theme.PrimaryDark.Color.ContrastingForegroundColor());

            SetSolidColorBrush(resourceDictionary, "SecondaryHueLightBrush", theme.SecondaryLight.Color);
            SetSolidColorBrush(resourceDictionary, "SecondaryHueLightForegroundBrush", theme.SecondaryLight.ForegroundColor ?? theme.SecondaryLight.Color.ContrastingForegroundColor());
            SetSolidColorBrush(resourceDictionary, "SecondaryHueMidBrush", theme.SecondaryMid.Color);
            SetSolidColorBrush(resourceDictionary, "SecondaryHueMidForegroundBrush", theme.SecondaryMid.ForegroundColor ?? theme.SecondaryMid.Color.ContrastingForegroundColor());
            SetSolidColorBrush(resourceDictionary, "SecondaryHueDarkBrush", theme.SecondaryDark.Color);
            SetSolidColorBrush(resourceDictionary, "SecondaryHueDarkForegroundBrush", theme.SecondaryDark.ForegroundColor ?? theme.SecondaryDark.Color.ContrastingForegroundColor());

            // these are here for backwards compatibility, and will be removed in a future version.
            SetSolidColorBrush(resourceDictionary, "SecondaryAccentBrush", theme.SecondaryMid.Color);
            SetSolidColorBrush(resourceDictionary, "SecondaryAccentForegroundBrush", theme.SecondaryMid.ForegroundColor ?? theme.SecondaryMid.Color.ContrastingForegroundColor());

            SetSolidColorBrush(resourceDictionary, "ValidationErrorBrush", theme.ValidationError);
            resourceDictionary["ValidationErrorColor"] = theme.ValidationError;

            GetColorProperties(theme.GetType(), true, false)
                .ForEach(property => SetSolidColorBrush(resourceDictionary, GetKey(property), (Color)property.GetValue(theme)));

            if (!(resourceDictionary.GetThemeManager() is ThemeManager themeManager))
            {
                resourceDictionary[ThemeManagerKey] = themeManager = new ThemeManager(resourceDictionary);
            }

            ITheme oldTheme = resourceDictionary.GetTheme();
            resourceDictionary[CurrentThemeKey] = theme;

            themeManager.OnThemeChange(oldTheme, theme);
        }

        public static IExtendedTheme GetExtendedTheme(this ResourceDictionary resourceDictionary)
        {
            if (resourceDictionary == null)
            {
                throw new ArgumentNullException(nameof(resourceDictionary));
            }

            if (resourceDictionary[CurrentThemeKey] is IExtendedTheme theme)
            {
                return theme;
            }

            Color secondaryMid = GetColor(resourceDictionary, "SecondaryHueMidBrush", "SecondaryAccentBrush");
            Color secondaryMidForeground = GetColor(resourceDictionary, "SecondaryHueMidForegroundBrush", "SecondaryAccentForegroundBrush");

            if (!TryGetColor(resourceDictionary, "SecondaryHueLightBrush", out Color secondaryLight))
            {
                secondaryLight = secondaryMid.Lighten();
            }

            if (!TryGetColor(resourceDictionary, "SecondaryHueLightForegroundBrush", out Color secondaryLightForeground))
            {
                secondaryLightForeground = secondaryLight.ContrastingForegroundColor();
            }

            if (!TryGetColor(resourceDictionary, "SecondaryHueDarkBrush", out Color secondaryDark))
            {
                secondaryDark = secondaryMid.Darken();
            }

            if (!TryGetColor(resourceDictionary, "SecondaryHueDarkForegroundBrush", out Color secondaryDarkForeground))
            {
                secondaryDarkForeground = secondaryDark.ContrastingForegroundColor();
            }

            ExtendedTheme newTheme = new ExtendedTheme
            {
                PrimaryLight = new ColorPair(GetColor(resourceDictionary, "PrimaryHueLightBrush"), GetColor(resourceDictionary, "PrimaryHueLightForegroundBrush")),
                PrimaryMid = new ColorPair(GetColor(resourceDictionary, "PrimaryHueMidBrush"), GetColor(resourceDictionary, "PrimaryHueMidForegroundBrush")),
                PrimaryDark = new ColorPair(GetColor(resourceDictionary, "PrimaryHueDarkBrush"), GetColor(resourceDictionary, "PrimaryHueDarkForegroundBrush")),

                SecondaryLight = new ColorPair(secondaryLight, secondaryLightForeground),
                SecondaryMid = new ColorPair(secondaryMid, secondaryMidForeground),
                SecondaryDark = new ColorPair(secondaryDark, secondaryDarkForeground),

                ValidationError = GetColor(resourceDictionary, "ValidationErrorBrush")
            };

            GetColorProperties(newTheme.GetType(), false, true)
                .ForEach(property => property.SetValue(newTheme, GetColor(resourceDictionary, GetKey(property))));

            return newTheme;
        }

        /// <summary>
        /// The implementation is based on MaterialDesignThemes.Wpf.ResourceDictionaryExtensions.SetSolidColorBrush(this ResourceDictionary sourceDictionary, string name, Color value).
        /// </summary>
        /// <param name="sourceDictionary"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        private static void SetSolidColorBrush(this ResourceDictionary sourceDictionary, string name, Color value)
        {
            if (sourceDictionary == null)
            {
                throw new ArgumentNullException(nameof(sourceDictionary));
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            sourceDictionary[name + "Color"] = value;

            if (sourceDictionary[name] is SolidColorBrush brush)
            {
                if (brush.Color == value)
                {
                    return;
                }

                if (!brush.IsFrozen)
                {
                    ColorAnimation animation = new ColorAnimation
                    {
                        From = brush.Color,
                        To = value,
                        Duration = new Duration(TimeSpan.FromMilliseconds(300))
                    };

                    brush.BeginAnimation(SolidColorBrush.ColorProperty, animation);

                    return;
                }
            }

            SolidColorBrush newBrush = new SolidColorBrush(value);
            newBrush.Freeze();
            sourceDictionary[name] = newBrush;
        }

        /// <summary>
        /// The implementation is based on MaterialDesignThemes.Wpf.ResourceDictionaryExtensions.GetColor(params string[] keys).
        /// </summary>
        /// <param name="resourceDictionary"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        private static Color GetColor(ResourceDictionary resourceDictionary, params string[] keys)
        {
            foreach (string key in keys)
            {
                if (TryGetColor(resourceDictionary, key, out Color color))
                {
                    return color;
                }
            }

            throw new InvalidOperationException($"Could not locate required resource with key(s) '{string.Join(", ", keys)}'");
        }

        /// <summary>
        /// The implementation is based on MaterialDesignThemes.Wpf.ResourceDictionaryExtensions.TryGetColor(string key, out Color color).
        /// </summary>
        /// <param name="resourceDictionary"></param>
        /// <param name="key"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        private static bool TryGetColor(ResourceDictionary resourceDictionary, string key, out Color color)
        {
            if (resourceDictionary[key] is SolidColorBrush brush)
            {
                color = brush.Color;

                return true;
            }
            else
            {
                color = default;

                return false;
            }
        }

        private static List<PropertyInfo> GetColorProperties(Type themeType, bool ceckCanRead, bool ceckCanWrite)
        {
            return themeType.GetProperties()
                .Where(property => (!ceckCanRead || (ceckCanRead && property.CanRead))
                                        && (!ceckCanWrite || (ceckCanWrite && property.CanWrite))
                                        && property.PropertyType == typeof(Color)
                                        && !property.Name.StartsWith("Primary")
                                        && !property.Name.StartsWith("Secondary")
                                        && property.Name != "ValidationError")
                .ToList();
        }

        private static string GetKey(PropertyInfo property)
        {
            string key = property.Name;

            if (!key.StartsWith(MaterialDesignPrefix))
            {
                key = MaterialDesignPrefix + key;
            }

            return key;
        }

        /// <summary>
        /// The implementation is based on MaterialDesignThemes.Wpf.ResourceDictionaryExtensions.ThemeManager.
        /// </summary>
        private class ThemeManager : IThemeManager
        {
            public event EventHandler<ThemeChangedEventArgs> ThemeChanged;

            private ResourceDictionary _resourceDictionary;

            public ThemeManager(ResourceDictionary resourceDictionary)
            {
                _resourceDictionary = resourceDictionary ?? throw new ArgumentNullException(nameof(resourceDictionary));
            }

            public void OnThemeChange(ITheme oldTheme, ITheme newTheme)
            {
                ThemeChanged?.Invoke(this, new ThemeChangedEventArgs(_resourceDictionary, oldTheme, newTheme));
            }
        }
    }
}
