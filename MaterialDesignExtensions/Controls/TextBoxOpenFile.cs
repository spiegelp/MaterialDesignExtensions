using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MaterialDesignExtensions.Controls
{
    public class TextBoxOpenFile : TextBoxFileSystemPath
    {
        /// <summary>
        /// The arguments for the dialog.
        /// </summary>
        public static readonly DependencyProperty DialogArgsProperty = DependencyProperty.Register(
            nameof(DialogArgs), typeof(OpenFileDialogArguments), typeof(TextBoxOpenFile));

        /// <summary>
        /// The arguments for the dialog.
        /// </summary>
        public OpenFileDialogArguments DialogArgs
        {
            get
            {
                return (OpenFileDialogArguments)GetValue(DialogArgsProperty);
            }

            set
            {
                SetValue(DialogArgsProperty, value);
            }
        }

        /// <summary>
        /// The selected file.
        /// </summary>
        public static readonly DependencyProperty FileProperty = DependencyProperty.Register(
            nameof(File), typeof(string), typeof(TextBoxOpenFile));

        /// <summary>
        /// The selected file.
        /// </summary>
        public string File
        {
            get
            {
                return (string)GetValue(FileProperty);
            }

            set
            {
                SetValue(FileProperty, value);
            }
        }

        static TextBoxOpenFile()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TextBoxOpenFile), new FrameworkPropertyMetadata(typeof(TextBoxOpenFile)));
        }

        public TextBoxOpenFile() : base() { }

        /// <summary>
        /// Shows the according dialog for the control.
        /// </summary>
        /// <returns></returns>
        protected override async Task ShowDialogAsync()
        {
            OpenFileDialogArguments args = null;

            if (DialogArgs != null)
            {
                args = new OpenFileDialogArguments(DialogArgs);
            }
            else
            {
                args = new OpenFileDialogArguments();
            }

            if (!string.IsNullOrWhiteSpace(File))
            {
                string directory = Path.GetDirectoryName(File);

                if (!string.IsNullOrWhiteSpace(directory) && Directory.Exists(directory))
                {
                    args.CurrentDirectory = directory;
                }
            }

            OpenFileDialogResult result = null;

            if (DialogHost != null)
            {
                result = await OpenFileDialog.ShowDialogAsync(DialogHost, args);
            }
            else
            {
                result = await OpenFileDialog.ShowDialogAsync(DialogHostName, args);
            }

            if (result != null && result.Confirmed)
            {
                File = result.FileInfo.FullName;
            }
        }
    }
}
