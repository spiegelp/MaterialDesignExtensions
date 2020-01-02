using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using MaterialDesignThemes.Wpf;

using MaterialDesignExtensions.Converters;
using MaterialDesignExtensions.Model;

namespace MaterialDesignExtensions.Controls
{
    /// <summary>
    /// A dialog for selecting multiple files.
    /// </summary>
    public class OpenMultipleFilesDialog : FileSystemDialog
    {
        private static readonly string OpenMultipleFilesControlName = "openMultipleFilesControl";

        /// <summary>
        /// The possible file filters to select from for applying to the files inside the current directory.
        /// Strings according to the original .NET API will be converted automatically
        /// (see https://docs.microsoft.com/de-de/dotnet/api/microsoft.win32.filedialog.filter?view=netframework-4.7.1#Microsoft_Win32_FileDialog_Filter).
        /// </summary>
        public static readonly DependencyProperty FiltersProperty = DependencyProperty.Register(
            nameof(Filters), typeof(IList<IFileFilter>), typeof(OpenMultipleFilesDialog), new PropertyMetadata(null));

        /// <summary>
        /// The possible file filters to select from for applying to the files inside the current directory.
        /// Strings according to the original .NET API will be converted automatically
        /// (see https://docs.microsoft.com/de-de/dotnet/api/microsoft.win32.filedialog.filter?view=netframework-4.7.1#Microsoft_Win32_FileDialog_Filter).
        /// </summary>
        [TypeConverter(typeof(FileFiltersTypeConverter))]
        public IList<IFileFilter> Filters
        {
            get
            {
                return (IList<IFileFilter>)GetValue(FiltersProperty);
            }

            set
            {
                SetValue(FiltersProperty, value);
            }
        }

        /// <summary>
        /// The index of the file filter to apply to the files inside the current directory.
        /// </summary>
        public static readonly DependencyProperty FilterIndexProperty = DependencyProperty.Register(
            nameof(FilterIndex), typeof(int), typeof(OpenMultipleFilesDialog), new PropertyMetadata(0));

        /// <summary>
        /// The index of the file filter to apply to the files inside the current directory.
        /// </summary>
        public int FilterIndex
        {
            get
            {
                return (int)GetValue(FilterIndexProperty);
            }

            set
            {
                SetValue(FilterIndexProperty, value);
            }
        }

        private OpenMultipleFilesControl m_openMultipleFilesControl;

