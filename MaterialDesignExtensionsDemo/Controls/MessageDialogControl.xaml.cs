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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading.Tasks;

using MaterialDesignThemes;

using MaterialDesignExtensions.Controls;
using MaterialDesignThemes.Wpf;
using MaterialDesignExtensionsDemo.ViewModel;
using System.Linq;

namespace MaterialDesignExtensionsDemo.Controls
{
    public partial class MessageDialogControl : UserControl
    {
        public MessageDialogControl()
        {
            InitializeComponent();
        }

        private async void ShowAlertDialogButtonClickHandler(object sender, RoutedEventArgs args)
        {
            AlertDialogArguments dialogArgs = new AlertDialogArguments
            {
                Title = "Greetings",
                Message = "Hello, I'm MaterialDesignExtensions.",
                OkButtonLabel = "GOT IT"
            };

            await AlertDialog.ShowDialogAsync(MainWindow.DialogHostName, dialogArgs);
        }

        private async void ShowConfirmationDialogButtonClickHandler(object sender, RoutedEventArgs args)
        {
            await ShowConfirmationDialogAsync(false);
        }

        private async void ShowConfirmationDialogStackedButtonClickHandler(object sender, RoutedEventArgs args)
        {
            await ShowConfirmationDialogAsync(true);
        }

        private async Task ShowConfirmationDialogAsync(bool stackedButtons)
        {
            ConfirmationDialogArguments dialogArgs = new ConfirmationDialogArguments
            {
                Title = "Delete files",
                Message = "Do you really want to permanently delete all files?",
                OkButtonLabel = "DELETE",
                CancelButtonLabel = "CANCEL",
                StackedButtons = stackedButtons
            };

            bool result = await ConfirmationDialog.ShowDialogAsync(MainWindow.DialogHostName, dialogArgs);

            System.Diagnostics.Debug.WriteLine($"{typeof(ConfirmationDialog)} result: {result}");
        }

        private async void ShowAlertDialogWithContentButtonClickHandler(object sender, RoutedEventArgs args)
        {
            AlertDialogArguments dialogArgs = new AlertDialogArguments
            {
                Title = "Sports",
                Message = "Some great sports:",
                OkButtonLabel = "OK",
                CustomContent = FindResource("SportsStackPanel")
            };

            await AlertDialog.ShowDialogAsync(MainWindow.DialogHostName, dialogArgs);
        }

        private async void ShowConfirmationDialogWithContentButtonClickHandler(object sender, RoutedEventArgs args)
        {
            ConfirmationDialogArguments dialogArgs = new ConfirmationDialogArguments
            {
                Title = "Sports",
                Message = "Which sports do you enjoy?",
                OkButtonLabel = "OK",
                CancelButtonLabel = "CANCEL",
                CustomContent = FindResource("SimpleTextBox")
            };

            await ConfirmationDialog.ShowDialogAsync(MainWindow.DialogHostName, dialogArgs);
        }

        private async void ShowInputDialogButtonClickHandler(object sender, RoutedEventArgs args)
        {
            m_sportsSelectionTextBlock.Text = null;

            SportsSelectionViewModel viewModel = new SportsSelectionViewModel();

            InputDialogArguments dialogArgs = new InputDialogArguments
            {
                Title = "Sports",
                Message = "Which sports do you enjoy?",
                OkButtonLabel = "THAT'S IT",
                CancelButtonLabel = "CANCEL",
                CustomContent = viewModel,
                CustomContentTemplate = FindResource("SportsSelectionViewModelTemplate") as DataTemplate,
                ValidationHandler = viewModel.ValidationHandler
            };

            bool result = await InputDialog.ShowDialogAsync(MainWindow.DialogHostName, dialogArgs);

            if (result)
            {
                List<string> sportsItems = viewModel.SportsItems
                    .Where(sportsItem => sportsItem.IsSelected)
                    .Select(sportsItem => sportsItem.Label)
                    .ToList();

                m_sportsSelectionTextBlock.Text = $"Selected sports: {string.Join(", ", sportsItems)}";
            }
            else
            {
                m_sportsSelectionTextBlock.Text = "Sports selection cancelled";
            }
        }
    }
}
