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
                m_controller.SelectFile(BuildFullFilename(newFilename));
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

        protected override void SelectFileSystemEntryCommandHandler(object sender, ExecutedRoutedEventArgs args)
        {
            if (args.Parameter != null)
            {
                if (args.Parameter is DirectoryInfo directoryInfo)
                {
                    CurrentDirectory = directoryInfo.FullName;

                    if (Filename != null && CurrentDirectory != null)
                    {
                        CurrentFile = BuildFullFilename(Filename);
                    }
                    else
                    {
                        CurrentFile = null;
                    }
                }
                else if (args.Parameter is FileInfo fileInfo)
                {
                    CurrentFile = fileInfo.FullName;
                }
            }
        }

        private string BuildFullFilename(string newFilename)
        {
            string filename = null;

            if (!string.IsNullOrWhiteSpace(newFilename))
            {
                string directory = CurrentDirectory;

                if (CurrentDirectory != null && !directory.EndsWith(@"\") && !directory.EndsWith("/"))
                {
                    directory = directory + @"\";
                }

                filename = directory + newFilename.Trim();
            }

            return filename;
        }
    }
}
