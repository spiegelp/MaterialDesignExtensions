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
