using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MaterialDesignExtensions.Controls
{
    /// <summary>
    /// A control with a text box and a button to open a directory dialog.
    /// </summary>
    public class TextBoxOpenDirectory : TextBoxFileSystemPath
    {
        /// <summary>
        /// The arguments for the dialog.
        /// </summary>
        public static readonly DependencyProperty DialogArgsProperty = DependencyProperty.Register(
            nameof(DialogArgs), typeof(OpenDirectoryDialogArguments), typeof(TextBoxOpenDirectory));

        /// <summary>
        /// The arguments for the dialog.
        /// </summary>
        public OpenDirectoryDialogArguments DialogArgs
        {
            get
            {
                return (OpenDirectoryDialogArguments)GetValue(DialogArgsProperty);
            }

            set
            {
                SetValue(DialogArgsProperty, value);
            }
        }

        /// <summary>
        /// The selected directory.
        /// </summary>
        public static readonly DependencyProperty DirectoryProperty = DependencyProperty.Register(
            nameof(Directory), typeof(string), typeof(TextBoxOpenDirectory));

        /// <summary>
        /// The selected directory.
        /// </summary>
        public string Directory
        {
            get
            {
                return (string)GetValue(DirectoryProperty);
            }

            set
            {
                SetValue(DirectoryProperty, value);
            }
        }

        static TextBoxOpenDirectory()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TextBoxOpenDirectory), new FrameworkPropertyMetadata(typeof(TextBoxOpenDirectory)));
        }

        /// <summary>
        /// Creates a new <see cref="TextBoxOpenDirectory" />.
        /// </summary>
        public TextBoxOpenDirectory() : base() { }

        /// <summary>
        /// Shows the according dialog for the control.
        /// </summary>
        /// <returns></returns>
        protected override async Task ShowDialogAsync()
        {
            OpenDirectoryDialogArguments args = null;

            if (DialogArgs != null)
            {
                args = new OpenDirectoryDialogArguments(DialogArgs);
            }
            else
            {
                args = new OpenDirectoryDialogArguments();
            }

            if (!string.IsNullOrWhiteSpace(Directory) && System.IO.Directory.Exists(Directory))
            {
                args.CurrentDirectory = Directory;
            }

            OpenDirectoryDialogResult result = null;

            if (DialogHost != null)
            {
                result = await OpenDirectoryDialog.ShowDialogAsync(DialogHost, args);
            } else
            {
                result = await OpenDirectoryDialog.ShowDialogAsync(DialogHostName, args);
            }

            if (result != null && result.Confirmed)
            {
                Directory = result.DirectoryInfo.FullName;
            }
        }
    }
}
