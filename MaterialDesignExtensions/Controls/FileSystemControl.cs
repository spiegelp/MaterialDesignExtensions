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
using System.Windows.Media;

using MaterialDesignThemes.Wpf;

using MaterialDesignExtensions.Controllers;
using MaterialDesignExtensions.Model;

namespace MaterialDesignExtensions.Controls
{
    /// <summary>
    /// The base control with common logic for <see cref="OpenDirectoryControl" />, <see cref="OpenFileControl" /> and <see cref="SaveFileControl" />.
    /// </summary>
    public abstract class FileSystemControl : Control
    {
        protected const string DrawerHostName = "drawerHost";
        protected const string PathPartsItemsControlName = "pathPartsItemsControl";
        protected const string FileSystemEntryItemsScrollViewerName = "fileSystemEntryItemsScrollViewer";
        protected const string FileSystemEntryItemsControlName = "fileSystemEntryItemsControl";
        protected const string EmptyDirectoryTextBlockName = "emptyDirectoryTextBlock";

        public static RoutedCommand OpenSpecialDirectoriesDrawerCommand = new RoutedCommand();
        public static RoutedCommand SelectDirectoryItemCommand = new RoutedCommand();
        public static RoutedCommand SelectFileSystemEntryCommand = new RoutedCommand();
        public static RoutedCommand ShowInfoCommand = new RoutedCommand();
        public static RoutedCommand CancelCommand = new RoutedCommand();

        public static readonly RoutedEvent CancelEvent = EventManager.RegisterRoutedEvent(
            nameof(Cancel), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(FileSystemControl));

        /// <summary>
        /// An event raised by canceling the operation.
        /// </summary>
        public event RoutedEventHandler Cancel
        {
            add
            {
                AddHandler(CancelEvent, value);
            }

            remove
            {
                RemoveHandler(CancelEvent, value);
            }
        }

        /// <summary>
        /// The underlying controller for file system logic. This property is intended for internal use only.
        /// </summary>
        public FileSystemController Controller
        {
            get
            {
                return m_controller;
            }
        }

        public static readonly DependencyProperty CurrentDirectoryProperty = DependencyProperty.Register(
                nameof(CurrentDirectory),
                typeof(string),
                typeof(FileSystemControl),
                new PropertyMetadata(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), CurrentDirectoryChangedHandler));

        /// <summary>
        /// The current directory of the control.
        /// </summary>
        public string CurrentDirectory
        {
            get
            {
                return (string)GetValue(CurrentDirectoryProperty);
            }

            set
            {
                SetValue(CurrentDirectoryProperty, value);
            }
        }

        public static readonly DependencyProperty FileSystemInfoToShowProperty = DependencyProperty.Register(
                nameof(FileSystemInfoToShow),
                typeof(FileSystemInfo),
                typeof(FileSystemControl),
                new PropertyMetadata(null));

        /// <summary>
        /// The currently selected directory or file to display additional information about it.
        /// </summary>
        public FileSystemInfo FileSystemInfoToShow
        {
            get
            {
                return (FileSystemInfo)GetValue(FileSystemInfoToShowProperty);
            }

            protected set
            {
                SetValue(FileSystemInfoToShowProperty, value);
            }
        }

        /// <summary>
        /// The ScrollViewer of the ItemsControls with the file system entries
        /// </summary>
        protected ScrollViewer ItemsScrollViewer
        {
            get
            {
                if (m_fileSystemEntryItemsScrollViewer == null)
                {
                    ScrollViewer FindItemsScrollViewer(ItemsControl itemsControl)
                    {
                        FrameworkElement control = itemsControl;

                        while (control != null && !(control is ScrollViewer))
                        {
                            if (VisualTreeHelper.GetChildrenCount(control) == 0)
                            {
                                return null;
                            }

                            control = VisualTreeHelper.GetChild(control, 0) as FrameworkElement;
                        }

                        return control as ScrollViewer;
                    };

                    m_fileSystemEntryItemsScrollViewer = FindItemsScrollViewer(m_fileSystemEntryItemsControl);
                }

                return m_fileSystemEntryItemsScrollViewer;
            }
        }

        public static readonly DependencyProperty ShowHiddenFilesAndDirectoriesProperty = DependencyProperty.Register(
                nameof(ShowHiddenFilesAndDirectories),
                typeof(bool),
                typeof(FileSystemControl),
                new PropertyMetadata(false, ShowHiddenFilesAndDirectoriesChangedHandler));

        /// <summary>
        /// Shows or hides hidden directories and files.
        /// </summary>
        public bool ShowHiddenFilesAndDirectories
        {
            get
            {
                return (bool)GetValue(ShowHiddenFilesAndDirectoriesProperty);
            }

            set
            {
                SetValue(ShowHiddenFilesAndDirectoriesProperty, value);
            }
        }

