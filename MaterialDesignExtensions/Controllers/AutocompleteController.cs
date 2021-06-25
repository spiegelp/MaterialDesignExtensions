using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MaterialDesignExtensions.Model;

namespace MaterialDesignExtensions.Controllers
{
    /// <summary>
    /// The controller with the technical logic for autocomplete controls.
    /// </summary>
    public class AutocompleteController
    {
        private readonly object m_lockObject = new object();

        /// <summary>
        /// An event raised to return the result of the autocomplete.
        /// </summary>
        public event AutocompleteItemsChangedEventHandler AutocompleteItemsChanged;

        // delay between input and actual search in milliseconds
        private const int SearchDelay = 300;

        private IAutocompleteSource m_autocompleteSource;

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
        /// The source providing the autocomplete items.
        /// </summary>
        public IAutocompleteSource AutocompleteSource
        {
            get
            {
                lock (m_lockObject)
                {
                    return m_autocompleteSource;
                }
            }

            set
            {
                lock (m_lockObject)
                {
                    m_autocompleteSource = value;
                }
            }
        }

        /// <summary>
        /// Creates a new <see cref="AutocompleteController" />.
        /// </summary>
        public AutocompleteController()
        {
            m_autocompleteSource = null;

            m_lastId = null;
        }

        /// <summary>
        /// Starts a new task for the autocomplete and propagates the result via the <see cref="AutocompleteItemsChanged" /> event.
        /// This method does some technical stuff and delegates the actual search to the <see cref="IAutocompleteSource" />.
        /// </summary>
        /// <param name="searchTerm">The term to search for</param>
        public void Search(string searchTerm)
        {
            Task.Run(async () =>
            {
                string id = Guid.NewGuid().ToString();

                IAutocompleteSource autocompleteSource = null;

                lock (m_lockObject)
                {
                    autocompleteSource = m_autocompleteSource;

                    // no source, no search
                    if (autocompleteSource == null)
                    {
                        return;
                    }

                    LastId = id;
                }

                await Task.Delay(SearchDelay).ConfigureAwait(false);

                // search only if there was no other request during the delay
                if (DoSearchWithId(id))
                {
                    IEnumerable items = autocompleteSource.Search(searchTerm);

                    // a final check if this result will not be replaced by another active search
                    if (DoSearchWithId(id))
                    {
                        AutocompleteItemsChanged?.Invoke(this, new AutocompleteItemsChangedEventArgs(items));
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
    /// The arguments for the <see cref="AutocompleteController.AutocompleteItemsChanged" /> event.
    /// </summary>
    public class AutocompleteItemsChangedEventArgs : EventArgs
    {
        /// <summary>
        /// The result of the autocomplete.
        /// </summary>
        public IEnumerable Items { get; private set; }

        /// <summary>
        /// Creates a new <see cref="AutocompleteItemsChangedEventArgs" />.
        /// </summary>
        /// <param name="items">The result of the autocomplete</param>
        public AutocompleteItemsChangedEventArgs(IEnumerable items)
        {
            Items = items;
        }
    }

    /// <summary>
    /// The delegate for handling the <see cref="AutocompleteController.AutocompleteItemsChanged" /> event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    public delegate void AutocompleteItemsChangedEventHandler(object sender, AutocompleteItemsChangedEventArgs args);
}
