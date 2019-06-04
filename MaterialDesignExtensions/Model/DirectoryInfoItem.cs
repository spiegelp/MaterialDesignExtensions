using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if LONG_PATH
using DirectoryInfo = Pri.LongPath.DirectoryInfo;
#endif

namespace MaterialDesignExtensions.Model
{
    public class DirectoryInfoItem : FileSystemEntryItem<DirectoryInfo>
    {
        public DirectoryInfoItem() : base() { }
    }
}
