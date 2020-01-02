using System.Windows;
using System.Windows.Controls;

namespace MaterialDesignExtensionsDemo.Controls
{
    public partial class AppBarControl : UserControl
    {
        public AppBarControl()
        {
            InitializeComponent();
        }

        private void BackButtonClickHandler(object sender, RoutedEventArgs args)
        {
            // do something after clicking the back button
            //     or use the AppBar.BackCommand to get notified about a click on the back button
        }
    }
}
