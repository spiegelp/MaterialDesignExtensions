using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialDesignExtensions.Model
{
    public class SearchSuggestionItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool m_isFromHistory;
        private string m_suggestion;

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
