using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using MaterialDesignThemes.Wpf;

namespace MaterialDesignExtensions.Controls
{
    public abstract class BaseFileDialog : FileSystemDialog
    {
        protected static readonly string FileControlName = "fileControl";

        public static readonly DependencyProperty CurrentFileProperty = DependencyProperty.Register(
                nameof(CurrentFile),
                typeof(string),
                typeof(BaseFileDialog),
                new PropertyMetadata(null));

        /// <summary>
        /// The current file of the dialog.
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

        protected BaseFileControl m_fileControl;

        static BaseFileDialog()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BaseFileDialog), new FrameworkPropertyMetadata(typeof(BaseFileDialog)));
        }

        public BaseFileDialog()
            : base()
        {
            m_fileControl = null;

            Loaded += LoadedHandler;
            Unloaded += UnloadedHandler;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            m_fileControl = Template.FindName(FileControlName, this) as BaseFileControl;
        }

        protected void LoadedHandler(object sender, RoutedEventArgs args)
        {
            m_fileControl.Cancel += CancelHandler;
            m_fileControl.FileSelected += OpenDirectoryControlFileSelectedHandler;
        }

        protected void UnloadedHandler(object sender, RoutedEventArgs args)
        {
            m_fileControl.Cancel -= CancelHandler;
            m_fileControl.FileSelected -= OpenDirectoryControlFileSelectedHandler;
        }

        protected abstract void CancelHandler(object sender, RoutedEventArgs args);

        protected abstract void OpenDirectoryControlFileSelectedHandler(object sender, RoutedEventArgs args);
    }

    public abstract class FileDialogResult : FileSystemDialogResult
    {
        /// <summary>
        /// The selected file as <see cref="FileInfo" />
        /// </summary>
        public FileInfo FileInfo { get; private set; }

        /// <summary>
        /// The selected file as full filename string.
        /// </summary>
        public string File
        {
            get
            {
                return FileInfo?.FullName;
            }
        }

        public FileDialogResult(bool canceled, FileInfo fileInfo)
            : base(canceled)
        {
            FileInfo = fileInfo;
        }
    }
}
