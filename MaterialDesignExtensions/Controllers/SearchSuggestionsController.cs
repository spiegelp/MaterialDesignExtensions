using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MaterialDesignExtensions.Model;

namespace MaterialDesignExtensions.Controllers
{
    /// <summary>
    /// Controller behind the <see cref="Controls.PersistentSearch"/> control.
    /// It does the technical stuff and delegates the actual search to the specified <see cref="SearchSuggestionsSource" />.
    /// </summary>
    public class SearchSuggestionsController
    {
        private readonly object m_lockObject = new object();

        /// <summary>
        /// An event raised to return the result of an search.
        /// </summary>
        public event SearchSuggestionsChangedEventHandler SearchSuggestionsChanged;

        // delay between input and actual search in milliseconds
        private const int SearchDelay = 300;

        private ISearchSuggestionsSource m_searchSuggestionsSource;

        private string m_lastId;

        private string LastId
        {
            get
            {
                lock (m_lockObject)
                {
                    return m_lastId;
                }
            }

            set
            {
                lock (m_lockObject)
                {
                    m_lastId = value;
                }
            }
        }

        /// <summary>
        /// The source providing the suggestions to do the actual search with.
        /// </summary>
        public ISearchSuggestionsSource SearchSuggestionsSource
        {
            get
            {
                lock (m_lockObject)
                {
                    return m_searchSuggestionsSource;
                }
            }

            set
            {
                lock (m_lockObject)
                {
                    m_searchSuggestionsSource = value;
                }
            }
        }

        /// <summary>
        /// Creates a new <see cref="SearchSuggestionsController" />.
        /// </summary>
        public SearchSuggestionsController() {
            m_searchSuggestionsSource = null;

            m_lastId = null;
        }

        /// <summary>
        /// Starts a new task for the search and propagates the result via the <see cref="SearchSuggestionsChanged" /> event.
        /// This method does some technical stuff and delegates the actual search to the <see cref="ISearchSuggestionsSource" />.
        /// </summary>
        /// <param name="searchTerm">The term to search for</param>
        public void Search(string searchTerm)
        {
            string id = Guid.NewGuid().ToString();

            ISearchSuggestionsSource searchSuggestionsSource = null;

            lock (m_lockObject)
            {
                searchSuggestionsSource = m_searchSuggestionsSource;

                // no source, no search
                if (searchSuggestionsSource == null)
                {
                    return;
                }

                LastId = id;
            }

            Task.Delay(SearchDelay)
                .ContinueWith((prevTask) =>
                {
                    // search only if there was no other request during the delay
                    if (DoSearchWithId(id))
                    {
                        IList<string> searchSuggestions = null;
                        bool isFromHistory = false;

                        if (!string.IsNullOrEmpty(searchTerm))
                        {
                            searchSuggestions = searchSuggestionsSource.GetAutoCompletion(searchTerm);
                            isFromHistory = false;
                        }
                        else
                        {
                            searchSuggestions = searchSuggestionsSource.GetSearchSuggestions();
                            isFromHistory = true;
                        }

                        // a final check if this result will not be replaced by another active search
                        if (DoSearchWithId(id))
                        {
                            SearchSuggestionsChanged?.Invoke(this, new SearchSuggestionsChangedEventArgs(searchSuggestions, isFromHistory));
                        }
                    }
                });
        }

        private bool DoSearchWithId(string id)
        {
            lock (m_lockObject)
            {
                return LastId == null || LastId == id;
            }
        }
    }

    /// <summary>
    /// The arguments for the <see cref="SearchSuggestionsController.SearchSuggestionsChanged" /> event.
    /// </summary>
    public class SearchSuggestionsChangedEventArgs : EventArgs
    {
        /// <summary>
        /// The result of the search.
        /// </summary>
        public IList<string> SearchSuggestions { get; private set; }

        /// <summary>
        /// Boolean indicating if the result contains suggestions of previous searches or based on the current input.
        /// </summary>
        public bool IsFromHistory { get; private set; }

        /// <summary>
        /// Creates a new <see cref="SearchSuggestionsChangedEventArgs" />.
        /// </summary>
        /// <param name="searchSuggestions">The result of the search</param>
        /// <param name="isFromHistory">Boolean indicating if the result contains suggestions of previous searches or based on the current input</param>
        public SearchSuggestionsChangedEventArgs(IList<string> searchSuggestions, bool isFromHistory)
        {
            SearchSuggestions = searchSuggestions;
            IsFromHistory = isFromHistory;
        }
    }

    /// <summary>
    /// The delegate for handling the <see cref="SearchSuggestionsController.SearchSuggestionsChanged" /> event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    public delegate void SearchSuggestionsChangedEventHandler(object sender, SearchSuggestionsChangedEventArgs args);
}
