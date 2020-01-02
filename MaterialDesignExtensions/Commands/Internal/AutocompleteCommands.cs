using System.Windows.Input;

namespace MaterialDesignExtensions.Commands.Internal
{
    /// <summary>
    /// Class containing commands used by the <see cref="Controls.Autocomplete"/> for internal use only.
    /// </summary>
    public class AutocompleteCommands
    {
        // abstract class with private constructor to prevent object initialization
        private AutocompleteCommands() { }

        /// <summary>
        /// Internal command used by the XAML template (public to be available in the XAML template). Not intended for external usage.
        /// </summary>
        public static readonly RoutedCommand SelectAutocompleteItemCommand = new RoutedCommand();
    }
}
