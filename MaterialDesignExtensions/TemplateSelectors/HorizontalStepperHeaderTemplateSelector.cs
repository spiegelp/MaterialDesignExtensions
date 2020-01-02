using System.Windows;
using System.Windows.Controls;

using MaterialDesignExtensions.Model;

namespace MaterialDesignExtensions.TemplateSelectors
{
    internal class HorizontalStepperHeaderTemplateSelector : DataTemplateSelector
    {
        public HorizontalStepperHeaderTemplateSelector() : base() { }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;
            StepperStepViewModel step = item as StepperStepViewModel;

            if (element != null && step != null)
            {
                if (step.IsFirstStep)
                {
                    return element.FindResource("firstHorizontalStepHeaderTemplate") as DataTemplate;
                }
                else if (step.IsLastStep)
                {
                    return element.FindResource("lastHorizontalStepHeaderTemplate") as DataTemplate;
                }
                else
                {
                    return element.FindResource("intermediateHorizontalStepHeaderTemplate") as DataTemplate;
                }
            }

            return null;
        }
    }
}
