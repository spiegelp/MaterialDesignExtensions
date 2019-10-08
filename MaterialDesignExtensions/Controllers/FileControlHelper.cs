using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using MaterialDesignExtensions.Model;

// use Pri.LongPath classes instead of System.IO for the MaterialDesignExtensions.LongPath build to support long file system paths on older Windows and .NET versions
#if LONG_PATH
using DirectoryInfo = Pri.LongPath.DirectoryInfo;
using FileInfo = Pri.LongPath.FileInfo;
#endif

namespace MaterialDesignExtensions.Controllers
{
    /// <summary>
    /// Helper class for file system controls with general code for reuse.
    /// </summary>
    public abstract class FileControlHelper
    {
        private FileControlHelper() { }

        /// <summary>
        /// Updates the visibility of the ComboBox with the filter options.
        /// </summary>
        /// <param name="fileFiltersComboBox"></param>
        public static void UpdateFileFiltersVisibility(ComboBox fileFiltersComboBox)
        {
            if (fileFiltersComboBox != null
                && fileFiltersComboBox.ItemsSource != null
                && fileFiltersComboBox.ItemsSource.GetEnumerator().MoveNext())
            {
                fileFiltersComboBox.Visibility = Visibility.Visible;
            }
            else
            {
                fileFiltersComboBox.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Builds a list with the items of a directory.
        /// </summary>
        /// <param name="directoriesAndFiles"></param>
        /// <param name="controller"></param>
        /// <param name="groupFoldersAndFiles"></param>
        /// <param name="isSelectedFunc"></param>
        /// <returns></returns>
        public static IEnumerable GetFileSystemEntryItems(List<FileSystemInfo> directoriesAndFiles, FileSystemController controller, bool groupFoldersAndFiles, Func<FileInfo, bool> isSelectedFunc)
        {
            if (directoriesAndFiles == null || !directoriesAndFiles.Any())
            {
                return new ArrayList(0);
            }

            int numberOfItems = directoriesAndFiles.Count;

            if (groupFoldersAndFiles)
            {
                numberOfItems = numberOfItems + 2;
            }

            ArrayList items = new ArrayList(numberOfItems);

            for (int i = 0; i < directoriesAndFiles.Count; i++)
            {
                FileSystemInfo item = directoriesAndFiles[i];

                if (item is DirectoryInfo directoryInfo)
                {
                    if (groupFoldersAndFiles && i == 0)
                    {
                        items.Add(new FileSystemEntriesGroupHeader() { Header = Localization.Strings.Folders, ShowSeparator = false });
                    }

                    bool isSelected = directoryInfo.FullName == controller.CurrentDirectory?.FullName;

                    items.Add(new DirectoryInfoItem() { IsSelected = isSelected, Value = directoryInfo });
                }
                else if (item is FileInfo fileInfo)
                {
                    if (groupFoldersAndFiles)
                    {
                        if (i == 0)
                        {
                            items.Add(new FileSystemEntriesGroupHeader() { Header = Localization.Strings.Files, ShowSeparator = false });
                        }
                        else if (directoriesAndFiles[i - 1] is DirectoryInfo)
                        {
                            items.Add(new FileSystemEntriesGroupHeader() { Header = Localization.Strings.Files, ShowSeparator = true });
                        }
                    }

                    items.Add(new FileInfoItem() { IsSelected = isSelectedFunc(fileInfo), Value = fileInfo });
                }
            }

            return items;
        }
    }
}
