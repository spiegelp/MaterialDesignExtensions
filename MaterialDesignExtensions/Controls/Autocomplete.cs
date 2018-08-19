using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Controls.Primitives;

using MaterialDesignExtensions.Controllers;
using MaterialDesignExtensions.Model;

namespace MaterialDesignExtensions.Controls
{
    /// <summary>
    /// A control for providing arbitrary items for a search string.
    /// </summary>
    public class Autocomplete : ControlWithAutocompletePopup
    {
        private static readonly string AutocompleteItemsControlName = "autocompleteItemsControl";
        private static readonly string AutocompleteItemsPopupName = "autocompleteItemsPopup";
        private static readonly string ClearButtonName = "clearButton";
        private static readonly string SearchTextBoxName = "searchTextBox";

        /// <summary>
        /// Internal command used by the XAML template (public to be available in the XAML template). Not intended for external usage.
        /// </summary>
        public static readonly RoutedCommand SelectAutocompleteItemCommand = new RoutedCommand();

        /// <summary>
        /// An event raised by changing the selected item.
        /// </summary>
        public static readonly RoutedEvent SelectedItemChangedEvent = EventManager.RegisterRoutedEvent(
            nameof(SelectedItemChanged), RoutingStrategy.Bubble, typeof(SelectedItemChangedEventHandler), typeof(Autocomplete));

        /// <summary>
        /// An event raised by changing the selected item.
        /// </summary>
        public event SelectedItemChangedEventHandler SelectedItemChanged
        {
            add
            {
                AddHandler(SelectedItemChangedEvent, value);
            }

            remove
            {
                RemoveHandler(SelectedItemChangedEvent, value);
            }
        }

        /// <summary>
        /// The size of the icon inside the clear button.
        /// </summary>
        public static readonly DependencyProperty ClearIconSizeProperty = DependencyProperty.Register(
            nameof(ClearIconSize), typeof(double), typeof(Autocomplete));

        /// <summary>
        /// The size of the icon inside the clear button.
        /// </summary>
        public double ClearIconSize
        {
            get
            {
                return (double)GetValue(ClearIconSizeProperty);
            }

            set
            {
                SetValue(ClearIconSizeProperty, value);
            }
        }

        /// <summary>
        /// A source for providing the autocomplete items.
        /// </summary>
        public static readonly DependencyProperty AutocompleteSourceProperty = DependencyProperty.Register(
            nameof(AutocompleteSource), typeof(IAutocompleteSource), typeof(Autocomplete), new PropertyMetadata(null, AutocompleteSourceChangedHandler));

        /// <summary>
        /// A source for providing the autocomplete items.
        /// </summary>
        public IAutocompleteSource AutocompleteSource
        {
            get
            {
                return (IAutocompleteSource)GetValue(AutocompleteSourceProperty);
            }

            set
            {
                SetValue(AutocompleteSourceProperty, value);
            }
        }

        /// <summary>
        /// A hint to show inside the empty text box.
        /// </summary>
        public static readonly DependencyProperty HintProperty = DependencyProperty.Register(
            nameof(Hint), typeof(object), typeof(Autocomplete));

        /// <summary>
        /// A hint to show inside the empty text box.
        /// </summary>
        public object Hint
        {
            get
            {
                return GetValue(HintProperty);
            }

            set
            {
                SetValue(HintProperty, value);
            }
        }

        /// <summary>
        /// The template for the items.
        /// </summary>
        public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register(
            nameof(ItemTemplate), typeof(DataTemplate), typeof(Autocomplete));

        /// <summary>
        /// The template for the items.
        /// </summary>
        public DataTemplate ItemTemplate
        {
            get
            {
                return (DataTemplate)GetValue(ItemTemplateProperty);
            }

            set
            {
                SetValue(ItemTemplateProperty, value);
            }
        }

        /// <summary>
        /// True for triggering a search on the initial focus of the control.
        /// </summary>
        public static readonly DependencyProperty SearchOnInitialFocusProperty = DependencyProperty.Register(
            nameof(SearchOnInitialFocus), typeof(bool), typeof(Autocomplete), new PropertyMetadata(false));

        /// <summary>
        /// True for triggering a search on the initial focus of the control.
        /// </summary>
        public bool SearchOnInitialFocus
        {
            get
            {
                return (bool)GetValue(SearchOnInitialFocusProperty);
            }

            set
            {
                SetValue(SearchOnInitialFocusProperty, value);
            }
        }

        /// <summary>
        /// The term to search for.
        /// </summary>
        public static readonly DependencyProperty SearchTermProperty = DependencyProperty.Register(
            nameof(SearchTerm), typeof(string), typeof(Autocomplete), new PropertyMetadata(null, SearchTermChangedHandler));

