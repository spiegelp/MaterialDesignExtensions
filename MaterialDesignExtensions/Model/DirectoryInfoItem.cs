using System.IO;

namespace MaterialDesignExtensions.Model
{
    /// <summary>
    /// A directory item in the current directory list of file system controls.
    /// </summary>
    public class DirectoryInfoItem : FileSystemEntryItem<DirectoryInfo>
    {
        /// <summary>
        /// Creates a new <see cref="DirectoryInfoItem" />.
        /// </summary>
        public DirectoryInfoItem() : base() { }
    }
}
