using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

using MaterialDesignExtensions.Controls;
using MaterialDesignExtensions.Model;
using MaterialDesignExtensions.TemplateSelectors;

namespace MaterialDesignExtensions.Converters
{
    /// <summary>
    /// A converter triggering a <see cref="StepIconTemplateSelector" /> for selecting an item for a step.
    /// This converter is necessary to react on changing data of the stepper and steps to call the selector logic again.
    /// </summary>
    public class StepIconTemplateConverter : IMultiValueConverter
    {
        private StepIconTemplateSelector _iconTemplateSelector;

        /// <summary>
        /// Creates a new <see cref="StepIconTemplateConverter" />.
        /// </summary>
        public StepIconTemplateConverter()
        {
            _iconTemplateSelector = new StepIconTemplateSelector();
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            IStepper stepper = values[0] as IStepper;
            FrameworkElement element = values[1] as FrameworkElement;
            StepperStepViewModel stepViewModel = values[2] as StepperStepViewModel;

            return _iconTemplateSelector.SelectTemplate(stepper, element, stepViewModel);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
