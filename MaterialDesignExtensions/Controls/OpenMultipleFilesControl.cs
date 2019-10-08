using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using MaterialDesignThemes.Wpf;

using MaterialDesignExtensions.Controllers;
using MaterialDesignExtensions.Converters;
using MaterialDesignExtensions.Model;

// use Pri.LongPath classes instead of System.IO for the MaterialDesignExtensions.LongPath build to support long file system paths on older Windows and .NET versions
#if LONG_PATH
using FileSystemInfo = Pri.LongPath.FileSystemInfo;
using DirectoryInfo = Pri.LongPath.DirectoryInfo;
using FileInfo = Pri.LongPath.FileInfo;
#endif

namespace MaterialDesignExtensions.Controls
{
    /// <summary>
    /// A control for selecting multiple files.
    /// </summary>
    public class OpenMultipleFilesControl : FileSystemControl
    {
        private const string FileFiltersComboBoxName = "fileFiltersComboBox";
        private const string SelectionItemsControlName = "selectionItemsControl";

        /// <summary>
        /// Internal command used by the XAML template (public to be available in the XAML template). Not intended for external usage.
        /// </summary>
        public static readonly RoutedCommand OpenSelectionDrawerCommand = new RoutedCommand();

        /// <summary>
        /// Internal command used by the XAML template (public to be available in the XAML template). Not intended for external usage.
        /// </summary>
        public static readonly RoutedCommand SelectFileCommand = new RoutedCommand();

        /// <summary>
        /// Internal command used by the XAML template (public to be available in the XAML template). Not intended for external usage.
        /// </summary>
        public static readonly RoutedCommand SelectMultipleFilesCommand = new RoutedCommand();

        /// <summary>
        /// An event raised by selecting files to open.
        /// </summary>
        public static readonly RoutedEvent FilesSelectedEvent = EventManager.RegisterRoutedEvent(
            nameof(FilesSelected), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(OpenMultipleFilesControl));

        /// <summary>
        /// An event raised by selecting files to open.
        /// </summary>
        public event RoutedEventHandler FilesSelected
        {
            add
            {
                AddHandler(FilesSelectedEvent, value);
            }

            remove
            {
                RemoveHandler(FilesSelectedEvent, value);
            }
        }

        /// <summary>
        /// True, to clear the cache during unloading the control.
        /// </summary>
        public static readonly DependencyProperty ClearCacheOnUnloadProperty = DependencyProperty.Register(
            nameof(ClearCacheOnUnload), typeof(bool), typeof(OpenMultipleFilesControl), new PropertyMetadata(true));

        /// <summary>
        /// True, to clear the cache during unloading the control.
        /// </summary>
        public bool ClearCacheOnUnload
        {
            get
            {
                return (bool)GetValue(ClearCacheOnUnloadProperty);
            }

            set
            {
                SetValue(ClearCacheOnUnloadProperty, value);
            }
        }

        /// <summary>
        /// An command called by selecting files to open.
        /// </summary>
        public static readonly DependencyProperty FilesSelectedCommandProperty = DependencyProperty.Register(
            nameof(FilesSelectedCommand), typeof(ICommand), typeof(OpenMultipleFilesControl), new PropertyMetadata(null, null));

        /// <summary>
        /// An command called by selecting files to open.
        /// </summary>
        public ICommand FilesSelectedCommand
        {
            get
            {
                return (ICommand)GetValue(FilesSelectedCommandProperty);
            }

            set
            {
                SetValue(FilesSelectedCommandProperty, value);
            }
        }

        /// <summary>
        /// The possible file filters to select from for applying to the files inside the current directory.
        /// Strings according to the original .NET API will be converted automatically
        /// (see https://docs.microsoft.com/de-de/dotnet/api/microsoft.win32.filedialog.filter?view=netframework-4.7.1#Microsoft_Win32_FileDialog_Filter).
        /// </summary>
        public static readonly DependencyProperty FiltersProperty = DependencyProperty.Register(
            nameof(Filters), typeof(IList<IFileFilter>), typeof(OpenMultipleFilesControl), new PropertyMetadata(null, FiltersChangedHandler));

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
            nameof(FilterIndex), typeof(int), typeof(OpenMultipleFilesControl), new PropertyMetadata(0, FiltersChangedHandler));

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

