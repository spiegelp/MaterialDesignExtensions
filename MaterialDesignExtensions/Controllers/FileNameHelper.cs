using System.Collections.Generic;

namespace MaterialDesignExtensions.Controllers
{
    /// <summary>
    /// Helper class for operations on file names.
    /// </summary>
    public abstract class FileNameHelper
    {
        private static readonly ISet<char> NotAllowedChars = new HashSet<char>() { '<', '>', ':', '"', '/', '\\', '|', '?', '*' };

        // abstract class with private constructor to prevent object initialization
        private FileNameHelper() { }

        /// <summary>
        /// Checks the specified file name if it is OK for the operation system.
        /// </summary>
        /// <param name="fileName">The file name to check</param>
        /// <returns>boolean indication if teh file name is OK or not</returns>
        public static bool CheckFileName(string fileName)
        {
            // see https://docs.microsoft.com/en-us/windows/desktop/fileio/naming-a-file#file-and-directory-names for file name constraints

            if (string.IsNullOrWhiteSpace(fileName))
            {
                return false;
            }

            for (int i = 0; i < fileName.Length; i++)
            {
                char c = fileName[i];
                ISet<char> NotAllowedChars = new HashSet<char>() { '<', '>', ':', '"', '/', '\\', '|', '?', '*' };

                if (NotAllowedChars.Contains(c) || ((int)c) <= 31)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
