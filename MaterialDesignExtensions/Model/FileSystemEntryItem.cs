using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialDesignExtensions.Model
{
    public abstract class FileSystemEntryItem<T> : INotifyPropertyChanged where T : FileSystemInfo
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool m_isSelected;
        private T m_value;

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