        public static readonly DependencyProperty ShowSystemFilesAndDirectoriesProperty = DependencyProperty.Register(
                nameof(ShowSystemFilesAndDirectories),
                typeof(bool),
                typeof(FileSystemControl),
                new PropertyMetadata(false, ShowSystemFilesAndDirectoriesChangedHandler));

        /// <summary>
        /// Shows or hides protected directories and files of the system.
        /// </summary>
        public bool ShowSystemFilesAndDirectories
        {
            get
            {
                return (bool)GetValue(ShowSystemFilesAndDirectoriesProperty);
            }

            set
            {
                SetValue(ShowSystemFilesAndDirectoriesProperty, value);
            }
        }

        public static readonly DependencyProperty SnackbarMessageQueueProperty = DependencyProperty.Register(
                nameof(SnackbarMessageQueue),
                typeof(ISnackbarMessageQueue),
                typeof(FileSystemControl),
                new PropertyMetadata(new SnackbarMessageQueue(TimeSpan.FromSeconds(5))));

        /// <summary>
        /// The message queue for the Snackbar. This property is intended for internal use.
        /// </summary>
        public ISnackbarMessageQueue SnackbarMessageQueue
        {
            get
            {
                return (ISnackbarMessageQueue)GetValue(SnackbarMessageQueueProperty);
            }

            protected set
            {
                SetValue(SnackbarMessageQueueProperty, value);
            }
        }

        protected FileSystemController m_controller;

        protected ItemsControl m_pathPartsItemsControl;
        // use an ItemsControl instead of a ListBox, because the ListBox raises several selection changed events without an explicit user input
        protected ItemsControl m_fileSystemEntryItemsControl;
        // private to force the usage of the lazy getter, because it only works after applying the template
        private ScrollViewer m_fileSystemEntryItemsScrollViewer;
        protected TextBlock m_emptyDirectoryTextBlock;