        /// <summary>
        /// Shows folders and files as a group with a header.
        /// </summary>
        public static readonly DependencyProperty GroupFoldersAndFilesProperty = DependencyProperty.Register(
            nameof(GroupFoldersAndFiles), typeof(bool), typeof(OpenMultipleFilesControl), new PropertyMetadata(true));

        /// <summary>
        /// Shows folders and files as a group with a header.
        /// </summary>
        public bool GroupFoldersAndFiles
        {
            get
            {
                return (bool)GetValue(GroupFoldersAndFilesProperty);
            }

            set
            {
                SetValue(GroupFoldersAndFilesProperty, value);
            }
        }

        private ComboBox m_fileFiltersComboBox;
        private ItemsControl m_selectionItemsControl;

        static OpenMultipleFilesControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(OpenMultipleFilesControl), new FrameworkPropertyMetadata(typeof(OpenMultipleFilesControl)));
        }

        /// <summary>
        /// Creates a new <see cref="OpenMultipleFilesControl" />.
        /// </summary>
        public OpenMultipleFilesControl()
            : base()
        {
            m_fileFiltersComboBox = null;
            m_selectionItemsControl = null;

            CommandBindings.Add(new CommandBinding(OpenSelectionDrawerCommand, OpenSelectionDrawerCommandHandler));
            CommandBindings.Add(new CommandBinding(SelectFileCommand, SelectFileCommandHandler));
            CommandBindings.Add(new CommandBinding(SelectMultipleFilesCommand, SelectMultipleFilesCommandHandler, CanExecuteSelectMultipleFilesCommand));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            m_fileFiltersComboBox = Template.FindName(FileFiltersComboBoxName, this) as ComboBox;
            m_fileFiltersComboBox.ItemsSource = Filters;

            m_selectionItemsControl = Template.FindName(SelectionItemsControlName, this) as ItemsControl;

            UpdateFileFiltersVisibility();
            UpdateSelectionList();
        }

        protected override void UnloadedHandler(object sender, RoutedEventArgs args)
        {
            if (ClearCacheOnUnload)
            {
                BitmapImageHelper.ClearCache();
            }

            base.UnloadedHandler(sender, args);
        }

        private void OpenSelectionDrawerCommandHandler(object sender, ExecutedRoutedEventArgs args)
        {
            DrawerHost drawerHost = (DrawerHost)Template.FindName(DrawerHostName, this);
            drawerHost.IsRightDrawerOpen = true;
        }

        private void SelectFileCommandHandler(object sender, ExecutedRoutedEventArgs args)
        {
            try
            {
                if (args.Parameter != null)
                {
                    if (args.Parameter is FileInfo fileInfo)
                    {
                        m_controller.SelectOrRemoveFileForMultipleSelection(fileInfo);
                    }
                }
            }
            catch (PathTooLongException)
            {
                SnackbarMessageQueue.Enqueue(Localization.Strings.LongPathsAreNotSupported);
            }
        }

        private void SelectMultipleFilesCommandHandler(object sender, ExecutedRoutedEventArgs args)
        {
            FilesSelectedEventArgs eventArgs = new FilesSelectedEventArgs(FilesSelectedEvent, this, m_controller.SelectedFiles.ToList());
            RaiseEvent(eventArgs);

            if (FilesSelectedCommand != null && FilesSelectedCommand.CanExecute(eventArgs.FileInfoList))
            {
                FilesSelectedCommand.Execute(eventArgs.FileInfoList);
            }
        }

        private void CanExecuteSelectMultipleFilesCommand(object sender, CanExecuteRoutedEventArgs args)
        {
            args.CanExecute = m_controller != null && m_controller.SelectedFiles != null && m_controller.SelectedFiles.Any();
        }

        private static void FiltersChangedHandler(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (args.Property == FiltersProperty)
            {
                (obj as OpenMultipleFilesControl)?.FileFiltersChangedHandler(args.NewValue as IList<IFileFilter>);
            }
            else if (args.Property == FilterIndexProperty)
            {
                (obj as OpenMultipleFilesControl)?.FileFiltersChangedHandler((int)args.NewValue);
            }
        }

        private void FileFiltersChangedHandler(IList<IFileFilter> newFilters)
        {
            FileFiltersChangedHandler(newFilters, FilterIndex);
        }

        private void FileFiltersChangedHandler(int newFilterIndex)
        {
            FileFiltersChangedHandler(Filters, newFilterIndex);
        }

        private void FileFiltersChangedHandler(IList<IFileFilter> newFilters, int newFilterIndex)
        {
            IFileFilter fileFilter = null;

            if (newFilters != null && newFilterIndex >= 0 && newFilterIndex < newFilters.Count)
            {
                fileFilter = newFilters[FilterIndex];
            }

            m_controller.SetFileFilter(newFilters, fileFilter);
        }

        protected override void ControllerPropertyChangedHandler(object sender, PropertyChangedEventArgs args)
        {
            if (sender == m_controller)
            {
                if (args.PropertyName == nameof(FileSystemController.DirectoriesAndFiles))
                {
                    List<FileSystemInfo> items = m_controller.DirectoriesAndFiles;
                    m_fileSystemEntryItemsControl.ItemsSource = GetFileSystemEntryItems();

                    if (items != null && items.Any())
                    {
                        ItemsScrollViewer?.ScrollToTop();
                    }

                    UpdateListVisibility();
                }
                else if (args.PropertyName == nameof(FileSystemController.FileFilters))
                {
                    Filters = m_controller.FileFilters;
                    m_fileFiltersComboBox.ItemsSource = m_controller.FileFilters;

                    UpdateFileFiltersVisibility();
                }
                else if (args.PropertyName == nameof(FileSystemController.FileFilterToApply))
                {
                    int filterIndex = -1;

                    if (m_controller.FileFilterToApply != null && m_controller.FileFilters != null)
                    {
                        for (int i = 0; i < m_controller.FileFilters.Count && filterIndex == -1; i++)
                        {
                            if (m_controller.FileFilters[i] == m_controller.FileFilterToApply)
                            {
                                filterIndex = i;
                            }
                        }
                    }

                    FilterIndex = filterIndex;
                }
                else if (args.PropertyName == nameof(FileSystemController.SelectedFiles))
                {
                    UpdateSelection();
                    UpdateSelectionList();
                }
            }

            base.ControllerPropertyChangedHandler(sender, args);
        }

        protected override IEnumerable GetFileSystemEntryItems()
        {
            ISet<string> fileNames = new HashSet<string>(m_controller.SelectedFiles.Select(fileInfo => fileInfo.FullName.ToLower()));

            return FileControlHelper.GetFileSystemEntryItems(
                m_controller.DirectoriesAndFiles,
                m_controller,
                GroupFoldersAndFiles,
                fileInfo => fileNames.Contains(fileInfo.FullName.ToLower())
            );
        }

        protected void UpdateFileFiltersVisibility()
        {
            FileControlHelper.UpdateFileFiltersVisibility(m_fileFiltersComboBox);
        }

        protected override void UpdateSelection()
        {
            IEnumerable items = m_fileSystemEntryItemsControl?.ItemsSource;

            if (items != null)
            {
                ISet<string> fileNames = new HashSet<string>(m_controller.SelectedFiles.Select(fileInfo => fileInfo.FullName.ToLower()));

                foreach (object item in items)
                {
                    if (item is FileInfoItem fileInfoItem)
                    {
                        fileInfoItem.IsSelected = fileNames.Contains(fileInfoItem.Value.FullName.ToLower());
                    }
                }
            }
        }

        private void UpdateSelectionList()
        {
            m_selectionItemsControl.ItemsSource = m_controller.SelectedFiles
                .OrderBy(file => file.Name.ToLower())
                .ThenBy(file => file.Directory.FullName.ToLower());
        }
    }

    /// <summary>
    /// The arguments for the <see cref="OpenMultipleFilesControl.FilesSelected" /> event.
    /// </summary>
    public class FilesSelectedEventArgs : RoutedEventArgs
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
        /// Creates a new <see cref="FilesSelectedEventArgs" />.
        /// </summary>
        /// <param name="routedEvent"></param>
        /// <param name="source">The source object</param>
        /// <param name="fileInfoList">The list selected files</param>
        public FilesSelectedEventArgs(RoutedEvent routedEvent, object source, List<FileInfo> fileInfoList)
            : base(routedEvent, source)
        {
            FileInfoList = fileInfoList
                .OrderBy(directoryInfo => directoryInfo.FullName.ToLower())
                .ToList();
        }
    }
}
