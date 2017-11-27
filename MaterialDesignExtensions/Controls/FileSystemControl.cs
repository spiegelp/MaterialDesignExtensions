using System;
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
using MaterialDesignExtensions.Model;

namespace MaterialDesignExtensions.Controls
{
    public abstract class FileSystemControl : Control
    {
        protected const string DrawerHostName = "drawerHost";
        protected const string PathPartsItemsControlName = "pathPartsItemsControl";
        protected const string DirectoryItemsListBoxName = "directoryItemsListBox";

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

        protected FileSystemController m_controller;

        protected ItemsControl m_pathPartsItemsControl;
        protected ListBox m_directoryItemsListBox;

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
            m_directoryItemsListBox = null;

            Loaded += LoadedHandler;
            Unloaded += UnloadedHandler;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            m_pathPartsItemsControl = Template.FindName(PathPartsItemsControlName, this) as ItemsControl;
            m_pathPartsItemsControl.ItemsSource = m_controller.CurrentDirectoryPathParts;

            m_directoryItemsListBox = Template.FindName(DirectoryItemsListBoxName, this) as ListBox;
            m_directoryItemsListBox.ItemsSource = m_controller.Directories;
        }

        protected virtual void LoadedHandler(object sender, RoutedEventArgs args)
        {
            m_controller.PropertyChanged += ControllerPropertyChangedHandler;
            m_directoryItemsListBox.SelectionChanged += DirectoryitemsListBoxSelectionChangedHandler;
        }

        protected virtual void UnloadedHandler(object sender, RoutedEventArgs args)
        {
            m_controller.PropertyChanged -= ControllerPropertyChangedHandler;
            m_directoryItemsListBox.SelectionChanged -= DirectoryitemsListBoxSelectionChangedHandler;
        }

        protected void OpenSpecialDirectoriesDrawerCommandHandler(object sender, ExecutedRoutedEventArgs args)
        {
            MaterialDesignThemes.Wpf.DrawerHost drawerHost = ((MaterialDesignThemes.Wpf.DrawerHost)Template.FindName(DrawerHostName, this));
            drawerHost.IsTopDrawerOpen = true;
        }

        protected void SelectDirectoryItemCommandHandler(object sender, ExecutedRoutedEventArgs args)
        {
            if (args.Parameter != null)
            {
                if (args.Parameter is DirectoryInfo directoryInfo)
                {
                    m_controller.SelectDirectory(directoryInfo);
                }
                else if (args.Parameter is SpecialDirectory specialDirectory)
                {
                    m_controller.SelectDirectory(specialDirectory.Info);
                }
                else if (args.Parameter is SpecialDrive specialDrive)
                {
                    m_controller.SelectDirectory(specialDrive.Info.RootDirectory);
                }

                MaterialDesignThemes.Wpf.DrawerHost drawerHost = ((MaterialDesignThemes.Wpf.DrawerHost)Template.FindName(DrawerHostName, this));

                if (drawerHost.IsTopDrawerOpen)
                {
                    drawerHost.IsTopDrawerOpen = false;
                }
            }
        }

        protected void ShowInfoCommandHandler(object sender, ExecutedRoutedEventArgs args)
        {
            FileSystemInfoToShow = args.Parameter as FileSystemInfo;

            MaterialDesignThemes.Wpf.DrawerHost drawerHost = ((MaterialDesignThemes.Wpf.DrawerHost)Template.FindName(DrawerHostName, this));
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
            m_controller.SelectDirectory(newCurrentDirectory);
        }

        protected void ControllerPropertyChangedHandler(object sender, PropertyChangedEventArgs args)
        {
            if (sender == m_controller)
            {
                if (args.PropertyName == nameof(FileSystemController.CurrentDirectory))
                {
                    CurrentDirectory = m_controller.CurrentDirectory.FullName;
                }
                else if (args.PropertyName == nameof(FileSystemController.Directories))
                {
                    m_directoryItemsListBox.ItemsSource = m_controller.Directories;

                    if (m_controller.Directories != null && m_controller.Directories.Count > 0)
                    {
                        m_directoryItemsListBox.ScrollIntoView(m_controller.Directories[0]);
                    }
                }
                else if (args.PropertyName == nameof(FileSystemController.CurrentDirectoryPathParts))
                {
                    m_pathPartsItemsControl.ItemsSource = m_controller.CurrentDirectoryPathParts;
                }
            }
        }

        protected void DirectoryitemsListBoxSelectionChangedHandler(object sender, SelectionChangedEventArgs args)
        {
            if (sender == m_directoryItemsListBox)
            {
                if (m_directoryItemsListBox.SelectedItem != null)
                {
                    if (m_directoryItemsListBox.SelectedItem is DirectoryInfo directoryInfo)
                    {
                        m_controller.SelectDirectory(directoryInfo);
                    }
                }
            }
        }
    }
}
