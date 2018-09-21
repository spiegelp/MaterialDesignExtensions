using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media.Animation;

using MaterialDesignExtensions.Controllers;
using MaterialDesignExtensions.Model;

namespace MaterialDesignExtensions.Controls
{
    /// <summary>
    /// A control which implements the Stepper of the Material design specification (https://material.io/archive/guidelines/components/steppers.html).
    /// </summary>
    public class TabControlStepper : TabControl, IStepper
    {
        /// <summary>
        /// Internal command used by the XAML template (public to be available in the XAML template). Not intended for external usage.
        /// </summary>
        public static readonly RoutedCommand BackCommand = new RoutedCommand();

        /// <summary>
        /// Internal command used by the XAML template (public to be available in the XAML template). Not intended for external usage.
        /// </summary>
        public static readonly RoutedCommand CancelCommand = new RoutedCommand();

        /// <summary>
        /// Internal command used by the XAML template (public to be available in the XAML template). Not intended for external usage.
        /// </summary>
        public static readonly RoutedCommand ContinueCommand = new RoutedCommand();

        /// <summary>
        /// Internal command used by the XAML template (public to be available in the XAML template). Not intended for external usage.
        /// </summary>
        public static readonly RoutedCommand StepSelectedCommand = new RoutedCommand();

        /// <summary>
        /// An event raised by navigating to the previous <see cref="IStep"/> in a linear order.
        /// </summary>
        public static readonly RoutedEvent BackNavigationEvent = EventManager.RegisterRoutedEvent(
            nameof(BackNavigation), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TabControlStepper));

        /// <summary>
        /// An event raised by navigating to the previous <see cref="IStep"/> in a linear order.
        /// </summary>
        public event RoutedEventHandler BackNavigation
        {
            add
            {
                AddHandler(BackNavigationEvent, value);
            }

            remove
            {
                RemoveHandler(BackNavigationEvent, value);
            }
        }

