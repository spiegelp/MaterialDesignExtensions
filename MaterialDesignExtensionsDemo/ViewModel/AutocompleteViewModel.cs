using System.Collections.Generic;
using System.Linq;
using MaterialDesignThemes.Wpf;

using MaterialDesignExtensions.Model;
using System.Collections;

namespace MaterialDesignExtensionsDemo.ViewModel
{
    public class AutocompleteViewModel : ViewModel
    {
        private IAutocompleteSource m_autocompleteSource;

        private object m_selectedItem;

        public IAutocompleteSource AutocompleteSource
        {
            get
            {
                return m_autocompleteSource;
            }
        }

        public override string DocumentationUrl
        {
            get
            {
                return "https://spiegelp.github.io/MaterialDesignExtensions/#documentation/autocomplete";
            }
        }

        public object SelectedItem
        {
            get
            {
                return m_selectedItem;
            }

            set
            {
                m_selectedItem = value;

                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        public AutocompleteViewModel()
            : base()
        {
            m_autocompleteSource = new OperatingSystemAutocompleteSource();

            m_selectedItem = null;
        }
    }

    public class OperatingSystemItem
    {
        public PackIconKind Icon { get; set; }

        public string Name { get; set; }

        public OperatingSystemItem() { }
    }

    public class OperatingSystemAutocompleteSource : IAutocompleteSource
    {
        private List<OperatingSystemItem> m_operatingSystemItems;

        public OperatingSystemAutocompleteSource()
        {
            m_operatingSystemItems = new List<OperatingSystemItem>()
            {
                new OperatingSystemItem() { Icon = PackIconKind.Android, Name = "Android Gingerbread" },
                new OperatingSystemItem() { Icon = PackIconKind.Android, Name = "Android Icecream Sandwich" },
                new OperatingSystemItem() { Icon = PackIconKind.Android, Name = "Android Jellybean" },
                new OperatingSystemItem() { Icon = PackIconKind.Android, Name = "Android Lollipop" },
                new OperatingSystemItem() { Icon = PackIconKind.Android, Name = "Android Nougat" },
                new OperatingSystemItem() { Icon = PackIconKind.Linux, Name = "Debian" },
                new OperatingSystemItem() { Icon = PackIconKind.DesktopMac, Name = "Mac OSX" },
                new OperatingSystemItem() { Icon = PackIconKind.DeveloperBoard, Name = "Raspbian" },
                new OperatingSystemItem() { Icon = PackIconKind.Ubuntu, Name = "Ubuntu Wily Werewolf" },
                new OperatingSystemItem() { Icon = PackIconKind.Ubuntu, Name = "Ubuntu Xenial Xerus" },
                new OperatingSystemItem() { Icon = PackIconKind.Ubuntu, Name = "Ubuntu Yakkety Yak" },
                new OperatingSystemItem() { Icon = PackIconKind.Ubuntu, Name = "Ubuntu Zesty Zapus" },
                new OperatingSystemItem() { Icon = PackIconKind.Windows, Name = "Windows 7" },
                new OperatingSystemItem() { Icon = PackIconKind.Windows, Name = "Windows 8" },
                new OperatingSystemItem() { Icon = PackIconKind.Windows, Name = "Windows 8.1" },
                new OperatingSystemItem() { Icon = PackIconKind.Windows, Name = "Windows 10" },
                new OperatingSystemItem() { Icon = PackIconKind.Windows, Name = "Windows Vista" },
                new OperatingSystemItem() { Icon = PackIconKind.Windows, Name = "Windows XP" }
            };
        }

        public IEnumerable Search(string searchTerm)
        {
            searchTerm = searchTerm ?? string.Empty;
            searchTerm = searchTerm.ToLower();

            return m_operatingSystemItems.Where(item => item.Name.ToLower().Contains(searchTerm));
        }
    }

    // using an abstract class instead of the interface
    //
    //public class OperatingSystemAutocompleteSource : AutocompleteSource<OperatingSystemItem>
    // or
    //public class OperatingSystemAutocompleteSource : AutocompleteSourceChangingItems<OperatingSystemItem>
    //{
    //    private List<OperatingSystemItem> m_operatingSystemItems;

