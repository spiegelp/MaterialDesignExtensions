using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MaterialDesignExtensions.Controls
{
    public class OpenFileControl : BaseFileControl
    {
        static OpenFileControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(OpenFileControl), new FrameworkPropertyMetadata(typeof(OpenFileControl)));
        }

        public OpenFileControl() : base() { }

        protected override void CurrentFileChangedHandler(string newCurrentFile)
        {
            m_controller.SelectFile(newCurrentFile);
        }

        protected override void FileSystemEntryItemsListBoxSelectionChangedHandler(object sender, SelectionChangedEventArgs args)
        {
            if (sender == m_fileSystemEntryItemsListBox)
            {
                if (m_fileSystemEntryItemsListBox.SelectedItem != null)
                {
                    if (m_fileSystemEntryItemsListBox.SelectedItem is DirectoryInfo directoryInfo)
                    {
                        CurrentDirectory = directoryInfo.FullName;
                        CurrentFile = null;
                    }
                    else if (m_fileSystemEntryItemsListBox.SelectedItem is FileInfo fileInfo)
                    {
                        CurrentFile = fileInfo.FullName;
                    }
                }
            }
        }
    }
}
