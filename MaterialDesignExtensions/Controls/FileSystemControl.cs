﻿using System;
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

using MaterialDesignExtensions.Commands.Internal;
using MaterialDesignExtensions.Controllers;
using MaterialDesignExtensions.Model;

// use Pri.LongPath classes instead of System.IO for the MaterialDesignExtensions.LongPath build to support long file system paths on older Windows and .NET versions
#if LONG_PATH
using FileSystemInfo = Pri.LongPath.FileSystemInfo;
using DirectoryInfo = Pri.LongPath.DirectoryInfo;
#endif

namespace MaterialDesignExtensions.Controls
{
    /// <summary>
    /// The base control with common logic for <see cref="OpenDirectoryControl" />, <see cref="OpenFileControl" /> and <see cref="SaveFileControl" />.
    /// </summary>
    public abstract class FileSystemControl : Control
    {
        protected const string DrawerHostName = "drawerHost";
        protected const string PathPartsScrollViewerName = "pathPartsScrollViewer";
        protected const string PathPartsItemsControlName = "pathPartsItemsControl";
        protected const string CurrentDirectoryTextBoxName = "currentDirectoryTextBox";
        protected const string FileSystemEntryItemsControlName = "fileSystemEntryItemsControl";
        protected const string EmptyDirectoryTextBlockName = "emptyDirectoryTextBlock";

        /// <summary>
        /// An event raised by canceling the operation.
        /// </summary>
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

        /// <summary>
        /// Enables the feature to create new directories.
        /// </summary>
        public static readonly DependencyProperty CreateNewDirectoryEnabledProperty = DependencyProperty.Register(
            nameof(CreateNewDirectoryEnabled),
            typeof(bool),
            typeof(FileSystemControl),
            new PropertyMetadata(false));

        /// <summary>
        /// Enables the feature to create new directories. Notice: It does not have any effects for <see cref="OpenFileControl" />.
        /// </summary>
        public bool CreateNewDirectoryEnabled
        {
            get
            {
                return (bool)GetValue(CreateNewDirectoryEnabledProperty);
            }

            set
            {
                SetValue(CreateNewDirectoryEnabledProperty, value);
            }
        }

        /// <summary>
        /// The current directory of the control.
        /// </summary>
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

        /// <summary>
        /// The currently selected directory or file to display additional information about it.
        /// </summary>
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
        /// The ScrollViewer of the ItemsControls with the file system entries.
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

        /// <summary>
        /// The name for the new directory.
        /// </summary>
        public static readonly DependencyProperty NewDirectoryNameProperty = DependencyProperty.Register(
                nameof(NewDirectoryName),
                typeof(string),
                typeof(FileSystemControl),
                new PropertyMetadata(null));

        /// <summary>
        /// The name for the new directory.
        /// </summary>
        public string NewDirectoryName
        {
            get
            {
                return (string)GetValue(NewDirectoryNameProperty);
            }

            set
            {
                SetValue(NewDirectoryNameProperty, value);
            }
        }

        /// <summary>
        /// Show the current directory path with a short cut button for each part in the path, instead of a text box.
        /// </summary>
        public static readonly DependencyProperty PathPartsAsButtonsProperty = DependencyProperty.Register(
            nameof(PathPartsAsButtons),
            typeof(bool),
            typeof(FileSystemControl),
            new PropertyMetadata(true));

        /// <summary>
        /// Show the current directory path with a short cut button for each part in the path, instead of a text box.
        /// </summary>
        public bool PathPartsAsButtons
        {
            get
            {
                return (bool)GetValue(PathPartsAsButtonsProperty);
            }

            set
            {
                SetValue(PathPartsAsButtonsProperty, value);
            }
        }

        /// <summary>
        /// Shows or hides hidden directories and files.
        /// </summary>
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

        /// <summary>
        /// Shows or hides protected directories and files of the system.
        /// </summary>
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

        /// <summary>
        /// The message queue for the Snackbar. This property is intended for internal use.
        /// </summary>
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

