using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialDesignExtensions.Model
{
    public class FileSystemEntriesGroupHeader : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string m_header;
        private bool m_showSeparator;

        public string Header
        {
            get
            {
                return m_header;
            }

            set
            {
                if (m_header != value)
                {
                    m_header = value;

                    OnPropertyChanged(nameof(Header));
                }
            }
        }

        public bool ShowSeparator
        {
            get
            {
                return m_showSeparator;
            }

            set
            {
                if (m_showSeparator != value)
                {
                    m_showSeparator = value;

                    OnPropertyChanged(nameof(ShowSeparator));
                }
            }
        }

        public FileSystemEntriesGroupHeader()
        {
            m_header = null;
            m_showSeparator = false;
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
