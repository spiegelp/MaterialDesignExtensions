using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialDesignExtensions.Model
{
    public interface IAutocompleteSource
    {
        IEnumerable Search(string searchTerm);
    }

    public interface IAutocompleteSource<T> : IAutocompleteSource
    {
        new IEnumerable<T> Search(string searchTerm);
    }

    public abstract class AutocompleteSource<T> : IAutocompleteSource<T>
    {
        public AutocompleteSource() { }

        public abstract IEnumerable<T> Search(string searchTerm);

        IEnumerable IAutocompleteSource.Search(string searchTerm)
        {
            return Search(searchTerm);
        }
    }
}
