﻿using System.IO;
using MaterialDesignThemes.Wpf;

namespace MaterialDesignExtensions.Model
{
    /// <summary>
    /// A helper class to provide special locations (user folders etc.) inside the file system controls.
    /// </summary>
    public class SpecialDirectory
    {
        private string m_label;

        /// <summary>
        /// The icon on the user inferface for this special location.
        /// </summary>
        public PackIconKind Icon { get; set; }

        /// <summary>
        /// The <see cref="DirectoryInfo" /> object with the information for this special location.
        /// </summary>
        public DirectoryInfo Info { get; set; }

        /// <summary>
        /// The label the user inferface for this special location.
        /// </summary>
        public string Label
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(m_label))
                {
                    return m_label;
                }
                else
                {
                    return Info?.Name;
                }
            }

            set
            {
                m_label = value;
            }
        }

        /// <summary>
        /// Creates a new <see cref="SpecialDirectory" />.
        /// </summary>
        public SpecialDirectory() { }
    }
}
