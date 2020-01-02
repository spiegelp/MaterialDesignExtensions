﻿namespace MaterialDesignExtensions.Model
{
    /// <summary>
    /// The base class for not selectable items (headers, dividers) in a <see cref="Controls.SideNavigation" />.
    /// </summary>
    public abstract class NotSelectableNavigationItem : BaseNavigationItem
    {
        /// <summary>
        /// True, if the user can select this navigation item.
        /// </summary>
        public override bool IsSelectable
        {
            get
            {
                return false;
            }

            set { }
        }

        /// <summary>
        /// True, if the navigation item is selected.
        /// </summary>
        public override bool IsSelected
        {
            get
            {
                return false;
            }

            set { }
        }

        /// <summary>
        /// The delegate for a <see cref="INavigationItem.NavigationItemSelectedCallback"/> method.
        /// </summary>
        /// <param name="navigationItem"></param>
        /// <returns></returns>
        public override NavigationItemSelectedCallback NavigationItemSelectedCallback
        {
            get
            {
                return null;
            }

            set { }
        }

        /// <summary>
        /// Creates a new <see cref="NotSelectableNavigationItem" />.
        /// </summary>
        public NotSelectableNavigationItem() : base() { }
    }
}
