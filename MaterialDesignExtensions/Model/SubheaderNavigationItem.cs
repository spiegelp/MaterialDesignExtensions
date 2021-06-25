using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialDesignExtensions.Model
{
    /// <summary>
    /// A header item of a <see cref="Controls.SideNavigation" />.
    /// </summary>
    public class SubheaderNavigationItem : NotSelectableNavigationItem
    {
        private string m_subheader;

        /// <summary>
        /// The label of this header.
        /// </summary>
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

        /// <summary>
        /// Creates a new <see cref="SubheaderNavigationItem" />.
        /// </summary>
        public SubheaderNavigationItem()
            : base()
        {
            m_subheader = null;
        }
    }
}
