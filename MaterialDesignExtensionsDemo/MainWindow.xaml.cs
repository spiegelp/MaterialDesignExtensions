﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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

using MaterialDesignThemes.Wpf;

using MaterialDesignExtensions.Controls;
using MaterialDesignExtensions.Model;

using MaterialDesignExtensionsDemo.ViewModel;

namespace MaterialDesignExtensionsDemo
{
    public partial class MainWindow : MaterialWindow
    {
        public const string DialogHostName = "dialogHost";

        private List<INavigationItem> m_navigationItems;

        public DialogHost DialogHost
        {
            get
            {
                return m_dialogHost;
            }
        }

        public List<INavigationItem> NavigationItems
        {
            get
            {
                return m_navigationItems;
            }
        }

        public MainWindow()
        {
            m_navigationItems = new List<INavigationItem>()
            {
                new SubheaderNavigationItem() { Subheader = "Controls" },
                new FirstLevelNavigationItem() { Label = "App bar", NavigationItemSelectedCallback = item => new AppBarViewModel() },
                new FirstLevelNavigationItem() { Label = "Oversized number spinner", NavigationItemSelectedCallback = item => new OversizedNumberSpinnerViewModel() },
                new FirstLevelNavigationItem() { Label = "Grid list", NavigationItemSelectedCallback = item => new GridListViewModel() },
                new FirstLevelNavigationItem() { Label = "TabControl", NavigationItemSelectedCallback = item => new TabControlViewModel() },
                new FirstLevelNavigationItem() { Label = "Stepper", NavigationItemSelectedCallback = item => new StepperViewModel() },
                new FirstLevelNavigationItem() { Label = "TabControl as stepper", NavigationItemSelectedCallback = item => new TabControlStepperViewModel() },
                new FirstLevelNavigationItem() { Label = "Autocomplete", NavigationItemSelectedCallback = item => new AutocompleteViewModel() },
                //new FirstLevelNavigationItem() { Label = "Autocomplete", NavigationItemSelectedCallback = item => new AutocompleteInTabControlViewModel() },
                new FirstLevelNavigationItem() { Label = "TextBoxSuggestions", NavigationItemSelectedCallback = item => new TextBoxSuggestionsViewModel() },
                new FirstLevelNavigationItem() { Label = "Message dialogs", NavigationItemSelectedCallback = item => new MessageDialogViewModel() },
                new FirstLevelNavigationItem() { Label = "Busy overlay", NavigationItemSelectedCallback = item => new BusyOverlayViewModel() },
                new DividerNavigationItem(),
                new SubheaderNavigationItem() { Subheader = "Directories and files" },
                new FirstLevelNavigationItem() { Label = "Open directory", Icon = PackIconKind.Folder, NavigationItemSelectedCallback = item => new OpenDirectoryControlViewModel() },
                new FirstLevelNavigationItem() { Label = "Open multiple directories", Icon = PackIconKind.Folder, NavigationItemSelectedCallback = item => new OpenMultipleDirectoriesControlViewModel() },
                new FirstLevelNavigationItem() { Label = "Open file", Icon = PackIconKind.File, NavigationItemSelectedCallback = item => new OpenFileControlViewModel() },
                new FirstLevelNavigationItem() { Label = "Open multiple files", Icon = PackIconKind.File, NavigationItemSelectedCallback = item => new OpenMultipleFilesControlViewModel() },
                new FirstLevelNavigationItem() { Label = "Save file", Icon = PackIconKind.File, NavigationItemSelectedCallback = item => new SaveFileControlViewModel() },
                new FirstLevelNavigationItem() { Label = "Directory and file dialogs", NavigationItemSelectedCallback = item => new FileSystemDialogViewModel() },
                new FirstLevelNavigationItem() { Label = "Text boxes with path", NavigationItemSelectedCallback = item => new TextBoxFileSystemPathsViewModel() },
                new DividerNavigationItem(),
                new SubheaderNavigationItem() { Subheader = "Navigation and searching" },
                new FirstLevelNavigationItem() { Label = "Navigation", Icon = PackIconKind.Menu, NavigationItemSelectedCallback = item => new NavigationViewModel() },
                new FirstLevelNavigationItem() { Label = "Navigation rail", Icon = PackIconKind.DotsVertical, NavigationItemSelectedCallback = item => new NavigationRailViewModel() },
                new FirstLevelNavigationItem() { Label = "Search", Icon = PackIconKind.Magnify, NavigationItemSelectedCallback = item => new SearchViewModel() },
                new DividerNavigationItem(),
                new SubheaderNavigationItem() { Subheader = "Themes" },
                new FirstLevelNavigationItem() { Label = "Themes", Icon = PackIconKind.Palette, NavigationItemSelectedCallback = item => new ThemesViewModel() },
                new FirstLevelNavigationItem() { Label = "Material window", Icon = PackIconKind.WindowMaximize, NavigationItemSelectedCallback = item => new WindowStyleViewModel() },
                new DividerNavigationItem(),
                new SubheaderNavigationItem() { Subheader = "i18n" },
                new FirstLevelNavigationItem() { Label = "Language", Icon = PackIconKind.Translate, NavigationItemSelectedCallback = item => new LanguageViewModel() }
            };

            InitializeComponent();

            sideNav.DataContext = this;
            navigationDrawerNav.DataContext = this;

            Loaded += LoadedHandler;
        }

        private void LoadedHandler(object sender, RoutedEventArgs args)
        {
            navigationDrawerNav.SelectedItem = m_navigationItems[1];
            sideNav.SelectedItem = m_navigationItems[1];
            m_navigationItems[1].IsSelected = true;
        }

        private void NavigationItemSelectedHandler(object sender, NavigationItemSelectedEventArgs args)
        {
            SelectNavigationItem(args.NavigationItem);
        }

        private void SelectNavigationItem(INavigationItem navigationItem)
        {
            if (navigationItem != null)
            {
                object newContent = navigationItem.NavigationItemSelectedCallback(navigationItem);

                if (contentControl.Content == null || contentControl.Content.GetType() != newContent.GetType())
                {
                    contentControl.Content = newContent;
                }
            }
            else
            {
                contentControl.Content = null;
            }

            if (appBar != null)
            {
                appBar.IsNavigationDrawerOpen = false;
            }
        }

        private void GoToGitHubButtonClickHandler(object sender, RoutedEventArgs args)
        {
            OpenLink("https://github.com/spiegelp/MaterialDesignExtensions");
        }

        private void GoToDocumentation(object sender, RoutedEventArgs args)
        {

            if (contentControl.Content is ViewModel.ViewModel viewModel && !string.IsNullOrWhiteSpace(viewModel.DocumentationUrl))
            {
                OpenLink(viewModel.DocumentationUrl);
            }
        }

        private void OpenLink(string url)
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            };

            Process.Start(psi);
        }
    }
}
