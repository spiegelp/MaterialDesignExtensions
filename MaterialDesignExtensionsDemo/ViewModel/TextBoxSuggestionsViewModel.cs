using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MaterialDesignExtensions.Model;

namespace MaterialDesignExtensionsDemo.ViewModel
{
    public class TextBoxSuggestionsViewModel : ViewModel
    {
        private ITextBoxSuggestionsSource m_textBoxSuggestionsSource;

        private string m_text;

        public override string DocumentationUrl
        {
            get
            {
                //TODO replace with link to specific docs
                return base.DocumentationUrl;
            }
        }

        public string Text
        {
            get
            {
                return m_text;
            }

            set
            {
                m_text = value;

                OnPropertyChanged(nameof(Text));
            }
        }

        public ITextBoxSuggestionsSource TextBoxSuggestionsSource
        {
            get
            {
                return m_textBoxSuggestionsSource;
            }
        }

        public TextBoxSuggestionsViewModel()
            : base()
        {
            m_textBoxSuggestionsSource = new OperatingSystemTextBoxSuggestionsSource();

            m_text = null;
        }
    }

    public class OperatingSystemTextBoxSuggestionsSource : TextBoxSuggestionsSource
    {
        private List<string> m_operatingSystemItems;

        public OperatingSystemTextBoxSuggestionsSource()
        {
            m_operatingSystemItems = new List<string>()
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

        public override IEnumerable<string> Search(string searchTerm)
        {
            searchTerm = searchTerm ?? string.Empty;
            searchTerm = searchTerm.ToLower();

            return m_operatingSystemItems.Where(item => item.ToLower().Contains(searchTerm));
        }
    }
}
