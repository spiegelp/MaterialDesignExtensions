function NavigationItem(id, label, icon, contentUrl) {

    this.id = id;
    this.label = label;
    this.icon = icon;
    this.contentUrl = contentUrl;

    this.isSelected = ko.observable();
    this.isSelected(false);
}
