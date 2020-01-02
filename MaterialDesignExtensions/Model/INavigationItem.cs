using System.ComponentModel;

namespace MaterialDesignExtensions.Model
{
    /// <summary>
    /// The basic interface for an item of a <see cref="Controls.SideNavigation" />.
    /// </summary>
    public interface INavigationItem : INotifyPropertyChanged
    {
        /// <summary>
        /// True, if the user can select this navigation item.
        /// </summary>
        bool IsSelectable { get; set; }

        /// <summary>
        /// True, if the navigation item is selected.
        /// </summary>
        bool IsSelected { get; set; }

        /// <summary>
        /// An optional callback method raised, when this navigation item will be selected.
        /// This API is necessary because events are not async.
        /// </summary>
        NavigationItemSelectedCallback NavigationItemSelectedCallback { get; set; }
    }

    /// <summary>
    /// The delegate for a <see cref="INavigationItem.NavigationItemSelectedCallback"/> method.
    /// </summary>
    /// <param name="navigationItem"></param>
    /// <returns></returns>
    public delegate object NavigationItemSelectedCallback(INavigationItem navigationItem);
}
