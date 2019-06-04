using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if LONG_PATH
using FileInfo = Pri.LongPath.FileInfo;
#endif

namespace MaterialDesignExtensions.Model
{
    public class FileInfoItem : FileSystemEntryItem<FileInfo>
    {
        public FileInfoItem() : base() { }
    }
}
