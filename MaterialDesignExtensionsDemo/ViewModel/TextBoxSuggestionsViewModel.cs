using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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
                return "https://spiegelp.github.io/MaterialDesignExtensions/#documentation/textboxsuggestions";
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

        public List<Device> Devices { get; private set; } = new List<Device>();

        public TextBoxSuggestionsViewModel()
            : base()
        {
            m_textBoxSuggestionsSource = new OperatingSystemTextBoxSuggestionsSource();

            m_text = null;
            m_text = "Windows 10";

            /*for (int i = 0; i < 200; i++)
            {
                Devices.Add(new Device { OperatingSystem = "Windows 10" });
            }*/
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

    public class Device : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string m_operatingSystem;

        public string OperatingSystem
        {
            get
            {
                return m_operatingSystem;
            }

            set
            {
                m_operatingSystem = value;

                OnPropertyChanged(nameof(OperatingSystem));
            }
        }

        public Device() { }

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null && !string.IsNullOrWhiteSpace(propertyName))
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
