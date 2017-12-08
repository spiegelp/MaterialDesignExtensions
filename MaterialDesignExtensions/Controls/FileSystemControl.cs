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
using MaterialDesignExtensions.Model;

namespace MaterialDesignExtensions.Controls
{
    public abstract class FileSystemControl : Control
    {
        protected const string DrawerHostName = "drawerHost";
        protected const string PathPartsItemsControlName = "pathPartsItemsControl";
        protected const string FileSystemEntryItemsListBoxName = "fileSystemEntryItemsListBox";
        protected const string EmptyDirectoryTextBlockName = "emptyDirectoryTextBlock";

        public static RoutedCommand OpenSpecialDirectoriesDrawerCommand = new RoutedCommand();
        public static RoutedCommand SelectDirectoryItemCommand = new RoutedCommand();
        public static RoutedCommand ShowInfoCommand = new RoutedCommand();
        public static RoutedCommand CancelCommand = new RoutedCommand();

        public static readonly RoutedEvent CancelEvent = EventManager.RegisterRoutedEvent(
            nameof(Cancel), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(FileSystemControl));

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

        public static readonly DependencyProperty CurrentDirectoryProperty = DependencyProperty.Register(
                nameof(CurrentDirectory),
                typeof(string),
                typeof(FileSystemControl),
                new PropertyMetadata(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), CurrentDirectoryChangedHandler));

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

        public FileSystemController Controller
        {
            get
            {
                return m_controller;
            }
        }

        public static readonly DependencyProperty ShowHiddenFilesAndDirectoriesProperty = DependencyProperty.Register(
                nameof(ShowHiddenFilesAndDirectories),
                typeof(bool),
                typeof(FileSystemControl),
                new PropertyMetadata(false, ShowHiddenFilesAndDirectoriesChangedHandler));

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
        protected ListBox m_fileSystemEntryItemsListBox;
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
            CommandBindings.Add(new CommandBinding(ShowInfoCommand, ShowInfoCommandHandler));
            CommandBindings.Add(new CommandBinding(CancelCommand, CancelCommandHandler));

            m_pathPartsItemsControl = null;
            m_fileSystemEntryItemsListBox = null;

            Loaded += LoadedHandler;
            Unloaded += UnloadedHandler;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            m_pathPartsItemsControl = Template.FindName(PathPartsItemsControlName, this) as ItemsControl;
            m_pathPartsItemsControl.ItemsSource = m_controller.CurrentDirectoryPathParts;

            m_fileSystemEntryItemsListBox = Template.FindName(FileSystemEntryItemsListBoxName, this) as ListBox;
            m_fileSystemEntryItemsListBox.ItemsSource = GetFileSystemEntryItems();

            m_emptyDirectoryTextBlock = Template.FindName(EmptyDirectoryTextBlockName, this) as TextBlock;

            UpdateListVisibility();
        }

        protected virtual void LoadedHandler(object sender, RoutedEventArgs args)
        {
            m_controller.PropertyChanged += ControllerPropertyChangedHandler;
            m_fileSystemEntryItemsListBox.SelectionChanged += FileSystemEntryItemsListBoxSelectionChangedHandler;
        }

        protected virtual void UnloadedHandler(object sender, RoutedEventArgs args)
        {
            m_controller.PropertyChanged -= ControllerPropertyChangedHandler;
            m_fileSystemEntryItemsListBox.SelectionChanged -= FileSystemEntryItemsListBoxSelectionChangedHandler;
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

        protected virtual void FileSystemEntryItemsListBoxSelectionChangedHandler(object sender, SelectionChangedEventArgs args)
        {
            if (sender == m_fileSystemEntryItemsListBox)
            {
                if (m_fileSystemEntryItemsListBox.SelectedItem != null)
                {
                    if (m_fileSystemEntryItemsListBox.SelectedItem is DirectoryInfo directoryInfo)
                    {
                        CurrentDirectory = directoryInfo.FullName;
                    }
                }
            }
        }

        protected abstract IEnumerable GetFileSystemEntryItems();

        protected void UpdateListVisibility()
        {
            if (m_fileSystemEntryItemsListBox != null
                && m_fileSystemEntryItemsListBox.ItemsSource != null
                && m_fileSystemEntryItemsListBox.ItemsSource.GetEnumerator().MoveNext())
            {
                m_fileSystemEntryItemsListBox.Visibility = Visibility.Visible;
                m_emptyDirectoryTextBlock.Visibility = Visibility.Collapsed;
            }
            else
            {
                m_fileSystemEntryItemsListBox.Visibility = Visibility.Collapsed;
                m_emptyDirectoryTextBlock.Visibility = Visibility.Visible;
            }
        }
    }
}
