using System.ComponentModel;
using System.IO;

namespace MaterialDesignExtensions.Model
{
    /// <summary>
    /// An item in the current directory list of file system controls.
    /// </summary>
    public abstract class FileSystemEntryItem<T> : INotifyPropertyChanged where T : FileSystemInfo
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool m_isSelected;
        private T m_value;

        /// <summary>
        /// True, if the list item is selected.
        /// </summary>
        public bool IsSelected
        {
            get
            {
                return m_isSelected;
            }

            set
            {
                if (m_isSelected != value)
                {
                    m_isSelected = value;

                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }

        /// <summary>
        /// The value (directory or file) of the lsit item.
        /// </summary>
        public T Value
        {
            get
            {
                return m_value;
            }

            set
            {
                if (m_value != value)
                {
                    m_value = value;

                    OnPropertyChanged(nameof(Value));
                }
            }
        }

        /// <summary>
        /// Creates a new <see cref="FileSystemEntryItem" />.
        /// </summary>
        public FileSystemEntryItem()
        {
            m_isSelected = false;
            m_value = null;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null && !string.IsNullOrWhiteSpace(propertyName))
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