        /// <summary>
        /// An event raised by cancelling the process.
        /// </summary>
        public static readonly RoutedEvent CancelNavigationEvent = EventManager.RegisterRoutedEvent(
            nameof(CancelNavigation), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TabControlStepper));

        /// <summary>
        /// An event raised by cancelling the process.
        /// </summary>
        public event RoutedEventHandler CancelNavigation
        {
            add
            {
                AddHandler(CancelNavigationEvent, value);
            }

            remove
            {
                RemoveHandler(CancelNavigationEvent, value);
            }
        }

        /// <summary>
        /// An event raised by navigating to the next <see cref="IStep"/> in a linear order.
        /// </summary>
        public static readonly RoutedEvent ContinueNavigationEvent = EventManager.RegisterRoutedEvent(
            nameof(ContinueNavigation), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TabControlStepper));

        /// <summary>
        /// An event raised by navigating to the next <see cref="IStep"/> in a linear order.
        /// </summary>
        public event RoutedEventHandler ContinueNavigation
        {
            add
            {
                AddHandler(ContinueNavigationEvent, value);
            }

            remove
            {
                RemoveHandler(ContinueNavigationEvent, value);
            }
        }

        /// <summary>
        /// An event raised by navigating to an arbitrary <see cref="IStep"/> in a non-linear <see cref="TabControlStepper"/>.
        /// </summary>
        public static readonly RoutedEvent StepNavigationEvent = EventManager.RegisterRoutedEvent(
            nameof(StepNavigation), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TabControlStepper));

        /// <summary>
        /// An event raised by navigating to an arbitrary <see cref="IStep"/> in a non-linear <see cref="TabControlStepper"/>.
        /// </summary>
        public event RoutedEventHandler StepNavigation
        {
            add
            {
                AddHandler(StepNavigationEvent, value);
            }

            remove
            {
                RemoveHandler(StepNavigationEvent, value);
            }
        }

        /// <summary>
        /// Enables the animation of the content triggered by navigation.
        /// The default is true (enabled).
        /// </summary>
        public static readonly DependencyProperty ContentAnimationsEnabledProperty = DependencyProperty.Register(
                nameof(ContentAnimationsEnabled), typeof(bool), typeof(TabControlStepper), new PropertyMetadata(true));

        /// <summary>
        /// Enables the animation of the content triggered by navigation.
        /// The default is true (enabled).
        /// </summary>
        public bool ContentAnimationsEnabled
        {
            get
            {
                return (bool)GetValue(ContentAnimationsEnabledProperty);
            }

            set
            {
                SetValue(ContentAnimationsEnabledProperty, value);
            }
        }

        /// <summary>
        /// Enables the linear mode by disabling the buttons of the header.
        /// The navigation must be accomplished by using the navigation commands.
        /// </summary>
        public static readonly DependencyProperty IsLinearProperty = DependencyProperty.Register(
                nameof(IsLinear), typeof(bool), typeof(TabControlStepper), new PropertyMetadata(false));

        /// <summary>
        /// Enables the linear mode by disabling the buttons of the header.
        /// The navigation must be accomplished by using the navigation commands.
        /// </summary>
        public bool IsLinear
        {
            get
            {
                return (bool)GetValue(IsLinearProperty);
            }

            set
            {
                SetValue(IsLinearProperty, value);
            }
        }

        /// <summary>
        /// Defines this <see cref="TabControlStepper" /> as either horizontal or vertical.
        /// </summary>
        public static readonly DependencyProperty LayoutProperty = DependencyProperty.Register(
                nameof(Layout), typeof(StepperLayout), typeof(TabControlStepper), new PropertyMetadata(StepperLayout.Horizontal));

        /// <summary>
        /// Defines this <see cref="TabControlStepper" /> as either horizontal or vertical.
        /// </summary>
        public StepperLayout Layout
        {
            get
            {
                return (StepperLayout)GetValue(LayoutProperty);
            }

            set
            {
                SetValue(LayoutProperty, value);
            }
        }

        /// <summary>
        /// Gets the controller for this <see cref="TabControlStepper" />.
        /// Must to be public for the bindings.
        /// </summary>
        public StepperController Controller
        {
            get
            {
                return _controller;
            }
        }

        private StepperController _controller;

        static TabControlStepper()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TabControlStepper), new FrameworkPropertyMetadata(typeof(TabControlStepper)));
        }

        /// <summary>
        /// Creates a new <see cref="TabControlStepper" />.
        /// </summary>
        public TabControlStepper()
            : base()
        {
            _controller = new StepperController();

            Loaded += LoadedHandler;
            Unloaded += UnloadedHandler;

            CommandBindings.Add(new CommandBinding(BackCommand, BackHandler));
            CommandBindings.Add(new CommandBinding(CancelCommand, CancelHandler));
            CommandBindings.Add(new CommandBinding(ContinueCommand, ContinueHandler));
            CommandBindings.Add(new CommandBinding(StepSelectedCommand, StepSelectedHandler, CanExecuteStepSelectedHandler));
        }

        private void LoadedHandler(object sender, RoutedEventArgs args)
        {
            _controller.PropertyChanged += PropertyChangedHandler;

            // there is no event raised if the Content of a ContentControl changes
            //     therefore trigger the animation in code
            PlayHorizontalContentAnimation();
        }

        private void UnloadedHandler(object sender, RoutedEventArgs args)
        {
            _controller.PropertyChanged -= PropertyChangedHandler;
        }

        private void InitSteps()
        {
            List<IStep> steps = new List<IStep>();

            foreach (TabItem tabItem in Items)
            {
                steps.Add(new Step() { Header = tabItem.Header, Content = tabItem.Content });
            }

            _controller.InitSteps(steps);
        }

        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs args)
        {
            base.OnItemsChanged(args);

            InitSteps();
        }

        private void BackHandler(object sender, ExecutedRoutedEventArgs args)
        {
            StepperNavigationEventArgs navigationArgs = new StepperNavigationEventArgs(BackNavigationEvent, this, _controller.ActiveStep, _controller.PreviousStep, false);
            RaiseEvent(navigationArgs);

            if (!navigationArgs.Cancel)
            {
                _controller.Back();
            }
        }

        private void CancelHandler(object sender, ExecutedRoutedEventArgs args)
        {
            if (args.Handled)
            {
                return;
            }

            StepperNavigationEventArgs navigationArgs = new StepperNavigationEventArgs(CancelNavigationEvent, this, _controller.ActiveStep, null, false);
            RaiseEvent(navigationArgs);
        }

        private void ContinueHandler(object sender, ExecutedRoutedEventArgs args)
        {
            StepperNavigationEventArgs navigationArgs = new StepperNavigationEventArgs(ContinueNavigationEvent, this, _controller.ActiveStep, _controller.NextStep, false);
            RaiseEvent(navigationArgs);

            if (!navigationArgs.Cancel)
            {
                _controller.Continue();
            }
        }

        private void CanExecuteStepSelectedHandler(object sender, CanExecuteRoutedEventArgs args)
        {
            args.CanExecute = !IsLinear;
        }

        private void StepSelectedHandler(object sender, ExecutedRoutedEventArgs args)
        {
            if (!IsLinear)
            {
                StepperNavigationEventArgs navigationArgs = new StepperNavigationEventArgs(StepNavigationEvent, this, _controller.ActiveStep, ((StepperStepViewModel)args.Parameter).Step, false);
                RaiseEvent(navigationArgs);

                if (!navigationArgs.Cancel)
                {
                    _controller.GotoStep((StepperStepViewModel)args.Parameter);
                }
            }
        }

        private void PropertyChangedHandler(object sender, PropertyChangedEventArgs args)
        {
            if (sender == _controller && args.PropertyName == nameof(_controller.ActiveStepContent)
                    && _controller.ActiveStepContent != null && Layout == StepperLayout.Horizontal)
            {
                // there is no event raised if the Content of a ContentControl changes
                //     therefore trigger the animation in code
                PlayHorizontalContentAnimation();
            }
        }

        private void PlayHorizontalContentAnimation()
        {
            if (ContentAnimationsEnabled)
            {
                // there is no event raised if the Content of a ContentControl changes
                //     therefore trigger the animation in code
                if (Layout == StepperLayout.Horizontal)
                {
                    Storyboard storyboard = (Storyboard)TryFindResource("horizontalContentChangedStoryboard");
                    FrameworkElement element = GetTemplateChild("PART_horizontalContent") as FrameworkElement;

                    if (storyboard != null && element != null)
                    {
                        storyboard.Begin(element);
                    }
                }
            }
        }
    }
}
