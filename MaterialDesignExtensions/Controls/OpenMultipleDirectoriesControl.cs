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

using MaterialDesignExtensions.Commands.Internal;
using MaterialDesignExtensions.Controllers;
using MaterialDesignExtensions.Model;

// use Pri.LongPath classes instead of System.IO for the MaterialDesignExtensions.LongPath build to support long file system paths on older Windows and .NET versions
#if LONG_PATH
using DirectoryInfo = Pri.LongPath.DirectoryInfo;
#endif

namespace MaterialDesignExtensions.Controls
{
    /// <summary>
    /// A control for selecting multiple directories.
    /// </summary>
    public class OpenMultipleDirectoriesControl : FileSystemControl
    {
        private const string SelectionItemsControlName = "selectionItemsControl";
        private const string EmptySelectionTextBlockName = "emptySelectionTextBlock";

        /// <summary>
        /// An event raised by selecting directories to open.
        /// </summary>
        public static readonly RoutedEvent DirectoriesSelectedEvent = EventManager.RegisterRoutedEvent(
            nameof(DirectoriesSelected), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(OpenMultipleDirectoriesControl));

        /// <summary>
        /// An event raised by selecting directories to open.
        /// </summary>
        public event RoutedEventHandler DirectoriesSelected
        {
            add
            {
                AddHandler(DirectoriesSelectedEvent, value);
            }

            remove
            {
                RemoveHandler(DirectoriesSelectedEvent, value);
            }
        }

        /// <summary>
        /// An command called by selecting directories to open.
        /// </summary>
        public static readonly DependencyProperty DirectoriesSelectedCommandProperty = DependencyProperty.Register(
            nameof(DirectoriesSelectedCommand), typeof(ICommand), typeof(OpenMultipleDirectoriesControl), new PropertyMetadata(null, null));

        /// <summary>
        /// An command called by selecting directories to open.
        /// </summary>
        public ICommand DirectoriesSelectedCommand
        {
            get
            {
                return (ICommand)GetValue(DirectoriesSelectedCommandProperty);
            }

            set
            {
                SetValue(DirectoriesSelectedCommandProperty, value);
            }
        }

        private ItemsControl m_selectionItemsControl;
        private TextBlock m_emptySelectionTextBlock;

        static OpenMultipleDirectoriesControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(OpenMultipleDirectoriesControl), new FrameworkPropertyMetadata(typeof(OpenMultipleDirectoriesControl)));
        }

        /// <summary>
        /// Creates a new <see cref="OpenMultipleDirectoriesControl" />.
        /// </summary>
        public OpenMultipleDirectoriesControl()
            : base()
        {

            m_selectionItemsControl = null;
            m_emptySelectionTextBlock = null;

            CommandBindings.Add(new CommandBinding(FileSystemControlCommands.OpenSelectionDrawerCommand, OpenSelectionDrawerCommandHandler));
            CommandBindings.Add(new CommandBinding(FileSystemControlCommands.SelectDirectoryCommand, SelectDirectoryCommandHandler));
            CommandBindings.Add(new CommandBinding(FileSystemControlCommands.SelectMultipleDirectoriesCommand, SelectMultipleDirectoriesCommandHandler, CanExecuteSelectMultipleDirectoriesCommand));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            m_selectionItemsControl = Template.FindName(SelectionItemsControlName, this) as ItemsControl;

            m_emptySelectionTextBlock = Template.FindName(EmptySelectionTextBlockName, this) as TextBlock;

            UpdateSelectionList();

            UpdateSelectionListVisibility();
        }

        private void OpenSelectionDrawerCommandHandler(object sender, ExecutedRoutedEventArgs args)
        {
            DrawerHost drawerHost = (DrawerHost)Template.FindName(DrawerHostName, this);
            drawerHost.IsRightDrawerOpen = true;
        }

        private void SelectDirectoryCommandHandler(object sender, ExecutedRoutedEventArgs args)
        {
            if (args.Parameter != null)
            {
                if (args.Parameter is DirectoryInfo directoryInfo)
                {
                    m_controller.SelectOrRemoveDirectoryForMultipleSelection(directoryInfo);
                }
            }
        }

        private void SelectMultipleDirectoriesCommandHandler(object sender, ExecutedRoutedEventArgs args)
        {
            DirectoriesSelectedEventArgs eventArgs = new DirectoriesSelectedEventArgs(DirectoriesSelectedEvent, this, m_controller.SelectedDirectories.ToList());
            RaiseEvent(eventArgs);

            if (DirectoriesSelectedCommand != null && DirectoriesSelectedCommand.CanExecute(eventArgs.DirectoryInfoList))
            {
                DirectoriesSelectedCommand.Execute(eventArgs.DirectoryInfoList);
            }
        }

        private void CanExecuteSelectMultipleDirectoriesCommand(object sender, CanExecuteRoutedEventArgs args)
        {
            args.CanExecute = m_controller != null && m_controller.SelectedDirectories != null && m_controller.SelectedDirectories.Any();
        }

        protected override void ControllerPropertyChangedHandler(object sender, PropertyChangedEventArgs args)
        {
            if (sender == m_controller)
            {
                if (args.PropertyName == nameof(FileSystemController.Directories))
                {
                    m_fileSystemEntryItemsControl.ItemsSource = GetFileSystemEntryItems();

                    if (m_controller.Directories != null && m_controller.Directories.Any())
                    {
                        ItemsScrollViewer?.ScrollToTop();
                    }

                    UpdateListVisibility();
                }
                else if (args.PropertyName == nameof(FileSystemController.SelectedDirectories))
                {
                    UpdateSelection();
                    UpdateSelectionList();

                    UpdateSelectionListVisibility();
                }
            }

            base.ControllerPropertyChangedHandler(sender, args);
        }

        protected override IEnumerable GetFileSystemEntryItems()
        {
            ISet<string> directoryNames = new HashSet<string>(m_controller.SelectedDirectories.Select(directoryInfo => directoryInfo.FullName.ToLower()));

            return m_controller.Directories
                .Select(directory =>
                {
                    bool isSelected = directoryNames.Contains(directory.FullName.ToLower());

                    return new DirectoryInfoItem() { IsSelected = isSelected, Value = directory };
                })
                // call ToList() to run the LINQ expression immediately, otherwise WPF will behave some kind of strange
                .ToList();
        }

        protected override void UpdateSelection()
        {
            IEnumerable items = m_fileSystemEntryItemsControl?.ItemsSource;

            if (items != null)
            {
                ISet<string> directoryNames = new HashSet<string>(m_controller.SelectedDirectories.Select(directoryInfo => directoryInfo.FullName.ToLower()));

                foreach (object item in items)
                {
                    if (item is DirectoryInfoItem directoryInfoItem)
                    {
                        directoryInfoItem.IsSelected = directoryNames.Contains(directoryInfoItem.Value.FullName.ToLower());
                    }
                }
            }
        }

        private void UpdateSelectionList()
        {
            m_selectionItemsControl.ItemsSource = m_controller.SelectedDirectories
                .OrderBy(directory => directory.Name.ToLower())
                .ThenBy(directory => directory.Parent.FullName.ToLower());
        }

        /// <summary>
        /// Shows the list of the selected directories or hides it if it is empty. A message will be show instead of an empty list.
        /// </summary>
        protected void UpdateSelectionListVisibility()
        {
            if (m_selectionItemsControl != null
                && m_selectionItemsControl.ItemsSource != null
                && m_selectionItemsControl.ItemsSource.GetEnumerator().MoveNext())
            {
                m_selectionItemsControl.Visibility = Visibility.Visible;
                m_emptySelectionTextBlock.Visibility = Visibility.Collapsed;
            }
            else
            {
                m_selectionItemsControl.Visibility = Visibility.Collapsed;
                m_emptySelectionTextBlock.Visibility = Visibility.Visible;
            }
        }
    }

    /// <summary>
    /// The arguments for the <see cref="OpenMultipleDirectoriesControl.DirectoriesSelected" /> event.
    /// </summary>
    public class DirectoriesSelectedEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// The selected directories as <see cref="DirectoryInfo" />.
        /// </summary>
        public List<DirectoryInfo> DirectoryInfoList { get; private set; }

        /// <summary>
        /// The selected directories as full filename string.
        /// </summary>
        public List<string> Directories
        {
            get
            {
                return DirectoryInfoList
                    .Select(directoryInfo => directoryInfo.FullName)
                    .ToList();
            }
        }

        /// <summary>
        /// Creates a new <see cref="DirectoriesSelectedEventArgs" />.
        /// </summary>
        /// <param name="routedEvent"></param>
        /// <param name="source">The source object</param>
        /// <param name="directoryInfoList">The list of selected directories</param>
        public DirectoriesSelectedEventArgs(RoutedEvent routedEvent, object source, List<DirectoryInfo> directoryInfoList)
            : base(routedEvent, source)
        {
            DirectoryInfoList = directoryInfoList
                .OrderBy(directoryInfo => directoryInfo.FullName.ToLower())
                .ToList();
        }
    }
}
