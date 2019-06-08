using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialDesignExtensions.Model
{
    /// <summary>
    /// An interface to notify the autocomplete control, that the underlying data source changed.
    /// </summary>
    public interface IAutocompleteSourceChangingItems : IAutocompleteSource
    {
        /// <summary>
        /// An event to notify the autocomplete control, that the underlying data source changed.
        /// </summary>
        event AutocompleteSourceItemsChangedEventHandler AutocompleteSourceItemsChanged;
    }

    /// <summary>
    /// Generic version of <see cref="IAutocompleteSourceChangingItems" /> interface to work with type safety.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IAutocompleteSourceChangingItems<T> : IAutocompleteSourceChangingItems, IAutocompleteSource<T> { }

    /// <summary>
    /// Base class to notify the autocomplete control, that the underlying data source changed.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class AutocompleteSourceChangingItems<T> : AutocompleteSource<T>, IAutocompleteSourceChangingItems<T>
    {
        /// <summary>
        /// An event to notify the autocomplete control, that the underlying data source changed.
        /// </summary>
        public event AutocompleteSourceItemsChangedEventHandler AutocompleteSourceItemsChanged;

        /// <summary>
        /// Creates a new <see cref="AutocompleteSourceChangingItems" />.
        /// </summary>
        public AutocompleteSourceChangingItems() : base() { }

        protected void OnAutocompleteSourceItemsChanged()
        {
            AutocompleteSourceItemsChanged?.Invoke(this, new AutocompleteSourceItemsChangedEventArgs());
        }
    }

    /// <summary>
    /// The delegate for handling the <see cref="IAutocompleteSourceChangingItems.AutocompleteSourceItemsChanged" /> event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    public delegate void AutocompleteSourceItemsChangedEventHandler(object sender, AutocompleteSourceItemsChangedEventArgs args);

    /// <summary>
    /// The argument for the <see cref="IAutocompleteSourceChangingItems.AutocompleteSourceItemsChanged" /> event.
    /// </summary>
    public class AutocompleteSourceItemsChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Creates a new <see cref="AutocompleteSourceItemsChangedEventArgs" />.
        /// </summary>
        public AutocompleteSourceItemsChangedEventArgs() : base() { }
    }
}
