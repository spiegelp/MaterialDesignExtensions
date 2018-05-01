using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MaterialDesignExtensions.Controls
{
    /// <summary>
    /// A control which implements the persistent search of the Material design specification (https://material.io/guidelines/patterns/search.html#search-in-app-search).
    /// </summary>
    public class PersistentSearch : SearchBase
    {
        static PersistentSearch()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PersistentSearch), new FrameworkPropertyMetadata(typeof(PersistentSearch)));
        }

        /// <summary>
        /// Creates a new <see cref="PersistentSearch" />.
        /// </summary>
        public PersistentSearch() : base() { }
    }
}
