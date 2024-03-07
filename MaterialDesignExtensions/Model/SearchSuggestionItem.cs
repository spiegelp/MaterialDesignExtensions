using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialDesignExtensions.Model
{
    /// <summary>
    /// An item out of the suggestions of a search control.
    /// </summary>
    public class SearchSuggestionItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool m_isFromHistory;
        private string m_suggestion;

        /// <summary>
        /// True, if this item is from an older search query.
        /// </summary>
        public bool IsFromHistory
        {
            get
            {
                return m_isFromHistory;
            }

            set
            {
                m_isFromHistory = value;

                OnPropertyChanged(nameof(IsFromHistory));
            }
        }

        /// <summary>
        /// The label of this suggestion.
        /// </summary>
        public string Suggestion
        {
            get
            {
                return m_suggestion;
            }

            set
            {
                m_suggestion = value;

                OnPropertyChanged(nameof(Suggestion));
            }
        }

        /// <summary>
        /// Creates a new <see cref="SearchSuggestionItem" />.
        /// </summary>
        public SearchSuggestionItem()
        {
            m_isFromHistory = false;
            m_suggestion = null;
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null && !string.IsNullOrWhiteSpace(propertyName))
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
