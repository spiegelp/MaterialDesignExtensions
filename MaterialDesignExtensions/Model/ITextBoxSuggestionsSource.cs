using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialDesignExtensions.Model
{
    /// <summary>
    /// Special version of <see cref="IAutocompleteSource" /> for the <see cref="Controls.TextBoxSuggestions" /> controls.
    /// </summary>
    public interface ITextBoxSuggestionsSource : IAutocompleteSource<string>
    {
    }

    /// <summary>
    /// Base class for text box suggestion sources providing default implementations of the necessary interface.
    /// </summary>
    public abstract class TextBoxSuggestionsSource : ITextBoxSuggestionsSource
    {
        /// <summary>
        /// Creates a new <see cref="TextBoxSuggestionsSource" />.
        /// </summary>
        public TextBoxSuggestionsSource() { }

        /// <summary>
        /// Does the search <see cref="Controls.TextBoxSuggestions" /> controls.
        /// </summary>
        /// <param name="searchTerm">The term to search for</param>
        /// <returns>The items found for the search term</returns>
        public abstract IEnumerable<string> Search(string searchTerm);

        /// <summary>
        /// Does the search for the <see cref="Controls.TextBoxSuggestions" /> controls.
        /// </summary>
        /// <param name="searchTerm">The term to search for</param>
        /// <returns>The items found for the search term</returns>
        IEnumerable IAutocompleteSource.Search(string searchTerm)
        {
            return Search(searchTerm);
        }
    }
}
