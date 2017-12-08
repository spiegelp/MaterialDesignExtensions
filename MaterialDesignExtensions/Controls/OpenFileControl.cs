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

namespace MaterialDesignExtensions.Controls
{
    public class OpenFileControl : FileSystemControl
    {
        public static RoutedCommand SelectFileCommand = new RoutedCommand();

        public static readonly RoutedEvent FileSelectedEvent = EventManager.RegisterRoutedEvent(
            nameof(FileSelected), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(OpenFileControl));

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

        public static readonly DependencyProperty CurrentFileProperty = DependencyProperty.Register(
                nameof(CurrentFile),
                typeof(string),
                typeof(OpenFileControl),
                new PropertyMetadata(null, CurrentFileChangedHandler));

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

        static OpenFileControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(OpenFileControl), new FrameworkPropertyMetadata(typeof(OpenFileControl)));
        }

        public OpenFileControl()
            : base()
        {
            CommandBindings.Add(new CommandBinding(SelectFileCommand, SelectFileCommandHandler));
        }

        private void SelectFileCommandHandler(object sender, ExecutedRoutedEventArgs args)
        {
            FileSelectedEventArgs eventArgs = new FileSelectedEventArgs(FileSelectedEvent, this, m_controller.CurrentFile);
            RaiseEvent(eventArgs);
        }

        protected static void CurrentFileChangedHandler(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            (obj as OpenFileControl)?.CurrentFileChangedHandler(args.NewValue as string);
        }

        protected virtual void CurrentFileChangedHandler(string newCurrentFile)
        {
            m_controller.SelectFile(newCurrentFile);
        }

        protected override void ControllerPropertyChangedHandler(object sender, PropertyChangedEventArgs args)
        {
            if (sender == m_controller)
            {
                if (args.PropertyName == nameof(FileSystemController.DirectoriesAndFiles))
                {
                    List<FileSystemInfo> items = m_controller.DirectoriesAndFiles;
                    m_fileSystemEntryItemsListBox.ItemsSource = items;

                    if (items != null && items.Any())
                    {
                        m_fileSystemEntryItemsListBox.ScrollIntoView(items[0]);
                    }

                    UpdateListVisibility();
                }
            }

            base.ControllerPropertyChangedHandler(sender, args);
        }

        protected override void FileSystemEntryItemsListBoxSelectionChangedHandler(object sender, SelectionChangedEventArgs args)
        {
            if (sender == m_fileSystemEntryItemsListBox)
            {
                if (m_fileSystemEntryItemsListBox.SelectedItem != null)
                {
                    if (m_fileSystemEntryItemsListBox.SelectedItem is DirectoryInfo directoryInfo)
                    {
                        CurrentDirectory = directoryInfo.FullName;
                        CurrentFile = null;
                    }
                    else if (m_fileSystemEntryItemsListBox.SelectedItem is FileInfo fileInfo)
                    {
                        CurrentFile = fileInfo.FullName;
                    }
                }
            }
        }

        protected override IEnumerable GetFileSystemEntryItems()
        {
            return m_controller.DirectoriesAndFiles;
        }
    }

    public class FileSelectedEventArgs : RoutedEventArgs
    {
        public FileInfo FileInfo { get; }

        public string File
        {
            get
            {
                return FileInfo?.FullName;
            }
        }

        public FileSelectedEventArgs(RoutedEvent routedEvent, object source, FileInfo fileInfo)
            : base(routedEvent, source)
        {
            FileInfo = fileInfo;
        }
    }
}
