using System.Globalization;
using System.Threading;
using System.Windows;

namespace MaterialDesignExtensionsDemo
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs args)
        {
            // change the culture to test localized resource files for display strings
            /*Thread.CurrentThread.CurrentCulture = new CultureInfo("en-us");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-us");*/
        }
    }
}
