using System.Windows.Input;

namespace MaterialDesignExtensions.Commands.Internal
{
    /// <summary>
    /// Class containing commands used by the <see cref="Controls.SideNavigation"/> for internal use only.
    /// </summary>
    public class SideNavigationCommands
    {
        // abstract class with private constructor to prevent object initialization
        private SideNavigationCommands() { }

        /// <summary>
        /// Internal command used by the XAML template (public to be available in the XAML template). Not intended for external usage.
        /// </summary>
        public static readonly RoutedCommand SelectNavigationItemCommand = new RoutedCommand();
    }
}
