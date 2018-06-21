using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

using MaterialDesignExtensions.Controllers;
using MaterialDesignExtensions.Model;

namespace MaterialDesignExtensions.Controls
{
    /// <summary>
    /// A control for selecting a directory.
    /// </summary>
    public class OpenDirectoryControl : FileSystemControl
    {
        /// <summary>
        /// Internal command used by the XAML template (public to be available in the XAML template). Not intended for external usage.
        /// </summary>
        public static readonly RoutedCommand SelectDirectoryCommand = new RoutedCommand();

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
            CommandBindings.Add(new CommandBinding(SelectDirectoryCommand, SelectDirectoryCommandHandler));
        }

        private void SelectDirectoryCommandHandler(object sender, ExecutedRoutedEventArgs args)
        {
            DirectorySelectedEventArgs eventArgs = new DirectorySelectedEventArgs(DirectorySelectedEvent, this, m_controller.CurrentDirectory);
            RaiseEvent(eventArgs);
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
                        ItemsScrollViewer.ScrollToTop();
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
        /// The selected directory as <see cref="DirectoryInfo" />
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