    //    public OperatingSystemAutocompleteSource()
    //    {
    //        m_operatingSystemItems = new List<OperatingSystemItem>()
    //        {
    //            new OperatingSystemItem() { Icon = PackIconKind.Android, Name = "Android Gingerbread" },
    //            new OperatingSystemItem() { Icon = PackIconKind.Android, Name = "Android Icecream Sandwich" },
    //            new OperatingSystemItem() { Icon = PackIconKind.Android, Name = "Android Jellybean" },
    //            new OperatingSystemItem() { Icon = PackIconKind.Android, Name = "Android Lollipop" },
    //            new OperatingSystemItem() { Icon = PackIconKind.Android, Name = "Android Nougat" },
    //            new OperatingSystemItem() { Icon = PackIconKind.Linux, Name = "Debian" },
    //            new OperatingSystemItem() { Icon = PackIconKind.DesktopMac, Name = "Mac OSX" },
    //            new OperatingSystemItem() { Icon = PackIconKind.Raspberrypi, Name = "Raspbian" },
    //            new OperatingSystemItem() { Icon = PackIconKind.Ubuntu, Name = "Ubuntu Wily Werewolf" },
    //            new OperatingSystemItem() { Icon = PackIconKind.Ubuntu, Name = "Ubuntu Xenial Xerus" },
    //            new OperatingSystemItem() { Icon = PackIconKind.Ubuntu, Name = "Ubuntu Yakkety Yak" },
    //            new OperatingSystemItem() { Icon = PackIconKind.Ubuntu, Name = "Ubuntu Zesty Zapus" },
    //            new OperatingSystemItem() { Icon = PackIconKind.Windows, Name = "Windows 7" },
    //            new OperatingSystemItem() { Icon = PackIconKind.Windows, Name = "Windows 8" },
    //            new OperatingSystemItem() { Icon = PackIconKind.Windows, Name = "Windows 8.1" },
    //            new OperatingSystemItem() { Icon = PackIconKind.Windows, Name = "Windows 10" },
    //            new OperatingSystemItem() { Icon = PackIconKind.Windows, Name = "Windows Vista" },
    //            new OperatingSystemItem() { Icon = PackIconKind.Windows, Name = "Windows XP" }
    //        };
    //    }

    //    public override IEnumerable<OperatingSystemItem> Search(string searchTerm)
    //    {
    //        searchTerm = searchTerm ?? string.Empty;
    //        searchTerm = searchTerm.ToLower();

    //        return m_operatingSystemItems.Where(item => item.Name.ToLower().Contains(searchTerm));
    //    }
    //}

    // a very unrealistic example, but it shows how the update of the items works
    /*public class OperatingSystemAutocompleteSource : AutocompleteSourceChangingItems<OperatingSystemItem>
    {
        private List<OperatingSystemItem> m_operatingSystemItems;

        public OperatingSystemAutocompleteSource()
        {
            m_operatingSystemItems = new List<OperatingSystemItem>()
            {
                new OperatingSystemItem() { Icon = PackIconKind.Android, Name = "Android Gingerbread" },
                new OperatingSystemItem() { Icon = PackIconKind.Android, Name = "Android Icecream Sandwich" },
                new OperatingSystemItem() { Icon = PackIconKind.Android, Name = "Android Jellybean" },
                new OperatingSystemItem() { Icon = PackIconKind.Android, Name = "Android Lollipop" },
                new OperatingSystemItem() { Icon = PackIconKind.Android, Name = "Android Nougat" },
                new OperatingSystemItem() { Icon = PackIconKind.Linux, Name = "Debian" },
                new OperatingSystemItem() { Icon = PackIconKind.DesktopMac, Name = "Mac OSX" },
                new OperatingSystemItem() { Icon = PackIconKind.Raspberrypi, Name = "Raspbian" },
                new OperatingSystemItem() { Icon = PackIconKind.Ubuntu, Name = "Ubuntu Wily Werewolf" },
                new OperatingSystemItem() { Icon = PackIconKind.Ubuntu, Name = "Ubuntu Xenial Xerus" },
                new OperatingSystemItem() { Icon = PackIconKind.Ubuntu, Name = "Ubuntu Yakkety Yak" },
                new OperatingSystemItem() { Icon = PackIconKind.Ubuntu, Name = "Ubuntu Zesty Zapus" },
                new OperatingSystemItem() { Icon = PackIconKind.Windows, Name = "Windows 7" },
                new OperatingSystemItem() { Icon = PackIconKind.Windows, Name = "Windows 8" },
                new OperatingSystemItem() { Icon = PackIconKind.Windows, Name = "Windows 8.1" },
                new OperatingSystemItem() { Icon = PackIconKind.Windows, Name = "Windows 10" },
                new OperatingSystemItem() { Icon = PackIconKind.Windows, Name = "Windows Vista" },
                new OperatingSystemItem() { Icon = PackIconKind.Windows, Name = "Windows XP" }
            };
        }

        public override IEnumerable<OperatingSystemItem> Search(string searchTerm)
        {
            searchTerm = searchTerm ?? string.Empty;
            searchTerm = searchTerm.ToLower();

            // trigger an update for testing purposes
            if (m_operatingSystemItems.Count <= 18)
            {
                Task.Delay(4000).ContinueWith(previousTask =>
                {
                    m_operatingSystemItems.AddRange(m_operatingSystemItems);

                    OnAutocompleteSourceItemsChanged();
                });
            }

            return m_operatingSystemItems.Where(item => item.Name.ToLower().Contains(searchTerm));
        }
    }*/
}
