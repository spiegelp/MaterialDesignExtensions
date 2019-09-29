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
    public partial class OpenMultipleDirectoriesControlControl : UserControl
    {
        public OpenMultipleDirectoriesControlControl()
        {
            InitializeComponent();
        }

        private void OpenMultipleDirectoriesControl_DirectoriesSelected(object sender, RoutedEventArgs args)
        {
            if (args is DirectoriesSelectedEventArgs eventArgs && DataContext is OpenMultipleDirectoriesControlViewModel viewModel)
            {
                StringBuilder sb = new StringBuilder("Selected directories: ");
                eventArgs.Directories.ForEach(directory => sb.Append($"{directory}; "));

                viewModel.SelectedAction = sb.ToString();
            }
        }

        private void OpenMultipleDirectoriesControl_Cancel(object sender, RoutedEventArgs args)
        {
            if (DataContext is OpenMultipleDirectoriesControlViewModel viewModel)
            {
                viewModel.SelectedAction = "Cancel open directories";
            }
        }
    }
}
