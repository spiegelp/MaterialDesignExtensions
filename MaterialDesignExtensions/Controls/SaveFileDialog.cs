using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using MaterialDesignThemes.Wpf;

using MaterialDesignExtensions.Converters;
using MaterialDesignExtensions.Model;

namespace MaterialDesignExtensions.Controls
{
    /// <summary>
    /// A dialog for selecting a file to save data into.
    /// </summary>
    public class SaveFileDialog : BaseFileDialog
    {
        /// <summary>
        /// The name of the file itself without the full path.
        /// </summary>
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
        /// Creates a new <see cref="SaveFileDialog" />.
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

        /// <summary>
        /// Shows a new <see cref="SaveFileDialog" />.
        /// </summary>
        /// <param name="dialogHostName">The name of the <see cref="DialogHost" /></param>
        /// <param name="width">The width of the dialog (optional)</param>
        /// <param name="height">The heigth of the dialog (optional)</param>
        /// <param name="currentDirectory">The current directory to show (optional)</param>
        /// <param name="filename">The name of the file without the full path (optional)</param>
        /// <param name="showHiddenFilesAndDirectories">Show or hide hidden files in the dialog (optional)</param>
        /// <param name="showSystemFilesAndDirectories">Show or hide system files in the dialog (optional)</param>
        /// <param name="openedHandler">Callback after openening the dialog (optional)</param>
        /// <param name="closingHandler">Callback after closing the dialog (optional)</param>
        /// <returns></returns>
        [Obsolete("Use the overloaded method with SaveFileDialogArguments instead")]
        public static async Task<SaveFileDialogResult> ShowDialogAsync(string dialogHostName, double? width = null, double? height = null,
            string currentDirectory = null, string filename = null,
            bool showHiddenFilesAndDirectories = false, bool showSystemFilesAndDirectories = false,
            DialogOpenedEventHandler openedHandler = null, DialogClosingEventHandler closingHandler = null)
        {
            SaveFileDialog dialog = InitDialog(width, height, currentDirectory, filename, null, -1, false, showHiddenFilesAndDirectories, showSystemFilesAndDirectories);

            return await DialogHost.Show(dialog, dialogHostName, openedHandler, closingHandler) as SaveFileDialogResult;
        }

        /// <summary>
        /// Shows a new <see cref="SaveFileDialog" />.
        /// </summary>
        /// <param name="dialogHostName">The name of the <see cref="DialogHost" /></param>
        /// <param name="args">The arguments for the dialog initialization</param>
        /// <returns></returns>
        public static async Task<SaveFileDialogResult> ShowDialogAsync(string dialogHostName, SaveFileDialogArguments args)
        {
            SaveFileDialog dialog = InitDialog(
                args.Width,
                args.Height,
                args.CurrentDirectory,
                args.Filename,
                args.Filters,
                args.FilterIndex,
                args.CreateNewDirectoryEnabled,
                args.ShowHiddenFilesAndDirectories,
                args.ShowSystemFilesAndDirectories
            );

            return await DialogHost.Show(dialog, dialogHostName, args.OpenedHandler, args.ClosingHandler) as SaveFileDialogResult;
        }

        /// <summary>
        /// Shows a new <see cref="SaveFileDialog" />.
        /// </summary>
        /// <param name="dialogHost">The <see cref="DialogHost" /></param>
        /// <param name="width">The width of the dialog (optional)</param>
        /// <param name="height">The heigth of the dialog (optional)</param>
        /// <param name="currentDirectory">The current directory to show (optional)</param>
        /// <param name="filename">The name of the file without the full path (optional)</param>
        /// <param name="showHiddenFilesAndDirectories">Show or hide hidden files in the dialog (optional)</param>
        /// <param name="showSystemFilesAndDirectories">Show or hide system files in the dialog (optional)</param>
        /// <param name="openedHandler">Callback after openening the dialog (optional)</param>
        /// <param name="closingHandler">Callback after closing the dialog (optional)</param>
        /// <returns></returns>
        [Obsolete("Use the overloaded method with SaveFileDialogArguments instead")]
        public static async Task<SaveFileDialogResult> ShowDialogAsync(DialogHost dialogHost, double? width = null, double? height = null,
            string currentDirectory = null, string filename = null,
            bool showHiddenFilesAndDirectories = false, bool showSystemFilesAndDirectories = false,
            DialogOpenedEventHandler openedHandler = null, DialogClosingEventHandler closingHandler = null)
        {
            SaveFileDialog dialog = InitDialog(width, height, currentDirectory, filename, null, -1, false, showHiddenFilesAndDirectories, showSystemFilesAndDirectories);

            return await dialogHost.ShowDialog(dialog, openedHandler, closingHandler) as SaveFileDialogResult;
        }

        /// <summary>
        /// Shows a new <see cref="SaveFileDialog" />.
        /// </summary>
        /// <param name="dialogHost">The <see cref="DialogHost" /></param>
        /// <param name="args">The arguments for the dialog initialization</param>
        /// <returns></returns>
        public static async Task<SaveFileDialogResult> ShowDialogAsync(DialogHost dialogHost, SaveFileDialogArguments args)
        {
            SaveFileDialog dialog = InitDialog(
                args.Width,
                args.Height,
                args.CurrentDirectory,
                args.Filename,
                args.Filters,
                args.FilterIndex,
                args.CreateNewDirectoryEnabled,
                args.ShowHiddenFilesAndDirectories,
                args.ShowSystemFilesAndDirectories
            );

            return await dialogHost.ShowDialog(dialog, args.OpenedHandler, args.ClosingHandler) as SaveFileDialogResult;
        }

        private static SaveFileDialog InitDialog(double? width, double? height,
            string currentDirectory, string filename,
            string filters, int filterIndex,
            bool createNewDirectoryEnabled,
            bool showHiddenFilesAndDirectories, bool showSystemFilesAndDirectories)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            InitDialog(dialog, width, height, currentDirectory, showHiddenFilesAndDirectories, showSystemFilesAndDirectories, createNewDirectoryEnabled);
            dialog.Filename = filename;
            dialog.Filters = new FileFiltersTypeConverter().ConvertFrom(null, null, filters) as IList<IFileFilter>;
            dialog.FilterIndex = filterIndex;

            return dialog;
        }
    }

    /// <summary>
    /// Arguments to initialize a save file dialog.
    /// </summary>
    public class SaveFileDialogArguments : FileDialogArguments
    {
        /// <summary>
        /// The name of the file itself without the full path.
        /// </summary>
        public string Filename { get; set; }

        /// <summary>
        /// Creates a new <see cref="SaveFileDialogArguments" />.
        /// </summary>
        public SaveFileDialogArguments() : base() { }
    }

    /// <summary>
    /// The dialog result for <see cref="SaveFileDialog" />.
    /// </summary>
    public class SaveFileDialogResult : FileDialogResult
    {
        /// <summary>
        /// Creates a new <see cref="SaveFileDialogResult" />.
        /// </summary>
        /// <param name="canceled">True if the dialog was canceled</param>
        /// <param name="fileInfo">The selected file</param>
        public SaveFileDialogResult(bool canceled, FileInfo fileInfo) : base(canceled, fileInfo) { }
    }
}
