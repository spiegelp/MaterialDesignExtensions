using MaterialDesignExtensions.Commands.Internal;
using MaterialDesignExtensions.Controllers;
using MaterialDesignExtensions.Model;
using MaterialDesignThemes.Wpf;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MaterialDesignExtensions.Controls
{
    /// <summary>
    /// The base control with common logic for <see cref="PersistentSearch" />.
    /// </summary>
    public abstract class SearchBase : ControlWithAutocompletePopup
    {
        protected const string CancelButtonName = "cancelButton";
        protected const string ClearButtonName = "clearButton";
        protected const string SearchTextBoxName = "searchTextBox";
        protected const string SearchSuggestionsPopupName = "searchSuggestionsPopup";
        protected const string SearchSuggestionsItemsControlName = "searchSuggestionsItemsControl";

        /// <summary>
        /// An event raised by triggering a search (select a suggestion or hit enter).
        /// </summary>
        public static readonly RoutedEvent SearchEvent = EventManager.RegisterRoutedEvent(
            nameof(Search), RoutingStrategy.Bubble, typeof(SearchEventHandler), typeof(SearchBase));

        /// <summary>
        /// An event raised by triggering a search (select a suggestion or hit enter).
        /// </summary>
        public event SearchEventHandler Search
        {
            add
            {
                AddHandler(SearchEvent, value);
            }

            remove
            {
                RemoveHandler(SearchEvent, value);
            }
        }

        /// <summary>
        /// A command called if a search is triggered (select a suggestion or hit enter).
        /// </summary>
        public static readonly DependencyProperty SearchCommandProperty = DependencyProperty.Register(
            nameof(SearchCommand), typeof(ICommand), typeof(SearchBase), new PropertyMetadata(null, null));

        /// <summary>
        /// A command called if a search is triggered (select a suggestion or hit enter).
        /// </summary>
        public ICommand SearchCommand
        {
            get
            {
                return (ICommand)GetValue(SearchCommandProperty);
            }

            set
            {
                SetValue(SearchCommandProperty, value);
            }
        }

        /// <summary>
        /// A hint to show inside the empty search text box.
        /// </summary>
        public static readonly DependencyProperty SearchHintProperty = DependencyProperty.Register(
            nameof(SearchHint), typeof(string), typeof(SearchBase), new PropertyMetadata(Localization.Strings.Search));

        /// <summary>
        /// A hint to show inside the empty search text box.
        /// </summary>
        public string SearchHint
        {
            get
            {
                return (string)GetValue(SearchHintProperty);
            }

            set
            {
                SetValue(SearchHintProperty, value);
            }
        }

        /// <summary>
        /// The icon on the left side of the search text box.
        /// </summary>
        public static readonly DependencyProperty SearchIconProperty = DependencyProperty.Register(
            nameof(SearchIcon), typeof(object), typeof(SearchBase), new PropertyMetadata(PackIconKind.Magnify, null));

        /// <summary>
        /// The icon on the left side of the search text box.
        /// </summary>
        public object SearchIcon
        {
            get
            {
                return GetValue(SearchIconProperty);
            }

            set
            {
                SetValue(SearchIconProperty, value);
            }
        }

        /// <summary>
        /// The icon on the left side of the search text box that appears when the text is not empty and replaced the search icon.
        /// </summary>
        public static readonly DependencyProperty CancelIconProperty = DependencyProperty.Register(
            nameof(CancelIcon), typeof(object), typeof(SearchBase), new PropertyMetadata(PackIconKind.ArrowLeft, null));

        /// <summary>
        /// The icon on the left side of the search text box that appears when the text is not empty and replaced the search icon.
        /// </summary>
        public object CancelIcon
        {
            get
            {
                return GetValue(CancelIconProperty);
            }

            set
            {
                SetValue(CancelIconProperty, value);
            }
        }

        /// <summary>
        /// The icon on the right side of the search text box that appears when the text is not empty.
        /// </summary>
        public static readonly DependencyProperty ClearIconProperty = DependencyProperty.Register(
            nameof(ClearIcon), typeof(object), typeof(SearchBase), new PropertyMetadata(PackIconKind.Close, null));

        /// <summary>
        /// The icon on the right side of the search text box that appears when the text is not empty.
        /// </summary>
        public object ClearIcon
        {
            get
            {
                return GetValue(ClearIconProperty);
            }

            set
            {
                SetValue(ClearIconProperty, value);
            }
        }

        /// <summary>
        /// A source for providing suggestions to search for.
        /// </summary>
        public static readonly DependencyProperty SearchSuggestionsSourceProperty = DependencyProperty.Register(
            nameof(SearchSuggestionsSource), typeof(ISearchSuggestionsSource), typeof(SearchBase), new PropertyMetadata(null, SearchSuggestionSourceChangedHandler));

        /// <summary>
        /// A source for providing suggestions to search for.
        /// </summary>
        public ISearchSuggestionsSource SearchSuggestionsSource
        {
            get
            {
                return (ISearchSuggestionsSource)GetValue(SearchSuggestionsSourceProperty);
            }

            set
            {
                SetValue(SearchSuggestionsSourceProperty, value);
            }
        }

        /// <summary>
        /// The term to search for.
        /// </summary>
        public static readonly DependencyProperty SearchTermProperty = DependencyProperty.Register(
            nameof(SearchTerm), typeof(string), typeof(SearchBase), new PropertyMetadata(null, SearchTermChangedHandler));

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

        private Button m_cancelButton;
        private Button m_clearButton;
        private TextBox m_searchTextBox;
        private ItemsControl m_searchSuggestionsItemsControl;

        private SearchSuggestionsController m_searchSuggestionsController;

        /// <summary>
        /// Creates a new <see cref="SearchBase" />.
        /// </summary>
        public SearchBase()
            : base()
        {
            m_cancelButton = null;
            m_searchTextBox = null;
            m_searchSuggestionsItemsControl = null;

            m_searchSuggestionsController = new SearchSuggestionsController() { SearchSuggestionsSource = SearchSuggestionsSource };

            CommandBindings.Add(new CommandBinding(SearchControlCommands.SelectSearchSuggestionCommand, SelectSearchSuggestionCommandHandler));

            Loaded += LoadedHandler;
            Unloaded += UnloadedHandler;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (m_cancelButton != null)
            {
                m_cancelButton.Click -= CancelClickHandler;
            }

            m_cancelButton = Template.FindName(CancelButtonName, this) as Button;
            m_cancelButton.Click += CancelClickHandler;

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

            m_popup = Template.FindName(SearchSuggestionsPopupName, this) as AutocompletePopup;

            m_searchSuggestionsItemsControl = Template.FindName(SearchSuggestionsItemsControlName, this) as ItemsControl;
        }

        protected override void LoadedHandler(object sender, RoutedEventArgs args)
        {
            base.LoadedHandler(sender, args);

            if (m_searchSuggestionsController != null)
            {
                m_searchSuggestionsController.SearchSuggestionsChanged += SearchSuggestionsChangedHandler;
            }
        }

        protected override void UnloadedHandler(object sender, RoutedEventArgs args)
        {
            base.UnloadedHandler(sender, args);

            if (m_searchSuggestionsController != null)
            {
                m_searchSuggestionsController.SearchSuggestionsChanged -= SearchSuggestionsChangedHandler;
            }
        }

        private void CancelClickHandler(object sender, RoutedEventArgs args)
        {
            SearchTerm = null;
        }

        private void ClearClickHandler(object sender, RoutedEventArgs args)
        {
            SearchTerm = null;

            m_searchTextBox.Focus();
        }

        private void SearchTextBoxGotFocusHandler(object sender, RoutedEventArgs args)
        {
            if (sender == m_searchTextBox && string.IsNullOrEmpty(SearchTerm) && SearchSuggestionsSource != null)
            {
                SetSuggestions(SearchSuggestionsSource.GetSearchSuggestions(), true);
            }
        }

        private void SearchTextBoxKeyUpHandler(object sender, KeyEventArgs args)
        {
            if (args.Key == Key.Enter)
            {
                DoSearch();
            }
        }

        private void DoSearch()
        {
            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                m_searchTextBox.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));

                SearchEventArgs eventArgs = new SearchEventArgs(SearchEvent, this, SearchTerm);
                RaiseEvent(eventArgs);

                if (SearchCommand != null && SearchCommand.CanExecute(SearchTerm))
                {
                    SearchCommand.Execute(SearchTerm);
                }
            }
        }

        private static void SearchSuggestionSourceChangedHandler(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            (obj as SearchBase)?.SearchSuggestionSourceChangedHandler(args.NewValue as ISearchSuggestionsSource);
        }

        private void SearchSuggestionSourceChangedHandler(ISearchSuggestionsSource newSearchSuggestionSource)
        {
            if (m_searchSuggestionsController != null)
            {
                m_searchSuggestionsController.SearchSuggestionsSource = newSearchSuggestionSource;
            }
        }

        private static void SearchTermChangedHandler(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            (obj as SearchBase)?.SearchTermChangedHandler(args.NewValue as string);
        }

        private void SearchTermChangedHandler(string searchTerm)
        {
            m_searchSuggestionsController?.Search(searchTerm);
        }

        private void SearchSuggestionsChangedHandler(object sender, SearchSuggestionsChangedEventArgs args)
        {
            Dispatcher.Invoke(() =>
            {
                SetSuggestions(args.SearchSuggestions, args.IsFromHistory);
            });
        }

        private void SetSuggestions(IList<string> suggestions, bool isFromHistory)
        {
            if (m_searchSuggestionsItemsControl != null)
            {
                if (suggestions != null)
                {
                    m_searchSuggestionsItemsControl.ItemsSource = suggestions.Select(suggestion => new SearchSuggestionItem() { Suggestion = suggestion, IsFromHistory = isFromHistory });
                }
                else
                {
                    m_searchSuggestionsItemsControl.ItemsSource = null;
                }
            }
        }

        private void SelectSearchSuggestionCommandHandler(object sender, ExecutedRoutedEventArgs args)
        {
            SearchTerm = args.Parameter as string;

            DoSearch();
        }
    }

    /// <summary>
    /// The arguments for the <see cref="SearchBase.Search" /> event.
    /// </summary>
    public class SearchEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// The term to search for.
        /// </summary>
        public string SearchTerm { get; private set; }

        /// <summary>
        /// Creates a new <see cref="SearchEventArgs" />.
        /// </summary>
        /// <param name="routedEvent"></param>
        /// <param name="source">The source object</param>
        /// <param name="searchTerm">The term to search for</param>
        public SearchEventArgs(RoutedEvent routedEvent, object source, string searchTerm)
            : base(routedEvent, source)
        {
            SearchTerm = searchTerm;
        }
    }

    /// <summary>
    /// The delegate for handling the <see cref="SearchBase.Search" /> event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    public delegate void SearchEventHandler(object sender, SearchEventArgs args);
}
