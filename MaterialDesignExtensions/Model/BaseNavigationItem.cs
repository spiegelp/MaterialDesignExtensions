using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialDesignExtensions.Model
{
    /// <summary>
    /// Base class for an item in a <see cref="Controls.SideNavigation" />.
    /// </summary>
    public abstract class BaseNavigationItem : INavigationItem
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected bool m_isSelectable;
        protected bool m_isSelected;
        protected NavigationItemSelectedCallback m_callback;

        /// <summary>
        /// True, if the user can select this navigation item.
        /// </summary>
        public virtual bool IsSelectable
        {
            get
            {
                return m_isSelectable;
            }

            set
            {
                m_isSelectable = value;
            }
        }

        /// <summary>
        /// True, if the navigation item is selected.
        /// </summary>
        public virtual bool IsSelected
        {
            get
            {
                return m_isSelected;
            }

            set
            {
                m_isSelected = value;

                OnPropertyChanged(nameof(IsSelected));
            }
        }

        /// <summary>
        /// An optional callback method raised, when this navigation item will be selected.
        /// This API is necessary because events are not async.
        /// </summary>
        public virtual NavigationItemSelectedCallback NavigationItemSelectedCallback
        {
            get
            {
                return m_callback;
            }

            set
            {
                m_callback = value;

                OnPropertyChanged(nameof(NavigationItemSelectedCallback));
            }
        }

        /// <summary>
        /// Creates a new <see cref="BaseNavigationItem" />.
        /// </summary>
        public BaseNavigationItem()
        {
            m_isSelectable = true;
            m_isSelected = false;
            m_callback = null;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null && !string.IsNullOrWhiteSpace(propertyName))
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
