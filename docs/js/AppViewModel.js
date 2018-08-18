function AppViewModel(contentDivId, drawer) {

    let self = this;

    self.contentDivId = contentDivId;
    self.drawer = drawer;
    self.selectedNavigationItem = ko.observable();
    
    self.navigationItems = [
        new NavigationItem('home', 'Home', 'home', 'snippets/home.html'),
        new NavigationItem('documentation', 'Documentation', 'help', 'snippets/documentation.html'),
        new NavigationItem('license', 'License', 'receipt', 'snippets/license.html')
    ];

    self.documentationItems = [
        new DocumentationItem('appbar', 'App bar', 'https://raw.githubusercontent.com/spiegelp/MaterialDesignExtensions/master/screenshots/AppBar1.png', 'snippets/documentation/appbar.html'),
        new DocumentationItem('autocomplete', 'Autocomplate', 'https://raw.githubusercontent.com/spiegelp/MaterialDesignExtensions/master/screenshots/Autocomplete.png', 'snippets/documentation/autocomplete.html'),
        new DocumentationItem('gridlist', 'Grid list', 'https://raw.githubusercontent.com/spiegelp/MaterialDesignExtensions/master/screenshots/GridList.png', 'snippets/documentation/gridlist.html'),
        new DocumentationItem('navigation', 'Navigation', 'https://raw.githubusercontent.com/spiegelp/MaterialDesignExtensions/master/screenshots/SideNavigation.png', 'snippets/documentation/sidenavigation.html'),
        new DocumentationItem('oversizednumberspinner', 'Oversized number spinner', 'https://raw.githubusercontent.com/spiegelp/MaterialDesignExtensions/master/screenshots/OversizedNumberSpinner.png', 'snippets/documentation/oversizednumberspinner.html'),
        new DocumentationItem('search', 'Search', 'https://raw.githubusercontent.com/spiegelp/MaterialDesignExtensions/master/screenshots/PersistentSearch.png', 'snippets/documentation/search.html'),
        new DocumentationItem('stepper', 'Stepper', 'https://raw.githubusercontent.com/spiegelp/MaterialDesignExtensions/master/screenshots/HorizontalStepper.png', 'snippets/documentation/stepper.html')
    ];

    self.goToNavigationItem = function (navigationItem) {
        self.goToNavigationItemId(navigationItem.id);
    };

    self.goToNavigationItemId = function (navigationItemId) {
        location.hash = navigationItemId;
    };

    self.goToDocumentationItem = function (documentationItem) {
        self.goToDocumentationItemId(documentationItem.id);
    };

    self.goToDocumentationItemId = function (documentationItemId) {
        location.hash = 'documentation/' + documentationItemId;
    };

    self.setHtmlForNavigationItem = function (navigationItem) {
        self.setHtmlForUrl(navigationItem.contentUrl);
    };

    self.setHtmlForUrl = function (url) {
        $('#' + self.contentDivId).load(url, null, function () { self.drawer.open = false; window.scrollTo(0, 0); });
    };

    self.prepareCodeSnippets = function () {
        /*let codeElements = $("code[class='language-markup']");

        for (let i = 0; i < codeElements.length; i++) {
            codeElements[i].innerHTML = codeElements[i].innerHTML.replace('<', '&lt;');
        }*/

        Prism.highlightAll();
    };

    Sammy(function () {
        this.get('#:navigationItemId', function () {
            let navigationItemId = this.params.navigationItemId;

            for (let i = 0; i < self.navigationItems.length; i++) {
                if (navigationItemId === self.navigationItems[i].id) {
                    self.selectedNavigationItem = self.navigationItems[i];
                    self.navigationItems[i].isSelected(true);

                    self.setHtmlForNavigationItem(self.navigationItems[i]);
                } else {
                    self.navigationItems[i].isSelected(false);
                }
            }
        });

        this.get('#:navigationItemId/:documentationItemId', function () {
            let navigationItemId = this.params.navigationItemId;

            if (navigationItemId === 'documentation') {
                let documentationItemId = this.params.documentationItemId;

                for (let i = 0; i < self.documentationItems.length; i++) {
                    if (self.documentationItems[i].id === documentationItemId) {
                        self.setHtmlForUrl(self.documentationItems[i].contentUrl);

                        break;
                    }
                }
            }
        });

        this.get('', function () { this.app.runRoute('get', '#home'); });
    }).run();
}
