﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using MaterialDesignExtensions.Model;

namespace MaterialDesignExtensions.TemplateSelectors
{
    internal class FileSystemEntryItemTemplateSelector : DataTemplateSelector
    {
        public bool ForMultipleSelection { get; set; }

        public FileSystemEntryItemTemplateSelector()
            : base()
        {
            ForMultipleSelection = false;
        }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (container is FrameworkElement element && item != null)
            {
                if (item is DirectoryInfoItem)
                {
                    return element.FindResource("directoryInfoItemTemplate") as DataTemplate;
                }
                else if (item is FileInfoItem)
                {
                    if (ForMultipleSelection)
                    {
                        return element.FindResource("fileInfoItemMultipleTemplate") as DataTemplate;
                    }
                    else
                    {
                        return element.FindResource("fileInfoItemTemplate") as DataTemplate;
                    }
                }
                else if (item is FileSystemEntriesGroupHeader)
                {
                    return element.FindResource("fileSystemEntriesGroupHeaderTemplate") as DataTemplate;
                }
            }

            return null;
        }
    }
}
