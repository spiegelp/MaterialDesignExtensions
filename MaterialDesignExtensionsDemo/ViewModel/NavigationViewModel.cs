using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MaterialDesignThemes.Wpf;

using MaterialDesignExtensions.Model;

namespace MaterialDesignExtensionsDemo.ViewModel
{
    public class NavigationViewModel : ViewModel
    {
        private List<INavigationItem> m_navigationItems;

        public override string DocumentationUrl
        {
            get
            {
                return "https://github.com/spiegelp/MaterialDesignExtensions/wiki/Navigation";
            }
        }

        public List<INavigationItem> NavigationItems
        {
            get
            {
                return m_navigationItems;
            }
        }

        public NavigationViewModel()
            : base()
        {
            m_navigationItems = new List<INavigationItem>()
            {
                new SubheaderNavigationItem() { Subheader = "My files" },
                new FirstLevelNavigationItem() { Label = "Documents", Icon = PackIconKind.File, IsSelected = true },
                new FirstLevelNavigationItem() { Label = "Pictures", Icon = PackIconKind.Image },
                new SecondLevelNavigationItem() { Label = "Camera" },
                new FirstLevelNavigationItem() { Label = "Music", Icon = PackIconKind.Music },
                new FirstLevelNavigationItem() { Label = "Videos", Icon = PackIconKind.Video },
                new FirstLevelNavigationItem() { Label = "Favorites", Icon = PackIconKind.Star },
                new DividerNavigationItem(),
                new SubheaderNavigationItem() { Subheader = "Other Places" },
                new FirstLevelNavigationItem() { Label = "Computer", Icon = PackIconKind.Monitor },
                new FirstLevelNavigationItem() { Label = "Network", Icon = PackIconKind.ServerNetwork }
            };
        }
    }
}
