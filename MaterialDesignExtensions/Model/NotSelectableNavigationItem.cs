using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialDesignExtensions.Model
{
    public abstract class NotSelectableNavigationItem : BaseNavigationItem
    {
        public override bool IsSelectable
        {
            get
            {
                return false;
            }

            set { }
        }

        public override bool IsSelected
        {
            get
            {
                return false;
            }

            set { }
        }

        public override NavigationItemSelectedCallback NavigationItemSelectedCallback
        {
            get
            {
                return null;
            }

            set { }
        }

        public NotSelectableNavigationItem() : base() { }
    }
}
