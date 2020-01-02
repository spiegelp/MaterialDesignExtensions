using System.Windows.Input;

namespace MaterialDesignExtensions.Commands.Internal
{
    /// <summary>
    /// Class containing commands used by the <see cref="Controls.Stepper"/> for internal use only.
    /// </summary>
    public class StepperCommands
    {
        // abstract class with private constructor to prevent object initialization
        private StepperCommands() { }

        /// <summary>
        /// Internal command used by the XAML template (public to be available in the XAML template). Not intended for external usage.
        /// </summary>
        public static readonly RoutedCommand BackCommand = new RoutedCommand();

        /// <summary>
        /// Internal command used by the XAML template (public to be available in the XAML template). Not intended for external usage.
        /// </summary>
        public static readonly RoutedCommand CancelCommand = new RoutedCommand();

        /// <summary>
        /// Internal command used by the XAML template (public to be available in the XAML template). Not intended for external usage.
        /// </summary>
        public static readonly RoutedCommand ContinueCommand = new RoutedCommand();

        /// <summary>
        /// Internal command used by the XAML template (public to be available in the XAML template). Not intended for external usage.
        /// </summary>
        public static readonly RoutedCommand StepSelectedCommand = new RoutedCommand();
    }
}
