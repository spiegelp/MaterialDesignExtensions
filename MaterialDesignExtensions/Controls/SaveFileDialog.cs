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
    public class SaveFileDialog : BaseFileDialog
    {
        public static readonly DependencyProperty FilenameProperty = DependencyProperty.Register(
                nameof(Filename),
                typeof(string),
                typeof(SaveFileDialog),
                new PropertyMetadata(null));

        /// <summary>
        /// The name of the file itself without the full path.
        /// </summary>
        public string Filename
        {
            get
            {
                return (string)GetValue(FilenameProperty);
            }

            set
            {
                SetValue(FilenameProperty, value);
            }
        }

        static SaveFileDialog()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SaveFileDialog), new FrameworkPropertyMetadata(typeof(SaveFileDialog)));
        }

        /// <summary>
        /// Creates a new <see cref="SaveFileDialog" />
        /// </summary>
        public SaveFileDialog() : base() { }

        protected override void CancelHandler(object sender, RoutedEventArgs args)
        {
            DialogHost.CloseDialogCommand.Execute(new SaveFileDialogResult(true, null), GetDialogHost());
        }

        protected override void OpenDirectoryControlFileSelectedHandler(object sender, RoutedEventArgs args)
        {
            DialogHost.CloseDialogCommand.Execute(new SaveFileDialogResult(false, (args as FileSelectedEventArgs)?.FileInfo), GetDialogHost());
        }

        public static async Task<SaveFileDialogResult> ShowDialogAsync(string dialogHostName, double? width = null, double? height = null,
            string currentDirectory = null, string filename = null,
            bool showHiddenFilesAndDirectories = false, bool showSystemFilesAndDirectories = false,
            DialogOpenedEventHandler openedHandler = null, DialogClosingEventHandler closingHandler = null)
        {
            SaveFileDialog dialog = InitDialog(width, height, filename, currentDirectory, showHiddenFilesAndDirectories, showSystemFilesAndDirectories);

            return await DialogHost.Show(dialog, dialogHostName, openedHandler, closingHandler) as SaveFileDialogResult;
        }

        public static async Task<SaveFileDialogResult> ShowDialogAsync(DialogHost dialogHost, double? width = null, double? height = null,
            string currentDirectory = null, string filename = null,
            bool showHiddenFilesAndDirectories = false, bool showSystemFilesAndDirectories = false,
            DialogOpenedEventHandler openedHandler = null, DialogClosingEventHandler closingHandler = null)
        {
            SaveFileDialog dialog = InitDialog(width, height, currentDirectory, filename, showHiddenFilesAndDirectories, showSystemFilesAndDirectories);

            return await dialogHost.ShowDialog(dialog, openedHandler, closingHandler) as SaveFileDialogResult;
        }

        private static SaveFileDialog InitDialog(double? width, double? height,
            string currentDirectory, string filename,
            bool showHiddenFilesAndDirectories, bool showSystemFilesAndDirectories)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            InitDialog(dialog, width, height, currentDirectory, showHiddenFilesAndDirectories, showSystemFilesAndDirectories);
            dialog.Filename = filename;

            return dialog;
        }
    }

    public class SaveFileDialogResult : FileDialogResult
    {
        public SaveFileDialogResult(bool canceled, FileInfo fileInfo) : base(canceled, fileInfo) { }
    }
}
