using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

using MaterialDesignExtensions.Controllers;
using MaterialDesignExtensions.Model;

namespace MaterialDesignExtensions.Controls
{
    /// <summary>
    /// An interface with basic members of a stepper.
    /// </summary>
    public interface IStepper
    {
        /// <summary>
        /// An event raised by navigating to the previous <see cref="IStep" /> in a linear order.
        /// </summary>
        event StepperNavigationEventHandler BackNavigation;

        /// <summary>
        /// An event raised by cancelling the process.
        /// </summary>
        event StepperNavigationEventHandler CancelNavigation;

        /// <summary>
        /// An event raised by navigating to the next <see cref="IStep" /> in a linear order.
        /// </summary>
        event StepperNavigationEventHandler ContinueNavigation;

        /// <summary>
        /// An event raised by navigating to an arbitrary <see cref="IStep" /> in a non-linear <see cref="TabControlStepper" />.
        /// </summary>
        event StepperNavigationEventHandler StepNavigation;

        /// <summary>
        /// A command called by navigating to the previous <see cref="IStep" /> in a linear order.
        /// </summary>
        ICommand BackNavigationCommand { get; set; }

        /// <summary>
        /// A command called by cancelling the process.
        /// </summary>
        ICommand CancelNavigationCommand { get; set; }

        /// <summary>
        /// Enables the animation of the content triggered by navigation.
        /// The default is true (enabled).
        /// </summary>
        bool ContentAnimationsEnabled { get; set; }

        /// <summary>
        /// A command called by navigating to the next <see cref="IStep" /> in a linear order.
        /// </summary>
        ICommand ContinueNavigationCommand { get; set; }

        /// <summary>
        /// Gets the controller for this <see cref="Stepper" />.
        /// </summary>
        StepperController Controller { get; }

        /// <summary>
        /// An alternative icon template done steps.
        /// </summary>
        DataTemplate DoneIconTemplate { get; set; }

        /// <summary>
        /// Enables the linear mode by disabling the buttons of the header.
        /// The navigation must be accomplished by using the navigation commands.
        /// </summary>
        bool IsLinear { get; set; }

        /// <summary>
        /// Defines the stepper as either horizontal or vertical.
        /// </summary>
        StepperLayout Layout { get; set; }

        /// <summary>
        /// A command called by navigating to an arbitrary <see cref="IStep" /> in a non-linear stepper.
        /// </summary>
        ICommand StepNavigationCommand { get; set; }

        /// <summary>
        /// An alternative icon template to indicate validation errors.
        /// </summary>
        DataTemplate ValidationErrorIconTemplate { get; set; }
    }
}
