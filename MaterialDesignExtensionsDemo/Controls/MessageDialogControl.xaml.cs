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

using MaterialDesignExtensions.Controls;

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
    }
}
