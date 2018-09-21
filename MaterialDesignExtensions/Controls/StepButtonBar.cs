using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MaterialDesignExtensions.Controls
{
    /// <summary>
    /// Convenience control for the navigation buttons inside a <see cref="Stepper"/>.
    /// Setting the content for a button will show it. Otherwise the button will be collapsed.
    /// </summary>
    public class StepButtonBar : Control
    {
        /// <summary>
        /// The content for the back button.
        /// </summary>
        public static readonly DependencyProperty BackProperty = DependencyProperty.Register(
                nameof(Back), typeof(object), typeof(StepButtonBar), new PropertyMetadata(null));

        /// <summary>
        /// The content for the back button.
        /// </summary>
        public object Back
        {
            get
            {
                return GetValue(BackProperty);
            }

            set
            {
                SetValue(BackProperty, value);
            }
        }
        /// <summary>
        /// The interal back command of the parent stepper.
        /// </summary>
        public static readonly DependencyProperty BackCommandProperty = DependencyProperty.Register(
            nameof(BackCommand), typeof(ICommand), typeof(StepButtonBar), new PropertyMetadata(null, null));

        /// <summary>
        /// The interal back command of the parent stepper.
        /// </summary>
        public ICommand BackCommand
        {
            get
            {
                return (ICommand)GetValue(BackCommandProperty);
            }

            set
            {
                SetValue(BackCommandProperty, value);
            }
        }

        /// <summary>
        /// The content for the cancel button.
        /// </summary>
        public static readonly DependencyProperty CancelProperty = DependencyProperty.Register(
                nameof(Cancel), typeof(object), typeof(StepButtonBar), new PropertyMetadata(null));

        /// <summary>
        /// The content for the cancel button.
        /// </summary>
        public object Cancel
        {
            get
            {
                return GetValue(CancelProperty);
            }

            set
            {
                SetValue(CancelProperty, value);
            }
        }
        /// <summary>
        /// The interal cancel command of the parent stepper.
        /// </summary>
        public static readonly DependencyProperty CancelCommandProperty = DependencyProperty.Register(
            nameof(CancelCommand), typeof(ICommand), typeof(StepButtonBar), new PropertyMetadata(null, null));

        /// <summary>
        /// The interal cancel command of the parent stepper.
        /// </summary>
        public ICommand CancelCommand
        {
            get
            {
                return (ICommand)GetValue(CancelCommandProperty);
            }

            set
            {
                SetValue(CancelCommandProperty, value);
            }
        }

        /// <summary>
        /// The content for the continue button.
        /// </summary>
        public static readonly DependencyProperty ContinueProperty = DependencyProperty.Register(
                nameof(Continue), typeof(object), typeof(StepButtonBar), new PropertyMetadata(null));

        /// <summary>
        /// The content for the continue button.
        /// </summary>
        public object Continue
        {
            get
            {
                return GetValue(ContinueProperty);
            }

            set
            {
                SetValue(ContinueProperty, value);
            }
        }

        /// <summary>
        /// The interal continue command of the parent stepper.
        /// </summary>
        public static readonly DependencyProperty ContinueCommandProperty = DependencyProperty.Register(
            nameof(ContinueCommand), typeof(ICommand), typeof(StepButtonBar), new PropertyMetadata(null, null));

        /// <summary>
        /// The interal continue command of the parent stepper.
        /// </summary>
        public ICommand ContinueCommand
        {
            get
            {
                return (ICommand)GetValue(ContinueCommandProperty);
            }

            set
            {
                SetValue(ContinueCommandProperty, value);
            }
        }

        /// <summary>
        /// Enables or disables the back button.
        /// </summary>
        public static readonly DependencyProperty IsBackEnabledProperty = DependencyProperty.Register(
                nameof(IsBackEnabled), typeof(bool), typeof(StepButtonBar), new PropertyMetadata(true));

        /// <summary>
        /// Enables or disables the back button.
        /// </summary>
        public bool IsBackEnabled
        {
            get
            {
                return (bool)GetValue(IsBackEnabledProperty);
            }

            set
            {
                SetValue(IsBackEnabledProperty, value);
            }
        }

        /// <summary>
        /// Enables or disables the cancel button.
        /// </summary>
        public static readonly DependencyProperty IsCancelEnabledProperty = DependencyProperty.Register(
                nameof(IsCancelEnabled), typeof(bool), typeof(StepButtonBar), new PropertyMetadata(true));

        /// <summary>
        /// Enables or disables the cancel button.
        /// </summary>
        public bool IsCancelEnabled
        {
            get
            {
                return (bool)GetValue(IsCancelEnabledProperty);
            }

            set
            {
                SetValue(IsCancelEnabledProperty, value);
            }
        }

        /// <summary>
        /// Enables or disables the continue button.
        /// </summary>
        public static readonly DependencyProperty IsContinueEnabledProperty = DependencyProperty.Register(
                nameof(IsContinueEnabled), typeof(bool), typeof(StepButtonBar), new PropertyMetadata(true));

        /// <summary>
        /// Enables or disables the continue button.
        /// </summary>
        public bool IsContinueEnabled
        {
            get
            {
                return (bool)GetValue(IsContinueEnabledProperty);
            }

            set
            {
                SetValue(IsContinueEnabledProperty, value);
            }
        }

        internal static readonly DependencyProperty ModeProperty = DependencyProperty.Register(
                nameof(Mode), typeof(StepperLayout), typeof(StepButtonBar), new PropertyMetadata(StepperLayout.Horizontal));

        internal StepperLayout Mode
        {
            get
            {
                return (StepperLayout)GetValue(ModeProperty);
            }

            set
            {
                SetValue(ModeProperty, value);
            }
        }

        static StepButtonBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(StepButtonBar), new FrameworkPropertyMetadata(typeof(StepButtonBar)));
        }

        public StepButtonBar() : base() { }

        public override void OnApplyTemplate()
        {
            // read the Orientation of the Stepper and set it as the Mode
            //     - changing the Layout throws the UI of the Stepper and builds a new one
            //     - therefore this method will be called for a new instance and the changes of the Layout will be applied to Mode
            IStepper stepper = FindStepper();

            if (stepper != null)
            {
                Mode = stepper.Layout;

                if (stepper is TabControlStepper)
                {
                    BackCommand = TabControlStepper.BackCommand;
                    CancelCommand = TabControlStepper.CancelCommand;
                    ContinueCommand = TabControlStepper.ContinueCommand;
                }
                else
                {
                    BackCommand = Stepper.BackCommand;
                    CancelCommand = Stepper.CancelCommand;
                    ContinueCommand = Stepper.ContinueCommand;
                }
            }

            base.OnApplyTemplate();
        }

        private IStepper FindStepper()
        {
            DependencyObject element = VisualTreeHelper.GetParent(this);

            while (element != null && !(element is IStepper))
            {
                element = VisualTreeHelper.GetParent(element);
            }

            return element as IStepper;
        }
    }
}
