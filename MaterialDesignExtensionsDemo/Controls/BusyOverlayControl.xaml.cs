using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class BusyOverlayControl : UserControl
    {
        public BusyOverlayControl()
        {
            InitializeComponent();
        }

        private async void BeBusyButtonClickHandler(object sender, RoutedEventArgs args)
        {
            await ((BusyOverlayViewModel)DataContext).BeBusyAsync();
        }
    }
}
