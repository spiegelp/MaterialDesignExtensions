using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

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
