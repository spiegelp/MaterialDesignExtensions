using System.Text;
using System.Windows;
using System.Windows.Controls;
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
            OpenDirectoryDialogArguments dialogArgs = new OpenDirectoryDialogArguments()
            {
                Width = 600,
                Height = 400,
                CreateNewDirectoryEnabled = true
            };

            OpenDirectoryDialogResult result = await OpenDirectoryDialog.ShowDialogAsync(MainWindow.DialogHostName, dialogArgs);

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
            OpenFileDialogArguments dialogArgs = new OpenFileDialogArguments()
            {
                Width = 600,
                Height = 400,
                Filters = "All files|*.*|C# files|*.cs|XAML files|*.xaml"
            };

            OpenFileDialogResult result = await OpenFileDialog.ShowDialogAsync(MainWindow.DialogHostName, dialogArgs);

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
            SaveFileDialogArguments dialogArgs = new SaveFileDialogArguments()
            {
                Width = 600,
                Height = 400,
                Filters = "All files|*.*|C# files|*.cs|XAML files|*.xaml",
                CreateNewDirectoryEnabled = true
            };

            SaveFileDialogResult result = await SaveFileDialog.ShowDialogAsync(MainWindow.DialogHostName, dialogArgs);

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

        private async void OpenMultipleDirectoriesDialogButtonClickHandler(object sender, RoutedEventArgs args)
        {
            OpenMultipleDirectoriesDialogArguments dialogArgs = new OpenMultipleDirectoriesDialogArguments()
            {
                Width = 600,
                Height = 400,
                CreateNewDirectoryEnabled = true
            };

            OpenMultipleDirectoriesDialogResult result = await OpenMultipleDirectoriesDialog.ShowDialogAsync(MainWindow.DialogHostName, dialogArgs);

            if (DataContext is FileSystemDialogViewModel viewModel)
            {
                if (!result.Canceled)
                {
                    StringBuilder sb = new StringBuilder("Selected directories: ");
                    result.Directories.ForEach(directory => sb.Append($"{directory}; "));

                    viewModel.SelectedAction = sb.ToString();
                }
                else
                {
                    viewModel.SelectedAction = "Cancel open multiple directories";
                }
            }
        }

        private async void OpenMultipleFilesDialogButtonClickHandler(object sender, RoutedEventArgs args)
        {
            OpenMultipleFilesDialogArguments dialogArgs = new OpenMultipleFilesDialogArguments()
            {
                Width = 600,
                Height = 400,
                Filters = "All files|*.*|C# files|*.cs|XAML files|*.xaml"
            };

            OpenMultipleFilesDialogResult result = await OpenMultipleFilesDialog.ShowDialogAsync(MainWindow.DialogHostName, dialogArgs);

            if (DataContext is FileSystemDialogViewModel viewModel)
            {
                if (!result.Canceled)
                {
                    StringBuilder sb = new StringBuilder("Selected files: ");
                    result.Files.ForEach(file => sb.Append($"{file}; "));

                    viewModel.SelectedAction = sb.ToString();
                }
                else
                {
                    viewModel.SelectedAction = "Cancel open multiple files";
                }
            }
        }
    }
}
