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
    /// <summary>
    /// The base class with common logic for <see cref="OpenDirectoryDialog" />, <see cref="OpenFileDialog" /> and <see cref="SaveFileDialog" />.
    /// </summary>
    public abstract class FileSystemDialog : Control
    {
        /// <summary>
        /// Enables the feature to create new directories.
        /// </summary>
        public static readonly DependencyProperty CreateNewDirectoryEnabledProperty = DependencyProperty.Register(
            nameof(CreateNewDirectoryEnabled),
            typeof(bool),
            typeof(FileSystemDialog),
            new PropertyMetadata(false));

        /// <summary>
        /// Enables the feature to create new directories. Notice: It does not have any effects for <see cref="OpenFileDialog" />.
        /// </summary>
        public bool CreateNewDirectoryEnabled
        {
            get
            {
                return (bool)GetValue(CreateNewDirectoryEnabledProperty);
            }

            set
            {
                SetValue(CreateNewDirectoryEnabledProperty, value);
            }
        }

        /// <summary>
        /// The current directory of the dialog.
        /// </summary>
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

        /// <summary>
        /// Show the current directory path with a short cut button for each part in the path, instead of a text box.
        /// </summary>
        public static readonly DependencyProperty PathPartsAsButtonsProperty = DependencyProperty.Register(
            nameof(PathPartsAsButtons),
            typeof(bool),
            typeof(FileSystemDialog),
            new PropertyMetadata(true));

        /// <summary>
        /// Show the current directory path with a short cut button for each part in the path, instead of a text box.
        /// </summary>
        public bool PathPartsAsButtons
        {
            get
            {
                return (bool)GetValue(PathPartsAsButtonsProperty);
            }

            set
            {
                SetValue(PathPartsAsButtonsProperty, value);
            }
        }

        /// <summary>
        /// Shows or hides hidden directories and files.
        /// </summary>
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

        /// <summary>
        /// Shows or hides protected directories and files of the system.
        /// </summary>
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

        /// <summary>
        /// Enable switching between a text box and the buttons for each sub directory of the current directory's path.
        /// </summary>
        public static readonly DependencyProperty SwitchPathPartsAsButtonsEnabledProperty = DependencyProperty.Register(
            nameof(SwitchPathPartsAsButtonsEnabled),
            typeof(bool),
            typeof(FileSystemDialog),
            new PropertyMetadata(false));

        /// <summary>
        /// Enable switching between a text box and the buttons for each sub directory of the current directory's path.
        /// </summary>
        public bool SwitchPathPartsAsButtonsEnabled
        {
            get
            {
                return (bool)GetValue(SwitchPathPartsAsButtonsEnabledProperty);
            }

            set
            {
                SetValue(SwitchPathPartsAsButtonsEnabledProperty, value);
            }
        }

        static FileSystemDialog()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FileSystemDialog), new FrameworkPropertyMetadata(typeof(FileSystemDialog)));
        }

        /// <summary>
        /// Creates a new <see cref="FileSystemDialog" />.
        /// </summary>
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
            bool showHiddenFilesAndDirectories, bool showSystemFilesAndDirectories,
            bool createNewDirectoryEnabled = false,
            bool switchPathPartsAsButtonsEnabled = false, bool pathPartsAsButtons = true)
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

            dialog.CreateNewDirectoryEnabled = createNewDirectoryEnabled;
            dialog.ShowHiddenFilesAndDirectories = showHiddenFilesAndDirectories;
            dialog.ShowSystemFilesAndDirectories = showSystemFilesAndDirectories;
            dialog.SwitchPathPartsAsButtonsEnabled = switchPathPartsAsButtonsEnabled;
            dialog.PathPartsAsButtons = pathPartsAsButtons;
        }
    }

    /// <summary>
    /// Arguments to initialize a dialog.
    /// </summary>
    public abstract class FileSystemDialogArguments
    {
        /// <summary>
        /// The fixed width of the dialog (nullable).
        /// </summary>
        public double? Width { get; set; }

        /// <summary>
        /// The fixed height of the dialog (nullable).
        /// </summary>
        public double? Height { get; set; }

        /// <summary>
        /// The current directory of the dialog.
        /// </summary>
        public string CurrentDirectory { get; set; }

        /// <summary>
        /// Enables the feature to create new directories. Notice: It does not have any effects for <see cref="OpenFileDialog" />.
        /// </summary>
        public bool CreateNewDirectoryEnabled { get; set; }

        /// <summary>
        /// Show the current directory path with a short cut button for each part in the path, instead of a text box.
        /// </summary>
        public bool PathPartsAsButtons { get; set; }

        /// <summary>
        /// Shows or hides hidden directories and files.
        /// </summary>
        public bool ShowHiddenFilesAndDirectories { get; set; }

        /// <summary>
        /// Shows or hides protected directories and files of the system.
        /// </summary>
        public bool ShowSystemFilesAndDirectories { get; set; }

        /// <summary>
        /// Enable switching between a text box and the buttons for each sub directory of the current directory's path.
        /// </summary>
        public bool SwitchPathPartsAsButtonsEnabled { get; set; }

        /// <summary>
        /// Callback after openening the dialog.
        /// </summary>
        public DialogOpenedEventHandler OpenedHandler { get; set; }

        /// <summary>
        /// Callback after closing the dialog.
        /// </summary>
        public DialogClosingEventHandler ClosingHandler { get; set; }

        /// <summary>
        /// Creates a new <see cref="FileSystemDialogArguments" />.
        /// </summary>
        public FileSystemDialogArguments()
        {
            Width = null;
            Height = null;
            CurrentDirectory = null;
            CreateNewDirectoryEnabled = false;
            ShowHiddenFilesAndDirectories = false;
            ShowSystemFilesAndDirectories = false;
            SwitchPathPartsAsButtonsEnabled = false;
            PathPartsAsButtons = true;
            OpenedHandler = null;
            ClosingHandler = null;
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="args"></param>
        public FileSystemDialogArguments(FileSystemDialogArguments args)
        {
            Width = args.Width;
            Height = args.Height;
            CurrentDirectory = args.CurrentDirectory;
            CreateNewDirectoryEnabled = args.CreateNewDirectoryEnabled;
            ShowHiddenFilesAndDirectories = args.ShowHiddenFilesAndDirectories;
            ShowSystemFilesAndDirectories = args.ShowSystemFilesAndDirectories;
            OpenedHandler = args.OpenedHandler;
            ClosingHandler = args.ClosingHandler;
        }
    }

    /// <summary>
    /// Base class for the result of a dialog.
    /// </summary>
    public abstract class FileSystemDialogResult
    {
        /// <summary>
        /// true, if the dialog was canceled
        /// </summary>
        public bool Canceled { get; protected set; }

        /// <summary>
        /// true, if the dialog was confirmed
        /// </summary>
        public bool Confirmed
        {
            get
            {
                return !Canceled;
            }
        }

        /// <summary>
        /// Creates a new <see cref="FileSystemDialogResult" />.
        /// </summary>
        /// <param name="canceled"></param>
        public FileSystemDialogResult(bool canceled)
        {
            Canceled = canceled;
        }
    }
}
