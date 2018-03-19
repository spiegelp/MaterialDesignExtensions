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
    }
}
