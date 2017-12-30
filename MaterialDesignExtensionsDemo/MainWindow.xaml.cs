using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using MaterialDesignExtensionsDemo.ViewModel;

namespace MaterialDesignExtensionsDemo
{
    public partial class MainWindow : Window
    {
        public const string DialogHostName = "dialogHost";

        public static RoutedCommand NavigationItemSelectedCommand = new RoutedCommand();

        private List<NavigationItem> m_navigationItems;

        public List<NavigationItem> NavigationItems
        {
            get
            {
                return m_navigationItems;
            }
        }

        public MainWindow()
        {
            m_navigationItems = new List<NavigationItem>()
            {
                new NavigationItem() { Label = "Oversized Number Spinner", Action = () => new OversizedNumberSpinnerViewModel() },
                new NavigationItem() { Label = "Grid list", Action = () => new GridListViewModel() },
                new NavigationItem() { Label = "Stepper", Action = () => new StepperViewModel() },
                new NavigationItem() { Label = "Open directory", Action = () => new OpenDirectoryControlViewModel() },
                new NavigationItem() { Label = "Open file", Action = () => new OpenFileControlViewModel() },
                new NavigationItem() { Label = "Save file", Action = () => new SaveFileControlViewModel() },
                new NavigationItem() { Label = "Directory and file dialogs", Action = () => new FileSystemDialogViewModel() }
            };

            InitializeComponent();

            SelectNavigationItem(m_navigationItems[0]);

            CommandBindings.Add(new CommandBinding(NavigationItemSelectedCommand, NavigationItemSelectedHandler));

            navigationItemsControl.DataContext = this;
        }

        private void NavigationItemSelectedHandler(object sender, ExecutedRoutedEventArgs args)
        {
            SelectNavigationItem(args.Parameter as NavigationItem);
        }

        private void SelectNavigationItem(NavigationItem navigationItem)
        {
            m_navigationItems.ForEach(item => item.IsSelected = item == navigationItem);

            if (navigationItem != null)
            {
                contentControl.Content = navigationItem.Action.Invoke();
            }
            else
            {
                contentControl.Content = null;
            }
        }
    }
}
