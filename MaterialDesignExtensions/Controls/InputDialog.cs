using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using MaterialDesignThemes.Wpf;

namespace MaterialDesignExtensions.Controls
{
    public class InputDialog : ConfirmationDialog
    {
        /// <summary>
        /// A handler called for validation right before confirming the dialog.
        /// </summary>
        public static readonly DependencyProperty ValidationHandlerProperty = DependencyProperty.Register(
            nameof(ValidationHandler), typeof(InputDialogValidationEventHandler), typeof(InputDialog));

        /// <summary>
        /// A handler called for validation right before confirming the dialog.
        /// </summary>
        public InputDialogValidationEventHandler ValidationHandler
        {
            get
            {
                return (InputDialogValidationEventHandler)GetValue(ValidationHandlerProperty);
            }

            set
            {
                SetValue(ValidationHandlerProperty, value);
            }
        }

        static InputDialog()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(InputDialog), new FrameworkPropertyMetadata(typeof(InputDialog)));
        }

        /// <summary>
        /// Creates a new <see cref="InputDialog" />.
        /// </summary>
        public InputDialog() : base() { }

        protected override void OkButtonClickHandler(object sender, RoutedEventArgs args)
        {
            InputDialogValidationEventArgs validationArgs = new InputDialogValidationEventArgs { CancelConfirmation = false };

            ValidationHandler?.Invoke(this, validationArgs);

            if (!validationArgs.CancelConfirmation)
            {
                DialogHost.CloseDialogCommand.Execute(true, GetDialogHost());
            }
        }

        /// <summary>
        /// Shows a new <see cref="InputDialog" />.
        /// </summary>
        /// <param name="dialogHostName">The name of the <see cref="DialogHost" /></param>
        /// <param name="args">The arguments for the dialog initialization</param>
        /// <returns></returns>
        public static async Task<bool> ShowDialogAsync(string dialogHostName, InputDialogArguments args)
        {
            InputDialog dialog = InitDialog(args);

            object result = await DialogHost.Show(dialog, dialogHostName, args.OpenedHandler, args.ClosingHandler);

            return (bool)result;
        }

        /// <summary>
        /// Shows a new <see cref="InputDialog" />.
        /// </summary>
        /// <param name="dialogHost">The <see cref="DialogHost" /></param>
        /// <param name="args">The arguments for the dialog initialization</param>
        /// <returns></returns>
        public static async Task<bool> ShowDialogAsync(DialogHost dialogHost, InputDialogArguments args)
        {
            InputDialog dialog = InitDialog(args);

            object result = await dialogHost.ShowDialog(dialog, args.OpenedHandler, args.ClosingHandler);

            return (bool)result;
        }

        private static InputDialog InitDialog(InputDialogArguments args)
        {
            InputDialog dialog = new InputDialog
            {
                Title = args.Title,
                Message = args.Message,
                StackedButtons = args.StackedButtons,
                CustomContent = args.CustomContent,
                CustomContentTemplate = args.CustomContentTemplate,
                ValidationHandler = args.ValidationHandler
            };

            if (!string.IsNullOrWhiteSpace(args.OkButtonLabel))
            {
                dialog.OkButtonLabel = args.OkButtonLabel;
            }

            if (!string.IsNullOrWhiteSpace(args.CancelButtonLabel))
            {
                dialog.CancelButtonLabel = args.CancelButtonLabel;
            }

            return dialog;
        }
    }

    /// <summary>
    /// The arguments for a confirmation dialog.
    /// </summary>
    public class InputDialogArguments : ConfirmationDialogArguments
    {
        /// <summary>
        /// A handler called for validation right before confirming the dialog.
        /// </summary>
        public InputDialogValidationEventHandler ValidationHandler { get; set; }

        /// <summary>
        /// Creates a new <see cref="InputDialogArguments" />.
        /// </summary>
        public InputDialogArguments()
            : base()
        {
            ValidationHandler = null;
        }
    }

    /// <summary>
    /// The arguments for the validation handler.
    /// </summary>
    public class InputDialogValidationEventArgs
    {
        public bool CancelConfirmation { get; set; }

        /// <summary>
        /// Creates a new <see cref="InputDialogValidationEventArgs" />.
        /// </summary>
        public InputDialogValidationEventArgs()
        {
            CancelConfirmation = true;
        }
    }

    /// <summary>
    /// The delegate for handling the input dialog validation.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    public delegate void InputDialogValidationEventHandler(object sender, InputDialogValidationEventArgs args);
}
