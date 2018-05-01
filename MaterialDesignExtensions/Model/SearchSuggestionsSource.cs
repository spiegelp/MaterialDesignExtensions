using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialDesignExtensions.Model
{
    /// <summary>
    /// Interface for a source for providing suggestions to search for.
    /// </summary>
    public interface ISearchSuggestionsSource
    {
        /// <summary>
        /// Returns historical search suggestions for an empty and focused search.
        /// </summary>
        /// <returns></returns>
        IList<string> GetSearchSuggestions();

        /// <summary>
        /// Returns suggestions based on auto-completion.
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        IList<string> GetAutoCompletion(string searchTerm);
    }

    /// <summary>
    /// Default implementation of <see cref="ISearchSuggestionsSource" />.
    /// </summary>
    public class SearchSuggestionsSource : ISearchSuggestionsSource
    {
        /// <summary>
        /// Creates a new <see cref="SearchSuggestionsSource" />.
        /// </summary>
        public SearchSuggestionsSource() { }

        /// <summary>
        /// Returns historical search suggestions for an empty and focused search.
        /// </summary>
        /// <returns></returns>
        public IList<string> GetAutoCompletion(string searchTerm)
        {
            return null;
        }

        /// <summary>
        /// Returns suggestions based on auto-completion.
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        public IList<string> GetSearchSuggestions()
        {
            return null;
        }
    }
}
