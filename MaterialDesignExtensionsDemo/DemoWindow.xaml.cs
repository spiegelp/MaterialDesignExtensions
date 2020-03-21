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
using System.Windows.Shapes;

using MaterialDesignExtensions.Controls;

namespace MaterialDesignExtensionsDemo
{
    public partial class DemoWindow : MaterialWindow
    {
        public DemoWindow()
        {
            InitializeComponent();
        }

        private void CloseButtonClickHandler(object sender, RoutedEventArgs args)
        {
            Close();
        }
    }
}