        /// <summary>
        /// The term to search for.
        /// </summary>
        public string SearchTerm
        {
            get
            {
                return (string)GetValue(SearchTermProperty);
            }

            set
            {
                SetValue(SearchTermProperty, value);
            }
        }

        /// <summary>
        /// The selected autocomplete item.
        /// </summary>
        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(
            nameof(SelectedItem), typeof(object), typeof(Autocomplete), new PropertyMetadata(null, SelectedItemChangedHandler));

        /// <summary>
        /// The selected autocomplete item.
        /// </summary>
        public object SelectedItem
        {
            get
            {
                return GetValue(SelectedItemProperty);
            }

            set
            {
                SetValue(SelectedItemProperty, value);
            }
        }

        /// <summary>
        /// A command called by changing the selected item.
        /// </summary>
        public static readonly DependencyProperty SelectedItemChangedCommandProperty = DependencyProperty.Register(
            nameof(SelectedItemChangedCommand), typeof(ICommand), typeof(Autocomplete), new PropertyMetadata(null, null));

        /// <summary>
        /// A command called by changing the selected item.
        /// </summary>
        public ICommand SelectedItemChangedCommand
        {
            get
            {
                return (ICommand)GetValue(SelectedItemChangedCommandProperty);
            }

            set
            {
                SetValue(SelectedItemChangedCommandProperty, value);
            }
        }

        /// <summary>
        /// True (default value) for showing the clear button only if <see cref="SelectedItem" /> != null.
        /// </summary>
        public static readonly DependencyProperty ShowClearButtonOnlyOnSelectedItemProperty = DependencyProperty.Register(
            nameof(ShowClearButtonOnlyOnSelectedItem), typeof(bool), typeof(Autocomplete), new PropertyMetadata(true, ShowClearButtonOnlyOnSelectedItemChangedHandler));

        /// <summary>
        /// True (default value) for showing the clear button only if <see cref="SelectedItem" /> != null.
        /// </summary>
        public bool ShowClearButtonOnlyOnSelectedItem
        {
            get
            {
                return (bool)GetValue(ShowClearButtonOnlyOnSelectedItemProperty);
            }

            set
            {
                SetValue(ShowClearButtonOnlyOnSelectedItemProperty, value);
            }
        }

        private Button m_clearButton;
        private TextBox m_searchTextBox;
        private ItemsControl m_autocompleteItemsControl;

        private AutocompleteController m_autocompleteController;

