using System.Collections;
using System.Collections.Generic;

namespace MaterialDesignExtensions.Model
{
    /// <summary>
    /// Interface for a autocomplete source as needed by controls for data binding.
    /// </summary>
    public interface IAutocompleteSource
    {
        /// <summary>
        /// Does the search for the autocomplete.
        /// </summary>
        /// <param name="searchTerm">The term to search for</param>
        /// <returns>The items found for the search term</returns>
        IEnumerable Search(string searchTerm);
    }

    /// <summary>
    /// Generic version of the <see cref="IAutocompleteSource" /> interface to work with type safety.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IAutocompleteSource<T> : IAutocompleteSource
    {
        /// <summary>
        /// Does the search for the autocomplete.
        /// </summary>
        /// <param name="searchTerm">The term to search for</param>
        /// <returns>The items found for the search term</returns>
        new IEnumerable<T> Search(string searchTerm);
    }

    /// <summary>
    /// Base class for autocomplete sources providing default implementations of the necessary interfaces for the controls.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class AutocompleteSource<T> : IAutocompleteSource<T>
    {
        /// <summary>
        /// Creates a new <see cref="AutocompleteSource" />.
        /// </summary>
        public AutocompleteSource() { }

        /// <summary>
        /// Does the search for the autocomplete.
        /// </summary>
        /// <param name="searchTerm">The term to search for</param>
        /// <returns>The items found for the search term</returns>
        public abstract IEnumerable<T> Search(string searchTerm);

        /// <summary>
        /// Does the search for the autocomplete.
        /// </summary>
        /// <param name="searchTerm">The term to search for</param>
        /// <returns>The items found for the search term</returns>
        IEnumerable IAutocompleteSource.Search(string searchTerm)
        {
            return Search(searchTerm);
        }
    }
}
