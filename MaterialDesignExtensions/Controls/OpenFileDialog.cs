using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using MaterialDesignThemes.Wpf;

namespace MaterialDesignExtensions.Controls
{
    public class OpenFileDialog : BaseFileDialog
    {
        static OpenFileDialog()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(OpenFileDialog), new FrameworkPropertyMetadata(typeof(OpenFileDialog)));
        }

        /// <summary>
        /// Creates a new <see cref="OpenFileDialog" />
        /// </summary>
        public OpenFileDialog() : base() { }

        protected override void CancelHandler(object sender, RoutedEventArgs args)
        {
            DialogHost.CloseDialogCommand.Execute(new OpenFileDialogResult(true, null), GetDialogHost());
        }

        protected override void OpenDirectoryControlFileSelectedHandler(object sender, RoutedEventArgs args)
        {
            DialogHost.CloseDialogCommand.Execute(new OpenFileDialogResult(false, (args as FileSelectedEventArgs)?.FileInfo), GetDialogHost());
        }

        public static async Task<OpenFileDialogResult> ShowDialogAsync(string dialogHostName, double? width = null, double? height = null,
            string currentDirectory = null,
            bool showHiddenFilesAndDirectories = false, bool showSystemFilesAndDirectories = false,
            DialogOpenedEventHandler openedHandler = null, DialogClosingEventHandler closingHandler = null)
        {
            OpenFileDialog dialog = InitDialog(width, height, currentDirectory, showHiddenFilesAndDirectories, showSystemFilesAndDirectories);

            return await DialogHost.Show(dialog, dialogHostName, openedHandler, closingHandler) as OpenFileDialogResult;
        }

        public static async Task<OpenFileDialogResult> ShowDialogAsync(DialogHost dialogHost, double? width = null, double? height = null,
            string currentDirectory = null,
            bool showHiddenFilesAndDirectories = false, bool showSystemFilesAndDirectories = false,
            DialogOpenedEventHandler openedHandler = null, DialogClosingEventHandler closingHandler = null)
        {
            OpenFileDialog dialog = InitDialog(width, height, currentDirectory, showHiddenFilesAndDirectories, showSystemFilesAndDirectories);

            return await dialogHost.ShowDialog(dialog, openedHandler, closingHandler) as OpenFileDialogResult;
        }

        private static OpenFileDialog InitDialog(double? width, double? height,
            string currentDirectory,
            bool showHiddenFilesAndDirectories, bool showSystemFilesAndDirectories)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            InitDialog(dialog, width, height, currentDirectory, showHiddenFilesAndDirectories, showSystemFilesAndDirectories);

            return dialog;
        }
    }

    public class OpenFileDialogResult : FileDialogResult
    {
        public OpenFileDialogResult(bool canceled, FileInfo fileInfo) : base(canceled, fileInfo) { }
    }
}
