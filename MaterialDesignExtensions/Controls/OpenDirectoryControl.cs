using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

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
    /// A control for selecting a directory.
    /// </summary>
    public class OpenDirectoryControl : FileSystemControl
    {
        /// <summary>
        /// An event raised by selecting a directory to open.
        /// </summary>
        public static readonly RoutedEvent DirectorySelectedEvent = EventManager.RegisterRoutedEvent(
            nameof(DirectorySelected), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(OpenDirectoryControl));

        /// <summary>
        /// An event raised by selecting a directory to open.
        /// </summary>
        public event RoutedEventHandler DirectorySelected
        {
            add
            {
                AddHandler(DirectorySelectedEvent, value);
            }

            remove
            {
                RemoveHandler(DirectorySelectedEvent, value);
            }
        }

        /// <summary>
        /// An command called by selecting a directory to open.
        /// </summary>
        public static readonly DependencyProperty DirectorySelectedCommandProperty = DependencyProperty.Register(
            nameof(DirectorySelectedCommand), typeof(ICommand), typeof(OpenDirectoryControl), new PropertyMetadata(null, null));

        /// <summary>
        /// An command called by selecting a directory to open.
        /// </summary>
        public ICommand DirectorySelectedCommand
        {
            get
            {
                return (ICommand)GetValue(DirectorySelectedCommandProperty);
            }

            set
            {
                SetValue(DirectorySelectedCommandProperty, value);
            }
        }

        static OpenDirectoryControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(OpenDirectoryControl), new FrameworkPropertyMetadata(typeof(OpenDirectoryControl)));
        }

        /// <summary>
        /// Creates a new <see cref="OpenDirectoryControl" />.
        /// </summary>
        public OpenDirectoryControl()
            : base()
        {
            CommandBindings.Add(new CommandBinding(FileSystemControlCommands.SelectDirectoryCommand, SelectDirectoryCommandHandler));
        }

        private void SelectDirectoryCommandHandler(object sender, ExecutedRoutedEventArgs args)
        {
            DirectorySelectedEventArgs eventArgs = new DirectorySelectedEventArgs(DirectorySelectedEvent, this, m_controller.CurrentDirectory);
            RaiseEvent(eventArgs);

            if (DirectorySelectedCommand != null && DirectorySelectedCommand.CanExecute(m_controller.CurrentDirectory))
            {
                DirectorySelectedCommand.Execute(m_controller.CurrentDirectory);
            }
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
            }

            base.ControllerPropertyChangedHandler(sender, args);
        }

        protected override IEnumerable GetFileSystemEntryItems()
        {
            return m_controller.Directories
                .Select(directory =>
                {
                    bool isSelected = directory.FullName == m_controller.CurrentDirectory?.FullName;

                    return new DirectoryInfoItem() { IsSelected = isSelected, Value = directory };
                });
        }
    }

    /// <summary>
    /// The arguments for the <see cref="OpenDirectoryControl.DirectorySelected" /> event.
    /// </summary>
    public class DirectorySelectedEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// The selected directory as <see cref="DirectoryInfo" />.
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

        /// <summary>
        /// Creates a new <see cref="DirectorySelectedEventArgs" />.
        /// </summary>
        /// <param name="routedEvent"></param>
        /// <param name="source">The source object</param>
        /// <param name="directoryInfo">The selected directory</param>
        public DirectorySelectedEventArgs(RoutedEvent routedEvent, object source, DirectoryInfo directoryInfo)
            : base(routedEvent, source)
        {
            DirectoryInfo = directoryInfo;
        }
    }
}
