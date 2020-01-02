using System.IO;
using System.Windows;

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
