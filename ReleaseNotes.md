# Release notes
## Material Design Extensions
### v1.0.0
#### Features
* Stepper from the [Material Design specification](https://material.io/guidelines/components/steppers.html)
* Oversized number spinner
* `ListBox` template to render as a grid list from the [Material Design specification](https://material.io/guidelines/components/grid-lists.html)
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
* New `AppBar` control with basic behavior defined in the [Material Design specification](https://material.io/guidelines/layout/structure.html#structure-app-bar)
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
### vX.X.X (upcoming release)
#### Features
* Additional layouts for `AppBar`
  * `ExtraProminent`
  * `DenseExtraProminent`
  * `Medium`
  * `MediumProminent`
  * `MediumExtraProminent`
* New `NavigationCanceledByValidation` event and `NavigationCanceledByValidationCommand` command for `Stepper`
* Methods on `Stepper` to navigate by code
* New `ContentAnimationsEnabled` property for `Stepper` to enable or disable its animations
* New control `PersistentSearch`
* New control `SideNavigation`
#### Fixes
* Unregister old event handlers on applying templates
