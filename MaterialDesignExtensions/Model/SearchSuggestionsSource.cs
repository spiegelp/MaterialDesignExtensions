using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialDesignExtensions.Model
{
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

    public class SearchSuggestionsSource : ISearchSuggestionsSource
    {
        public SearchSuggestionsSource() { }

        public IList<string> GetAutoCompletion(string searchTerm)
        {
            return null;
        }

        public IList<string> GetSearchSuggestions()
        {
            return null;
        }
    }
}
