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

// use Pri.LongPath classes instead of System.IO for the MaterialDesignExtensions.LongPath build to support long file system paths on older Windows and .NET versions
#if LONG_PATH
using DirectoryInfo = Pri.LongPath.DirectoryInfo;
using FileInfo = Pri.LongPath.FileInfo;
using FileSystemInfo = Pri.LongPath.FileSystemInfo;
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

        protected override void SelectFileSystemEntry(FileSystemInfo fileSystemInfo)
        {
            if (fileSystemInfo != null)
            {
                if (fileSystemInfo is DirectoryInfo directoryInfo)
                {
                    CurrentDirectory = directoryInfo.FullName;
                    CurrentFile = null;
                }
                else if (fileSystemInfo is FileInfo fileInfo)
                {
                    CurrentFile = fileInfo.FullName;
                }
            }
        }
    }
}
