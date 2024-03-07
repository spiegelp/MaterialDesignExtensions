using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using MaterialDesignThemes.Wpf;

namespace MaterialDesignExtensions.Controls
{
    /// <summary>
    /// A confirmation dialog to show a title and message with an OK and a cancel button.
    /// </summary>
    public class ConfirmationDialog : MessageDialog
    {
        private static readonly string CancelButtonName = "cancelButton";

        /// <summary>
        /// The label of the cancel button.
        /// </summary>
        public static readonly DependencyProperty CancelButtonLabelProperty = DependencyProperty.Register(
            nameof(CancelButtonLabel), typeof(string), typeof(ConfirmationDialog));

        /// <summary>
        /// The label of the cancel button.
        /// </summary>
        public string CancelButtonLabel
        {
            get
            {
                return (string)GetValue(CancelButtonLabelProperty);
            }

            set
            {
                SetValue(CancelButtonLabelProperty, value);
            }
        }

        /// <summary>
        /// True to stack the OK and cancel buttons instead of showing them side by side.
        /// </summary>
        public static readonly DependencyProperty StackedButtonsProperty = DependencyProperty.Register(
            nameof(StackedButtons), typeof(bool), typeof(ConfirmationDialog));

        /// <summary>
        /// True to stack the OK and cancel buttons instead of showing them side by side.
        /// </summary>
        public bool StackedButtons
        {
            get
            {
                return (bool)GetValue(StackedButtonsProperty);
            }

            set
            {
                SetValue(StackedButtonsProperty, value);
            }
        }

        private Button m_cancelButton;

        static ConfirmationDialog()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ConfirmationDialog), new FrameworkPropertyMetadata(typeof(ConfirmationDialog)));
        }

        /// <summary>
        /// Creates a new <see cref="ConfirmationDialog" />.
        /// </summary>
        public ConfirmationDialog()
            : base()
        {
            m_cancelButton = null;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (m_cancelButton != null)
            {
                m_cancelButton.Click -= CancelButtonClickHandler;
            }

            m_cancelButton = Template.FindName(CancelButtonName, this) as Button;
        }

        protected override void LoadedHandler(object sender, RoutedEventArgs args)
        {
            base.LoadedHandler(sender, args);

            m_cancelButton.Click += CancelButtonClickHandler;
        }

        protected override void UnloadedHandler(object sender, RoutedEventArgs args)
        {
            base.UnloadedHandler(sender, args);

            m_cancelButton.Click -= CancelButtonClickHandler;
        }

        protected override void OkButtonClickHandler(object sender, RoutedEventArgs args)
        {
            DialogHost.CloseDialogCommand.Execute(true, GetDialogHost());
        }

        private void CancelButtonClickHandler(object sender, RoutedEventArgs args)
        {
            DialogHost.CloseDialogCommand.Execute(false, GetDialogHost());
        }

        /// <summary>
        /// Shows a new <see cref="ConfirmationDialog" />.
        /// </summary>
        /// <param name="dialogHostName">The name of the <see cref="DialogHost" /></param>
        /// <param name="args">The arguments for the dialog initialization</param>
        /// <returns></returns>
        public static async Task<bool> ShowDialogAsync(string dialogHostName, ConfirmationDialogArguments args)
        {
            ConfirmationDialog dialog = InitDialog(args);

            object result = await DialogHost.Show(dialog, dialogHostName, args.OpenedHandler, args.ClosingHandler);

            return (bool)result;
        }

        /// <summary>
        /// Shows a new <see cref="ConfirmationDialog" />.
        /// </summary>
        /// <param name="dialogHost">The <see cref="DialogHost" /></param>
        /// <param name="args">The arguments for the dialog initialization</param>
        /// <returns></returns>
        public static async Task<bool> ShowDialogAsync(DialogHost dialogHost, ConfirmationDialogArguments args)
        {
            ConfirmationDialog dialog = InitDialog(args);

            object result = await dialogHost.ShowDialog(dialog, args.OpenedHandler, args.ClosingHandler);

            return (bool)result;
        }

        private static ConfirmationDialog InitDialog(ConfirmationDialogArguments args)
        {
            ConfirmationDialog dialog = new ConfirmationDialog
            {
                Title = args.Title,
                Message = args.Message,
                StackedButtons = args.StackedButtons,
                CustomContent = args.CustomContent,
                CustomContentTemplate = args.CustomContentTemplate
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
    public class ConfirmationDialogArguments : MessageDialogArguments
    {
        /// <summary>
        /// The label of the cancel button.
        /// </summary>
        public string CancelButtonLabel { get; set; }

        /// <summary>
        /// True to stack the OK and cancel buttons instead of showing them side by side.
        /// </summary>
        public bool StackedButtons { get; set; }

        /// <summary>
        /// Creates a new <see cref="ConfirmationDialogArguments" />.
        /// </summary>
        public ConfirmationDialogArguments()
            : base()
        {
            CancelButtonLabel = null;
            StackedButtons = false;
        }
    }
}
