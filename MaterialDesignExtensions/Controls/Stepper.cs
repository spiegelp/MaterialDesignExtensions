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
    /// A control which implements the Stepper of the Material design specification (https://material.google.com/components/steppers.html).
    /// </summary>
    [ContentProperty(nameof(Steps))]
    public class Stepper : Control
    {
        /// <summary>
        /// Internal command used by the XAML template (public to be available in the XAML template). Not intended for external usage.
        /// </summary>
        public static RoutedCommand BackCommand = new RoutedCommand();

        /// <summary>
        /// Internal command used by the XAML template (public to be available in the XAML template). Not intended for external usage.
        /// </summary>
        public static RoutedCommand CancelCommand = new RoutedCommand();

        /// <summary>
        /// Internal command used by the XAML template (public to be available in the XAML template). Not intended for external usage.
        /// </summary>
        public static RoutedCommand ContinueCommand = new RoutedCommand();

        /// <summary>
        /// Internal command used by the XAML template (public to be available in the XAML template). Not intended for external usage.
        /// </summary>
        public static RoutedCommand StepSelectedCommand = new RoutedCommand();

        /// <summary>
        /// An event raised by changing to active <see cref="IStep" />.
        /// </summary>
        public static readonly RoutedEvent ActiveStepChangedEvent = EventManager.RegisterRoutedEvent(
            nameof(ActiveStepChanged), RoutingStrategy.Bubble, typeof(ActiveStepChangedEventHandler), typeof(Stepper));

        /// <summary>
        /// An event raised by changing to active <see cref="IStep" />.
        /// </summary>
        public event ActiveStepChangedEventHandler ActiveStepChanged
        {
            add
            {
                AddHandler(ActiveStepChangedEvent, value);
            }

            remove
            {
                RemoveHandler(ActiveStepChangedEvent, value);
            }
        }

        /// <summary>
        /// An event raised by navigating to the previous <see cref="IStep" /> in a linear order.
        /// </summary>
        public static readonly RoutedEvent BackNavigationEvent = EventManager.RegisterRoutedEvent(
            nameof(BackNavigation), RoutingStrategy.Bubble, typeof(StepperNavigationEventHandler), typeof(Stepper));

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
            nameof(CancelNavigation), RoutingStrategy.Bubble, typeof(StepperNavigationEventHandler), typeof(Stepper));

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
            nameof(ContinueNavigation), RoutingStrategy.Bubble, typeof(StepperNavigationEventHandler), typeof(Stepper));

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
        /// An event raised if the navigation was canceled because of validation errors of the current <see cref="IStep" />.
        /// </summary>
        public static readonly RoutedEvent NavigationCanceledByValidationEvent = EventManager.RegisterRoutedEvent(
            nameof(NavigationCanceledByValidation), RoutingStrategy.Bubble, typeof(NavigationCanceledByValidationEventHandler), typeof(Stepper));

        /// <summary>
        /// An event raised if the navigation was canceled because of validation errors of the current <see cref="IStep" />.
        /// </summary>
        public event NavigationCanceledByValidationEventHandler NavigationCanceledByValidation
        {
            add
            {
                AddHandler(NavigationCanceledByValidationEvent, value);
            }

            remove
            {
                RemoveHandler(NavigationCanceledByValidationEvent, value);
            }
        }

        /// <summary>
        /// An event raised by navigating to an arbitrary <see cref="IStep" /> in a non-linear <see cref="Stepper" />.
        /// </summary>
        public static readonly RoutedEvent StepNavigationEvent = EventManager.RegisterRoutedEvent(
            nameof(StepNavigation), RoutingStrategy.Bubble, typeof(StepperNavigationEventHandler), typeof(Stepper));

        /// <summary>
        /// An event raised by navigating to an arbitrary <see cref="IStep" /> in a non-linear <see cref="Stepper" />.
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
        /// An event raised by starting the validation of an <see cref="IStep" />.
        /// </summary>
        public static readonly RoutedEvent StepValidationEvent = EventManager.RegisterRoutedEvent(
            nameof(StepValidation), RoutingStrategy.Bubble, typeof(StepValidationEventHandler), typeof(Stepper));

        /// <summary>
        /// An event raised by starting the validation of an <see cref="IStep" />.
        /// </summary>
        public event StepValidationEventHandler StepValidation
        {
            add
            {
                AddHandler(StepValidationEvent, value);
            }

            remove
            {
                RemoveHandler(StepValidationEvent, value);
            }
        }

        /// <summary>
        /// The active <see cref="IStep" />.
        /// </summary>
        public static readonly DependencyProperty ActiveStepProperty = DependencyProperty.Register(
                nameof(ActiveStep), typeof(IStep), typeof(Stepper), new PropertyMetadata(null, ActiveStepChangedHandler));

        /// <summary>
        /// The active <see cref="IStep" />.
        /// </summary>
        public IStep ActiveStep
        {
            get
            {
                return (IStep)GetValue(ActiveStepProperty);
            }

            set
            {
                SetValue(ActiveStepProperty, value);
            }
        }

        /// <summary>
        /// A command called by changing the active <see cref="IStep" />.
        /// </summary>
        public static readonly DependencyProperty ActiveStepChangedCommandProperty = DependencyProperty.Register(
            nameof(ActiveStepChangedCommand), typeof(ICommand), typeof(Stepper), new PropertyMetadata(null, null));

        /// <summary>
        /// A command called by changing the active <see cref="IStep" />.
        /// </summary>
        public ICommand ActiveStepChangedCommand
        {
            get
            {
                return (ICommand)GetValue(ActiveStepChangedCommandProperty);
            }

            set
            {
                SetValue(ActiveStepChangedCommandProperty, value);
            }
        }

        /// <summary>
        /// A command called by navigating to the previous <see cref="IStep" /> in a linear order.
        /// </summary>
        public static readonly DependencyProperty BackNavigationCommandProperty = DependencyProperty.Register(
            nameof(BackNavigationCommand), typeof(ICommand), typeof(Stepper), new PropertyMetadata(null, null));

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
        /// Specifies whether validation errors will block the navigation or not.
        /// </summary>
        public static readonly DependencyProperty BlockNavigationOnValidationErrorsProperty = DependencyProperty.Register(
                nameof(BlockNavigationOnValidationErrors), typeof(bool), typeof(Stepper), new PropertyMetadata(false));

        /// <summary>
        /// Specifies whether validation errors will block the navigation or not.
        /// </summary>
        public bool BlockNavigationOnValidationErrors
        {
            get
            {
                return (bool)GetValue(BlockNavigationOnValidationErrorsProperty);
            }

            set
            {
                SetValue(BlockNavigationOnValidationErrorsProperty, value);
            }
        }

        /// <summary>
        /// A command called by cancelling the process.
        /// </summary>
        public static readonly DependencyProperty CancelNavigationCommandProperty = DependencyProperty.Register(
            nameof(CancelNavigationCommand), typeof(ICommand), typeof(Stepper), new PropertyMetadata(null, null));

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
            nameof(ContentAnimationsEnabled), typeof(bool), typeof(Stepper), new PropertyMetadata(true, null));

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
            nameof(ContinueNavigationCommand), typeof(ICommand), typeof(Stepper), new PropertyMetadata(null, null));

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
        /// Enables the linear mode by disabling the buttons of the header.
        /// The navigation must be accomplished by using the navigation commands.
        /// </summary>
        public static readonly DependencyProperty IsLinearProperty = DependencyProperty.Register(
                nameof(IsLinear), typeof(bool), typeof(Stepper), new PropertyMetadata(false));

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
        /// Defines this <see cref="Stepper" /> as either horizontal or vertical.
        /// </summary>
        public static readonly DependencyProperty LayoutProperty = DependencyProperty.Register(
                nameof(Layout), typeof(StepperLayout), typeof(Stepper), new PropertyMetadata(StepperLayout.Horizontal));

        /// <summary>
        /// Defines this <see cref="Stepper" /> as either horizontal or vertical.
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
        /// A command called if the navigation was canceled because of validation errors of the current <see cref="IStep" />.
        /// </summary>
        public static readonly DependencyProperty NavigationCanceledByValidationCommandProperty = DependencyProperty.Register(
            nameof(NavigationCanceledByValidationCommand), typeof(ICommand), typeof(Stepper), new PropertyMetadata(null, null));

        /// <summary>
        /// A command called if the navigation was canceled because of validation errors of the current <see cref="IStep" />.
        /// </summary>
        public ICommand NavigationCanceledByValidationCommand
        {
            get
            {
                return (ICommand)GetValue(NavigationCanceledByValidationCommandProperty);
            }

            set
            {
                SetValue(NavigationCanceledByValidationCommandProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the steps which will be shown inside this <see cref="Stepper" />.
        /// </summary>
        public static readonly DependencyProperty StepsProperty = DependencyProperty.Register(
                nameof(Steps), typeof(IList), typeof(Stepper), new PropertyMetadata(null, StepsChangedHandler));

        /// <summary>
        /// Gets or sets the steps which will be shown inside this <see cref="Stepper" />.
        /// </summary>
        public IList Steps
        {
            get
            {
                return (IList)GetValue(StepsProperty);
            }

            set
            {
                SetValue(StepsProperty, value);
            }
        }

        /// <summary>
        /// A command called by navigating to an arbitrary <see cref="IStep" /> in a non-linear <see cref="Stepper" />.
        /// </summary>
        public static readonly DependencyProperty StepNavigationCommandProperty = DependencyProperty.Register(
            nameof(StepNavigationCommand), typeof(ICommand), typeof(Stepper), new PropertyMetadata(null, null));

        /// <summary>
        /// A command called by navigating to an arbitrary <see cref="IStep" /> in a non-linear <see cref="Stepper" />.
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
        /// A command called by starting the validation of an <see cref="IStep" />.
        /// </summary>
        public static readonly DependencyProperty StepValidationCommandProperty = DependencyProperty.Register(
            nameof(StepValidationCommand), typeof(ICommand), typeof(Stepper), new PropertyMetadata(null, null));

        /// <summary>
        /// A command called by starting the validation of an <see cref="IStep" />.
        /// </summary>
        public ICommand StepValidationCommand
        {
            get
            {
                return (ICommand)GetValue(StepValidationCommandProperty);
            }

            set
            {
                SetValue(StepValidationCommandProperty, value);
            }
        }

        /// <summary>
        /// Gets the controller for this <see cref="Stepper" />.
        /// </summary>
        public StepperController Controller
        {
            get
            {
                return m_controller;
            }
        }

        private StepperController m_controller;

        static Stepper()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Stepper), new FrameworkPropertyMetadata(typeof(Stepper)));
        }

        /// <summary>
        /// Creates a new <see cref="Stepper" />.
        /// </summary>
        public Stepper()
            : base()
        {
            m_controller = new StepperController();

            Steps = new ObservableCollection<IStep>();

            Loaded += LoadedHandler;
            Unloaded += UnloadedHandler;

            CommandBindings.Add(new CommandBinding(BackCommand, BackHandler));
            CommandBindings.Add(new CommandBinding(CancelCommand, CancelHandler));
            CommandBindings.Add(new CommandBinding(ContinueCommand, ContinueHandler));
            CommandBindings.Add(new CommandBinding(StepSelectedCommand, StepSelectedHandler, CanExecuteStepSelectedHandler));
        }

        private void LoadedHandler(object sender, RoutedEventArgs args)
        {
            m_controller.PropertyChanged += PropertyChangedHandler;

            if (Steps is ObservableCollection<IStep> steps)
            {
                steps.CollectionChanged -= StepsCollectionChanged;
                steps.CollectionChanged += StepsCollectionChanged;
            }

            InitSteps(Steps);

            // there is no event raised if the Content of a ContentControl changes
            //     therefore trigger the animation in code
            PlayHorizontalContentAnimation();
        }

        private void UnloadedHandler(object sender, RoutedEventArgs args)
        {
            m_controller.PropertyChanged -= PropertyChangedHandler;

            if (Steps is ObservableCollection<IStep> steps)
            {
                steps.CollectionChanged -= StepsCollectionChanged;
            }
        }

        private bool ValidateActiveStep()
        {
            IStep step = m_controller.ActiveStepViewModel?.Step;

            if (step != null)
            {
                // call the validation method on the step itself
                step.Validate();

                // raise the event and call the command
                StepValidationEventArgs eventArgs = new StepValidationEventArgs(StepValidationEvent, this, step);
                RaiseEvent(eventArgs);

                if (StepValidationCommand != null && StepValidationCommand.CanExecute(step))
                {
                    StepValidationCommand.Execute(step);
                }

                // the event handlers can set the validation state on the step
                return !step.HasValidationErrors;
            }
            else
            {
                // no active step to validate
                return true;
            }
        }

        private void RaiseNavigationCanceledByValidation()
        {
            IStep step = m_controller.ActiveStepViewModel?.Step;

            if (step != null)
            {
                NavigationCanceledByValidationEventArgs eventArgs = new NavigationCanceledByValidationEventArgs(NavigationCanceledByValidationEvent, this, step);
                RaiseEvent(eventArgs);

                if (NavigationCanceledByValidationCommand != null && NavigationCanceledByValidationCommand.CanExecute(step))
                {
                    NavigationCanceledByValidationCommand.Execute(step);
                }
            }
        }

        private void SelectStep(IStep step)
        {
            if (step != null && step != m_controller.ActiveStep && !IsLinear)
            {
                bool isValid = ValidateActiveStep();

                if (BlockNavigationOnValidationErrors && !isValid)
                {
                    RaiseNavigationCanceledByValidation();

                    return;
                }

                StepperNavigationEventArgs navigationArgs = new StepperNavigationEventArgs(StepNavigationEvent, this, m_controller.ActiveStep, step, false);
                RaiseEvent(navigationArgs);

                if (StepNavigationCommand != null && StepNavigationCommand.CanExecute(navigationArgs))
                {
                    StepNavigationCommand.Execute(navigationArgs);
                }

                if (!navigationArgs.Cancel)
                {
                    m_controller.GotoStep(step);
                }
                else
                {
                    // refresh the property with the old state
                    ActiveStep = m_controller.ActiveStep;
                }
            }
        }

        private static void ActiveStepChangedHandler(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            (obj as Stepper)?.ActiveStepChangedHandler(args.NewValue as IStep);
        }

        private void ActiveStepChangedHandler(IStep step)
        {
            SelectStep(step);
        }

        private static void StepsChangedHandler(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            (obj as Stepper)?.StepsChangedHandler(args.OldValue, args.NewValue);
        }

        private void StepsChangedHandler(object oldValue, object newValue)
        {
            if (oldValue is ObservableCollection<IStep> oldSteps)
            {
                oldSteps.CollectionChanged -= StepsCollectionChanged;
            }

            if (newValue is ObservableCollection<IStep> newSteps)
            {
                newSteps.CollectionChanged += StepsCollectionChanged;
            }

            InitSteps(newValue as IList);
        }

        private void StepsCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            InitSteps(Steps);
        }

        private void InitSteps(IList values)
        {
            IList<IStep> steps = new List<IStep>();

            if (values != null)
            {
                foreach (object item in values)
                {
                    if (item is IStep step)
                    {
                        steps.Add(step);
                    }
                }
            }

            m_controller.InitSteps(steps);
        }

        private void BackHandler(object sender, ExecutedRoutedEventArgs args)
        {
            bool isValid = ValidateActiveStep();

            if (BlockNavigationOnValidationErrors && !isValid)
            {
                RaiseNavigationCanceledByValidation();

                return;
            }

            StepperNavigationEventArgs navigationArgs = new StepperNavigationEventArgs(BackNavigationEvent, this, m_controller.ActiveStep, m_controller.PreviousStep, false);
            RaiseEvent(navigationArgs);

            if (BackNavigationCommand != null && BackNavigationCommand.CanExecute(navigationArgs))
            {
                BackNavigationCommand.Execute(navigationArgs);
            }

            if (!navigationArgs.Cancel)
            {
                m_controller.Back();
            }
        }

        private void CancelHandler(object sender, ExecutedRoutedEventArgs args)
        {
            if (args.Handled)
            {
                return;
            }

            StepperNavigationEventArgs navigationArgs = new StepperNavigationEventArgs(CancelNavigationEvent, this, m_controller.ActiveStep, null, false);
            RaiseEvent(navigationArgs);

            if (CancelNavigationCommand != null && CancelNavigationCommand.CanExecute(navigationArgs))
            {
                CancelNavigationCommand.Execute(navigationArgs);
            }
        }

        private void ContinueHandler(object sender, ExecutedRoutedEventArgs args)
        {
            bool isValid = ValidateActiveStep();

            if (BlockNavigationOnValidationErrors && !isValid)
            {
                RaiseNavigationCanceledByValidation();

                return;
            }

            StepperNavigationEventArgs navigationArgs = new StepperNavigationEventArgs(ContinueNavigationEvent, this, m_controller.ActiveStep, m_controller.NextStep, false);
            RaiseEvent(navigationArgs);

            if (ContinueNavigationCommand != null && ContinueNavigationCommand.CanExecute(navigationArgs))
            {
                ContinueNavigationCommand.Execute(navigationArgs);
            }

            if (!navigationArgs.Cancel)
            {
                m_controller.Continue();
            }
        }

        private void CanExecuteStepSelectedHandler(object sender, CanExecuteRoutedEventArgs args)
        {
            args.CanExecute = !IsLinear;
        }

        private void StepSelectedHandler(object sender, ExecutedRoutedEventArgs args)
        {
            SelectStep(((StepperStepViewModel)args.Parameter).Step);
        }

        private void PropertyChangedHandler(object sender, PropertyChangedEventArgs args)
        {
            if (sender == m_controller)
            {
                if (args.PropertyName == nameof(m_controller.ActiveStep))
                {
                    // set the property
                    ActiveStep = m_controller.ActiveStep;

                    // raise the event and call the command
                    ActiveStepChangedEventArgs eventArgs = new ActiveStepChangedEventArgs(StepValidationEvent, this, ActiveStep);
                    RaiseEvent(eventArgs);

                    if (ActiveStepChangedCommand != null && ActiveStepChangedCommand.CanExecute(ActiveStep))
                    {
                        ActiveStepChangedCommand.Execute(ActiveStep);
                    }
                }
                else if (args.PropertyName == nameof(m_controller.ActiveStepContent)
                    && m_controller.ActiveStepContent != null
                    && Layout == StepperLayout.Horizontal)
                {
                    // there is no event raised if the Content of a ContentControl changes
                    //     therefore trigger the animation in code
                    PlayHorizontalContentAnimation();
                }
            }
        }

        private void PlayHorizontalContentAnimation()
        {
            // there is no event raised if the Content of a ContentControl changes
            //     therefore trigger the animation in code
            if (ContentAnimationsEnabled && Layout == StepperLayout.Horizontal)
            {
                Storyboard storyboard = (Storyboard)FindResource("horizontalContentChangedStoryboard");
                FrameworkElement element = GetTemplateChild("PART_horizontalContent") as FrameworkElement;

                if (storyboard != null && element != null)
                {
                    storyboard.Begin(element);
                }
            }
        }

        /// <summary>
        /// Goes to the next <see cref="IStep"/> if the active <see cref="IStep"/> is not the last one.
        /// The behavior and validation logic will be bypassed.
        /// </summary>
        public void Continue()
        {
            m_controller.Continue();
        }

        /// <summary>
        /// Goes to the previous <see cref="IStep"/> if the active <see cref="IStep"/> is not the first one.
        /// The behavior and validation logic will be bypassed.
        /// </summary>
        public void Back()
        {
            m_controller.Back();
        }

        /// <summary>
        /// Goes to the <see cref="IStep"/> specified by the index.
        /// The behavior and validation logic will be bypassed.
        /// </summary>
        /// <param name="index"></param>
        public void GotoStep(int index)
        {
            m_controller.GotoStep(index);
        }

        /// <summary>
        /// Goes to the specified <see cref="IStep"/>.
        /// The behavior and validation logic will be bypassed.
        /// Throws an <see cref="ArgumentNullException"/> if step is null or step is not inside this <see cref="Stepper"/>.
        /// </summary>
        /// <param name="step"></param>
        public void GotoStep(IStep step)
        {
            m_controller.GotoStep(step);
        }
    }

    /// <summary>
    /// The layout of a <see cref="Stepper" />.
    /// </summary>
    public enum StepperLayout : byte
    {
        /// <summary>
        /// Horizontal stepper layout
        /// </summary>
        Horizontal,

        /// <summary>
        /// Vertical stepper layout
        /// </summary>
        Vertical
    }

    /// <summary>
    /// The argument for the <see cref="Stepper.ActiveStepChanged" /> event and the <see cref="Stepper.ActiveStepChangedCommand" /> command.
    /// It holds the new active <see cref="IStep" />.
    /// </summary>
    public class ActiveStepChangedEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// The new active <see cref="IStep" />.
        /// </summary>
        public IStep Step { get; protected set; }

        /// <summary>
        /// Creates a new <see cref="ActiveStepChangedEventArgs" />.
        /// </summary>
        /// <param name="routedEvent"></param>
        /// <param name="source"></param>
        /// <param name="step"></param>
        public ActiveStepChangedEventArgs(RoutedEvent routedEvent, object source, IStep step)
            : base(routedEvent, source)
        {
            Step = step;
        }
    }

    /// <summary>
    /// The delegate for handling the <see cref="Stepper.ActiveStepChanged" /> event.
    /// </summary>
    public delegate void ActiveStepChangedEventHandler(object sender, ActiveStepChangedEventArgs args);

    /// <summary>
    /// The argument for the <see cref="Stepper.StepValidation" /> event and the <see cref="Stepper.StepValidationCommand" /> command.
    /// It holds the <see cref="IStep" /> to validate.
    /// </summary>
    public class StepValidationEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// The <see cref="IStep" /> to validate.
        /// </summary>
        public IStep Step { get; protected set; }

        /// <summary>
        /// Creates a new <see cref="StepValidationEventArgs" />
        /// </summary>
        /// <param name="routedEvent"></param>
        /// <param name="source"></param>
        /// <param name="step"></param>
        public StepValidationEventArgs(RoutedEvent routedEvent, object source, IStep step)
            : base(routedEvent, source)
        {
            Step = step;
        }
    }

    /// <summary>
    /// The delegate for handling the <see cref="Stepper.StepValidation" /> event.
    /// </summary>
    public delegate void StepValidationEventHandler(object sender, StepValidationEventArgs args);

    /// <summary>
    /// The argument for the <see cref="Stepper.BackNavigation" />, <see cref="Stepper.ContinueNavigation" />, <see cref="Stepper.StepNavigation" /> and <see cref="Stepper.CancelNavigation" /> event.
    /// It holds the current <see cref="IStep" /> an the one to navigate to.
    /// The events are raised before the actal navigation and the navigation can be canceled by setting <see cref="Stepper.ContinueNavigation" /> to false.
    /// </summary>
    public class StepperNavigationEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// The current <see cref="IStep" /> of the <see cref="Stepper" />.
        /// </summary>
        public IStep CurrentStep { get; protected set; }

        /// <summary>
        /// The next <see cref="IStep" /> to navigate to.
        /// </summary>
        public IStep NextStep { get; protected set; }

        /// <summary>
        /// A flag to cancel the navigation by setting it to false.
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// Creates a new <see cref="StepperNavigationEventArgs" />.
        /// </summary>
        /// <param name="routedEvent"></param>
        /// <param name="source"></param>
        /// <param name="currentStep"></param>
        /// <param name="nextStep"></param>
        /// <param name="cancel"></param>
        public StepperNavigationEventArgs(RoutedEvent routedEvent, object source, IStep currentStep, IStep nextStep, bool cancel)
            : base(routedEvent, source)
        {
            CurrentStep = currentStep;
            NextStep = nextStep;
            Cancel = cancel;
        }
    }

    /// <summary>
    /// The delegate for handling the <see cref="Stepper.BackNavigation" />, <see cref="Stepper.ContinueNavigation" />, <see cref="Stepper.StepNavigation" /> and <see cref="Stepper.CancelNavigation" /> event.
    /// </summary>
    public delegate void StepperNavigationEventHandler(object sender, StepperNavigationEventArgs args);

    /// <summary>
    /// The argument for the <see cref="Stepper.NavigationCanceledByValidation" /> event and the <see cref="Stepper.NavigationCanceledByValidationCommand" /> command.
    /// It holds the active <see cref="IStep" /> which has validation errors.
    /// </summary>
    public class NavigationCanceledByValidationEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// The active <see cref="IStep" /> which has validation errors.
        /// </summary>
        public IStep StepWithValidationErrors { get; protected set; }

        /// <summary>
        /// Creates a new <see cref="NavigationCanceledByValidationEventArgs" />.
        /// </summary>
        /// <param name="routedEvent"></param>
        /// <param name="source"></param>
        /// <param name="stepWithValidationErrors"></param>
        public NavigationCanceledByValidationEventArgs(RoutedEvent routedEvent, object source, IStep stepWithValidationErrors)
            : base(routedEvent, source)
        {
            StepWithValidationErrors = stepWithValidationErrors;
        }
    }

    /// <summary>
    /// The delegate for handling the <see cref="Stepper.NavigationCanceledByValidation" /> event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    public delegate void NavigationCanceledByValidationEventHandler(object sender, NavigationCanceledByValidationEventArgs args);
}
