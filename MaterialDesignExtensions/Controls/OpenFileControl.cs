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

#if LONG_PATH
using DirectoryInfo = Pri.LongPath.DirectoryInfo;
using FileInfo = Pri.LongPath.FileInfo;
#endif

namespace MaterialDesignExtensions.Controls
{
    /// <summary>
    /// A control for selecting a file to open.
    /// </summary>
    public class OpenFileControl : BaseFileControl
    {
        static OpenFileControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(OpenFileControl), new FrameworkPropertyMetadata(typeof(OpenFileControl)));
        }

        /// <summary>
        /// Creates a new <see cref="OpenFileControl" />.
        /// </summary>
        public OpenFileControl() : base() { }

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

        protected override void SelectFileSystemEntryCommandHandler(object sender, ExecutedRoutedEventArgs args)
        {
            if (args.Parameter != null)
            {
                if (args.Parameter is DirectoryInfo directoryInfo)
                {
                    CurrentDirectory = directoryInfo.FullName;
                    CurrentFile = null;
                }
                else if (args.Parameter is FileInfo fileInfo)
                {
                    CurrentFile = fileInfo.FullName;
                }
            }
        }
    }
}
