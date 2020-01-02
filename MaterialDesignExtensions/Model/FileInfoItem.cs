using System.IO;

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
