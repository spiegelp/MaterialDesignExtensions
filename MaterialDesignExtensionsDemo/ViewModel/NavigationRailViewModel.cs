using System;
using System.Collections.Generic;
using System.Text;

using MaterialDesignThemes.Wpf;

using MaterialDesignExtensions.Model;

namespace MaterialDesignExtensionsDemo.ViewModel
{

    public class NavigationRailViewModel : ViewModel
    {
        public override string DocumentationUrl
        {
            get
            {
                return "https://spiegelp.github.io/MaterialDesignExtensions/#documentation/navigation";
            }
        }

        public List<INavigationItem> NavigationItems
        {
            get
            {
                return new List<INavigationItem>()
                {
                    new FirstLevelNavigationItem() { Label = "Files", Icon = PackIconKind.FileDocument, IsSelected = true },
                    new FirstLevelNavigationItem() { Label = "Images", Icon = PackIconKind.Image },
                    new FirstLevelNavigationItem() { Label = "Music", Icon = PackIconKind.Music },
                    new FirstLevelNavigationItem() { Label = "Videos", Icon = PackIconKind.Video }
                };
            }
        }

        public NavigationRailViewModel() { }
    }
}
