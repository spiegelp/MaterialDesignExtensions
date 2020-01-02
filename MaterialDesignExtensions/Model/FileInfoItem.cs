using System.IO;

// use Pri.LongPath classes instead of System.IO for the MaterialDesignExtensions.LongPath build to support long file system paths on older Windows and .NET versions
#if LONG_PATH
using FileInfo = Pri.LongPath.FileInfo;
#endif

namespace MaterialDesignExtensions.Model
{
    /// <summary>
    /// A file item in the current directory list of file system controls.
    /// </summary>
    public class FileInfoItem : FileSystemEntryItem<FileInfo>
    {
        /// <summary>
        /// Creates a new <see cref="FileInfoItem" />.
        /// </summary>
        public FileInfoItem() : base() { }
    }
}
