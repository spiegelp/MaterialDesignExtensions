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
    /// The base control with common logic for <see cref="OpenFileControl" /> and <see cref="SaveFileControl" />.
    /// </summary>
    public abstract class BaseFileControl : FileSystemControl
    {
        protected const string FileFiltersComboBoxName = "fileFiltersComboBox";

        /// <summary>
        /// Internal command used by the XAML template (public to be available in the XAML template). Not intended for external usage.
        /// </summary>
        public static readonly RoutedCommand SelectFileCommand = new RoutedCommand();

        /// <summary>
        /// An event raised by selecting a file.
        /// </summary>
        public static readonly RoutedEvent FileSelectedEvent = EventManager.RegisterRoutedEvent(
            nameof(FileSelected), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(BaseFileControl));

        /// <summary>
        /// An event raised by selecting a file.
        /// </summary>
        public event RoutedEventHandler FileSelected
        {
            add
            {
                AddHandler(FileSelectedEvent, value);
            }

            remove
            {
                RemoveHandler(FileSelectedEvent, value);
            }
        }

        /// <summary>
        /// True, to clear the cache during unloading the control.
        /// </summary>
        public static readonly DependencyProperty ClearCacheOnUnloadProperty = DependencyProperty.Register(
                nameof(ClearCacheOnUnload),
                typeof(bool),
                typeof(FileSystemControl),
                new PropertyMetadata(true));

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
        /// The current file of the control.
        /// </summary>
        public static readonly DependencyProperty CurrentFileProperty = DependencyProperty.Register(
                nameof(CurrentFile),
                typeof(string),
                typeof(BaseFileControl),
                new PropertyMetadata(null, CurrentFileChangedHandler));

        /// <summary>
        /// The current file of the control.
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
        /// An command called by by selecting a file.
        /// </summary>
        public static readonly DependencyProperty FileSelectedCommandProperty = DependencyProperty.Register(
            nameof(FileSelectedCommand), typeof(ICommand), typeof(BaseFileControl), new PropertyMetadata(null, null));

        /// <summary>
        /// An command called by by selecting a file.
        /// </summary>
        public ICommand FileSelectedCommand
        {
            get
            {
                return (ICommand)GetValue(FileSelectedCommandProperty);
            }

            set
            {
                SetValue(FileSelectedCommandProperty, value);
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
            typeof(BaseFileControl),
            new PropertyMetadata(null, FiltersChangedHandler));

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
            typeof(BaseFileControl),
            new PropertyMetadata(0, FiltersChangedHandler));

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
            nameof(GroupFoldersAndFiles),
            typeof(bool),
            typeof(BaseFileControl),
            new PropertyMetadata(true));

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

        protected ComboBox m_fileFiltersComboBox;

        /// <summary>
        /// Creates a new <see cref="BaseFileControl" />.
        /// </summary>
        public BaseFileControl()
            : base()
        {
            m_fileFiltersComboBox = null;

            CommandBindings.Add(new CommandBinding(SelectFileCommand, SelectFileCommandHandler));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            m_fileFiltersComboBox = Template.FindName(FileFiltersComboBoxName, this) as ComboBox;
            m_fileFiltersComboBox.ItemsSource = Filters;

            UpdateFileFiltersVisibility();
        }

        protected override void UnloadedHandler(object sender, RoutedEventArgs args)
        {
            BitmapImageHelper.ClearCache();

            base.UnloadedHandler(sender, args);
        }

        protected virtual void SelectFileCommandHandler(object sender, ExecutedRoutedEventArgs args)
        {
            try
            {
                FileSelectedEventArgs eventArgs = new FileSelectedEventArgs(FileSelectedEvent, this, m_controller.CurrentFileFullName);
                RaiseEvent(eventArgs);

                if (FileSelectedCommand != null && FileSelectedCommand.CanExecute(m_controller.CurrentFileFullName))
                {
                    FileSelectedCommand.Execute(m_controller.CurrentFileFullName);
                }
            }
            catch (PathTooLongException)
            {
                SnackbarMessageQueue.Enqueue(Localization.Strings.LongPathsAreNotSupported);
            }
        }

        private static void CurrentFileChangedHandler(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            (obj as BaseFileControl)?.CurrentFileChangedHandler(args.NewValue as string);
        }

        protected abstract void CurrentFileChangedHandler(string newCurrentFile);

        private static void FiltersChangedHandler(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (args.Property == FiltersProperty)
            {
                (obj as BaseFileControl)?.FileFiltersChangedHandler(args.NewValue as IList<IFileFilter>);
            }
            else if (args.Property == FilterIndexProperty)
            {
                (obj as BaseFileControl)?.FileFiltersChangedHandler((int)args.NewValue);
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
                    m_fileSystemEntryItemsControl.ItemsSource = GetFileSystemEntryItems(items);

                    if (items != null && items.Any())
                    {
                        ItemsScrollViewer?.ScrollToTop();
                    }

                    UpdateListVisibility();
                }
                else if (args.PropertyName == nameof(FileSystemController.CurrentFileFullName))
                {
                    if (m_controller.CurrentFileFullName != null)
                    {
                        CurrentFile = m_controller.CurrentFileFullName;
                    }
                    else
                    {
                        CurrentFile = null;
                    }

                    UpdateSelection();
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
            }

            base.ControllerPropertyChangedHandler(sender, args);
        }

        protected override IEnumerable GetFileSystemEntryItems()
        {
            return GetFileSystemEntryItems(m_controller.DirectoriesAndFiles);
        }

        protected IEnumerable GetFileSystemEntryItems(List<FileSystemInfo> directoriesAndFiles)
        {
            if (directoriesAndFiles == null || !directoriesAndFiles.Any())
            {
                return new ArrayList(0);
            }

            int numberOfItems = directoriesAndFiles.Count;

            if (GroupFoldersAndFiles)
            {
                numberOfItems = numberOfItems + 2;
            }

            ArrayList items = new ArrayList(numberOfItems);

            for (int i = 0; i < directoriesAndFiles.Count; i++)
            {
                FileSystemInfo item = directoriesAndFiles[i];

                if (item is DirectoryInfo directoryInfo)
                {
                    if (GroupFoldersAndFiles && i == 0)
                    {
                        items.Add(new FileSystemEntriesGroupHeader() { Header = Localization.Strings.Folders, ShowSeparator = false });
                    }

                    bool isSelected = directoryInfo.FullName == m_controller.CurrentDirectory?.FullName;

                    items.Add(new DirectoryInfoItem() { IsSelected = isSelected, Value = directoryInfo });
                }
                else if (item is FileInfo fileInfo)
                {
                    if (GroupFoldersAndFiles)
                    {
                        if (i == 0)
                        {
                            items.Add(new FileSystemEntriesGroupHeader() { Header = Localization.Strings.Files, ShowSeparator = false });
                        }
                        else if (directoriesAndFiles[i - 1] is DirectoryInfo)
                        {
                            items.Add(new FileSystemEntriesGroupHeader() { Header = Localization.Strings.Files, ShowSeparator = true });
                        }
                    }

                    bool isSelected = fileInfo.FullName == m_controller.CurrentFileFullName;

                    items.Add(new FileInfoItem() { IsSelected = isSelected, Value = fileInfo });
                }
            }

            return items;
        }

        protected void UpdateFileFiltersVisibility()
        {
            if (m_fileFiltersComboBox != null
                && m_fileFiltersComboBox.ItemsSource != null
                && m_fileFiltersComboBox.ItemsSource.GetEnumerator().MoveNext())
            {
                m_fileFiltersComboBox.Visibility = Visibility.Visible;
            }
            else
            {
                m_fileFiltersComboBox.Visibility = Visibility.Collapsed;
            }
        }
    }

    /// <summary>
    /// The arguments for the <see cref="BaseFileControl.FileSelected" /> event.
    /// </summary>
    public class FileSelectedEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// The selected file as <see cref="FileInfo" />.
        /// </summary>
        public FileInfo FileInfo { get; private set; }

        /// <summary>
        /// The selected file as full filename string.
        /// </summary>
        public string File { get; private set; }

        /// <summary>
        /// Creates a new <see cref="FileSelectedEventArgs" />.
        /// </summary>
        /// <param name="routedEvent"></param>
        /// <param name="source">The source object</param>
        /// <param name="fileInfo">The selected file</param>
        public FileSelectedEventArgs(RoutedEvent routedEvent, object source, FileInfo fileInfo)
            : base(routedEvent, source)
        {
            FileInfo = fileInfo;
            File = fileInfo?.FullName;
        }

        /// <summary>
        /// Creates a new <see cref="FileSelectedEventArgs" />.
        /// </summary>
        /// <param name="routedEvent"></param>
        /// <param name="source">The source object</param>
        /// <param name="file">The full filename of the selected file</param>
        public FileSelectedEventArgs(RoutedEvent routedEvent, object source, string file)
            : base(routedEvent, source)
        {
            File = file;

            if (!string.IsNullOrWhiteSpace(file))
            {
                FileInfo = new FileInfo(file);
            }
        }
    }
}
