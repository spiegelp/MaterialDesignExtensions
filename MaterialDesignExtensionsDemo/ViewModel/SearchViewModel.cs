using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MaterialDesignExtensions.Model;

namespace MaterialDesignExtensionsDemo.ViewModel
{
    public class SearchViewModel : ViewModel
    {

        public override string DocumentationUrl
        {
            get
            {
                return "https://spiegelp.github.io/MaterialDesignExtensions/#documentation/search";
            }
        }

        public ISearchSuggestionsSource SearchSuggestionsSource
        {
            get
            {
                return new OperatingSystemsSearchSuggestionsSource();
            }
        }

        public SearchViewModel() : base() { }
    }

    public class OperatingSystemsSearchSuggestionsSource : ISearchSuggestionsSource
    {
        private List<string> m_operatingSystems;

        public OperatingSystemsSearchSuggestionsSource()
        {
            m_operatingSystems = new List<string>
            {
                "Android Gingerbread",
                "Android Icecream Sandwich",
                "Android Jellybean",
                "Android Lollipop",
                "Android Nougat",
                "Debian",
                "Mac OSX",
                "Raspbian",
                "Ubuntu Wily Werewolf",
                "Ubuntu Xenial Xerus",
                "Ubuntu Yakkety Yak",
                "Ubuntu Zesty Zapus",
                "Windows 7",
                "Windows 8",
                "Windows 8.1",
                "Windows 10",
                "Windows Vista",
                "Windows XP"
            };
        }

        public IList<string> GetAutoCompletion(string searchTerm)
        {
            return m_operatingSystems.Where(os => os.ToLower().Contains(searchTerm.ToLower())).ToList();
        }

        public IList<string> GetSearchSuggestions()
        {
            return new List<string> { "Android Lollipop", "Windows 10" };
        }
    }
}
