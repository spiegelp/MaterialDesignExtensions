using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MaterialDesignExtensions.Model
{
    /// <summary>
    /// An navigation item of a <see cref="Controls.SideNavigation" />.
    /// </summary>
    public class NavigationItem : BaseNavigationItem
    {
        private object m_icon;
        private DataTemplate m_iconTemplate;
        private string m_label;

        /// <summary>
        /// The icon of this navigation item.
        /// </summary>
        public object Icon
        {
            get
            {
                return m_icon;
            }

            set
            {
                m_icon = value;

                OnPropertyChanged(nameof(Icon));
            }
        }

        /// <summary>
        /// An optional template for the icon.
        /// </summary>
        public DataTemplate IconTemplate
        {
            get
            {
                return m_iconTemplate;
            }

            set
            {
                m_iconTemplate = value;

                OnPropertyChanged(nameof(IconTemplate));
            }
        }

        /// <summary>
        /// The label of this navigation item.
        /// </summary>
        public string Label
        {
            get
            {
                return m_label;
            }

            set
            {
                m_label = value;

                OnPropertyChanged(nameof(Label));
            }
        }

        /// <summary>
        /// Creates a new <see cref="NavigationItem" />.
        /// </summary>
        public NavigationItem()
        {
            m_icon = null;
            m_label = null;
        }
    }
}
