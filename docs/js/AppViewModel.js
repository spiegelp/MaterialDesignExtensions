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

    self.goToNavigationItem = function (navigationItem) {
        self.goToNavigationItemId(navigationItem.id);
    };

    self.goToNavigationItemId = function (navigationItemId) {
        location.hash = navigationItemId;
    };

    self.setHtmlForNavigationItem = function (navigationItem) {
        $('#' + self.contentDivId).load(navigationItem.contentUrl, null, function () { self.drawer.open = false; });
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

        this.get('', function () { this.app.runRoute('get', '#home') });
    }).run();
}
