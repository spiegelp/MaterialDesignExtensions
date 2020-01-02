﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
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
                new DividerNavigationItem(),
                new SubheaderNavigationItem() { Subheader = "Directories and files" },
                new FirstLevelNavigationItem() { Label = "Open directory", Icon = PackIconKind.Folder, NavigationItemSelectedCallback = item => new OpenDirectoryControlViewModel() },
                new FirstLevelNavigationItem() { Label = "Open multiple directories", Icon = PackIconKind.Folder, NavigationItemSelectedCallback = item => new OpenMultipleDirectoriesControlViewModel() },
                new FirstLevelNavigationItem() { Label = "Open file", Icon = PackIconKind.File, NavigationItemSelectedCallback = item => new OpenFileControlViewModel() },
                new FirstLevelNavigationItem() { Label = "Open multiple files", Icon = PackIconKind.File, NavigationItemSelectedCallback = item => new OpenMultipleFilesControlViewModel() },
                new FirstLevelNavigationItem() { Label = "Save file", Icon = PackIconKind.File, NavigationItemSelectedCallback = item => new SaveFileControlViewModel() },
                new FirstLevelNavigationItem() { Label = "Directory and file dialogs", NavigationItemSelectedCallback = item => new FileSystemDialogViewModel() },
                new DividerNavigationItem(),
                new SubheaderNavigationItem() { Subheader = "Navigation and searching" },
                new FirstLevelNavigationItem() { Label = "Navigation", Icon = PackIconKind.Menu, NavigationItemSelectedCallback = item => new NavigationViewModel() },
                new FirstLevelNavigationItem() { Label = "Search", Icon = PackIconKind.Magnify, NavigationItemSelectedCallback = item => new SearchViewModel() },
                new DividerNavigationItem(),
                new SubheaderNavigationItem() { Subheader = "Themes" },
                new FirstLevelNavigationItem() { Label = "Themes", Icon = PackIconKind.Palette, NavigationItemSelectedCallback = item => new ThemesViewModel() },
                new DividerNavigationItem(),
                new SubheaderNavigationItem() { Subheader = "i18n" },
                new FirstLevelNavigationItem() { Label = "Language", Icon = PackIconKind.Translate, NavigationItemSelectedCallback = item => new LanguageViewModel() }
            };

            InitializeComponent();

            navigationDrawerNav.SelectedItem = m_navigationItems[1];
            sideNav.SelectedItem = m_navigationItems[1];
            m_navigationItems[1].IsSelected = true;

            sideNav.DataContext = this;
            navigationDrawerNav.DataContext = this;
        }

        private void NavigationItemSelectedHandler(object sender, NavigationItemSelectedEventArgs args)
        {
            SelectNavigationItem(args.NavigationItem);
        }

        private void SelectNavigationItem(INavigationItem navigationItem)
        {
            if (navigationItem != null)
            {
                contentControl.Content = navigationItem.NavigationItemSelectedCallback(navigationItem);
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
            Process.Start("https://github.com/spiegelp/MaterialDesignExtensions");
        }

        private void GoToDocumentation(object sender, RoutedEventArgs args)
        {

            if (contentControl.Content is ViewModel.ViewModel viewModel && !string.IsNullOrWhiteSpace(viewModel.DocumentationUrl))
            {
                Process.Start(viewModel.DocumentationUrl);
            }
        }
    }
}
