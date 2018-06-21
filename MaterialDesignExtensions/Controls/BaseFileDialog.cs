using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// The base dialog with common logic for <see cref="OpenFileDialog" /> and <see cref="SaveFileDialog" />.
    /// </summary>
    public abstract class BaseFileDialog : FileSystemDialog
    {
        protected static readonly string FileControlName = "fileControl";

        /// <summary>
        /// The current file of the dialog.
        /// </summary>
        public static readonly DependencyProperty CurrentFileProperty = DependencyProperty.Register(
                nameof(CurrentFile),
                typeof(string),
                typeof(BaseFileDialog),
                new PropertyMetadata(null));

        /// <summary>
        /// The current file of the dialog.
        /// </summary>
        public string CurrentFile
        {
            get
            {
                return (string)GetValue(CurrentFileProperty);
            }

            set
            {
                SetValue(CurrentFileProperty, value);
            }
        }

        /// <summary>
        /// The possible file filters to select from for applying to the files inside the current directory.
        /// Strings according to the original .NET API will be converted automatically
        /// (see https://docs.microsoft.com/de-de/dotnet/api/microsoft.win32.filedialog.filter?view=netframework-4.7.1#Microsoft_Win32_FileDialog_Filter).
        /// </summary>
        public static readonly DependencyProperty FiltersProperty = DependencyProperty.Register(
            nameof(Filters),
            typeof(IList<IFileFilter>),
            typeof(BaseFileDialog),
            new PropertyMetadata(null));

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
            nameof(FilterIndex),
            typeof(int),
            typeof(BaseFileDialog),
            new PropertyMetadata(0));

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

        protected BaseFileControl m_fileControl;

        static BaseFileDialog()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BaseFileDialog), new FrameworkPropertyMetadata(typeof(BaseFileDialog)));
        }

        /// <summary>
        /// Creates a new <see cref="BaseFileDialog" />.
        /// </summary>
        public BaseFileDialog()
            : base()
        {
            m_fileControl = null;

            Loaded += LoadedHandler;
            Unloaded += UnloadedHandler;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (m_fileControl != null)
            {
                m_fileControl.Cancel -= CancelHandler;
                m_fileControl.FileSelected -= OpenDirectoryControlFileSelectedHandler;
            }

            m_fileControl = Template.FindName(FileControlName, this) as BaseFileControl;
        }

        protected void LoadedHandler(object sender, RoutedEventArgs args)
        {
            m_fileControl.Cancel += CancelHandler;
            m_fileControl.FileSelected += OpenDirectoryControlFileSelectedHandler;
        }

        protected void UnloadedHandler(object sender, RoutedEventArgs args)
        {
            m_fileControl.Cancel -= CancelHandler;
            m_fileControl.FileSelected -= OpenDirectoryControlFileSelectedHandler;
        }

        protected abstract void CancelHandler(object sender, RoutedEventArgs args);

        protected abstract void OpenDirectoryControlFileSelectedHandler(object sender, RoutedEventArgs args);
    }

    /// <summary>
    /// Arguments to initialize a file dialog.
    /// </summary>
    public abstract class FileDialogArguments : FileSystemDialogArguments
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
        /// Creates a new <see cref="FileDialogArguments" />.
        /// </summary>
        public FileDialogArguments()
            : base()
        {
            Filters = null;
            FilterIndex = 0;
        }
    }

    /// <summary>
    /// The base class for the dialog result of <see cref="OpenFileDialog" /> and <see cref="SaveFileDialog" />.
    /// </summary>
    public abstract class FileDialogResult : FileSystemDialogResult
    {
        /// <summary>
        /// The selected file as <see cref="FileInfo" />.
        /// </summary>
        public FileInfo FileInfo { get; private set; }

        /// <summary>
        /// The selected file as full filename string.
        /// </summary>
        public string File
        {
            get
            {
                return FileInfo?.FullName;
            }
        }

        /// <summary>
        /// Creates a new <see cref="FileDialogResult" />.
        /// </summary>
        /// <param name="canceled">True if the dialog was canceled</param>
        /// <param name="fileInfo">The selected file</param>
        public FileDialogResult(bool canceled, FileInfo fileInfo)
            : base(canceled)
        {
            FileInfo = fileInfo;
        }
    }
}