        /// <summary>
        /// Enable switching between a text box and the buttons for each sub directory of the current directory's path.
        /// </summary>
        public static readonly DependencyProperty SwitchPathPartsAsButtonsEnabledProperty = DependencyProperty.Register(
            nameof(SwitchPathPartsAsButtonsEnabled),
            typeof(bool),
            typeof(FileSystemControl),
            new PropertyMetadata(false));

        /// <summary>
        /// Enable switching between a text box and the buttons for each sub directory of the current directory's path.
        /// </summary>
        public bool SwitchPathPartsAsButtonsEnabled
        {
            get
            {
                return (bool)GetValue(SwitchPathPartsAsButtonsEnabledProperty);
            }

            set
            {
                SetValue(SwitchPathPartsAsButtonsEnabledProperty, value);
            }
        }

        protected FileSystemController m_controller;

        protected ScrollViewer m_pathPartsScrollViewer;
        protected ItemsControl m_pathPartsItemsControl;
        protected TextBox m_currentDirectoryTextBox;
        // use an ItemsControl instead of a ListBox, because the ListBox raises several selection changed events without an explicit user input
        // another advantage: non selectable items such as headers can be added
        protected ItemsControl m_fileSystemEntryItemsControl;
        // private to force the usage of the lazy getter, because it only works after applying the template
        private ScrollViewer m_fileSystemEntryItemsScrollViewer;
        protected TextBlock m_emptyDirectoryTextBlock;

