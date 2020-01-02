using System.ComponentModel;

namespace MaterialDesignExtensions.Model
{
    /// <summary>
    /// Convenience class for building a text header with a first and a second level header.
    /// The corresponding data template is already implemented and will be automatically applied.
    /// </summary>
    public class StepTitleHeader : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string m_firstLevelTitle;
        private string m_secondLevelTitle;

        /// <summary>
        /// The text of the first level title.
        /// A value of null will hide this title.
        /// </summary>
        public string FirstLevelTitle
        {
            get
            {
                return m_firstLevelTitle;
            }

            set
            {
                m_firstLevelTitle = value;

                OnPropertyChanged(nameof(FirstLevelTitle));
            }
        }

        /// <summary>
        /// The text of the second level title beneath the first level title.
        /// A value of null will hide the this title.
        /// It uses a smaller font size.
        /// </summary>
        public string SecondLevelTitle
        {
            get
            {
                return m_secondLevelTitle;
            }

            set
            {
                m_secondLevelTitle = value;

                OnPropertyChanged(nameof(SecondLevelTitle));
            }
        }

        /// <summary>
        /// Creates a new <see cref="StepTitleHeader" />.
        /// </summary>
        public StepTitleHeader()
        {
            m_firstLevelTitle = null;
            m_secondLevelTitle = null;
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
