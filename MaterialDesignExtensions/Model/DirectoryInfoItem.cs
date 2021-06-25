using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// use Pri.LongPath classes instead of System.IO for the MaterialDesignExtensions.LongPath build to support long file system paths on older Windows and .NET versions
#if LONG_PATH
using DirectoryInfo = Pri.LongPath.DirectoryInfo;
#endif

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
