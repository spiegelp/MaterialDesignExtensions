using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MaterialDesignExtensions.Model;

namespace MaterialDesignExtensions.Controls
{
    /// <summary>
    /// A special version of the side navigation.
    /// </summary>
    public class NavigationRail : SideNavigation
    {
        /// <summary>
        /// Creates a new <see cref="NavigationRail" />.
        /// </summary>
        public NavigationRail() : base() { }

        protected override void InitItems(IList values)
        {
            if (m_navigationItemsControl != null)
            {
                IList<FirstLevelNavigationItem> navigationItems = new List<FirstLevelNavigationItem>();

                if (values != null)
                {
                    foreach (object item in values)
                    {
                        if (item is FirstLevelNavigationItem navigationItem)
                        {
                            navigationItems.Add(navigationItem);
                        }
                        else
                        {
                            throw new NotSupportedException($"Not supported item type {item.GetType().FullName}");
                        }
                    }
                }

                m_navigationItemsControl.ItemsSource = navigationItems;

                INavigationItem selectedItem = navigationItems.FirstOrDefault(item => item.IsSelected);

                if (selectedItem != null)
                {
                    if (SelectedItem != selectedItem)
                    {
                        SelectedItem = selectedItem;
                    }
                }
                else
                {
                    SelectedItem = null;
                }
            }
        }
    }
}
