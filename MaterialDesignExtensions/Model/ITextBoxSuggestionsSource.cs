using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialDesignExtensions.Model
{
    public interface ITextBoxSuggestionsSource : IAutocompleteSource<string>
    {
    }

    public abstract class TextBoxSuggestionsSource : ITextBoxSuggestionsSource
    {
        public TextBoxSuggestionsSource() { }

        public abstract IEnumerable<string> Search(string searchTerm);

        IEnumerable IAutocompleteSource.Search(string searchTerm)
        {
            return Search(searchTerm);
        }
    }
}
