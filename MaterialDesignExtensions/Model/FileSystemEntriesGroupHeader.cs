using System.ComponentModel;

namespace MaterialDesignExtensions.Model
{
    /// <summary>
    /// A group header in the items lists of file system controls to group directories and files.
    /// </summary>
    public class FileSystemEntriesGroupHeader : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string m_header;
        private bool m_showSeparator;

        /// <summary>
        /// The header label.
        /// </summary>
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

        /// <summary>
        /// True, to display a separator above the label.
        /// </summary>
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

        /// <summary>
        /// Creates a new <see cref="FileSystemEntriesGroupHeader" />.
        /// </summary>
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
