# Release notes
## Material Design Extensions
### v1.0.0
#### Features
* Stepper from the [Material Design specification](https://material.io/guidelines/components/steppers.html)
* Oversized number spinner
* `ListBox` template to render as a grid list from the [Material Design specification](https://material.io/design/components/image-lists.html#usage)
### v1.1.0
#### Features
* Assign steps to the `Stepper` via XAML
* `OpenDirectoryControl` and `OpenDirectoryDialog` to select a directory
* `OpenFileControl` and `OpenFileDialog` to select a file for opening
* `SaveFileControl` and `SaveFileDialog` to select a file for saving data into
### v1.1.1
#### Fixes
* Pack localized resources into NuGet package
### v1.2.0
#### Features
* New `AppBar` control with basic behavior defined in the [Material Design specification](https://material.io/design/components/app-bars-top.html#usage)
* Extensions for the `Stepper` API
  * Read-only `ActiveStep` property
  * Event `ActiveStepChangedEvent`
  * Command properties for navigation callbacks
    * `ActiveStepChangedCommand`
	* `BackNavigationCommand`
	* `CancelNavigationCommand`
	* `ContinueNavigationCommand`
	* `StepNavigationCommand`
#### Fixes
* Initialize `Stepper.Steps` in the constructor
### v2.0.0
#### Features
* File filtering for `OpenFileControl`, `SaveFileControl`, `OpenFileDialog` and `SaveFileDialog`
* Improved user interface for the file system controls
#### Fixes
* Type dots into `SaveFileControl`'s text field
* Use `ItemsControl` instead of `ListBox` inside the file system controls
  * `ListBox.SelectionChanged` was raised several times without any explicit user input causing undesired behavior
#### Obsolete
* Old overloads of the show dialog methods are obsolete (in `OpenDirectoryControl`, `OpenFileControl` and `SaveFileControl`)
#### Breaking API
* Modified and removed members in `FileSystemControl` and sub classes
### v2.0.1
#### Fixes
* Handle `null` in `FileFiltersTypeConverter`
* Consistent default value for `FileDialogArguments.FilterIndex`
### v2.1.0
#### Features
* Additional layouts for `AppBar`
  * `ExtraProminent`
  * `DenseExtraProminent`
  * `Medium`
  * `MediumProminent`
  * `MediumExtraProminent`
* Extensions for the `Stepper` API
  * `NavigationCanceledByValidation` event and `NavigationCanceledByValidationCommand` command
  * `ContentAnimationsEnabled` property to enable or disable animations
  * Methods to navigate by code
* New control `PersistentSearch`
* New control `SideNavigation`
#### Fixes
* Unregister old event handlers on applying templates
### v2.2.0
#### Features
* `IsBackEnabled`, `IsCancelEnabled` and `IsContinueEnabled` properties for `StepButtonBar`
* `Back` event and `BackCommand` command for `AppBar`
* Image thumbnails in `OpenFileControl` and `SaveFileControl`
* Extensions to file system controls API
  * `FileSelectedCommand` command for `BaseFileControl`
  * `DirectorySelectedCommand` command for `OpenDirectoryControl`
### v2.3.0
#### Features
* `SearchHint` and `SearchIcon` properties for `SearchBase`
* Setter for `Stepper.ActiveStep` property in order to navigate by a data binding
* Change the horizontal `Stepper` separator color via the resource `MaterialDesignStepperSeparator`
* New control `Autocomplete`
* New control `TextBoxSuggestions`
* Improved keyboard navigation
#### Fixes
* Fixed `AppBar` title in the prominent area
* Prevent `NullReferenceException` in `BaseFileControl` and `OpenDirectoryControl`
### v2.4.0
#### Features
* Improved layout for file system controls
* New control `TabControlStepper`
* Improved layout for `OversizedNumberSpinner`
* New interface `IAutocompleteSourceChangingItems` to propagate source changes to `Autocomplete`
#### Fixes
* Handle `PathTooLongException`
### v2.5.0
#### Features
* New control `StepTitleHeaderControl` to use bindings in a XAML defined step
* `TabControl` styles to render it in [Material Design](https://material.io/design/components/tabs.html)
* New icons for file system controls
#### Fixes
* Handle broken image files in `BitmapImageHelper`
* Updated dependency to MaterialDesignThemes version 2.5.1 to bypass a bug in version 2.5.0.1205
* Use `Dispatcher` in `Autocomplete.AutocompleteSourceItemsChangedHandler()`
### v2.6.0
#### Features
* Localization for Uzbekistan
* Create new directories inside `OpenDirectoryControl` and `SaveFileControl`
* Helper `PaletteHelper` for changing the theme at runtime
#### Fixes
* List used resources explicitly in the XAML files containing the templates
* Prevent `NullReferenceException` in `SideNavigation` before the template will be applied
#### Important notice
The configuration of Material Design Extensions changed in order to enable changing the theme at runtime.
Please change your configuration according to [App.xaml](https://github.com/spiegelp/MaterialDesignExtensions/blob/master/MaterialDesignExtensionsDemo/App.xaml) of the demo.
### v2.7.0
#### Features
* New package `MaterialDesignExtensions.LongPath` for supporting long file system paths on older Windows and .NET versions
* Improvements for file system controls
  * Improved loading of preview images
  * Smoother scrolling in directories and files list
* New `SaveFileControl.ForceFileExtensionOfFileFilter` property to enforce a file extension of the selected filter
* Better support of dark theme in file system dialogs
* Localization for Russian
* Updated dependency to MaterialDesignThemes version 2.6.0
#### Fixes
* Fixed `DateTime` conversion inside `DateTimeAgoConverter`
### v2.8.0
#### Features
* New window class `MaterialWindow` for a Material Design like styled window
* Support of `TabStripPlacement` in `TabControl` styles
* `OpenMultipleDirectoriesControl` and `OpenMultipleDirectoriesDialog` to select multiple directories
* `OpenMultipleFilesControl` and `OpenMultipleFilesDialog` to select multiple files
#### Fixes
* Fixed content layout of horizontal steppers
#### Obsolete
* Attached property `TabControlAssist.TabHeaderAlignment` will be replaced by `TabControlAssist.TabHeaderHorizontalAlignment`
### v3.0.0
#### Features
* Support for .NET Core 3
* Select file with double click in `OpenFileDialog` and `SaveFileDialog`
* Improved keyboard navigation for file system controls
* Dense style for `PersistentSearch`
* Optional icon templates for steps inside a stepper
* Improved stepper navigation
* Added `MaterialWindow.TitleBarIcon` property
#### Breaking API
* Removed obsolete members
  * `TabControlAssist.TabHeaderAlignment` attached property
  * `OpenDirectoryDialog.ShowDialogAsync` methods
  * `OpenFileDialog.ShowDialogAsync` methods
  * `SaveFileDialog.ShowDialogAsync` methods
* Extendend `IStepper` interface to implement new features for steppers
* Extendend `IStep` interface to implement new features for steppers
### v3.1.0
#### Features
* Update to .NET Core 3.1
* Controls for simple alert and confirmation dialogs
* New `TextBoxSuggestions.KeepFocusOnSelection` property to control the focus after selecting a suggestion
* New controls combining a `TextBox` and a `FileSystemDialog`
  * `TextBoxOpenDirectory`
  * `TextBoxOpenFile`
  * `TextBoxSaveFile`
* Improvements for `MaterialWindow`
  * New `MaterialWindow.TitleTemplate` property to customize the window title bar
  * Different looks according to `Window.WindowStyle` property
* New control `NavigationRail`
#### Fixes
* Catch exception if network drive is not accessible
### v3.2.0 (upcoming release)
#### Features
* New control `BusyOverlay`
* New `TextBoxFileSystemPath.TextBoxStyle` property to enable custom styles for the embedded `TextBox`
* New localizations
  * Czech
  * Portuguese
* Save file in `SaveFileControl` and `SaveFileDialog` by hitting enter
* New `ClearSelection` method for `Autocomplete`
#### Fixes
* Fixed exception in `Stepper` events
