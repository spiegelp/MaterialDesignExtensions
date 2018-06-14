<img src="https://github.com/spiegelp/MaterialDesignExtensions/raw/master/icon/icon.png" width="128px" />

# Material Design Extensions
Material Design Extensions is based on [Material Design in XAML Toolkit](https://github.com/ButchersBoy/MaterialDesignInXamlToolkit) to provide additional controls and features for WPF apps. The controls might not be specified in the [Material Design specification](https://material.io/guidelines/material-design/introduction.html) or would crash the scope of [Material Design in XAML Toolkit](https://github.com/ButchersBoy/MaterialDesignInXamlToolkit).

# NuGet

[![NuGet Status](http://img.shields.io/nuget/v/MaterialDesignExtensions.svg?style=flat&label=MaterialDesignExtensions)](https://www.nuget.org/packages/MaterialDesignExtensions/)

Install NuGet package. `PM> Install-Package MaterialDesignExtensions`

Assemblies are compiled for .NET Framework 4.5

# Getting started
1. Create a WPF desktop application
2. Install Material Design Extensions via [NuGet](https://www.nuget.org/packages/MaterialDesignExtensions/)
3. Add the styles to your App.xaml (see [App.xaml](https://github.com/spiegelp/MaterialDesignExtensions/blob/master/MaterialDesignExtensionsDemo/App.xaml) in the demo)
4. Add the namespace `xmlns:controls="clr-namespace:MaterialDesignExtensions.Controls;assembly=MaterialDesignExtensions"` to your XAML 
5. You are ready to use the controls

# Controls
Material Design Extensions features the following controls:

| Control | Details | Status |
| --- | --- | --- |
| [Stepper](https://github.com/spiegelp/MaterialDesignExtensions/wiki/Stepper) | Custom `Stepper` control ([specification](https://material.io/guidelines/components/steppers.html)) | Done |
| [Oversized number spinner](https://github.com/spiegelp/MaterialDesignExtensions/wiki/Oversized-number-spinner) | Custom `OversizedNumberSpinner` control | Done |
| [Grid list](https://github.com/spiegelp/MaterialDesignExtensions/wiki/Grid-list) | Templates for `ListBox` to render as a grid list ([specification](https://material.io/design/components/image-lists.html#usage)) | Done |
| [Open directory](https://github.com/spiegelp/MaterialDesignExtensions/wiki/File-system-controls) | Custom `OpenDirectoryControl` and `OpenDirectoryDialog` control | Done |
| [Open file](https://github.com/spiegelp/MaterialDesignExtensions/wiki/File-system-controls) | Custom `OpenFileControl` and `OpenFileDialog` control | Done |
| [Save file](https://github.com/spiegelp/MaterialDesignExtensions/wiki/File-system-controls) | Custom `SaveFileControl` and `SaveFileDialog` control | Done |
| [App bar](https://github.com/spiegelp/MaterialDesignExtensions/wiki/App-bar) | Custom `AppBar` control ([specification](https://material.io/design/components/app-bars-top.html#usage)) | Done |
| [Persistent search](https://github.com/spiegelp/MaterialDesignExtensions/wiki/Search) | Custom `PersistentSearch` control ([specification](https://material.io/design/navigation/search.html)) | Done |
| [Side navigation](https://github.com/spiegelp/MaterialDesignExtensions/wiki/Navigation) | Custom `SideNavigation` control ([specification](https://material.io/design/components/navigation-drawer.html#usage)) | Done |

# Screenshots
### Horizontal stepper
![](https://github.com/spiegelp/MaterialDesignExtensions/raw/master/screenshots/HorizontalStepper.png)

### Vertical stepper
![](https://github.com/spiegelp/MaterialDesignExtensions/raw/master/screenshots/VerticalStepper.png)

### Side navigation
![](https://github.com/spiegelp/MaterialDesignExtensions/raw/master/screenshots/SideNavigation.png)

### App bar
![](https://github.com/spiegelp/MaterialDesignExtensions/raw/master/screenshots/AppBar.png)

### Grid list
![](https://github.com/spiegelp/MaterialDesignExtensions/raw/master/screenshots/GridList.png)

### Persistent search
![](https://github.com/spiegelp/MaterialDesignExtensions/raw/master/screenshots/PersistentSearch.png)

### Oversized number spinner
![](https://github.com/spiegelp/MaterialDesignExtensions/raw/master/screenshots/OversizedNumberSpinner.png)

# Documentation
You find the API documentation inside the [wiki](https://github.com/spiegelp/MaterialDesignExtensions/wiki).

# License
Material Design Extensions is licensed under the [MIT](https://github.com/spiegelp/MaterialDesignExtensions/blob/master/LICENSE) license.
