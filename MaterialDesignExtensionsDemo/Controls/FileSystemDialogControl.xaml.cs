using System;
using System.Collections.Generic;
using System.IO;
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
    public partial class FileSystemDialogControl : UserControl
    {
        public FileSystemDialogControl()
        {
            InitializeComponent();
        }

        private async void OpenDirectoryDialogButtonClickHandler(object sender, RoutedEventArgs args)
        {
            OpenDirectoryDialogResult result = await OpenDirectoryDialog.ShowDialogAsync(MainWindow.DialogHostName, 600, 400);

            if (DataContext is FileSystemDialogViewModel viewModel)
            {
                if (!result.Canceled)
                {
                    viewModel.SelectedAction = "Selected directory: " + result.DirectoryInfo.FullName;
                }
                else
                {
                    viewModel.SelectedAction = "Cancel open directory";
                }
            }
        }

        private async void OpenFileDialogButtonClickHandler(object sender, RoutedEventArgs args)
        {
            OpenFileDialogResult result = await OpenFileDialog.ShowDialogAsync(MainWindow.DialogHostName, 600, 400);

            if (DataContext is FileSystemDialogViewModel viewModel)
            {
                if (!result.Canceled)
                {
                    viewModel.SelectedAction = "Selected file: " + result.FileInfo.FullName;
                }
                else
                {
                    viewModel.SelectedAction = "Cancel open file";
                }
            }
        }

        private async void SaveFileDialogButtonClickHandler(object sender, RoutedEventArgs args)
        {
            SaveFileDialogResult result = await SaveFileDialog.ShowDialogAsync(MainWindow.DialogHostName, 600, 400);

            if (DataContext is FileSystemDialogViewModel viewModel)
            {
                if (!result.Canceled)
                {
                    viewModel.SelectedAction = "Selected file: " + result.FileInfo.FullName;
                }
                else
                {
                    viewModel.SelectedAction = "Cancel save file";
                }
            }
        }
    }
}
