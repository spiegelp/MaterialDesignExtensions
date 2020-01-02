using System.Windows;

namespace MaterialDesignExtensions.Controls
{
    /// <summary>
    /// A control which implements the persistent search of the Material design specification (https://material.io/guidelines/patterns/search.html#search-in-app-search).
    /// </summary>
    public class PersistentSearch : SearchBase
    {
        /// <summary>
        /// The size of the displayed icons.
        /// </summary>
        public static readonly DependencyProperty IconSizeProperty = DependencyProperty.Register(
            nameof(IconSize), typeof(int), typeof(PersistentSearch), new PropertyMetadata(24));

        /// <summary>
        /// The size of the displayed icons.
        /// </summary>
        public string IconSize
        {
            get
            {
                return (string)GetValue(IconSizeProperty);
            }

            set
            {
                SetValue(IconSizeProperty, value);
            }
        }

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
