using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialDesignExtensions.Model
{
    public abstract class BaseNavigationItem : INavigationItem
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected bool m_isSelectable;
        protected bool m_isSelected;
        protected NavigationItemSelectedCallback m_callback;

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
