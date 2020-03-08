using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using MaterialDesignThemes.Wpf;

// use Pri.LongPath classes instead of System.IO for the MaterialDesignExtensions.LongPath build to support long file system paths on older Windows and .NET versions
#if LONG_PATH
using DirectoryInfo = Pri.LongPath.DirectoryInfo;
#endif

namespace MaterialDesignExtensions.Controls
{
    /// <summary>
    /// A dialog for selecting a directory.
    /// </summary>
    public class OpenDirectoryDialog : FileSystemDialog
    {
        private static readonly string OpenDirectoryControlName = "openDirectoryControl";

        private OpenDirectoryControl m_openDirectoryControl;

        static OpenDirectoryDialog()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(OpenDirectoryDialog), new FrameworkPropertyMetadata(typeof(OpenDirectoryDialog)));
        }

        /// <summary>
        /// Creates a new <see cref="OpenDirectoryDialog" />.
        /// </summary>
        public OpenDirectoryDialog()
            : base()
        {
            m_openDirectoryControl = null;

            Loaded += LoadedHandler;
            Unloaded += UnloadedHandler;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (m_openDirectoryControl != null)
            {
                m_openDirectoryControl.Cancel -= CancelHandler;
                m_openDirectoryControl.DirectorySelected -= OpenDirectoryControlDirectorySelectedHandler;
            }

            m_openDirectoryControl = Template.FindName(OpenDirectoryControlName, this) as OpenDirectoryControl;
        }

        private void LoadedHandler(object sender, RoutedEventArgs args)
        {
            m_openDirectoryControl.Cancel += CancelHandler;
            m_openDirectoryControl.DirectorySelected += OpenDirectoryControlDirectorySelectedHandler;
        }

        private void UnloadedHandler(object sender, RoutedEventArgs args)
        {
            m_openDirectoryControl.Cancel -= CancelHandler;
            m_openDirectoryControl.DirectorySelected -= OpenDirectoryControlDirectorySelectedHandler;
        }

        private void CancelHandler(object sender, RoutedEventArgs args)
        {
            DialogHost.CloseDialogCommand.Execute(new OpenDirectoryDialogResult(true, null), GetDialogHost());
        }

        private void OpenDirectoryControlDirectorySelectedHandler(object sender, RoutedEventArgs args)
        {
            DialogHost.CloseDialogCommand.Execute(new OpenDirectoryDialogResult(false, (args as DirectorySelectedEventArgs)?.DirectoryInfo), GetDialogHost());
        }

        /// <summary>
        /// Shows a new <see cref="OpenDirectoryDialog" />.
        /// </summary>
        /// <param name="dialogHostName">The name of the <see cref="DialogHost" /></param>
        /// <param name="args">The arguments for the dialog initialization</param>
        /// <returns></returns>
        public static async Task<OpenDirectoryDialogResult> ShowDialogAsync(string dialogHostName, OpenDirectoryDialogArguments args)
        {
            OpenDirectoryDialog dialog = InitDialog(
                args.Width,
                args.Height,
                args.CurrentDirectory,
                args.CreateNewDirectoryEnabled,
                args.ShowHiddenFilesAndDirectories,
                args.ShowSystemFilesAndDirectories
            );

            return await DialogHost.Show(dialog, dialogHostName, args.OpenedHandler, args.ClosingHandler) as OpenDirectoryDialogResult;
        }

        /// <summary>
        /// Shows a new <see cref="OpenDirectoryDialog" />.
        /// </summary>
        /// <param name="dialogHost">The <see cref="DialogHost" /></param>
        /// <param name="args">The arguments for the dialog initialization</param>
        /// <returns></returns>
        public static async Task<OpenDirectoryDialogResult> ShowDialogAsync(DialogHost dialogHost, OpenDirectoryDialogArguments args)
        {
            OpenDirectoryDialog dialog = InitDialog(
                args.Width,
                args.Height,
                args.CurrentDirectory,
                args.CreateNewDirectoryEnabled,
                args.ShowHiddenFilesAndDirectories,
                args.ShowSystemFilesAndDirectories
            );

            return await dialogHost.ShowDialog(dialog, args.OpenedHandler, args.ClosingHandler) as OpenDirectoryDialogResult;
        }

        private static OpenDirectoryDialog InitDialog(double? width, double? height,
            string currentDirectory,
            bool createNewDirectoryEnabled,
            bool showHiddenFilesAndDirectories, bool showSystemFilesAndDirectories)
        {
            OpenDirectoryDialog dialog = new OpenDirectoryDialog();
            InitDialog(dialog, width, height, currentDirectory, showHiddenFilesAndDirectories, showSystemFilesAndDirectories, createNewDirectoryEnabled);

            return dialog;
        }
    }

    /// <summary>
    /// Arguments to initialize an open directory dialog.
    /// </summary>
    public class OpenDirectoryDialogArguments : FileSystemDialogArguments
    {
        /// <summary>
        /// Creates a new <see cref="OpenDirectoryDialogArguments" />.
        /// </summary>
        public OpenDirectoryDialogArguments() : base() { }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="args"></param>
        public OpenDirectoryDialogArguments(OpenDirectoryDialogArguments args)
            : base(args) { }
    }

    /// <summary>
    /// The dialog result for <see cref="OpenDirectoryDialog" />.
    /// </summary>
    public class OpenDirectoryDialogResult : FileSystemDialogResult
    {
        /// <summary>
        /// The selected directory as <see cref="DirectoryInfo" />.
        /// </summary>
        public DirectoryInfo DirectoryInfo { get; private set; }

        /// <summary>
        /// The selected directory as full filename string.
        /// </summary>
        public string Directory
        {
            get
            {
                return DirectoryInfo?.FullName;
            }
        }

        /// <summary>
        /// Creates a new <see cref="OpenDirectoryDialogResult" />.
        /// </summary>
        /// <param name="canceled">True if the dialog was canceled</param>
        /// <param name="directoryInfo">The selected directory</param>
        public OpenDirectoryDialogResult(bool canceled, DirectoryInfo directoryInfo)
            : base(canceled)
        {
            DirectoryInfo = directoryInfo;
        }
    }
}
