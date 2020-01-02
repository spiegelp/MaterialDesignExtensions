using System.Windows;
using System.Windows.Controls;
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
