using System.Windows;
using System.Windows.Controls;
using MaterialDesignExtensions.Controls;

using MaterialDesignExtensionsDemo.ViewModel;

namespace MaterialDesignExtensionsDemo.Controls
{
    public partial class SaveFileControlControl : UserControl
    {
        public SaveFileControlControl()
        {
            InitializeComponent();
        }

        private void SaveFileControl_FileSelected(object sender, RoutedEventArgs args)
        {
            if (args is FileSelectedEventArgs eventArgs && DataContext is SaveFileControlViewModel viewModel)
            {
                viewModel.SelectedAction = "Selected file: " + eventArgs.File;
            }
        }

        private void SaveFileControl_Cancel(object sender, RoutedEventArgs args)
        {
            if (DataContext is SaveFileControlViewModel viewModel)
            {
                viewModel.SelectedAction = "Cancel save file";
            }
        }
    }
}
