using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using MaterialDesignExtensionsDemo.ViewModel;

namespace MaterialDesignExtensionsDemo.Controls
{
    public partial class WindowStyleControl : UserControl
    {
        public WindowStyleControl()
        {
            InitializeComponent();
        }

        private void ShowWindowButtonClickHandler(object sender, RoutedEventArgs args)
        {
            DemoWindow window = new DemoWindow { WindowStyle = ((WindowStyleViewModel)DataContext).SelectedWindowStyle };
            window.ShowDialog();
        }
    }
}
