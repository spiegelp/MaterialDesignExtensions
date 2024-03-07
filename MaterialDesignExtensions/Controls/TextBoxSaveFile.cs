using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MaterialDesignExtensions.Controls
{
    public class TextBoxSaveFile : TextBoxFileSystemPath
    {
        /// <summary>
        /// The arguments for the dialog.
        /// </summary>
        public static readonly DependencyProperty DialogArgsProperty = DependencyProperty.Register(
            nameof(DialogArgs), typeof(SaveFileDialogArguments), typeof(TextBoxSaveFile));

        /// <summary>
        /// The arguments for the dialog.
        /// </summary>
        public SaveFileDialogArguments DialogArgs
        {
            get
            {
                return (SaveFileDialogArguments)GetValue(DialogArgsProperty);
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
            nameof(File), typeof(string), typeof(TextBoxSaveFile));

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

        static TextBoxSaveFile()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TextBoxSaveFile), new FrameworkPropertyMetadata(typeof(TextBoxSaveFile)));
        }

        public TextBoxSaveFile() : base() { }

        protected override async Task ShowDialogAsync()
        {
            SaveFileDialogArguments args = null;

            if (DialogArgs != null)
            {
                args = new SaveFileDialogArguments(DialogArgs);
            }
            else
            {
                args = new SaveFileDialogArguments();
            }

            if (!string.IsNullOrWhiteSpace(File))
            {
                string directory = Path.GetDirectoryName(File);

                if (!string.IsNullOrWhiteSpace(directory) && Directory.Exists(directory))
                {
                    args.CurrentDirectory = directory;
                }
            }

            SaveFileDialogResult result = null;

            if (DialogHost != null)
            {
                result = await SaveFileDialog.ShowDialogAsync(DialogHost, args);
            }
            else
            {
                result = await SaveFileDialog.ShowDialogAsync(DialogHostName, args);
            }

            if (result != null && result.Confirmed)
            {
                File = result.FileInfo.FullName;
            }
        }
    }
}
