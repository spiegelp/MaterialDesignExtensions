using System.Text;
using System.Windows;
using System.Windows.Controls;
using MaterialDesignExtensions.Controls;

using MaterialDesignExtensionsDemo.ViewModel;

namespace MaterialDesignExtensionsDemo.Controls
{
    public partial class OpenMultipleFilesControlControl : UserControl
    {
        public OpenMultipleFilesControlControl()
        {
            InitializeComponent();
        }

        private void OpenMultipleFilesControl_DirectoriesSelected(object sender, RoutedEventArgs args)
        {
            if (args is FilesSelectedEventArgs eventArgs && DataContext is OpenMultipleFilesControlViewModel viewModel)
            {
                StringBuilder sb = new StringBuilder("Selected files: ");
                eventArgs.Files.ForEach(file => sb.Append($"{file}; "));

                viewModel.SelectedAction = sb.ToString();
            }
        }

        private void OpenMultipleFilesControl_Cancel(object sender, RoutedEventArgs args)
        {
            if (DataContext is OpenMultipleFilesControlViewModel viewModel)
            {
                viewModel.SelectedAction = "Cancel open files";
            }
        }
    }
}
