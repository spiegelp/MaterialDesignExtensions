using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using MaterialDesignThemes.Wpf;

namespace MaterialDesignExtensions.Controls
{
    /// <summary>
    /// An alert dialog to show a title and message with an OK button only.
    /// </summary>
    public class AlertDialog : MessageDialog
    {
        static AlertDialog()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AlertDialog), new FrameworkPropertyMetadata(typeof(AlertDialog)));
        }

        /// <summary>
        /// Creates a new <see cref="AlertDialog" />.
        /// </summary>
        public AlertDialog() : base() { }

        /// <summary>
        /// Shows a new <see cref="AlertDialog" />.
        /// </summary>
        /// <param name="dialogHostName">The name of the <see cref="DialogHost" /></param>
        /// <param name="args">The arguments for the dialog initialization</param>
        /// <returns></returns>
        public static async Task ShowDialogAsync(string dialogHostName, AlertDialogArguments args)
        {
            AlertDialog dialog = InitDialog(args);

            await DialogHost.Show(dialog, dialogHostName, args.OpenedHandler, args.ClosingHandler);
        }

        /// <summary>
        /// Shows a new <see cref="AlertDialog" />.
        /// </summary>
        /// <param name="dialogHost">The <see cref="DialogHost" /></param>
        /// <param name="args">The arguments for the dialog initialization</param>
        /// <returns></returns>
        public static async Task ShowDialogAsync(DialogHost dialogHost, AlertDialogArguments args)
        {
            AlertDialog dialog = InitDialog(args);

            await dialogHost.ShowDialog(dialog, args.OpenedHandler, args.ClosingHandler);
        }

        private static AlertDialog InitDialog(AlertDialogArguments args)
        {
            AlertDialog dialog = new AlertDialog
            {
                Title = args.Title,
                Message = args.Message,
                CustomContent = args.CustomContent,
                CustomContentTemplate = args.CustomContentTemplate
            };

            if (!string.IsNullOrWhiteSpace(args.OkButtonLabel))
            {
                dialog.OkButtonLabel = args.OkButtonLabel;
            }

            return dialog;
        }
    }

    /// <summary>
    /// The arguments for an alert dialog.
    /// </summary>
    public class AlertDialogArguments : MessageDialogArguments
    {
        /// <summary>
        /// Creates a new <see cref="AlertDialogArguments" />.
        /// </summary>
        public AlertDialogArguments() : base() { }
    }
}
