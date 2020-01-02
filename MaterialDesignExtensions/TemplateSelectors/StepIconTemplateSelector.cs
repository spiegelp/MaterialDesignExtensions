using System.Windows;
using System.Windows.Controls;

using MaterialDesignExtensions.Controls;
using MaterialDesignExtensions.Model;

namespace MaterialDesignExtensions.TemplateSelectors
{
    /// <summary>
    /// A template selector for selecting an item for a step.
    /// </summary>
    public class StepIconTemplateSelector : DataTemplateSelector
    {
        /// <summary>
        /// The stepper with the steps to apply this icon template selector on.
        /// </summary>
        public IStepper Stepper { get; set; }

        /// <summary>
        /// Creates a new <see cref="StepIconTemplateSelector" />.
        /// </summary>
        public StepIconTemplateSelector() : this(null) { }

        /// <summary>
        /// Creates a new <see cref="StepIconTemplateSelector" />.
        /// </summary>
        /// <param name="stepper"></param>
        public StepIconTemplateSelector(IStepper stepper)
        {
            Stepper = stepper;
        }

        /// <summary>
        /// Selects a template for the icon of the specified step.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="container"></param>
        /// <returns></returns>
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (Stepper != null && item is StepperStepViewModel stepViewModel && container is FrameworkElement element)
            {
                return SelectTemplate(Stepper, element, stepViewModel);
            }
            else
            {
                return base.SelectTemplate(item, container);
            }
        }

        /// <summary>
        /// Selects a template for the icon of the specified step.
        /// </summary>
        /// <param name="stepper"></param>
        /// <param name="element"></param>
        /// <param name="stepViewModel"></param>
        /// <returns></returns>
        public DataTemplate SelectTemplate(IStepper stepper, FrameworkElement element, StepperStepViewModel stepViewModel)
        {
            if (stepper != null && stepViewModel != null && element != null)
            {
                if (stepViewModel.Step.IconTemplate != null)
                {
                    return stepViewModel.Step.IconTemplate;
                }
                else if (stepViewModel.Step.HasValidationErrors && stepper.ValidationErrorIconTemplate != null)
                {
                    return stepper.ValidationErrorIconTemplate;
                }
                else if (stepper.Controller.ActiveStepViewModel != null && stepper.Controller.ActiveStepViewModel.Number > stepViewModel.Number
                    && stepper.DoneIconTemplate != null)
                {
                    return stepper.DoneIconTemplate;
                } else {
                    return element.FindResource("MaterialDesignStepNumberIconTemplate") as DataTemplate;
                }
            }
            else
            {
                return base.SelectTemplate(stepViewModel, element);
            }
        }
    }
}
