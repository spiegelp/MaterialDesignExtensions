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
    public partial class OpenDirectoryControlControl : UserControl
    {
        public OpenDirectoryControlControl()
        {
            InitializeComponent();
        }

        private void OpenDirectoryControl_DirectorySelected(object sender, RoutedEventArgs args)
        {
            if (args is DirectorySelectedEventArgs eventArgs && DataContext is OpenDirectoryControlViewModel viewModel)
            {
                viewModel.SelectedAction = "Selected directory: " + eventArgs.Directory;
            }
        }

        private void OpenDirectoryControl_Cancel(object sender, RoutedEventArgs args)
        {
            if (DataContext is OpenDirectoryControlViewModel viewModel)
            {
                viewModel.SelectedAction = "Cancel open directory";
            }
        }
    }
}
