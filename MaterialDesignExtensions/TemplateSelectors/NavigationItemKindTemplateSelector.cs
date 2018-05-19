using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using MaterialDesignExtensions.Model;

namespace MaterialDesignExtensions.TemplateSelectors
{
    internal class NavigationItemKindTemplateSelector : DataTemplateSelector
    {
        public NavigationItemKindTemplateSelector() : base() { }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (container is FrameworkElement element && item is INavigationItem navigationItem)
            {
                if (navigationItem.IsSelectable)
                {
                    return element.FindResource("selectableNavigationItemTemplate") as DataTemplate;
                }
                else
                {
                    return element.FindResource("notSelectableNavigationItemTemplate") as DataTemplate;
                }
            }

            return null;
        }
    }
}
