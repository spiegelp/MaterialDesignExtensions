using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using MaterialDesignExtensions.Controllers;

namespace MaterialDesignExtensions.Model
{
    /// <summary>
    /// A filter to apply to files in <see cref="MaterialDesignExtensions.Controls.OpenFileControl" /> and <see cref="MaterialDesignExtensions.Controls.SaveFileControl" />.
    /// </summary>
    public class FileFilter : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string m_label;
        private string m_filters;
        private IEnumerable<string> m_regularExpressions;

        /// <summary>
        /// The orignal filter portion string according to the original .NET API.
        /// (see https://docs.microsoft.com/de-de/dotnet/api/microsoft.win32.filedialog.filter?view=netframework-4.7.1#Microsoft_Win32_FileDialog_Filter)
        /// </summary>
        public string Filters
        {
            get
            {
                return m_filters;
            }

            set
            {
                if (m_filters != value)
                {
                    m_filters = value;

                    OnPropertyChanged(nameof(Filters));
                }
            }
        }

        /// <summary>
        /// The label of the filter to show in the user interface.
        /// </summary>
        public string Label
        {
            get
            {
                return m_label;
            }

            set
            {
                if (m_label != value)
                {
                    m_label = value;

                    OnPropertyChanged(nameof(Label));
                }
            }
        }

        /// <summary>
        /// The regular expressions used by this filter for filtering files.
        /// </summary>
        public IEnumerable<string> RegularExpressions
        {
            get
            {
                return m_regularExpressions;
            }

            set
            {
                if (m_regularExpressions != value)
                {
                    m_regularExpressions = value;

                    OnPropertyChanged(nameof(RegularExpressions));
                }
            }
        }

        /// <summary>
        /// Creates a new <see cref="FileFilter" />.
        /// </summary>
        public FileFilter()
        {
            m_label = null;
            m_filters = null;
            m_regularExpressions = null;
        }

        /// <summary>
        /// Tests if the specified filename matches any regular expression of this filter.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public bool IsMatch(string filename)
        {
            return m_regularExpressions.Any(regex => Regex.IsMatch(filename.ToLower(), regex));
        }

        /// <summary>
        /// Tests if the filename of the specified file matches any regular expression of this filter.
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        public bool IsMatch(FileInfo fileInfo)
        {
            return IsMatch(fileInfo.Name);
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null && !string.IsNullOrWhiteSpace(propertyName))
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public override string ToString()
        {
            return FileFilterHelper.ConvertFileFilterToString(this);
        }
    }
}