        static FileSystemControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FileSystemControl), new FrameworkPropertyMetadata(typeof(FileSystemControl)));
        }

        public FileSystemControl()
            : base()
        {
            m_controller = new FileSystemController();
            m_controller.SelectDirectory(CurrentDirectory);

            CommandBindings.Add(new CommandBinding(OpenSpecialDirectoriesDrawerCommand, OpenSpecialDirectoriesDrawerCommandHandler));
            CommandBindings.Add(new CommandBinding(SelectDirectoryItemCommand, SelectDirectoryItemCommandHandler));
            CommandBindings.Add(new CommandBinding(SelectFileSystemEntryCommand, SelectFileSystemEntryCommandHandler));
            CommandBindings.Add(new CommandBinding(ShowInfoCommand, ShowInfoCommandHandler));
            CommandBindings.Add(new CommandBinding(CancelCommand, CancelCommandHandler));

            m_pathPartsItemsControl = null;
            m_fileSystemEntryItemsScrollViewer = null;
            m_fileSystemEntryItemsControl = null;

            Loaded += LoadedHandler;
            Unloaded += UnloadedHandler;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            m_pathPartsItemsControl = Template.FindName(PathPartsItemsControlName, this) as ItemsControl;
            m_pathPartsItemsControl.ItemsSource = m_controller.CurrentDirectoryPathParts;

            m_fileSystemEntryItemsControl = Template.FindName(FileSystemEntryItemsControlName, this) as ItemsControl;
            m_fileSystemEntryItemsControl.ItemsSource = GetFileSystemEntryItems();

            m_emptyDirectoryTextBlock = Template.FindName(EmptyDirectoryTextBlockName, this) as TextBlock;

            UpdateListVisibility();
        }

        protected virtual void LoadedHandler(object sender, RoutedEventArgs args)
        {
            m_controller.PropertyChanged += ControllerPropertyChangedHandler;
        }

        protected virtual void UnloadedHandler(object sender, RoutedEventArgs args)
        {
            m_controller.PropertyChanged -= ControllerPropertyChangedHandler;
        }

        protected void OpenSpecialDirectoriesDrawerCommandHandler(object sender, ExecutedRoutedEventArgs args)
        {
            DrawerHost drawerHost = ((DrawerHost)Template.FindName(DrawerHostName, this));
            drawerHost.IsTopDrawerOpen = true;
        }

        protected void SelectDirectoryItemCommandHandler(object sender, ExecutedRoutedEventArgs args)
        {
            if (args.Parameter != null)
            {
                if (args.Parameter is DirectoryInfo directoryInfo)
                {
                    CurrentDirectory = directoryInfo.FullName;
                }
                else if (args.Parameter is SpecialDirectory specialDirectory)
                {
                    CurrentDirectory = specialDirectory.Info.FullName;
                }
                else if (args.Parameter is SpecialDrive specialDrive)
                {
                    CurrentDirectory = specialDrive.Info.RootDirectory.FullName;
                }

                DrawerHost drawerHost = ((DrawerHost)Template.FindName(DrawerHostName, this));

                if (drawerHost.IsTopDrawerOpen)
                {
                    drawerHost.IsTopDrawerOpen = false;
                }
            }
        }

        protected virtual void SelectFileSystemEntryCommandHandler(object sender, ExecutedRoutedEventArgs args)
        {
            if (args.Parameter != null)
            {
                if (args.Parameter is DirectoryInfo directoryInfo)
                {
                    CurrentDirectory = directoryInfo.FullName;
                }
            }
        }

        protected void ShowInfoCommandHandler(object sender, ExecutedRoutedEventArgs args)
        {
            FileSystemInfoToShow = args.Parameter as FileSystemInfo;

            DrawerHost drawerHost = ((DrawerHost)Template.FindName(DrawerHostName, this));
            drawerHost.IsLeftDrawerOpen = true;
        }

        protected void CancelCommandHandler(object sender, ExecutedRoutedEventArgs args)
        {
            RoutedEventArgs eventArgs = new RoutedEventArgs(CancelEvent, this);
            RaiseEvent(eventArgs);
        }

        protected static void CurrentDirectoryChangedHandler(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            (obj as FileSystemControl)?.CurrentDirectoryChangedHandler(args.NewValue as string);
        }

        protected virtual void CurrentDirectoryChangedHandler(string newCurrentDirectory)
        {
            try
            {
                m_controller.SelectDirectory(newCurrentDirectory);
            }
            catch (Exception exc)
                when (exc is UnauthorizedAccessException || exc is FileNotFoundException)
            {
                SnackbarMessageQueue.Enqueue(exc.Message);
            }
        }

        protected static void ShowHiddenFilesAndDirectoriesChangedHandler(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            (obj as FileSystemControl)?.m_controller.SetShowHiddenFilesAndDirectories((bool)args.NewValue);
        }

        protected static void ShowSystemFilesAndDirectoriesChangedHandler(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            (obj as FileSystemControl)?.m_controller.SetShowSystemFilesAndDirectories((bool)args.NewValue);
        }

        protected virtual void ControllerPropertyChangedHandler(object sender, PropertyChangedEventArgs args)
        {
            if (sender == m_controller)
            {
                if (args.PropertyName == nameof(FileSystemController.CurrentDirectory))
                {
                    CurrentDirectory = m_controller.CurrentDirectory.FullName;

                    UpdateSelection();
                }
                else if (args.PropertyName == nameof(FileSystemController.CurrentDirectoryPathParts))
                {
                    m_pathPartsItemsControl.ItemsSource = m_controller.CurrentDirectoryPathParts;
                }
                else if (args.PropertyName == nameof(FileSystemController.ShowHiddenFilesAndDirectories))
                {
                    ShowHiddenFilesAndDirectories = m_controller.ShowHiddenFilesAndDirectories;
                }
                else if (args.PropertyName == nameof(FileSystemController.ShowSystemFilesAndDirectories))
                {
                    ShowSystemFilesAndDirectories = m_controller.ShowSystemFilesAndDirectories;
                }
            }
        }

        protected void UpdateSelection()
        {
            IEnumerable items = m_fileSystemEntryItemsControl?.ItemsSource;

            if (items != null)
            {
                foreach (object item in items)
                {
                    if (item is DirectoryInfoItem directoryInfoItem)
                    {
                        directoryInfoItem.IsSelected = directoryInfoItem.Value.FullName == m_controller.CurrentDirectory?.FullName;
                    }
                    else if (item is FileInfoItem fileInfoItem)
                    {
                        fileInfoItem.IsSelected = fileInfoItem.Value.FullName == m_controller.CurrentFileFullName;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the entries of the current directory. Depending of the implementing sub class, it will contain either only directories or direcoties and files.
        /// </summary>
        /// <returns></returns>
        protected abstract IEnumerable GetFileSystemEntryItems();

        /// <summary>
        /// Shows the list of the current directory or hides it if it is empty. A message will be show instead of an empty list.
        /// </summary>
        protected void UpdateListVisibility()
        {
            if (m_fileSystemEntryItemsControl != null
                && m_fileSystemEntryItemsControl.ItemsSource != null
                && m_fileSystemEntryItemsControl.ItemsSource.GetEnumerator().MoveNext())
            {
                m_fileSystemEntryItemsControl.Visibility = Visibility.Visible;
                m_emptyDirectoryTextBlock.Visibility = Visibility.Collapsed;
            }
            else
            {
                m_fileSystemEntryItemsControl.Visibility = Visibility.Collapsed;
                m_emptyDirectoryTextBlock.Visibility = Visibility.Visible;
            }
        }
    }
}
