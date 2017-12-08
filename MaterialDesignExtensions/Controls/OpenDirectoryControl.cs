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

namespace MaterialDesignExtensions.Controls
{
    public class OpenDirectoryControl : FileSystemControl
    {
        public static RoutedCommand SelectDirectoryCommand = new RoutedCommand();

        public static readonly RoutedEvent DirectorySelectedEvent = EventManager.RegisterRoutedEvent(
            nameof(DirectorySelected), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(OpenDirectoryControl));

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
                    m_fileSystemEntryItemsListBox.ItemsSource = m_controller.Directories;

                    if (m_controller.Directories != null && m_controller.Directories.Any())
                    {
                        m_fileSystemEntryItemsListBox.ScrollIntoView(m_controller.Directories[0]);
                    }

                    UpdateListVisibility();
                }
            }

            base.ControllerPropertyChangedHandler(sender, args);
        }

        protected override IEnumerable GetFileSystemEntryItems()
        {
            return m_controller.Directories;
        }
    }

    public class DirectorySelectedEventArgs : RoutedEventArgs
    {
        public DirectoryInfo DirectoryInfo { get; }

        public string Directory
        {
            get
            {
                return DirectoryInfo?.FullName;
            }
        }

        public DirectorySelectedEventArgs(RoutedEvent routedEvent, object source, DirectoryInfo directoryInfo)
            : base(routedEvent, source)
        {
            DirectoryInfo = directoryInfo;
        }
    }
}
