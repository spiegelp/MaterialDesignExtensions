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

using MaterialDesignExtensions.Commands.Internal;
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
        /// An event raised by navigating to the previous <see cref="IStep" /> in a linear order.
        /// </summary>
        public static readonly RoutedEvent BackNavigationEvent = EventManager.RegisterRoutedEvent(
            nameof(BackNavigation), RoutingStrategy.Bubble, typeof(StepperNavigationEventHandler), typeof(TabControlStepper));

        /// <summary>
        /// An event raised by navigating to the previous <see cref="IStep" /> in a linear order.
        /// </summary>
        public event StepperNavigationEventHandler BackNavigation
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
            nameof(CancelNavigation), RoutingStrategy.Bubble, typeof(StepperNavigationEventHandler), typeof(TabControlStepper));

        /// <summary>
        /// An event raised by cancelling the process.
        /// </summary>
        public event StepperNavigationEventHandler CancelNavigation
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
        /// An event raised by navigating to the next <see cref="IStep" /> in a linear order.
        /// </summary>
        public static readonly RoutedEvent ContinueNavigationEvent = EventManager.RegisterRoutedEvent(
            nameof(ContinueNavigation), RoutingStrategy.Bubble, typeof(StepperNavigationEventHandler), typeof(TabControlStepper));

        /// <summary>
        /// An event raised by navigating to the next <see cref="IStep" /> in a linear order.
        /// </summary>
        public event StepperNavigationEventHandler ContinueNavigation
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
        /// An event raised by navigating to an arbitrary <see cref="IStep" /> in a non-linear <see cref="TabControlStepper" />.
        /// </summary>
        public static readonly RoutedEvent StepNavigationEvent = EventManager.RegisterRoutedEvent(
            nameof(StepNavigation), RoutingStrategy.Bubble, typeof(StepperNavigationEventHandler), typeof(TabControlStepper));

        /// <summary>
        /// An event raised by navigating to an arbitrary <see cref="IStep" /> in a non-linear <see cref="TabControlStepper" />.
        /// </summary>
        public event StepperNavigationEventHandler StepNavigation
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
        /// A command called by navigating to the previous <see cref="IStep" /> in a linear order.
        /// </summary>
        public static readonly DependencyProperty BackNavigationCommandProperty = DependencyProperty.Register(
            nameof(BackNavigationCommand), typeof(ICommand), typeof(TabControlStepper), new PropertyMetadata(null, null));

        /// <summary>
        /// A command called by navigating to the previous <see cref="IStep" /> in a linear order.
        /// </summary>
        public ICommand BackNavigationCommand
        {
            get
            {
                return (ICommand)GetValue(BackNavigationCommandProperty);
            }

            set
            {
                SetValue(BackNavigationCommandProperty, value);
            }
        }

        /// <summary>
        /// A command called by cancelling the process.
        /// </summary>
        public static readonly DependencyProperty CancelNavigationCommandProperty = DependencyProperty.Register(
            nameof(CancelNavigationCommand), typeof(ICommand), typeof(TabControlStepper), new PropertyMetadata(null, null));

        /// <summary>
        /// A command called by cancelling the process.
        /// </summary>
        public ICommand CancelNavigationCommand
        {
            get
            {
                return (ICommand)GetValue(CancelNavigationCommandProperty);
            }

            set
            {
                SetValue(CancelNavigationCommandProperty, value);
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
        /// A command called by navigating to the next <see cref="IStep" /> in a linear order.
        /// </summary>
        public static readonly DependencyProperty ContinueNavigationCommandProperty = DependencyProperty.Register(
            nameof(ContinueNavigationCommand), typeof(ICommand), typeof(TabControlStepper), new PropertyMetadata(null, null));

        /// <summary>
        /// A command called by navigating to the next <see cref="IStep" /> in a linear order.
        /// </summary>
        public ICommand ContinueNavigationCommand
        {
            get
            {
                return (ICommand)GetValue(ContinueNavigationCommandProperty);
            }

            set
            {
                SetValue(ContinueNavigationCommandProperty, value);
            }
        }

        /// <summary>
        /// An alternative icon template done steps.
        /// </summary>
        public static readonly DependencyProperty DoneIconTemplateProperty = DependencyProperty.Register(
            nameof(DoneIconTemplate), typeof(DataTemplate), typeof(TabControlStepper), new PropertyMetadata(null, null));

        /// <summary>
        /// An alternative icon template done steps.
        /// </summary>
        public DataTemplate DoneIconTemplate
        {
            get
            {
                return (DataTemplate)GetValue(DoneIconTemplateProperty);
            }

            set
            {
                SetValue(DoneIconTemplateProperty, value);
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
        /// A command called by navigating to an arbitrary <see cref="IStep" /> in a non-linear <see cref="TabControlStepper" />.
        /// </summary>
        public static readonly DependencyProperty StepNavigationCommandProperty = DependencyProperty.Register(
            nameof(StepNavigationCommand), typeof(ICommand), typeof(TabControlStepper), new PropertyMetadata(null, null));

        /// <summary>
        /// A command called by navigating to an arbitrary <see cref="IStep" /> in a non-linear <see cref="TabControlStepper" />.
        /// </summary>
        public ICommand StepNavigationCommand
        {
            get
            {
                return (ICommand)GetValue(StepNavigationCommandProperty);
            }

            set
            {
                SetValue(StepNavigationCommandProperty, value);
            }
        }

        /// <summary>
        /// An alternative icon template to indicate validation errors.
        /// </summary>
        public static readonly DependencyProperty ValidationErrorIconTemplateProperty = DependencyProperty.Register(
            nameof(ValidationErrorIconTemplate), typeof(DataTemplate), typeof(TabControlStepper), new PropertyMetadata(null, null));

        /// <summary>
        /// An alternative icon template to indicate validation errors.
        /// </summary>
        public DataTemplate ValidationErrorIconTemplate
        {
            get
            {
                return (DataTemplate)GetValue(ValidationErrorIconTemplateProperty);
            }

            set
            {
                SetValue(ValidationErrorIconTemplateProperty, value);
            }
        }

        /// <summary>
        /// Gets the controller for this <see cref="TabControlStepper" />.
        /// Must to be public for the bindings.
        /// </summary>
        public StepperController Controller { get; private set; }

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
            Controller = new StepperController();

            Loaded += LoadedHandler;
            Unloaded += UnloadedHandler;

            CommandBindings.Add(new CommandBinding(StepperCommands.BackCommand, BackHandler));
            CommandBindings.Add(new CommandBinding(StepperCommands.CancelCommand, CancelHandler));
            CommandBindings.Add(new CommandBinding(StepperCommands.ContinueCommand, ContinueHandler));
            CommandBindings.Add(new CommandBinding(StepperCommands.StepSelectedCommand, StepSelectedHandler, CanExecuteStepSelectedHandler));
        }

        private void LoadedHandler(object sender, RoutedEventArgs args)
        {
            Controller.PropertyChanged += PropertyChangedHandler;

            // there is no event raised if the Content of a ContentControl changes
            //     therefore trigger the animation in code
            PlayHorizontalContentAnimation();
        }

        private void UnloadedHandler(object sender, RoutedEventArgs args)
        {
            Controller.PropertyChanged -= PropertyChangedHandler;
        }

        private void InitSteps()
        {
            List<IStep> steps = new List<IStep>();

            foreach (TabItem tabItem in Items)
            {
                steps.Add(new Step() { Header = tabItem.Header, Content = tabItem.Content });
            }

            Controller.InitSteps(steps);
        }

        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs args)
        {
            base.OnItemsChanged(args);

            InitSteps();
        }

        private void BackHandler(object sender, ExecutedRoutedEventArgs args)
        {
            StepperNavigationEventArgs navigationArgs = new StepperNavigationEventArgs(BackNavigationEvent, this, Controller.ActiveStep, Controller.PreviousStep, false);
            RaiseEvent(navigationArgs);

            if (BackNavigationCommand != null && BackNavigationCommand.CanExecute(navigationArgs))
            {
                BackNavigationCommand.Execute(navigationArgs);
            }

            if (!navigationArgs.Cancel)
            {
                Controller.Back();
            }
        }

        private void CancelHandler(object sender, ExecutedRoutedEventArgs args)
        {
            if (args.Handled)
            {
                return;
            }

            StepperNavigationEventArgs navigationArgs = new StepperNavigationEventArgs(CancelNavigationEvent, this, Controller.ActiveStep, null, false);
            RaiseEvent(navigationArgs);

            if (CancelNavigationCommand != null && CancelNavigationCommand.CanExecute(navigationArgs))
            {
                CancelNavigationCommand.Execute(navigationArgs);
            }
        }

        private void ContinueHandler(object sender, ExecutedRoutedEventArgs args)
        {
            StepperNavigationEventArgs navigationArgs = new StepperNavigationEventArgs(ContinueNavigationEvent, this, Controller.ActiveStep, Controller.NextStep, false);
            RaiseEvent(navigationArgs);

            if (ContinueNavigationCommand != null && ContinueNavigationCommand.CanExecute(navigationArgs))
            {
                ContinueNavigationCommand.Execute(navigationArgs);
            }

            if (!navigationArgs.Cancel)
            {
                Controller.Continue();
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
                StepperNavigationEventArgs navigationArgs = new StepperNavigationEventArgs(StepNavigationEvent, this, Controller.ActiveStep, ((StepperStepViewModel)args.Parameter).Step, false);
                RaiseEvent(navigationArgs);

                if (StepNavigationCommand != null && StepNavigationCommand.CanExecute(navigationArgs))
                {
                    StepNavigationCommand.Execute(navigationArgs);
                }

                if (!navigationArgs.Cancel)
                {
                    Controller.GotoStep((StepperStepViewModel)args.Parameter);
                }
            }
        }

        private void PropertyChangedHandler(object sender, PropertyChangedEventArgs args)
        {
            if (sender == Controller && args.PropertyName == nameof(Controller.ActiveStepContent)
                    && Controller.ActiveStepContent != null && Layout == StepperLayout.Horizontal)
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
