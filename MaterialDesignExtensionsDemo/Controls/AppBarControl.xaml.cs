using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
