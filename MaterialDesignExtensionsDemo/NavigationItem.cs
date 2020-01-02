using System;
using System.ComponentModel;

namespace MaterialDesignExtensionsDemo
{
    public class NavigationItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string m_label;
        private Func<object> m_action;
        private bool m_isSelected;

        public string Label
        {
            get
            {
                return m_label;
            }

            set
            {
                m_label = value;

                OnPropertyChanged(nameof(Label));
            }
        }

        public Func<object> Action
        {
            get
            {
                return m_action;
            }

            set
            {
                m_action = value;

                OnPropertyChanged(nameof(Action));
            }
        }

        public bool IsSelected
        {
            get
            {
                return m_isSelected;
            }

            set
            {
                m_isSelected = value;

                OnPropertyChanged(nameof(IsSelected));
            }
        }

        public NavigationItem()
        {
            m_label = null;
            m_action = null;
            m_isSelected = false;
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
