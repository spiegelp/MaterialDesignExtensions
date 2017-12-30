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

        public static async Task<OpenDirectoryDialogResult> ShowDialogAsync(string dialogHostName, double? width = null, double? height = null,
            string currentDirectory = null,
            bool showHiddenFilesAndDirectories = false, bool showSystemFilesAndDirectories = false,
            DialogOpenedEventHandler openedHandler = null, DialogClosingEventHandler closingHandler = null)
        {
            OpenDirectoryDialog dialog = InitDialog(width, height, currentDirectory, showHiddenFilesAndDirectories, showSystemFilesAndDirectories);

            return await DialogHost.Show(dialog, dialogHostName, openedHandler, closingHandler) as OpenDirectoryDialogResult;
        }

        public static async Task<OpenDirectoryDialogResult> ShowDialogAsync(DialogHost dialogHost, double? width = null, double? height = null,
            string currentDirectory = null,
            bool showHiddenFilesAndDirectories = false, bool showSystemFilesAndDirectories = false,
            DialogOpenedEventHandler openedHandler = null, DialogClosingEventHandler closingHandler = null)
        {
            OpenDirectoryDialog dialog = InitDialog(width, height, currentDirectory, showHiddenFilesAndDirectories, showSystemFilesAndDirectories);

            return await dialogHost.ShowDialog(dialog, openedHandler, closingHandler) as OpenDirectoryDialogResult;
        }

        private static OpenDirectoryDialog InitDialog(double? width, double? height,
            string currentDirectory,
            bool showHiddenFilesAndDirectories, bool showSystemFilesAndDirectories)
        {
            OpenDirectoryDialog dialog = new OpenDirectoryDialog();
            InitDialog(dialog, width, height, currentDirectory, showHiddenFilesAndDirectories, showSystemFilesAndDirectories);

            return dialog;
        }
    }

    public class OpenDirectoryDialogResult : FileSystemDialogResult
    {
        /// <summary>
        /// The selected directory as <see cref="DirectoryInfo" />
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

        public OpenDirectoryDialogResult(bool canceled, DirectoryInfo directoryInfo)
            : base(canceled)
        {
            DirectoryInfo = directoryInfo;
        }
    }
}
