using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using MaterialDesignThemes.Wpf;

namespace MaterialDesignExtensions.Controls
{
    /// <summary>
    /// A dialog for selecting multiple directories.
    /// </summary>
    public class OpenMultipleDirectoriesDialog : FileSystemDialog
    {
        private static readonly string OpenMultipleDirectoriesControlName = "openMultipleDirectoriesControl";

        private OpenMultipleDirectoriesControl m_openMultipleDirectoriesControl;

        static OpenMultipleDirectoriesDialog()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(OpenMultipleDirectoriesDialog), new FrameworkPropertyMetadata(typeof(OpenMultipleDirectoriesDialog)));
        }

        /// <summary>
        /// Creates a new <see cref="OpenMultipleDirectoriesDialog" />.
        /// </summary>
        public OpenMultipleDirectoriesDialog()
            : base()
        {
            m_openMultipleDirectoriesControl = null;

            Loaded += LoadedHandler;
            Unloaded += UnloadedHandler;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (m_openMultipleDirectoriesControl != null)
            {
                m_openMultipleDirectoriesControl.Cancel -= CancelHandler;
                m_openMultipleDirectoriesControl.DirectoriesSelected -= OpenMultipleDirectoriesControlDirectoriesSelectedHandler;
            }

            m_openMultipleDirectoriesControl = Template.FindName(OpenMultipleDirectoriesControlName, this) as OpenMultipleDirectoriesControl;
        }

        private void LoadedHandler(object sender, RoutedEventArgs args)
        {
            m_openMultipleDirectoriesControl.Cancel += CancelHandler;
            m_openMultipleDirectoriesControl.DirectoriesSelected += OpenMultipleDirectoriesControlDirectoriesSelectedHandler;
        }

        private void UnloadedHandler(object sender, RoutedEventArgs args)
        {
            m_openMultipleDirectoriesControl.Cancel -= CancelHandler;
            m_openMultipleDirectoriesControl.DirectoriesSelected -= OpenMultipleDirectoriesControlDirectoriesSelectedHandler;
        }

        private void CancelHandler(object sender, RoutedEventArgs args)
        {
            DialogHost.CloseDialogCommand.Execute(new OpenMultipleDirectoriesDialogResult(true, null), GetDialogHost());
        }

        private void OpenMultipleDirectoriesControlDirectoriesSelectedHandler(object sender, RoutedEventArgs args)
        {
            DialogHost.CloseDialogCommand.Execute(new OpenMultipleDirectoriesDialogResult(false, (args as DirectoriesSelectedEventArgs)?.DirectoryInfoList), GetDialogHost());
        }

        /// <summary>
        /// Shows a new <see cref="OpenMultipleDirectoriesDialog" />.
        /// </summary>
        /// <param name="dialogHostName">The name of the <see cref="DialogHost" /></param>
        /// <param name="args">The arguments for the dialog initialization</param>
        /// <returns></returns>
        public static async Task<OpenMultipleDirectoriesDialogResult> ShowDialogAsync(string dialogHostName, OpenMultipleDirectoriesDialogArguments args)
        {
            OpenMultipleDirectoriesDialog dialog = InitDialog(
                args.Width,
                args.Height,
                args.CurrentDirectory,
                args.CreateNewDirectoryEnabled,
                args.ShowHiddenFilesAndDirectories,
                args.ShowSystemFilesAndDirectories
            );

            return await DialogHost.Show(dialog, dialogHostName, args.OpenedHandler, args.ClosingHandler) as OpenMultipleDirectoriesDialogResult;
        }

        /// <summary>
        /// Shows a new <see cref="OpenMultipleDirectoriesDialog" />.
        /// </summary>
        /// <param name="dialogHost">The <see cref="DialogHost" /></param>
        /// <param name="args">The arguments for the dialog initialization</param>
        /// <returns></returns>
        public static async Task<OpenMultipleDirectoriesDialogResult> ShowDialogAsync(DialogHost dialogHost, OpenMultipleDirectoriesDialogArguments args)
        {
            OpenMultipleDirectoriesDialog dialog = InitDialog(
                args.Width,
                args.Height,
                args.CurrentDirectory,
                args.CreateNewDirectoryEnabled,
                args.ShowHiddenFilesAndDirectories,
                args.ShowSystemFilesAndDirectories
            );

            return await dialogHost.ShowDialog(dialog, args.OpenedHandler, args.ClosingHandler) as OpenMultipleDirectoriesDialogResult;
        }

        private static OpenMultipleDirectoriesDialog InitDialog(double? width, double? height,
            string currentDirectory,
            bool createNewDirectoryEnabled,
            bool showHiddenFilesAndDirectories, bool showSystemFilesAndDirectories)
        {
            OpenMultipleDirectoriesDialog dialog = new OpenMultipleDirectoriesDialog();
            InitDialog(dialog, width, height, currentDirectory, showHiddenFilesAndDirectories, showSystemFilesAndDirectories, createNewDirectoryEnabled);

            return dialog;
        }
    }

    /// <summary>
    /// Arguments to initialize an open multiple directories dialog.
    /// </summary>
    public class OpenMultipleDirectoriesDialogArguments : FileSystemDialogArguments
    {
        /// <summary>
        /// Creates a new <see cref="OpenMultipleDirectoriesDialogArguments" />.
        /// </summary>
        public OpenMultipleDirectoriesDialogArguments() : base() { }
    }

    /// <summary>
    /// The dialog result for <see cref="OpenMultipleDirectoriesDialogResult" />.
    /// </summary>
    public class OpenMultipleDirectoriesDialogResult : FileSystemDialogResult
    {
        /// <summary>
        /// The selected directories as <see cref="DirectoryInfo" />.
        /// </summary>
        public List<DirectoryInfo> DirectoryInfoList { get; private set; }

        /// <summary>
        /// The selected directories as full filename string.
        /// </summary>
        public List<string> Directories
        {
            get
            {
                return DirectoryInfoList
                    .Select(directoryInfo => directoryInfo.FullName)
                    .ToList();
            }
        }

        /// <summary>
        /// Creates a new <see cref="OpenMultipleDirectoriesDialogResult" />.
        /// </summary>
        /// <param name="canceled">True if the dialog was canceled</param>
        /// <param name="directoryInfoList">The list of selected directories</param>
        public OpenMultipleDirectoriesDialogResult(bool canceled, List<DirectoryInfo> directoryInfoList)
            : base(canceled)
        {
            if (directoryInfoList != null)
            {
                DirectoryInfoList = directoryInfoList
                    .OrderBy(directoryInfo => directoryInfo.FullName.ToLower())
                    .ToList();
            }
        }
    }
}
