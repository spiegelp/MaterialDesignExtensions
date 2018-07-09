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
    public class Autocomplete : Control
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
            nameof(Hint), typeof(string), typeof(Autocomplete));

        /// <summary>
        /// A hint to show inside the empty text box.
        /// </summary>
        public string Hint
        {
            get
            {
                return (string)GetValue(HintProperty);
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
        /// A command called by changing the selected item;
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

        private Button m_clearButton;
        private TextBox m_searchTextBox;
        private Popup m_autocompleteItemsPopup;
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
            m_autocompleteItemsPopup = null;
            m_autocompleteItemsControl = null;

            m_autocompleteController = new AutocompleteController() { AutocompleteSource = AutocompleteSource };

            CommandBindings.Add(new CommandBinding(SelectAutocompleteItemCommand, SelectAutocompleteItemCommandHandler));

            Loaded += LoadedHandler;
            Unloaded += UnloadedHandler;
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

            m_searchTextBox = Template.FindName(SearchTextBoxName, this) as TextBox;

            m_autocompleteItemsPopup = Template.FindName(AutocompleteItemsPopupName, this) as Popup;

            m_autocompleteItemsControl = Template.FindName(AutocompleteItemsControlName, this) as ItemsControl;
        }

        private void LoadedHandler(object sender, RoutedEventArgs args)
        {
            if (m_autocompleteController != null)
            {
                m_autocompleteController.AutocompleteItemsChanged += AutocompleteItemsChangedHandler;
            }
        }

        private void UnloadedHandler(object sender, RoutedEventArgs args)
        {
            if (m_autocompleteController != null)
            {
                m_autocompleteController.AutocompleteItemsChanged -= AutocompleteItemsChangedHandler;
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
        }

        private static void SelectedItemChangedHandler(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            (obj as Autocomplete)?.SelectedItemChangedHandler(args.NewValue);
        }

        private void SelectedItemChangedHandler(object selectedItem)
        {
            SelectedItemChangedEventArgs eventArgs = new SelectedItemChangedEventArgs(SelectedItemChangedEvent, this, selectedItem);
            RaiseEvent(eventArgs);

            if (SelectedItemChangedCommand != null && SelectedItemChangedCommand.CanExecute(selectedItem))
            {
                SelectedItemChangedCommand.Execute(selectedItem);
            }
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
