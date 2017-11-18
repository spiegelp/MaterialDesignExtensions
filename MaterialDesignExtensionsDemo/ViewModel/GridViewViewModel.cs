using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MaterialDesignThemes.Wpf;

namespace MaterialDesignExtensionsDemo.ViewModel
{
    public class GridViewViewModel : ViewModel
    {
        private List<GridViewItem> m_items;

        public List<GridViewItem> Items
        {
            get
            {
                return m_items;
            }
        }

        public GridViewViewModel()
            : base()
        {
            m_items = new List<GridViewItem>()
            {
                new GridViewItem() { Kind = PackIconKind.Android, Label = "Android" },
                new GridViewItem() { Kind = PackIconKind.Windows, Label = "Windows" },
                new GridViewItem() { Kind = PackIconKind.AppleIos, Label = "iOS" },
                new GridViewItem() { Kind = PackIconKind.Android, Label = "Android" },
                new GridViewItem() { Kind = PackIconKind.Windows, Label = "Windows" },
                new GridViewItem() { Kind = PackIconKind.AppleIos, Label = "iOS" },
                new GridViewItem() { Kind = PackIconKind.Android, Label = "Android" },
                new GridViewItem() { Kind = PackIconKind.Windows, Label = "Windows" },
                new GridViewItem() { Kind = PackIconKind.AppleIos, Label = "iOS" },
                new GridViewItem() { Kind = PackIconKind.Android, Label = "Android" },
                new GridViewItem() { Kind = PackIconKind.Windows, Label = "Windows" },
                new GridViewItem() { Kind = PackIconKind.AppleIos, Label = "iOS" }
            };
        }
    }

    public class GridViewItem : ViewModel
    {
        public PackIconKind Kind { get; set; }
        public string Label { get; set; }

        public GridViewItem() { }
    }
}
