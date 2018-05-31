using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using MaterialDesignThemes.Wpf;

using MaterialDesignExtensions.Controls;
using MaterialDesignExtensions.Model;

namespace MaterialDesignExtensionsDemo.ViewModel
{
    public class NavigationViewModel : ViewModel
    {
        private List<INavigationItem> m_navigationItems;
        private ICommand m_willSelectNavigationItemCommand;

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

        public ICommand WillSelectNavigationItemCommand
        {
            get
            {
                return m_willSelectNavigationItemCommand;
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
                new SecondLevelNavigationItem() { Label = "Camera", Icon = PackIconKind.Camera },
                new FirstLevelNavigationItem() { Label = "Music", Icon = PackIconKind.Music },
                new FirstLevelNavigationItem() { Label = "Videos", Icon = PackIconKind.Video },
                new FirstLevelNavigationItem() { Label = "Favorites", Icon = PackIconKind.Star },
                new DividerNavigationItem(),
                new SubheaderNavigationItem() { Subheader = "Other Places" },
                new FirstLevelNavigationItem() { Label = "Computer", Icon = PackIconKind.Monitor },
                new FirstLevelNavigationItem() { Label = "Network", Icon = PackIconKind.ServerNetwork },
                new FirstLevelNavigationItem() { Label = "Locked", Icon = PackIconKind.Lock }
            };

            m_willSelectNavigationItemCommand = new WillSelectNavigationItemCommand(parameter =>
            {

                if (parameter is WillSelectNavigationItemEventArgs args)
                {
                    // as a demo, cancel the navigation to the last item
                    if (args.NavigationItemToSelect == m_navigationItems[m_navigationItems.Count - 1])
                    {
                        args.Cancel = true;

                        DialogHost.Show(new AlertDialogViewModel("I do not like to select this locked item."), MainWindow.DialogHostName);
                    }
                }
            });
        }
    }

    public class WillSelectNavigationItemCommand : ICommand
    {
        private Action<object> m_execute;

        public WillSelectNavigationItemCommand(Action<object> execute)
        {
            m_execute = execute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            m_execute(parameter);
        }
    }
}
