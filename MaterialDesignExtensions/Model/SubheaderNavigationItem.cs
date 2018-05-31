using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialDesignExtensions.Model
{
    public class SubheaderNavigationItem : NotSelectableNavigationItem
    {
        private string m_subheader;

        public string Subheader
        {
            get
            {
                return m_subheader;
            }

            set
            {
                m_subheader = value;

                OnPropertyChanged(nameof(Subheader));
            }
        }

        public SubheaderNavigationItem()
            : base()
        {
            m_subheader = null;
        }
    }
}
