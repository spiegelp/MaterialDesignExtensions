<img src="https://github.com/spiegelp/MaterialDesignExtensions/raw/master/icon/icon.png" width="128px" />

# Material Design Extensions
Material Design Extensions is based on [Material Design in XAML Toolkit](https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit) to provide additional controls and features for WPF apps. The controls might not be specified in the [Material Design specification](https://material.io/guidelines/material-design/introduction.html) or would crash the scope of [Material Design in XAML Toolkit](https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit).

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
| [Stepper](https://spiegelp.github.io/MaterialDesignExtensions/#documentation/stepper) | Custom `Stepper` control ([specification](https://material.io/archive/guidelines/components/steppers.html)) | Done |
| [Oversized number spinner](https://spiegelp.github.io/MaterialDesignExtensions/#documentation/oversizednumberspinner) | Custom `OversizedNumberSpinner` control | Done |
| [Grid list](https://spiegelp.github.io/MaterialDesignExtensions/#documentation/gridlist) | Templates for `ListBox` to render as a grid list ([specification](https://material.io/design/components/image-lists.html#usage)) | Done |
| [Open directory](https://spiegelp.github.io/MaterialDesignExtensions/#documentation/filesystemcontrols) | Custom `OpenDirectoryControl` and `OpenDirectoryDialog` control | Done |
| [Open file](https://spiegelp.github.io/MaterialDesignExtensions/#documentation/filesystemcontrols) | Custom `OpenFileControl` and `OpenFileDialog` control | Done |
| [Save file](https://spiegelp.github.io/MaterialDesignExtensions/#documentation/filesystemcontrols) | Custom `SaveFileControl` and `SaveFileDialog` control | Done |
| [App bar](https://spiegelp.github.io/MaterialDesignExtensions/#documentation/appbar) | Custom `AppBar` control ([specification](https://material.io/design/components/app-bars-top.html#usage)) | Done |
| [Persistent search](https://spiegelp.github.io/MaterialDesignExtensions/#documentation/search) | Custom `PersistentSearch` control ([specification](https://material.io/design/navigation/search.html)) | Done |
| [Side navigation](https://spiegelp.github.io/MaterialDesignExtensions/#documentation/navigation) | Custom `SideNavigation` control ([specification](https://material.io/design/components/navigation-drawer.html#usage)) | Done |
| [Autocomplete](https://spiegelp.github.io/MaterialDesignExtensions/#documentation/autocomplete) | Custom `Autocomplete` control | Done |
| [Text box suggestions](https://spiegelp.github.io/MaterialDesignExtensions/#documentation/textboxsuggestions) | Custom `TextBoxSuggestions` control | Done |
| [Tabs](https://spiegelp.github.io/MaterialDesignExtensions/#documentation/tabs) | Templates for `TabControl` ([specification](https://material.io/design/components/tabs.html)) | In development |

# Screenshots
### Horizontal stepper
![](https://github.com/spiegelp/MaterialDesignExtensions/raw/master/screenshots/HorizontalStepper.png)

### Vertical stepper
![](https://github.com/spiegelp/MaterialDesignExtensions/raw/master/screenshots/VerticalStepper.png)

### Side navigation
![](https://github.com/spiegelp/MaterialDesignExtensions/raw/master/screenshots/SideNavigation.png)

### Tabs
![](https://github.com/spiegelp/MaterialDesignExtensions/raw/master/screenshots/TabControl1.png)

### App bar
![](https://github.com/spiegelp/MaterialDesignExtensions/raw/master/screenshots/AppBar1.png)

### Grid list
![](https://github.com/spiegelp/MaterialDesignExtensions/raw/master/screenshots/GridList.png)

### Persistent search
![](https://github.com/spiegelp/MaterialDesignExtensions/raw/master/screenshots/PersistentSearch.png)

### Autocomplete
![](https://github.com/spiegelp/MaterialDesignExtensions/raw/master/screenshots/Autocomplete.png)

### Oversized number spinner
![](https://github.com/spiegelp/MaterialDesignExtensions/raw/master/screenshots/OversizedNumberSpinner.png)

# Documentation
You will find the API documentation on the [website](https://spiegelp.github.io/MaterialDesignExtensions/#documentation).

# License
Material Design Extensions is licensed under the [MIT](https://github.com/spiegelp/MaterialDesignExtensions/blob/master/LICENSE) license.
