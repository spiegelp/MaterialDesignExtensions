using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;

using MaterialDesignExtensions.Controllers;

// use Pri.LongPath classes instead of System.IO for the MaterialDesignExtensions.LongPath build to support long file system paths on older Windows and .NET versions
#if LONG_PATH
using DirectoryInfo = Pri.LongPath.DirectoryInfo;
using FileInfo = Pri.LongPath.FileInfo;
using FileSystemInfo = Pri.LongPath.FileSystemInfo;
#endif

namespace MaterialDesignExtensions.Controls
{
    /// <summary>
    /// A control for selecting a file to save data into.
    /// </summary>
    public class SaveFileControl : BaseFileControl
    {
        /// <summary>
        /// The name of the file itself without the full path.
        /// </summary>
        public static readonly DependencyProperty FilenameProperty = DependencyProperty.Register(
                nameof(Filename),
                typeof(string),
                typeof(SaveFileControl),
                new PropertyMetadata(null, FilenameChangedHandler));

        /// <summary>
        /// The name of the file itself without the full path.
        /// </summary>
        public string Filename
        {
            get
            {
                return (string)GetValue(FilenameProperty);
            }

            set
            {
                SetValue(FilenameProperty, value);
            }
        }

        /// <summary>
        /// Forces the possible file extension of the selected file filter for new filenames.
        /// </summary>
        public static readonly DependencyProperty ForceFileExtensionOfFileFilterProperty = DependencyProperty.Register(
            nameof(ForceFileExtensionOfFileFilter),
            typeof(bool),
            typeof(SaveFileControl),
            new PropertyMetadata(false, ForceFileExtensionOfFileFilterChangedHandler));

        /// <summary>
        /// Forces the possible file extension of the selected file filter for new filenames.
        /// </summary>
        public bool ForceFileExtensionOfFileFilter
        {
            get
            {
                return (bool)GetValue(ForceFileExtensionOfFileFilterProperty);
            }

            set
            {
                SetValue(ForceFileExtensionOfFileFilterProperty, value);
            }
        }

        static SaveFileControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SaveFileControl), new FrameworkPropertyMetadata(typeof(SaveFileControl)));
        }

        /// <summary>
        /// Creates a new <see cref="SaveFileControl" />.
        /// </summary>
        public SaveFileControl() : base() { }

        protected override void SelectFileCommandHandler(object sender, ExecutedRoutedEventArgs args)
        {
            if (m_controller.CurrentFile != null)
            {
                base.SelectFileCommandHandler(sender, args);
            }
        }

        protected override void CurrentFileChangedHandler(string newCurrentFile)
        {
            try
            {
                m_controller.SelectFile(newCurrentFile);
            }
            catch (PathTooLongException)
            {
                SnackbarMessageQueue.Enqueue(Localization.Strings.LongPathsAreNotSupported);
            }
        }

        private static void FilenameChangedHandler(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            (obj as SaveFileControl)?.FilenameChangedHandler(args.NewValue as string);
        }

        protected virtual void FilenameChangedHandler(string newFilename)
        {
            try
            {
                m_controller.SelectFile(m_controller.BuildFullFileNameForInCurrentDirectory(newFilename));
            }
            catch (PathTooLongException)
            {
                SnackbarMessageQueue.Enqueue(Localization.Strings.LongPathsAreNotSupported);
            }
        }

        protected override void ControllerPropertyChangedHandler(object sender, PropertyChangedEventArgs args)
        {
            if (sender == m_controller)
            {
                // use a string for the filename, because FileInfo.FullName trims dots at the end
                //     this bevahior has negative side effects (you cannot enter a dot) for the binding (update on property change) of the text box
                if (args.PropertyName == nameof(FileSystemController.CurrentFileFullName))
                {
                    if (m_controller.CurrentFileFullName != null)
                    {
                        string fullName = m_controller.CurrentFileFullName.Replace("/", @"\");
                        int index = fullName.LastIndexOf(@"\");

                        if (index > -1)
                        {
                            Filename = fullName.Substring(index + 1);
                        }
                        else
                        {
                            Filename = fullName;
                        }
                    }
                    else
                    {
                        Filename = null;
                    }
                }
            }

            base.ControllerPropertyChangedHandler(sender, args);
        }

        protected override void SelectFileSystemEntry(FileSystemInfo fileSystemInfo)
        {
            if (fileSystemInfo != null)
            {
                if (fileSystemInfo is DirectoryInfo directoryInfo)
                {
                    CurrentDirectory = directoryInfo.FullName;

                    if (Filename != null && CurrentDirectory != null)
                    {
                        CurrentFile = m_controller.BuildFullFileNameForInCurrentDirectory(Filename);
                    }
                    else
                    {
                        CurrentFile = null;
                    }
                }
                else if (fileSystemInfo is FileInfo fileInfo)
                {
                    CurrentFile = fileInfo.FullName;
                }
            }
        }

        protected static void ForceFileExtensionOfFileFilterChangedHandler(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (obj is SaveFileControl saveFileControl)
            {
                saveFileControl.m_controller.ForceFileExtensionOfFileFilter = (bool)args.NewValue;
            }
        }
    }
}
