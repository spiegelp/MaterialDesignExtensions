using System.Windows.Input;

namespace MaterialDesignExtensions.Commands.Internal
{
    /// <summary>
    /// Class containing commands used by the <see cref="Controls.OversizedNumberSpinner"/> for internal use only.
    /// </summary>
    public class OversizedNumberSpinnerCommands
    {
        // abstract class with private constructor to prevent object initialization
        private OversizedNumberSpinnerCommands() { }

        /// <summary>
        /// Internal command used by the XAML template (public to be available in the XAML template). Not intended for external usage.
        /// </summary>
        public static readonly RoutedCommand EditValueCommand = new RoutedCommand();

        /// <summary>
        /// Internal command used by the XAML template (public to be available in the XAML template). Not intended for external usage.
        /// </summary>
        public static readonly RoutedCommand MinusCommand = new RoutedCommand();

        /// <summary>
        /// Internal command used by the XAML template (public to be available in the XAML template). Not intended for external usage.
        /// </summary>
        public static readonly RoutedCommand PlusCommand = new RoutedCommand();
    }
}
