using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialDesignExtensions.Controls
{
    /// <summary>
    /// An interface with basic members of a stepper.
    /// </summary>
    public interface IStepper
    {
        /// <summary>
        /// Enables the animation of the content triggered by navigation.
        /// The default is true (enabled).
        /// </summary>
        bool ContentAnimationsEnabled { get; set; }

        /// <summary>
        /// Enables the linear mode by disabling the buttons of the header.
        /// The navigation must be accomplished by using the navigation commands.
        /// </summary>
        bool IsLinear { get; set; }

        /// <summary>
        /// Defines the stepper as either horizontal or vertical.
        /// </summary>
        StepperLayout Layout { get; set; }
    }
}
