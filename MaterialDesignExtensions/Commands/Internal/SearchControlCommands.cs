using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MaterialDesignExtensions.Commands.Internal
{
    /// <summary>
    /// Class containing commands used by the search controls for internal use only.
    /// </summary>
    public class SearchControlCommands
    {

        // abstract class with private constructor to prevent object initialization
        private SearchControlCommands() { }

        /// <summary>
        /// Internal command used by the XAML template (public to be available in the XAML template). Not intended for external usage.
        /// </summary>
        public static readonly RoutedCommand SelectSearchSuggestionCommand = new RoutedCommand();
    }
}
