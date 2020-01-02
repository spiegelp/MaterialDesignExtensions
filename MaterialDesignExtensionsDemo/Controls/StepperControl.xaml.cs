using System;
using System.Windows;
using System.Windows.Controls;
using MaterialDesignThemes.Wpf;

using MaterialDesignExtensions.Controls;
using MaterialDesignExtensions.Model;

using MaterialDesignExtensionsDemo.ViewModel;

namespace MaterialDesignExtensionsDemo.Controls
{
    public partial class StepperControl : UserControl
    {
        public StepperControl()
        {
            InitializeComponent();

            stepper.NavigationCanceledByValidation += Stepper_NavigationCanceledByValidation;

            // demo for custom icon template
            //Loaded += StepperControl_Loaded;
        }

        private void StepperControl_Loaded(object sender, RoutedEventArgs args)
        {
            if (FindResource("CustomStepIconTemplate") is DataTemplate iconTemplate)
            {
                ((IStep)stepper.Steps[1]).IconTemplate = iconTemplate;
            }
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
