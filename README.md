# Material Design Extensions
<p align="center">
  <img src="https://github.com/spiegelp/MaterialDesignExtensions/raw/master/icon/icon.png" alt="Material Design Extensions icon" width="128px" />
</p>
<p>
Material Design Extensions is based on <a href="https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit">Material Design in XAML Toolkit</a> to provide additional controls and features for WPF apps. The controls might not be specified in the <a href="https://material.io/guidelines/material-design/introduction.html">Material Design specification</a> or would crash the scope of <a href="https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit">Material Design in XAML Toolkit</a>.
</p>

[![Build status](https://dev.azure.com/spiegelp/MaterialDesignExtensions/_apis/build/status/MaterialDesignExtensions-.NET%20Desktop-CI)](https://dev.azure.com/spiegelp/MaterialDesignExtensions/_build/latest?definitionId=2)

## NuGet

[![NuGet Status](https://img.shields.io/nuget/v/MaterialDesignExtensions.svg?style=flat&label=MaterialDesignExtensions&logo=nuget&color=blue)](https://www.nuget.org/packages/MaterialDesignExtensions/)

Install NuGet package. `PM> Install-Package MaterialDesignExtensions`

Assemblies are compiled for .NET Core 3.0 and .NET Framework 4.5

## Getting started
1. Create a WPF desktop application
2. Install Material Design Extensions via [NuGet](https://www.nuget.org/packages/MaterialDesignExtensions/)
3. Add the styles to your App.xaml (see [App.xaml](https://github.com/spiegelp/MaterialDesignExtensions/blob/master/MaterialDesignExtensionsDemo/App.xaml) in the demo)
4. Add the namespace `xmlns:controls="clr-namespace:MaterialDesignExtensions.Controls;assembly=MaterialDesignExtensions"` to your XAML 
5. You are ready to use the controls

## Important notice
The configuration of Material Design Extensions v2.6.0 changed in order to enable changing the theme at runtime.
Please change your configuration according to [App.xaml](https://github.com/spiegelp/MaterialDesignExtensions/blob/master/MaterialDesignExtensionsDemo/App.xaml) of the demo.

## Controls
Material Design Extensions features the following controls:

| Control | Details | Status |
| --- | --- | --- |
| [Stepper](https://spiegelp.github.io/MaterialDesignExtensions/#documentation/stepper) | Custom `Stepper` control ([specification](https://material.io/archive/guidelines/components/steppers.html)) | Done |
| [Oversized number spinner](https://spiegelp.github.io/MaterialDesignExtensions/#documentation/oversizednumberspinner) | Custom `OversizedNumberSpinner` control | Done |
| [Grid list](https://spiegelp.github.io/MaterialDesignExtensions/#documentation/gridlist) | Templates for `ListBox` to render as a grid list ([specification](https://material.io/design/components/image-lists.html#usage)) | Done |
| [Open directory](https://spiegelp.github.io/MaterialDesignExtensions/#documentation/filesystemcontrols) | Custom `OpenDirectoryControl` and `OpenDirectoryDialog` control | Done |
| [Open file](https://spiegelp.github.io/MaterialDesignExtensions/#documentation/filesystemcontrols) | Custom `OpenFileControl` and `OpenFileDialog` control | Done |
| [Save file](https://spiegelp.github.io/MaterialDesignExtensions/#documentation/filesystemcontrols) | Custom `SaveFileControl` and `SaveFileDialog` control | Done |
| [Open multiple directories](https://spiegelp.github.io/MaterialDesignExtensions/#documentation/filesystemcontrols) | Custom `OpenMultipleDirectoriesControl` and `OpenMultipleDirectoriesDialog` control | Done |
| [Open multiple files](https://spiegelp.github.io/MaterialDesignExtensions/#documentation/filesystemcontrols) | Custom `OpenMultipleFilesControl` and `OpenMultipleFilesDialog` control | Done |
| [Text box with file path](https://spiegelp.github.io/MaterialDesignExtensions/#documentation/filesystemcontrols) | Custom `TextBoxOpenDirectory`, `TextBoxOpenFile` and `TextBoxSaveFile` control | In development |
| [App bar](https://spiegelp.github.io/MaterialDesignExtensions/#documentation/appbar) | Custom `AppBar` control ([specification](https://material.io/design/components/app-bars-top.html#usage)) | Done |
| [Persistent search](https://spiegelp.github.io/MaterialDesignExtensions/#documentation/search) | Custom `PersistentSearch` control ([specification](https://material.io/design/navigation/search.html)) | Done |
| [Side navigation](https://spiegelp.github.io/MaterialDesignExtensions/#documentation/navigation) | Custom `SideNavigation` control ([specification](https://material.io/design/components/navigation-drawer.html#usage)) | Done |
| [Navigation rail](https://spiegelp.github.io/MaterialDesignExtensions/#documentation/navigation) | Custom `NavigationRail` control ([specification](https://material.io/components/navigation-rail/)) | In development |
| [Autocomplete](https://spiegelp.github.io/MaterialDesignExtensions/#documentation/autocomplete) | Custom `Autocomplete` control | Done |
| [Text box suggestions](https://spiegelp.github.io/MaterialDesignExtensions/#documentation/textboxsuggestions) | Custom `TextBoxSuggestions` control | Done |
| [Tabs](https://spiegelp.github.io/MaterialDesignExtensions/#documentation/tabs) | Templates for `TabControl` ([specification](https://material.io/design/components/tabs.html)) | Done |
| [Material window](https://spiegelp.github.io/MaterialDesignExtensions/#documentation/materialwindow) | Custom `MaterialWindow` control | Done |

## Screenshots
### Horizontal stepper
![](https://github.com/spiegelp/MaterialDesignExtensions/raw/master/screenshots/HorizontalStepper.png)

### Vertical stepper
![](https://github.com/spiegelp/MaterialDesignExtensions/raw/master/screenshots/VerticalStepper.png)

### Side navigation
![](https://github.com/spiegelp/MaterialDesignExtensions/raw/master/screenshots/SideNavigation.png)

### Navigation rail
![](https://github.com/spiegelp/MaterialDesignExtensions/raw/master/screenshots/NavigationRail1.png)

### Tabs
![](https://github.com/spiegelp/MaterialDesignExtensions/raw/master/screenshots/TabControl1.png)

### Material window and app bar
![](https://github.com/spiegelp/MaterialDesignExtensions/raw/master/screenshots/MaterialWindow1.png)

### Open directory
![](https://github.com/spiegelp/MaterialDesignExtensions/raw/master/screenshots/OpenDirectoryControl1.png)

### Open file
![](https://github.com/spiegelp/MaterialDesignExtensions/raw/master/screenshots/OpenFileControl1.png)

### Save file
![](https://github.com/spiegelp/MaterialDesignExtensions/raw/master/screenshots/SaveFileControl1.png)

### Grid list
![](https://github.com/spiegelp/MaterialDesignExtensions/raw/master/screenshots/GridList.png)

### Persistent search
![](https://github.com/spiegelp/MaterialDesignExtensions/raw/master/screenshots/PersistentSearch.png)

### Autocomplete
![](https://github.com/spiegelp/MaterialDesignExtensions/raw/master/screenshots/Autocomplete.png)

### Oversized number spinner
![](https://github.com/spiegelp/MaterialDesignExtensions/raw/master/screenshots/OversizedNumberSpinner.png)

## Documentation
You will find the API documentation on the [website](https://spiegelp.github.io/MaterialDesignExtensions/#documentation).

## License
Material Design Extensions is licensed under the [MIT](https://github.com/spiegelp/MaterialDesignExtensions/blob/master/LICENSE) license.
