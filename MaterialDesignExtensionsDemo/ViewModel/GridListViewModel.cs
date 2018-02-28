using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MaterialDesignThemes.Wpf;

namespace MaterialDesignExtensionsDemo.ViewModel
{
    public class GridListViewModel : ViewModel
    {
        private List<GridListItem> m_items;

        public override string DocumentationUrl
        {
            get
            {
                return "https://github.com/spiegelp/MaterialDesignExtensions/wiki/Grid-list";
            }
        }

        public List<GridListItem> Items
        {
            get
            {
                return m_items;
            }
        }

        public GridListViewModel()
            : base()
        {
            m_items = new List<GridListItem>()
            {
                new GridListItem() { Kind = PackIconKind.Android, Label = "Android" },
                new GridListItem() { Kind = PackIconKind.Windows, Label = "Windows" },
                new GridListItem() { Kind = PackIconKind.AppleIos, Label = "iOS" },
                new GridListItem() { Kind = PackIconKind.Android, Label = "Android" },
                new GridListItem() { Kind = PackIconKind.Windows, Label = "Windows" },
                new GridListItem() { Kind = PackIconKind.AppleIos, Label = "iOS" },
                new GridListItem() { Kind = PackIconKind.Android, Label = "Android" },
                new GridListItem() { Kind = PackIconKind.Windows, Label = "Windows" },
                new GridListItem() { Kind = PackIconKind.AppleIos, Label = "iOS" },
                new GridListItem() { Kind = PackIconKind.Android, Label = "Android" },
                new GridListItem() { Kind = PackIconKind.Windows, Label = "Windows" },
                new GridListItem() { Kind = PackIconKind.AppleIos, Label = "iOS" }
            };
        }
    }

    public class GridListItem : ViewModel
    {
        public PackIconKind Kind { get; set; }
        public string Label { get; set; }

        public GridListItem() { }
    }
}
