﻿using System.IO;
using System.Windows;
using System.Windows.Controls;

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
