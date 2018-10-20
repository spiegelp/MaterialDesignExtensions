using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialDesignExtensions.Model
{
    public interface IAutocompleteSourceChangingItems : IAutocompleteSource
    {
        event AutocompleteSourceItemsChangedEventHandler AutocompleteSourceItemsChanged;
    }

    public interface IAutocompleteSourceChangingItems<T> : IAutocompleteSourceChangingItems, IAutocompleteSource<T> { }

    public abstract class AutocompleteSourceChangingItems<T> : AutocompleteSource<T>, IAutocompleteSourceChangingItems<T>
    {
        public event AutocompleteSourceItemsChangedEventHandler AutocompleteSourceItemsChanged;

        public AutocompleteSourceChangingItems() : base() { }

        protected void OnAutocompleteSourceItemsChanged()
        {
            AutocompleteSourceItemsChanged?.Invoke(this, new AutocompleteSourceItemsChangedEventArgs());
        }
    }

    public delegate void AutocompleteSourceItemsChangedEventHandler(object sender, AutocompleteSourceItemsChangedEventArgs args);

    public class AutocompleteSourceItemsChangedEventArgs : EventArgs
    {
        public AutocompleteSourceItemsChangedEventArgs() : base() { }
    }
}
