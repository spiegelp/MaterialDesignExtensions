using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using MaterialDesignThemes.Wpf;

namespace MaterialDesignExtensions.Controls
{
    public abstract class FileSystemDialog : Control
    {
        public static readonly DependencyProperty CurrentDirectoryProperty = DependencyProperty.Register(
                nameof(CurrentDirectory),
                typeof(string),
                typeof(FileSystemDialog),
                new PropertyMetadata(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)));

        /// <summary>
        /// The current directory of the dialog.
        /// </summary>
        public string CurrentDirectory
        {
            get
            {
                return (string)GetValue(CurrentDirectoryProperty);
            }

            set
            {
                SetValue(CurrentDirectoryProperty, value);
            }
        }

        public static readonly DependencyProperty ShowHiddenFilesAndDirectoriesProperty = DependencyProperty.Register(
                nameof(ShowHiddenFilesAndDirectories),
                typeof(bool),
                typeof(FileSystemDialog),
                new PropertyMetadata(false));

        /// <summary>
        /// Shows or hides hidden directories and files.
        /// </summary>
        public bool ShowHiddenFilesAndDirectories
        {
            get
            {
                return (bool)GetValue(ShowHiddenFilesAndDirectoriesProperty);
            }

            set
            {
                SetValue(ShowHiddenFilesAndDirectoriesProperty, value);
            }
        }

        public static readonly DependencyProperty ShowSystemFilesAndDirectoriesProperty = DependencyProperty.Register(
                nameof(ShowSystemFilesAndDirectories),
                typeof(bool),
                typeof(FileSystemDialog),
                new PropertyMetadata(false));

        /// <summary>
        /// Shows or hides protected directories and files of the system.
        /// </summary>
        public bool ShowSystemFilesAndDirectories
        {
            get
            {
                return (bool)GetValue(ShowSystemFilesAndDirectoriesProperty);
            }

            set
            {
                SetValue(ShowSystemFilesAndDirectoriesProperty, value);
            }
        }

        static FileSystemDialog()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FileSystemDialog), new FrameworkPropertyMetadata(typeof(FileSystemDialog)));
        }

        public FileSystemDialog() : base() { }

        protected DialogHost GetDialogHost()
        {
            DependencyObject element = VisualTreeHelper.GetParent(this);

            while (element != null && !(element is DialogHost))
            {
                element = VisualTreeHelper.GetParent(element);
            }

            return element as DialogHost;
        }

        protected static void InitDialog(FileSystemDialog dialog, double? width, double? height,
            string currentDirectory,
            bool showHiddenFilesAndDirectories, bool showSystemFilesAndDirectories)
        {
            if (width != null)
            {
                dialog.Width = (double)width;
            }

            if (height != null)
            {
                dialog.Height = (double)height;
            }

            if (!string.IsNullOrWhiteSpace(currentDirectory))
            {
                dialog.CurrentDirectory = currentDirectory;
            }

            dialog.ShowHiddenFilesAndDirectories = showHiddenFilesAndDirectories;
            dialog.ShowSystemFilesAndDirectories = showSystemFilesAndDirectories;
        }
    }

    public abstract class FileSystemDialogResult
    {
        public bool Canceled { get; protected set; }

        public FileSystemDialogResult(bool canceled)
        {
            Canceled = canceled;
        }
    }
}
