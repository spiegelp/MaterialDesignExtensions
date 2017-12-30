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
            m_controller.SelectFile(newCurrentFile);
        }

        private static void FilenameChangedHandler(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            (obj as SaveFileControl)?.FilenameChangedHandler(args.NewValue as string);
        }

        protected virtual void FilenameChangedHandler(string newFilename)
        {
            m_controller.SelectFile(BuildFullFilename(newFilename));
        }

        protected override void ControllerPropertyChangedHandler(object sender, PropertyChangedEventArgs args)
        {
            if (sender == m_controller)
            {
                if (args.PropertyName == nameof(FileSystemController.CurrentFile))
                {
                    if (m_controller.CurrentFile != null)
                    {
                        string fullName = m_controller.CurrentFile.FullName.Replace("/", @"\");
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

        protected override void FileSystemEntryItemsListBoxSelectionChangedHandler(object sender, SelectionChangedEventArgs args)
        {
            if (sender == m_fileSystemEntryItemsListBox)
            {
                if (m_fileSystemEntryItemsListBox.SelectedItem != null)
                {
                    if (m_fileSystemEntryItemsListBox.SelectedItem is DirectoryInfo directoryInfo)
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
                    else if (m_fileSystemEntryItemsListBox.SelectedItem is FileInfo fileInfo)
                    {
                        CurrentFile = fileInfo.FullName;
                    }
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
