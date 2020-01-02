using System.Collections.Generic;
using System.IO;
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

        /// <summary>
        /// Forces the possible file extension of the selected file filter for new filenames.
        /// </summary>
        public static readonly DependencyProperty ForceFileExtensionOfFileFilterProperty = DependencyProperty.Register(
            nameof(ForceFileExtensionOfFileFilter),
            typeof(bool),
            typeof(SaveFileDialog),
            new PropertyMetadata(false));

        /// <summary>
        /// Forces the possible file extension of the selected file filter for new filenames.
        /// </summary>
        public bool ForceFileExtensionOfFileFilter
        {
            get
            {
                return (bool)GetValue(ForceFileExtensionOfFileFilterProperty);
            }

            set
            {
                SetValue(ForceFileExtensionOfFileFilterProperty, value);
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
                args.ShowSystemFilesAndDirectories,
                args.ForceFileExtensionOfFileFilter
            );

            return await DialogHost.Show(dialog, dialogHostName, args.OpenedHandler, args.ClosingHandler) as SaveFileDialogResult;
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
                args.ShowSystemFilesAndDirectories,
                args.ForceFileExtensionOfFileFilter
            );

            return await dialogHost.ShowDialog(dialog, args.OpenedHandler, args.ClosingHandler) as SaveFileDialogResult;
        }

        private static SaveFileDialog InitDialog(double? width, double? height,
            string currentDirectory, string filename,
            string filters, int filterIndex,
            bool createNewDirectoryEnabled,
            bool showHiddenFilesAndDirectories, bool showSystemFilesAndDirectories,
            bool forceFileExtensionOfFileFilter)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            InitDialog(dialog, width, height, currentDirectory, showHiddenFilesAndDirectories, showSystemFilesAndDirectories, createNewDirectoryEnabled);
            dialog.Filename = filename;
            dialog.ForceFileExtensionOfFileFilter = forceFileExtensionOfFileFilter;
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
        /// Forces the possible file extension of the selected file filter for new filenames.
        /// </summary>
        public bool ForceFileExtensionOfFileFilter { get; set; }

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
