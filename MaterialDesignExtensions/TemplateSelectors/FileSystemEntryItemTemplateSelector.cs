using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MaterialDesignExtensions.TemplateSelectors
{
    internal class FileSystemEntryItemTemplateSelector : DataTemplateSelector
    {
        public FileSystemEntryItemTemplateSelector() : base() { }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (container is FrameworkElement element && item != null)
            {
                if (item is DirectoryInfo)
                {
                    return element.FindResource("directoryInfoItemTemplate") as DataTemplate;
                }
                else if (item is FileInfo)
                {
                    return element.FindResource("fileInfoItemTemplate") as DataTemplate;
                }
            }

            return null;
        }
    }
}
