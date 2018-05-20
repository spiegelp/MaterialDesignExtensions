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

using MaterialDesignThemes.Wpf;

using MaterialDesignExtensions.Controls;

using MaterialDesignExtensionsDemo.ViewModel;

namespace MaterialDesignExtensionsDemo.Controls
{
    public partial class StepperControl : UserControl
    {
        public StepperControl()
        {
            InitializeComponent();

            stepper.NavigationCanceledByValidation += Stepper_NavigationCanceledByValidation;
        }

        private void Stepper_NavigationCanceledByValidation(object sender, NavigationCanceledByValidationEventArgs args)
        {
            if (args.StepWithValidationErrors.Content is StepperTutorialFourViewModel)
            {
                DialogHost.Show(new AlertDialogViewModel("Accept the license terms please."), MainWindow.DialogHostName);
            }
        }

        private async void OpenFileDialogButtonClickHandler(object sender, RoutedEventArgs args)
        {
            OpenFileDialogArguments dialogArgs = new OpenFileDialogArguments()
            {
                Width = 600,
                Height = 400
            };

            // await the result of OpenFileDialogResult and do something with it
            OpenFileDialogResult result = await OpenFileDialog.ShowDialogAsync(MainWindow.DialogHostName, dialogArgs);
            
            if (!result.Canceled)
            {
                Console.WriteLine("Selected file: " + result.FileInfo.FullName);
            }
            else
            {
                Console.WriteLine("Cancel open file");
            }
        }
    }
}
