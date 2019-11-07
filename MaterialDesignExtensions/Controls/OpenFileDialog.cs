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

// use Pri.LongPath classes instead of System.IO for the MaterialDesignExtensions.LongPath build to support long file system paths on older Windows and .NET versions
#if LONG_PATH
using FileInfo = Pri.LongPath.FileInfo;
#endif

namespace MaterialDesignExtensions.Controls
{
    /// <summary>
    /// A dialog for selecting a file to open.
    /// </summary>
    public class OpenFileDialog : BaseFileDialog
    {
        static OpenFileDialog()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(OpenFileDialog), new FrameworkPropertyMetadata(typeof(OpenFileDialog)));
        }

        /// <summary>
        /// Creates a new <see cref="OpenFileDialog" />.
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

        /// <summary>
        /// Shows a new <see cref="OpenFileDialog" />.
        /// </summary>
        /// <param name="dialogHostName">The name of the <see cref="DialogHost" /></param>
        /// <param name="args">The arguments for the dialog initialization</param>
        /// <returns></returns>
        public static async Task<OpenFileDialogResult> ShowDialogAsync(string dialogHostName, OpenFileDialogArguments args)
        {
            OpenFileDialog dialog = InitDialog(
                args.Width,
                args.Height,
                args.CurrentDirectory,
                args.Filters,
                args.FilterIndex,
                args.ShowHiddenFilesAndDirectories,
                args.ShowSystemFilesAndDirectories
            );

            return await DialogHost.Show(dialog, dialogHostName, args.OpenedHandler, args.ClosingHandler) as OpenFileDialogResult;
        }

        /// <summary>
        /// Shows a new <see cref="OpenFileDialog" />.
        /// </summary>
        /// <param name="dialogHost">The <see cref="DialogHost" /></param>
        /// <param name="args">The arguments for the dialog initialization</param>
        /// <returns></returns>
        public static async Task<OpenFileDialogResult> ShowDialogAsync(DialogHost dialogHost, OpenFileDialogArguments args)
        {
            OpenFileDialog dialog = InitDialog(
                args.Width,
                args.Height,
                args.CurrentDirectory,
                args.Filters,
                args.FilterIndex,
                args.ShowHiddenFilesAndDirectories,
                args.ShowSystemFilesAndDirectories
            );

            return await dialogHost.ShowDialog(dialog, args.OpenedHandler, args.ClosingHandler) as OpenFileDialogResult;
        }

        private static OpenFileDialog InitDialog(double? width, double? height,
            string currentDirectory,
            string filters, int filterIndex,
            bool showHiddenFilesAndDirectories, bool showSystemFilesAndDirectories)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            InitDialog(dialog, width, height, currentDirectory, showHiddenFilesAndDirectories, showSystemFilesAndDirectories);
            dialog.Filters = new FileFiltersTypeConverter().ConvertFrom(null, null, filters) as IList<IFileFilter>;
            dialog.FilterIndex = filterIndex;

            return dialog;
        }
    }

    /// <summary>
    /// Arguments to initialize an open file dialog.
    /// </summary>
    public class OpenFileDialogArguments : FileDialogArguments
    {
        /// <summary>
        /// Creates a new <see cref="OpenFileDialogArguments" />.
        /// </summary>
        public OpenFileDialogArguments() : base() { }
    }

    /// <summary>
    /// The dialog result for <see cref="OpenFileDialog" />.
    /// </summary>
    public class OpenFileDialogResult : FileDialogResult
    {
        /// <summary>
        /// Creates a new <see cref="OpenFileDialogResult" />.
        /// </summary>
        /// <param name="canceled">True if the dialog was canceled</param>
        /// <param name="fileInfo">The selected file</param>
        public OpenFileDialogResult(bool canceled, FileInfo fileInfo) : base(canceled, fileInfo) { }
    }
}