        static OpenMultipleFilesDialog()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(OpenMultipleFilesDialog), new FrameworkPropertyMetadata(typeof(OpenMultipleFilesDialog)));
        }

        /// <summary>
        /// Creates a new <see cref="OpenMultipleFilesDialog" />.
        /// </summary>
        public OpenMultipleFilesDialog()
            : base()
        {
            m_openMultipleFilesControl = null;

            Loaded += LoadedHandler;
            Unloaded += UnloadedHandler;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (m_openMultipleFilesControl != null)
            {
                m_openMultipleFilesControl.Cancel -= CancelHandler;
                m_openMultipleFilesControl.FilesSelected -= OpenMultipleFilesControlFilesSelectedHandler;
            }

            m_openMultipleFilesControl = Template.FindName(OpenMultipleFilesControlName, this) as OpenMultipleFilesControl;
        }

        private void LoadedHandler(object sender, RoutedEventArgs args)
        {
            m_openMultipleFilesControl.Cancel += CancelHandler;
            m_openMultipleFilesControl.FilesSelected += OpenMultipleFilesControlFilesSelectedHandler;
        }

        private void UnloadedHandler(object sender, RoutedEventArgs args)
        {
            m_openMultipleFilesControl.Cancel -= CancelHandler;
            m_openMultipleFilesControl.FilesSelected -= OpenMultipleFilesControlFilesSelectedHandler;
        }

        private void CancelHandler(object sender, RoutedEventArgs args)
        {
            DialogHost.CloseDialogCommand.Execute(new OpenMultipleFilesDialogResult(true, null), GetDialogHost());
        }

        private void OpenMultipleFilesControlFilesSelectedHandler(object sender, RoutedEventArgs args)
        {
            DialogHost.CloseDialogCommand.Execute(new OpenMultipleFilesDialogResult(false, (args as FilesSelectedEventArgs)?.FileInfoList), GetDialogHost());
        }

        /// <summary>
        /// Shows a new <see cref="OpenMultipleFilesDialog" />.
        /// </summary>
        /// <param name="dialogHostName">The name of the <see cref="DialogHost" /></param>
        /// <param name="args">The arguments for the dialog initialization</param>
        /// <returns></returns>
        public static async Task<OpenMultipleFilesDialogResult> ShowDialogAsync(string dialogHostName, OpenMultipleFilesDialogArguments args)
        {
            OpenMultipleFilesDialog dialog = InitDialog(
                args.Width,
                args.Height,
                args.CurrentDirectory,
                args.Filters,
                args.FilterIndex,
                args.ShowHiddenFilesAndDirectories,
                args.ShowSystemFilesAndDirectories
            );

            return await DialogHost.Show(dialog, dialogHostName, args.OpenedHandler, args.ClosingHandler) as OpenMultipleFilesDialogResult;
        }

        /// <summary>
        /// Shows a new <see cref="OpenMultipleFilesDialog" />.
        /// </summary>
        /// <param name="dialogHost">The <see cref="DialogHost" /></param>
        /// <param name="args">The arguments for the dialog initialization</param>
        /// <returns></returns>
        public static async Task<OpenMultipleFilesDialogResult> ShowDialogAsync(DialogHost dialogHost, OpenMultipleFilesDialogArguments args)
        {
            OpenMultipleFilesDialog dialog = InitDialog(
                args.Width,
                args.Height,
                args.CurrentDirectory,
                args.Filters,
                args.FilterIndex,
                args.ShowHiddenFilesAndDirectories,
                args.ShowSystemFilesAndDirectories
            );

            return await dialogHost.ShowDialog(dialog, args.OpenedHandler, args.ClosingHandler) as OpenMultipleFilesDialogResult;
        }

        private static OpenMultipleFilesDialog InitDialog(double? width, double? height,
            string currentDirectory,
            string filters, int filterIndex,
            bool showHiddenFilesAndDirectories, bool showSystemFilesAndDirectories)
        {
            OpenMultipleFilesDialog dialog = new OpenMultipleFilesDialog();
            InitDialog(dialog, width, height, currentDirectory, showHiddenFilesAndDirectories, showSystemFilesAndDirectories);
            dialog.Filters = new FileFiltersTypeConverter().ConvertFrom(null, null, filters) as IList<IFileFilter>;
            dialog.FilterIndex = filterIndex;

            return dialog;
        }
    }

    /// <summary>
    /// Arguments to initialize an open multiple files dialog.
    /// </summary>
    public class OpenMultipleFilesDialogArguments : FileSystemDialogArguments
    {
        /// <summary>
        /// The possible file filters to select from for applying to the files inside the current directory.
        /// Strings according to the original .NET API will be converted automatically
        /// (see https://docs.microsoft.com/de-de/dotnet/api/microsoft.win32.filedialog.filter?view=netframework-4.7.1#Microsoft_Win32_FileDialog_Filter).
        /// </summary>
        public string Filters { get; set; }

        /// <summary>
        /// The index of the file filter to apply to the files inside the current directory.
        /// </summary>
        public int FilterIndex { get; set; }

        /// <summary>
        /// Creates a new <see cref="OpenMultipleFilesDialogArguments" />.
        /// </summary>
        public OpenMultipleFilesDialogArguments()
            : base()
        {
            Filters = null;
            FilterIndex = 0;
        }
    }

    /// <summary>
    /// The dialog result for <see cref="OpenMultipleFilesDialog" />.
    /// </summary>
    public class OpenMultipleFilesDialogResult : FileSystemDialogResult
    {

        /// <summary>
        /// The selected files as <see cref="FileInfo" />.
        /// </summary>
        public List<FileInfo> FileInfoList { get; private set; }

        /// <summary>
        /// The selected files as full filename string.
        /// </summary>
        public List<string> Files
        {
            get
            {
                return FileInfoList
                    .Select(fileInfo => fileInfo.FullName)
                    .ToList();
            }
        }

        /// <summary>
        /// Creates a new <see cref="OpenMultipleFilesDialogResult" />.
        /// </summary>
        /// <param name="canceled">True if the dialog was canceled</param>
        /// <param name="fileInfoList">The list of selected files</param>
        public OpenMultipleFilesDialogResult(bool canceled, List<FileInfo> fileInfoList)
            : base(canceled)
        {
            if (fileInfoList != null)
            {
                FileInfoList = fileInfoList
                    .OrderBy(fileInfo => fileInfo.FullName.ToLower())
                    .ToList();
            }
        }
    }
}
