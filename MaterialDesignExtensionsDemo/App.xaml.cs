using System.Globalization;
using System.Threading;
using System.Windows;

namespace MaterialDesignExtensionsDemo
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            // Change CurrentUICulture to uz-Latn-UZ
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("uz-Latn-UZ");
        }
    }
}
