using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MaterialDesignExtensions.Model;

namespace MaterialDesignExtensions.Controllers
{
    public class SearchSuggestionsController
    {
        private readonly object m_lockObject = new object();

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

        public SearchSuggestionsController() {
            m_searchSuggestionsSource = null;

            m_lastId = null;
        }

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

    public class SearchSuggestionsChangedEventArgs : EventArgs
    {
        public IList<string> SearchSuggestions { get; private set; }

        public bool IsFromHistory { get; private set; }

        public SearchSuggestionsChangedEventArgs(IList<string> searchSuggestions, bool isFromHistory)
        {
            SearchSuggestions = searchSuggestions;
            IsFromHistory = isFromHistory;
        }
    }

    public delegate void SearchSuggestionsChangedEventHandler(object sender, SearchSuggestionsChangedEventArgs args);
}