        static FileSystemControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FileSystemControl), new FrameworkPropertyMetadata(typeof(FileSystemControl)));
        }

        /// <summary>
        /// Creates a new <see cref="FileSystemControl" />.
        /// </summary>
        public FileSystemControl()
            : base()
        {
            m_controller = new FileSystemController();
            m_controller.SelectDirectory(CurrentDirectory);

            CommandBindings.Add(new CommandBinding(FileSystemControlCommands.OpenSpecialDirectoriesDrawerCommand, OpenSpecialDirectoriesDrawerCommandHandler));
            CommandBindings.Add(new CommandBinding(FileSystemControlCommands.SwitchPathPartsAsButtonsCommand, SwitchPathPartsAsButtonsHandler));
            CommandBindings.Add(new CommandBinding(FileSystemControlCommands.SelectDirectoryItemCommand, SelectDirectoryItemCommandHandler));
            CommandBindings.Add(new CommandBinding(FileSystemControlCommands.SelectFileSystemEntryCommand, SelectFileSystemEntryCommandHandler));
            CommandBindings.Add(new CommandBinding(FileSystemControlCommands.ShowInfoCommand, ShowInfoCommandHandler));
            CommandBindings.Add(new CommandBinding(FileSystemControlCommands.CancelCommand, CancelCommandHandler));
            CommandBindings.Add(new CommandBinding(FileSystemControlCommands.ShowCreateNewDirectoryCommand, ShowCreateNewDirectoryCommandHandler));
            CommandBindings.Add(new CommandBinding(FileSystemControlCommands.CancelNewDirectoryCommand, CancelNewDirectoryCommandHandler));
            CommandBindings.Add(new CommandBinding(FileSystemControlCommands.CreateNewDirectoryCommand, CreateNewDirectoryCommandHandler));

            InputBindings.Add(new KeyBinding(FileSystemControlCommands.CancelCommand, new KeyGesture(Key.Escape)));

            m_pathPartsScrollViewer = null;
            m_pathPartsItemsControl = null;
            m_currentDirectoryTextBox = null;
            m_fileSystemEntryItemsScrollViewer = null;
            m_fileSystemEntryItemsControl = null;

            Loaded += LoadedHandler;
            Unloaded += UnloadedHandler;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            m_pathPartsScrollViewer = Template.FindName(PathPartsScrollViewerName, this) as ScrollViewer;

            m_pathPartsItemsControl = Template.FindName(PathPartsItemsControlName, this) as ItemsControl;
            m_pathPartsItemsControl.ItemsSource = m_controller.CurrentDirectoryPathParts;

            m_currentDirectoryTextBox = Template.FindName(CurrentDirectoryTextBoxName, this) as TextBox;
            m_currentDirectoryTextBox.Text = CurrentDirectory;

            m_fileSystemEntryItemsControl = Template.FindName(FileSystemEntryItemsControlName, this) as ItemsControl;
            m_fileSystemEntryItemsControl.ItemsSource = GetFileSystemEntryItems();

            m_emptyDirectoryTextBlock = Template.FindName(EmptyDirectoryTextBlockName, this) as TextBlock;

            UpdateListVisibility();
        }

        protected virtual void LoadedHandler(object sender, RoutedEventArgs args)
        {
            m_controller.PropertyChanged += ControllerPropertyChangedHandler;

            if (m_currentDirectoryTextBox != null)
            {
                m_currentDirectoryTextBox.KeyDown += CurrentDirectoryTextBoxKeyDownHandler;
            }

            // not sure if the control should get keyboard focus on loading
            //Keyboard.Focus(this);
        }

        protected virtual void UnloadedHandler(object sender, RoutedEventArgs args)
        {
            m_controller.PropertyChanged -= ControllerPropertyChangedHandler;

            if (m_currentDirectoryTextBox != null)
            {
                m_currentDirectoryTextBox.KeyDown -= CurrentDirectoryTextBoxKeyDownHandler;
            }
        }

        protected void OpenSpecialDirectoriesDrawerCommandHandler(object sender, ExecutedRoutedEventArgs args)
        {
            DrawerHost drawerHost = ((DrawerHost)Template.FindName(DrawerHostName, this));
            drawerHost.IsTopDrawerOpen = true;
        }

        protected void SwitchPathPartsAsButtonsHandler(object sender, ExecutedRoutedEventArgs args)
        {
            PathPartsAsButtons = !PathPartsAsButtons;
        }

        private void CurrentDirectoryTextBoxKeyDownHandler(object sender, KeyEventArgs args)
        {
            if (sender == m_currentDirectoryTextBox && args.Key == Key.Enter)
            {
                string directory = m_currentDirectoryTextBox.Text
                    .Replace("\n", string.Empty)
                    .Replace("\r", string.Empty)
                    .Replace("\r", string.Empty)
                    .Trim();

                if (!string.IsNullOrWhiteSpace(directory))
                {
                    CurrentDirectory = directory;
                }
            }
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

            Keyboard.Focus(this);
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

            Keyboard.Focus(this);
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

        protected void ShowCreateNewDirectoryCommandHandler(object sender, ExecutedRoutedEventArgs args)
        {
            NewDirectoryName = null;

            DrawerHost drawerHost = ((DrawerHost)Template.FindName(DrawerHostName, this));
            drawerHost.IsBottomDrawerOpen = true;
        }

        protected void CancelNewDirectoryCommandHandler(object sender, ExecutedRoutedEventArgs args)
        {
            DrawerHost drawerHost = ((DrawerHost)Template.FindName(DrawerHostName, this));

            if (drawerHost.IsBottomDrawerOpen)
            {
                drawerHost.IsBottomDrawerOpen = false;
            }
        }

        protected void CreateNewDirectoryCommandHandler(object sender, ExecutedRoutedEventArgs args)
        {
            try
            {
                m_controller.CreateNewDirectory(NewDirectoryName);

                DrawerHost drawerHost = ((DrawerHost)Template.FindName(DrawerHostName, this));

                if (drawerHost.IsBottomDrawerOpen)
                {
                    drawerHost.IsBottomDrawerOpen = false;
                }
            }
            catch (Exception exc)
            {
                SnackbarMessageQueue.Enqueue(exc.Message);
            }
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

                if (m_currentDirectoryTextBox != null)
                {
                    m_currentDirectoryTextBox.Text = newCurrentDirectory;
                }
            }
            catch (PathTooLongException)
            {
                SnackbarMessageQueue.Enqueue(Localization.Strings.LongPathsAreNotSupported);
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

                    m_pathPartsScrollViewer.ScrollToEnd();
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

        protected virtual void UpdateSelection()
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
