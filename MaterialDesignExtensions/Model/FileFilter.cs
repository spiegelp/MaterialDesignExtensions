using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using MaterialDesignExtensions.Controllers;

// use Pri.LongPath classes instead of System.IO for the MaterialDesignExtensions.LongPath build to support long file system paths on older Windows and .NET versions
#if LONG_PATH
using FileInfo = Pri.LongPath.FileInfo;
#endif

namespace MaterialDesignExtensions.Model
{
    /// <summary>
    /// A filter to apply to files in <see cref="MaterialDesignExtensions.Controls.OpenFileControl" /> and <see cref="MaterialDesignExtensions.Controls.SaveFileControl" />.
    /// </summary>
    public interface IFileFilter
    {
        /// <summary>
        /// The orignal filter portion string according to the original .NET API.
        /// (see https://docs.microsoft.com/de-de/dotnet/api/microsoft.win32.filedialog.filter?view=netframework-4.7.1#Microsoft_Win32_FileDialog_Filter)
        /// </summary>
        string Filters { get; }

        /// <summary>
        /// The label of the filter to show in the user interface.
        /// </summary>
        string Label { get; }

        /// <summary>
        /// The regular expressions used by this filter for filtering files.
        /// </summary>
        IEnumerable<string> RegularExpressions { get; }

        /// <summary>
        /// Tests if the specified filename matches any regular expression of this filter.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        bool IsMatch(string filename);

        /// <summary>
        /// Tests if the filename of the specified file matches any regular expression of this filter.
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        bool IsMatch(FileInfo fileInfo);

        string ToString();
    }

    /// <summary>
    /// An immutable filter to apply to files in <see cref="MaterialDesignExtensions.Controls.OpenFileControl" /> and <see cref="MaterialDesignExtensions.Controls.SaveFileControl" />.
    /// </summary>
    public class FileFilter : IFileFilter, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly string m_label;
        private readonly string m_filters;
        private readonly IEnumerable<string> m_regularExpressions;

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
        }
        
        private FileFilter(string label, string filters, IEnumerable<string> regularExpressions)
        {
            m_label = label;
            m_filters = filters;
            m_regularExpressions = regularExpressions;
        }

        /// <summary>
        /// Creates a new, immutable <see cref="FileFilter" />.
        /// <param name="label">The label of the filter</param>
        /// <param name="filters">The filter portion string like: *.cs;*.xaml</param>
        /// </summary>
        public static FileFilter Create(string label, string filters)
        {
            CheckArguments(label, filters);

            IEnumerable<string> regularExpressions = FileFilterHelper.ParseFilterRegularExpressions(filters);

            return Create(label, filters, regularExpressions);
        }

        /// <summary>
        /// Creates a new, immutable <see cref="FileFilter" />.
        /// <param name="label">The label of the filter</param>
        /// <param name="filters">The filter portion string like: *.cs;*.xaml</param>
        /// <param name="regularExpressions">The regular experssions to use</param>
        /// </summary>
        public static FileFilter Create(string label, string filters, IEnumerable<string> regularExpressions)
        {
            CheckArguments(label, filters);

            return new FileFilter(label, filters, regularExpressions);
        }

        private static void CheckArguments(string label, string filters)
        {
            if (string.IsNullOrWhiteSpace(label))
            {
                throw new ArgumentException(nameof(label) + " must not be empty");
            }

            if (string.IsNullOrWhiteSpace(filters))
            {
                throw new ArgumentException(nameof(filters) + " must not be empty");
            }
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
