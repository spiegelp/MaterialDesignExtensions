using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

// use Pri.LongPath classes instead of System.IO for the MaterialDesignExtensions.LongPath build to support long file system paths on older Windows and .NET versions
#if LONG_PATH
using DirectoryInfo = Pri.LongPath.DirectoryInfo;
using FileInfo = Pri.LongPath.FileInfo;
#endif

namespace MaterialDesignExtensions.TemplateSelectors
{
    internal class FileSystemEntryInfoTemplateSelector : DataTemplateSelector
    {
        public FileSystemEntryInfoTemplateSelector() : base() { }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (container is FrameworkElement element && item != null)
            {
                if (item is DirectoryInfo)
                {
                    return element.FindResource("directoryInfoInfoTemplate") as DataTemplate;
                }
                else if (item is FileInfo)
                {
                    return element.FindResource("fileInfoInfoTemplate") as DataTemplate;
                }
            }

            return null;
        }
    }
}
