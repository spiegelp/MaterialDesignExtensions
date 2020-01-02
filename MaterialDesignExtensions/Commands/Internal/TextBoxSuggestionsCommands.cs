using System.Windows.Input;

namespace MaterialDesignExtensions.Commands.Internal
{
    /// <summary>
    /// Class containing commands used by the <see cref="Controls.TextBoxSuggestions"/> for internal use only.
    /// </summary>
    public class TextBoxSuggestionsCommands
    {
        // abstract class with private constructor to prevent object initialization
        private TextBoxSuggestionsCommands() { }

        /// <summary>
        /// Internal command used by the XAML template (public to be available in the XAML template). Not intended for external usage.
        /// </summary>
        public static readonly RoutedCommand SelectSuggestionItemCommand = new RoutedCommand();
    }
}
