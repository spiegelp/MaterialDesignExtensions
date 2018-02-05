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
using MaterialDesignExtensions.Model;

namespace MaterialDesignExtensions.Controls
{
    /// <summary>
    /// The base control with common logic for <see cref="OpenFileControl" /> and <see cref="SaveFileControl" />.
    /// </summary>
    public abstract class BaseFileControl : FileSystemControl
    {
        public static RoutedCommand SelectFileCommand = new RoutedCommand();

        public static readonly RoutedEvent FileSelectedEvent = EventManager.RegisterRoutedEvent(
            nameof(FileSelected), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(BaseFileControl));

        /// <summary>
        /// An event raised by selecting a file.
        /// </summary>
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
                typeof(BaseFileControl),
                new PropertyMetadata(null, CurrentFileChangedHandler));

        /// <summary>
        /// The current file of the control.
        /// </summary>
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

        public BaseFileControl()
            : base()
        {
            CommandBindings.Add(new CommandBinding(SelectFileCommand, SelectFileCommandHandler));
        }

        protected virtual void SelectFileCommandHandler(object sender, ExecutedRoutedEventArgs args)
        {
            FileSelectedEventArgs eventArgs = new FileSelectedEventArgs(FileSelectedEvent, this, m_controller.CurrentFileFullName);
            RaiseEvent(eventArgs);
        }

        private static void CurrentFileChangedHandler(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            (obj as BaseFileControl)?.CurrentFileChangedHandler(args.NewValue as string);
        }

        protected abstract void CurrentFileChangedHandler(string newCurrentFile);

        protected override void ControllerPropertyChangedHandler(object sender, PropertyChangedEventArgs args)
        {
            if (sender == m_controller)
            {
                if (args.PropertyName == nameof(FileSystemController.DirectoriesAndFiles))
                {
                    List<FileSystemInfo> items = m_controller.DirectoriesAndFiles;
                    m_fileSystemEntryItemsControl.ItemsSource = GetFileSystemEntryItems(items);

                    if (items != null && items.Any())
                    {
                        m_fileSystemEntryItemsScrollViewer.ScrollToTop();
                    }

                    UpdateListVisibility();
                }
                else if (args.PropertyName == nameof(FileSystemController.CurrentFileFullName))
                {
                    if (m_controller.CurrentFileFullName != null)
                    {
                        CurrentFile = m_controller.CurrentFileFullName;
                    }
                    else
                    {
                        CurrentFile = null;
                    }

                    UpdateSelection();
                }
            }

            base.ControllerPropertyChangedHandler(sender, args);
        }

        protected override IEnumerable GetFileSystemEntryItems()
        {
            return GetFileSystemEntryItems(m_controller.DirectoriesAndFiles);
        }

        protected IEnumerable GetFileSystemEntryItems(List<FileSystemInfo> directoriesAndFiles)
        {
            ArrayList items = new ArrayList(directoriesAndFiles.Count);

            foreach (FileSystemInfo item in directoriesAndFiles)
            {
                if (item is DirectoryInfo directoryInfo)
                {
                    bool isSelected = directoryInfo.FullName == m_controller.CurrentDirectory?.FullName;

                    items.Add(new DirectoryInfoItem() { IsSelected = isSelected, Value = directoryInfo });
                }
                else if (item is FileInfo fileInfo)
                {
                    bool isSelected = fileInfo.FullName == m_controller.CurrentFileFullName;

                    items.Add(new FileInfoItem() { IsSelected = isSelected, Value = fileInfo });
                }
            }

            return items;
        }
    }

    /// <summary>
    /// The arguments for the <see cref="BaseFileControl.FileSelected" /> event.
    /// </summary>
    public class FileSelectedEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// The selected file as <see cref="FileInfo" />
        /// </summary>
        public FileInfo FileInfo { get; private set; }

        /// <summary>
        /// The selected file as full filename string.
        /// </summary>
        public string File { get; private set; }

        /// <summary>
        /// Creates a new <see cref="FileSelectedEventArgs" />.
        /// </summary>
        /// <param name="routedEvent"></param>
        /// <param name="source">The source object</param>
        /// <param name="fileInfo">The selected file</param>
        public FileSelectedEventArgs(RoutedEvent routedEvent, object source, FileInfo fileInfo)
            : base(routedEvent, source)
        {
            FileInfo = fileInfo;
            File = fileInfo?.FullName;
        }

        /// <summary>
        /// Creates a new <see cref="FileSelectedEventArgs" />.
        /// </summary>
        /// <param name="routedEvent"></param>
        /// <param name="source">The source object</param>
        /// <param name="file">The full filename of the selected file</param>
        public FileSelectedEventArgs(RoutedEvent routedEvent, object source, string file)
            : base(routedEvent, source)
        {
            File = file;

            if (!string.IsNullOrWhiteSpace(file))
            {
                FileInfo = new FileInfo(file);
            }
        }
    }
}
