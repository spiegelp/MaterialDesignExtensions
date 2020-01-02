using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

using MaterialDesignThemes.Wpf;

namespace MaterialDesignExtensions.TemplateSelectors
{
    internal class FileSystemEntryItemIconTemplateSelector : DataTemplateSelector
    {
        public FileSystemEntryItemIconTemplateSelector() : base() { }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (container is FrameworkElement element && item != null)
            {
                if (item is PackIconKind)
                {
                    return element.FindResource("fileSystemEntryItemPackIconTemplate") as DataTemplate;
                }
                else if (item is BitmapImage)
                {
                    return element.FindResource("fileSystemEntryItemImageIconTemplate") as DataTemplate;
                }
                else if (item is Converters.AsyncImageTask)
                {
                    return element.FindResource("fileSystemEntryItemAsyncImageTaskIconTemplate") as DataTemplate;
                }
            }

            return null;
        }
    }
}
