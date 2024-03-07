using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

using MaterialDesignThemes.Wpf;

namespace MaterialDesignExtensions.Converters
{
    /// <summary>
    /// Converts a <see cref="PackIcon" /> to a special color for displaying it.
    /// </summary>
    public class FileSystemInfoPackIconColorConverter : IValueConverter
    {
        private IDictionary<PackIconKind, Brush> m_brushesForPackIcon;

        /// <summary>
        /// Creates a new <see cref="FileSystemInfoPackIconColorConverter" />.
        /// </summary>
        public FileSystemInfoPackIconColorConverter()
        {
            m_brushesForPackIcon = new Dictionary<PackIconKind, Brush>()
            {
                // code
                [PackIconKind.LanguageCsharp] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1E9D24")),
                [PackIconKind.LanguageJavascript] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFEB3B")),
                [PackIconKind.LanguagePhp] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1976D2")),

                // common document types
                [PackIconKind.FileWord] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2196F3")),
                [PackIconKind.FileExcel] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#186C3F")),
                [PackIconKind.FilePowerpoint] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF9800")),
                [PackIconKind.FilePdfBox] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F44336"))
            };
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Brush brush = null;

            if (value is PackIconKind packIconKind)
            {
                m_brushesForPackIcon.TryGetValue(packIconKind, out brush);
            }

            if (brush == null)
            {
                brush = System.Windows.Application.Current.TryFindResource("MaterialDesignDivider") as Brush;
            }

            return brush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