        static Autocomplete()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Autocomplete), new FrameworkPropertyMetadata(typeof(Autocomplete)));
        }

        /// <summary>
        /// Creates a new <see cref="Autocomplete" />.
        /// </summary>
        public Autocomplete()
            : base()
        {
            m_clearButton = null;
            m_searchTextBox = null;
            m_autocompleteItemsControl = null;

            m_autocompleteController = new AutocompleteController() { AutocompleteSource = AutocompleteSource };

            CommandBindings.Add(new CommandBinding(SelectAutocompleteItemCommand, SelectAutocompleteItemCommandHandler));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (m_clearButton != null)
            {
                m_clearButton.Click -= ClearClickHandler;
            }

            m_clearButton = Template.FindName(ClearButtonName, this) as Button;
            m_clearButton.Click += ClearClickHandler;

            if (m_searchTextBox != null)
            {
                m_searchTextBox.GotFocus -= SearchTextBoxGotFocusHandler;
                m_searchTextBox.KeyUp -= SearchTextBoxKeyUpHandler;
            }

            m_searchTextBox = Template.FindName(SearchTextBoxName, this) as TextBox;
            m_searchTextBox.GotFocus += SearchTextBoxGotFocusHandler;
            m_searchTextBox.KeyUp += SearchTextBoxKeyUpHandler;

            m_popup = Template.FindName(AutocompleteItemsPopupName, this) as AutocompletePopup;

            m_autocompleteItemsControl = Template.FindName(AutocompleteItemsControlName, this) as ItemsControl;

            UpdateClearButtonVisibility();
        }

        protected override void LoadedHandler(object sender, RoutedEventArgs args)
        {
            base.LoadedHandler(sender, args);

            if (m_autocompleteController != null)
            {
                m_autocompleteController.AutocompleteItemsChanged += AutocompleteItemsChangedHandler;
            }
        }

        protected override void UnloadedHandler(object sender, RoutedEventArgs args)
        {
            base.UnloadedHandler(sender, args);

            if (m_autocompleteController != null)
            {
                m_autocompleteController.AutocompleteItemsChanged -= AutocompleteItemsChangedHandler;
            }
        }

        private void SearchTextBoxGotFocusHandler(object sender, RoutedEventArgs args)
        {
            if (SearchOnInitialFocus
                && m_autocompleteItemsControl != null
                && m_autocompleteItemsControl.ItemsSource == null)
            {
                m_autocompleteController?.Search(SearchTerm);
            }
        }

        private void SearchTextBoxKeyUpHandler(object sender, KeyEventArgs args)
        {
            if (args.Key == Key.Down)
            {
                if (m_autocompleteItemsControl != null
                    && m_autocompleteItemsControl.ItemsSource != null
                    && m_autocompleteItemsControl.ItemsSource.GetEnumerator().MoveNext())
                {
                    m_autocompleteItemsControl.Focus();
                }
            }
        }

        private void ClearClickHandler(object sender, RoutedEventArgs args)
        {
            SelectedItem = null;
            SearchTerm = null;

            m_searchTextBox.Focus();
        }

        private static void AutocompleteSourceChangedHandler(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            (obj as Autocomplete)?.AutocompleteSourceChangedHandler(args.NewValue as IAutocompleteSource);
        }

        private void AutocompleteSourceChangedHandler(IAutocompleteSource newAutocompleteSource)
        {
            if (m_autocompleteController != null)
            {
                m_autocompleteController.AutocompleteSource = newAutocompleteSource;
            }
        }

        private static void SearchTermChangedHandler(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            (obj as Autocomplete)?.SearchTermChangedHandler(args.NewValue as string);
        }

        private void SearchTermChangedHandler(string searchTerm)
        {
            m_autocompleteController?.Search(searchTerm);

            UpdateClearButtonVisibility();
        }

        private static void SelectedItemChangedHandler(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            (obj as Autocomplete)?.SelectedItemChangedHandler(args.NewValue);
        }

        private void SelectedItemChangedHandler(object selectedItem)
        {
            if (m_autocompleteItemsControl != null)
            {
                m_autocompleteItemsControl.ItemsSource = null;
            }

            UpdateClearButtonVisibility();

            SelectedItemChangedEventArgs eventArgs = new SelectedItemChangedEventArgs(SelectedItemChangedEvent, this, selectedItem);
            RaiseEvent(eventArgs);

            if (SelectedItemChangedCommand != null && SelectedItemChangedCommand.CanExecute(selectedItem))
            {
                SelectedItemChangedCommand.Execute(selectedItem);
            }
        }

        private static void ShowClearButtonOnlyOnSelectedItemChangedHandler(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            (obj as Autocomplete)?.ShowClearButtonOnlyOnSelectedItemChangedHandler();
        }

        private void ShowClearButtonOnlyOnSelectedItemChangedHandler()
        {
            UpdateClearButtonVisibility();
        }

        private void SelectAutocompleteItemCommandHandler(object sender, ExecutedRoutedEventArgs args)
        {
            SelectedItem = args.Parameter;
        }

        private void AutocompleteItemsChangedHandler(object sender, AutocompleteItemsChangedEventArgs args)
        {
            Dispatcher.Invoke(() =>
            {
                SetAutocompleteItems(args.Items);
            });
        }

        private void SetAutocompleteItems(IEnumerable autocompleteItems)
        {
            if (m_autocompleteItemsControl != null)
            {
                if (autocompleteItems != null)
                {
                    m_autocompleteItemsControl.ItemsSource = autocompleteItems;
                }
                else
                {
                    m_autocompleteItemsControl.ItemsSource = null;
                }
            }
        }

        private void UpdateClearButtonVisibility()
        {
            if (m_clearButton != null)
            {
                if (SelectedItem != null || (!ShowClearButtonOnlyOnSelectedItem && !string.IsNullOrEmpty(SearchTerm)))
                {
                    m_clearButton.Visibility = Visibility.Visible;
                }
                else
                {
                    m_clearButton.Visibility = Visibility.Collapsed;
                }
            }
        }
    }

    /// <summary>
    /// The argument for the <see cref="Autocomplete.SelectedItemChanged" /> event.
    /// </summary>
    public class SelectedItemChangedEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// The new selected item.
        /// </summary>
        public object SelectedItem { get; private set; }

        /// <summary>
        /// Creates a new <see cref="SelectedItemChangedEventArgs" />.
        /// </summary>
        /// <param name="routedEvent"></param>
        /// <param name="source"></param>
        /// <param name="selectedItem"></param>
        public SelectedItemChangedEventArgs(RoutedEvent routedEvent, object source, object selectedItem)
            : base(routedEvent, source)
        {
            SelectedItem = selectedItem;
        }
    }

    /// <summary>
    /// The delegate for handling the <see cref="Autocomplete.SelectedItemChanged" /> event.
    /// </summary>
    public delegate void SelectedItemChangedEventHandler(object sender, SelectedItemChangedEventArgs args);
}
