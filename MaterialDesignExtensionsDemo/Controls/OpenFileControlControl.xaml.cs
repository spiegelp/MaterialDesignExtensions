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

using MaterialDesignExtensions.Controls;

using MaterialDesignExtensionsDemo.ViewModel;

namespace MaterialDesignExtensionsDemo.Controls
{
    public partial class OpenFileControlControl : UserControl
    {
        public OpenFileControlControl()
        {
            InitializeComponent();
        }

        private void OpenFileControl_FileSelected(object sender, RoutedEventArgs args)
        {
            if (args is FileSelectedEventArgs eventArgs && DataContext is OpenFileControlViewModel viewModel)
            {
                viewModel.SelectedAction = "Selected file: " + eventArgs.File;
            }
        }

        private void OpenFileControl_Cancel(object sender, RoutedEventArgs args)
        {
            if (DataContext is OpenFileControlViewModel viewModel)
            {
                viewModel.SelectedAction = "Cancel open file";
            }
        }
    }
}
