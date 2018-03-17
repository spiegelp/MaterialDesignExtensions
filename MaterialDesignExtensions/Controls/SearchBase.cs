using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MaterialDesignExtensions.Controls
{
    public class SearchBase : Control
    {
        protected const string CancelButtonName = "cancelButton";
        protected const string SearchTextBoxName = "searchTextBox";

        public static readonly RoutedEvent SearchEvent = EventManager.RegisterRoutedEvent(
            nameof(Search), RoutingStrategy.Bubble, typeof(SearchEventHandler), typeof(SearchBase));

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

        public static readonly DependencyProperty SearchCommandProperty = DependencyProperty.Register(
            nameof(SearchCommand), typeof(ICommand), typeof(SearchBase), new PropertyMetadata(null, null));

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

        public static readonly DependencyProperty SearchTermProperty = DependencyProperty.Register(
            nameof(SearchTerm), typeof(string), typeof(SearchBase), new PropertyMetadata(null, null));

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

        protected Button m_cancelButton;
        protected TextBox m_searchTextBox;

        public SearchBase()
            : base()
        {
            m_cancelButton = null;
            m_searchTextBox = null;
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

            if (m_searchTextBox != null)
            {
                m_searchTextBox.KeyUp -= SearchTextBoxKeyUpHandler;
            }

            m_searchTextBox = Template.FindName(SearchTextBoxName, this) as TextBox;
            m_searchTextBox.KeyUp += SearchTextBoxKeyUpHandler;
        }

        protected void CancelClickHandler(object sender, RoutedEventArgs args)
        {
            SearchTerm = null;
        }

        protected void SearchClickHander()
        {
            DoSearch();
        }

        private void SearchTextBoxKeyUpHandler(object sender, KeyEventArgs args)
        {
            if (args.Key == Key.Enter)
            {
                DoSearch();
            }
        }

        protected void DoSearch()
        {
            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                SearchEventArgs eventArgs = new SearchEventArgs(SearchEvent, this, SearchTerm);
                RaiseEvent(eventArgs);

                if (SearchCommand != null && SearchCommand.CanExecute(SearchTerm))
                {
                    SearchCommand.Execute(SearchTerm);
                }
            }
        }
    }

    public class SearchEventArgs : RoutedEventArgs
    {
        public string SearchTerm { get; private set; }

        public SearchEventArgs(RoutedEvent routedEvent, object source, string searchTerm)
            : base(routedEvent, source)
        {
            SearchTerm = searchTerm;
        }
    }

    public delegate void SearchEventHandler(object sender, SearchEventArgs args);
}
