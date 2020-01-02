using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MaterialDesignExtensions.Model;

namespace MaterialDesignExtensions.Controllers
{
    /// <summary>
    /// Helper class for filtering files in <see cref="MaterialDesignExtensions.Controls.OpenFileControl" /> and <see cref="MaterialDesignExtensions.Controls.SaveFileControl" />.
    /// The string format is similar to the original .NET controls
    /// (see https://docs.microsoft.com/de-de/dotnet/api/microsoft.win32.filedialog.filter?view=netframework-4.7.1#Microsoft_Win32_FileDialog_Filter).
    /// </summary>
    public abstract class FileFilterHelper
    {
        // abstract class with private constructor to prevent object initialization
        private FileFilterHelper() { }

        /// <summary>
        /// Parses the file filters out of a string similar to the original .NET API
        /// (see https://docs.microsoft.com/de-de/dotnet/api/microsoft.win32.filedialog.filter?view=netframework-4.7.1#Microsoft_Win32_FileDialog_Filter).
        /// </summary>
        /// <param name="filtersStr">The string containing the filters like: C#|*.cs|Web|*.html;*.css;*.js</param>
        /// <returns>A list of the parsed filters or null if the input is empty</returns>
        public static IList<IFileFilter> ParseFileFilters(string filtersStr)
        {
            if (string.IsNullOrWhiteSpace(filtersStr))
            {
                return null;
            }

            string[] filtersSplit = filtersStr.Split('|');

            // if the split is not a multiple of 2, the filter string is invalid
            if ((filtersSplit.Length % 2) != 0)
            {
                throw new ArgumentException("invalid filter string");
            }

            IList<IFileFilter> filters = new List<IFileFilter>();

            for (int i = 0; i < filtersSplit.Length; i = i + 2)
            {
                filters.Add(ParseFileFilter(filtersSplit[i], filtersSplit[i + 1]));
            }

            return filters;
        }

        /// <summary>
        /// Parses a file filter out of a filter portion string similar to the original .NET API
        /// (see https://docs.microsoft.com/de-de/dotnet/api/microsoft.win32.filedialog.filter?view=netframework-4.7.1#Microsoft_Win32_FileDialog_Filter).
        /// </summary>
        /// <param name="label">The label of the filter</param>
        /// <param name="filters">The filter portion string like: *.cs;*.xaml</param>
        /// <returns></returns>
        public static IFileFilter ParseFileFilter(string label, string filters)
        {
            if (string.IsNullOrWhiteSpace(label))
            {
                throw new ArgumentException(nameof(label) + " must not be empty");
            }

            IEnumerable<string> regularExpressions = ParseFilterRegularExpressions(filters);

            return FileFilter.Create(label, filters, regularExpressions);
        }

        /// <summary>
        /// Creates regular expressions out of a filter portion string.
        /// </summary>
        /// <param name="filters">The filter portion string like: *.cs;*.xaml</param>
        /// <returns></returns>
        public static IEnumerable<string> ParseFilterRegularExpressions(string filters)
        {
            if (string.IsNullOrWhiteSpace(filters))
            {
                throw new ArgumentException(nameof(filters) + " must not be empty");
            }

            string[] filtersSplit = filters.Split(';');

            // convert the filters into regular expressions
            List<string> regularExpressions = new List<string>();

            for (int i = 0; i < filtersSplit.Length; i++)
            {
                string regex = filtersSplit[i].Trim();
                bool matchesAtTheStart = !regex.StartsWith("*");
                bool matchesAtTheEnd = !regex.EndsWith("*");

                // 1. work in lower case to do a case insensitive matching
                regex = regex.ToLower();

                // 2. replace * with any match
                regex = regex.Replace("*", @"(\w|\W)*");

                // 3. escape the dot
                regex = regex.Replace(".", @"\.");

                // 4. match the filter at the start or the end of the filename
                if (matchesAtTheStart)
                {
                    regex = "^" + regex;
                }

                if (matchesAtTheEnd)
                {
                    regex = regex + "$";
                }

                regularExpressions.Add(regex);
            }

            return regularExpressions;
        }

        /// <summary>
        /// Converts a file filter into back into a string according to the original .NET API
        /// (see https://docs.microsoft.com/de-de/dotnet/api/microsoft.win32.filedialog.filter?view=netframework-4.7.1#Microsoft_Win32_FileDialog_Filter).
        /// </summary>
        /// <param name="fileFilter"></param>
        /// <returns></returns>
        public static string ConvertFileFilterToString(IFileFilter fileFilter)
        {
            return fileFilter.Label + "|" + fileFilter.Filters;
        }

        /// <summary>
        /// Converts the file filters into back into a string according to the original .NET API
        /// (see https://docs.microsoft.com/de-de/dotnet/api/microsoft.win32.filedialog.filter?view=netframework-4.7.1#Microsoft_Win32_FileDialog_Filter).
        /// </summary>
        /// <param name="fileFilters"></param>
        /// <returns></returns>
        public static string ConvertFileFiltersToString(IEnumerable<IFileFilter> fileFilters)
        {
            StringBuilder sb = new StringBuilder();

            foreach (IFileFilter fileFilter in fileFilters)
            {
                if (sb.Length > 0)
                {
                    sb.Append('|');
                }

                sb.Append(ConvertFileFilterToString(fileFilter));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Extracts the file extensions out of the file filter.
        /// </summary>
        /// <param name="fileFilter"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetFileExtensionsFromFilter(IFileFilter fileFilter)
        {
            if (fileFilter == null)
            {
                return null;
            }

            string[] split = fileFilter.Filters.Split(';');

            return fileFilter.Filters.Split(';')
                .Select(filterStr => filterStr.Trim())
                .Where(filterStr => filterStr.Length > 1 && filterStr.Contains(".") && !filterStr.EndsWith("."))
                .Select(filterStr => filterStr.Substring(filterStr.LastIndexOf(".") + 1).Replace("*", string.Empty))
                .Where(fileExtension => !string.IsNullOrWhiteSpace(fileExtension));
        }
    }
}
