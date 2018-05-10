using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialDesignExtensions.Model
{
    public class NavigationItem : BaseNavigationItem
    {
        private object m_icon;
        private string m_label;

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

        public NavigationItem()
        {
            m_icon = null;
            m_label = null;
        }
    }
}
