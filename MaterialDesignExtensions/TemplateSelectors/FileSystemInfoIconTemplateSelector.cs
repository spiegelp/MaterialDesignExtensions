using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

using MaterialDesignThemes.Wpf;

namespace MaterialDesignExtensions.TemplateSelectors
{
    internal class FileSystemInfoIconTemplateSelector : DataTemplateSelector
    {
        public FileSystemInfoIconTemplateSelector() : base() { }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (container is FrameworkElement element && item != null)
            {
                if (item is PackIconKind)
                {
                    return element.FindResource("fileSystemInfoPackIconTemplate") as DataTemplate;
                }
                else if (item is BitmapImage)
                {
                    return element.FindResource("fileSystemInfoImageTemplate") as DataTemplate;
                }
                else if (item is Converters.AsyncImageTask)
                {
                    return element.FindResource("fileSystemInfoAsyncImageTaskTemplate") as DataTemplate;
                }
            }

            return null;
        }
    }
}
