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

        public string SearchTerm
        {
            get; set;
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

        protected Button m_cancelButton;

        public SearchBase()
            : base()
        {
            m_cancelButton = null;
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
        }

        protected void CancelClickHandler(object sender, RoutedEventArgs args)
        {
            //
        }

        protected void SearchClickHander()
        {

        }

        protected void DoSearch()
        {
            SearchEventArgs eventArgs = new SearchEventArgs(SearchEvent, this, SearchTerm);
            RaiseEvent(eventArgs);

            if (SearchCommand != null && SearchCommand.CanExecute(SearchTerm))
            {
                SearchCommand.Execute(SearchTerm);
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
