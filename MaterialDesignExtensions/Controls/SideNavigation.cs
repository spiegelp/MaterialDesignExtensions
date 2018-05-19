using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

using MaterialDesignExtensions.Model;

namespace MaterialDesignExtensions.Controls
{
    /// <summary>
    /// A control to be used as side navigation or inside a navigation drawer.
    /// </summary>
    public class SideNavigation : Control
    {
        private const string NavigationItemsControlName = "navigationItemsControl";

        /// <summary>
        /// Internal command used by the XAML template (public to be available in the XAML template). Not intended for external usage.
        /// </summary>
        public static readonly RoutedCommand SelectNavigationItemCommand = new RoutedCommand();

        /// <summary>
        /// An event raised by selecting an item.
        /// </summary>
        public static readonly RoutedEvent NavigationItemSelectedEvent = EventManager.RegisterRoutedEvent(
            nameof(NavigationItemSelected), RoutingStrategy.Bubble, typeof(NavigationItemSelectedEventHandler), typeof(SideNavigation));

        /// <summary>
        /// An event raised by selecting an item.
        /// </summary>
        public event NavigationItemSelectedEventHandler NavigationItemSelected
        {
            add
            {
                AddHandler(NavigationItemSelectedEvent, value);
            }

            remove
            {
                RemoveHandler(NavigationItemSelectedEvent, value);
            }
        }

        /// <summary>
        /// The color of the click ripple by selecting an item.
        /// </summary>
        public static readonly DependencyProperty NavigationItemFeedbackProperty = DependencyProperty.Register(
            nameof(NavigationItemFeedback), typeof(Brush), typeof(SideNavigation), new PropertyMetadata(null, null));

        /// <summary>
        /// The color of the click ripple by selecting an item.
        /// </summary>
        public Brush NavigationItemFeedback
        {
            get
            {
                return (Brush)GetValue(NavigationItemFeedbackProperty);
            }

            set
            {
                SetValue(NavigationItemFeedbackProperty, value);
            }
        }

        /// <summary>
        /// The background color of the selected item. It will get an opacity of 15%.
        /// </summary>
        public static readonly DependencyProperty SelectionBackgroundProperty = DependencyProperty.Register(
            nameof(SelectionBackground), typeof(Brush), typeof(SideNavigation), new PropertyMetadata(null, null));

        /// <summary>
        /// The background color of the selected item. It will get an opacity of 15%.
        /// </summary>
        public Brush SelectionBackground
        {
            get
            {
                return (Brush)GetValue(SelectionBackgroundProperty);
            }

            set
            {
                SetValue(SelectionBackgroundProperty, value);
            }
        }

        /// <summary>
        /// The corner radius of the selected item.
        /// </summary>
        public static readonly DependencyProperty SelectionCornerRadiusProperty = DependencyProperty.Register(
            nameof(SelectionCornerRadius), typeof(CornerRadius), typeof(SideNavigation), new PropertyMetadata(new CornerRadius(0), null));

        /// <summary>
        /// The corner radius of the selected item.
        /// </summary>
        public CornerRadius SelectionCornerRadius
        {
            get
            {
                return (CornerRadius)GetValue(SelectionCornerRadiusProperty);
            }

            set
            {
                SetValue(SelectionCornerRadiusProperty, value);
            }
        }

        /// <summary>
        /// The foreground color of the selected item.
        /// </summary>
        public static readonly DependencyProperty SelectionForegroundProperty = DependencyProperty.Register(
            nameof(SelectionForeground), typeof(Brush), typeof(SideNavigation), new PropertyMetadata(null, null));

        /// <summary>
        /// The foreground color of the selected item.
        /// </summary>
        public Brush SelectionForeground
        {
            get
            {
                return (Brush)GetValue(SelectionForegroundProperty);
            }

            set
            {
                SetValue(SelectionForegroundProperty, value);
            }
        }

        /// <summary>
        /// The margin of the background of the selected item.
        /// </summary>
        public static readonly DependencyProperty SelectionMarginProperty = DependencyProperty.Register(
            nameof(SelectionMargin), typeof(Thickness), typeof(SideNavigation), new PropertyMetadata(new Thickness(0), null));

        /// <summary>
        /// The margin of the background of the selected item.
        /// </summary>
        public Thickness SelectionMargin
        {
            get
            {
                return (Thickness)GetValue(SelectionMarginProperty);
            }

            set
            {
                SetValue(SelectionMarginProperty, value);
            }
        }

        /// <summary>
        /// A command called if an item is selected.
        /// </summary>
        public static readonly DependencyProperty NavigationItemSelectedCommandProperty = DependencyProperty.Register(
            nameof(NavigationItemSelectedCommand), typeof(ICommand), typeof(SideNavigation), new PropertyMetadata(null, null));

        /// <summary>
        /// A command called if an item is selected.
        /// </summary>
        public ICommand NavigationItemSelectedCommand
        {
            get
            {
                return (ICommand)GetValue(NavigationItemSelectedCommandProperty);
            }

            set
            {
                SetValue(NavigationItemSelectedCommandProperty, value);
            }
        }

        /// <summary>
        /// The navigation items.
        /// </summary>
        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register(
            nameof(Items),
            typeof(IList),
            typeof(SideNavigation),
            new PropertyMetadata(null, ItemsChangedHandler));

        /// <summary>
        /// The navigation items.
        /// </summary>
        public IList Items
        {
            get
            {
                return (IList)GetValue(ItemsProperty);
            }

            set
            {
                SetValue(ItemsProperty, value);
            }
        }

        /// <summary>
        /// The selected navigation item.
        /// </summary>
        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(
            nameof(SelectedItem),
            typeof(INavigationItem),
            typeof(SideNavigation),
            new PropertyMetadata(null, SelectedItemChangedHandler));

        /// <summary>
        /// The selected navigation item.
        /// </summary>
        public INavigationItem SelectedItem
        {
            get
            {
                return (INavigationItem)GetValue(SelectedItemProperty);
            }

            set
            {
                SetValue(SelectedItemProperty, value);
            }
        }

        private ItemsControl m_navigationItemsControl;

        static SideNavigation()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SideNavigation), new FrameworkPropertyMetadata(typeof(SideNavigation)));
        }

        /// <summary>
        /// Creates a new <see cref="SideNavigation" />.
        /// </summary>
        public SideNavigation()
            : base()
        {
            Loaded += LoadedHandler;
            Unloaded += UnloadedHandler;

            CommandBindings.Add(new CommandBinding(SelectNavigationItemCommand, SelectNavigationItemCommandHandler));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            m_navigationItemsControl = Template.FindName(NavigationItemsControlName, this) as ItemsControl;
        }

        private void LoadedHandler(object sender, RoutedEventArgs args)
        {
            if (Items is ObservableCollection<INavigationItem> items)
            {
                items.CollectionChanged -= ItemsCollectionChanged;
                items.CollectionChanged += ItemsCollectionChanged;
            }

            InitItems(Items);
        }

        private void UnloadedHandler(object sender, RoutedEventArgs args)
        {
            if (Items is ObservableCollection<INavigationItem> items)
            {
                items.CollectionChanged -= ItemsCollectionChanged;
            }
        }

        private static void ItemsChangedHandler(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            (obj as SideNavigation)?.ItemsChangedHandler(args.OldValue, args.NewValue);
        }

        private void ItemsChangedHandler(object oldValue, object newValue)
        {
            if (oldValue is ObservableCollection<INavigationItem> oldItems)
            {
                oldItems.CollectionChanged -= ItemsCollectionChanged;
            }

            if (newValue is ObservableCollection<INavigationItem> newItems)
            {
                newItems.CollectionChanged += ItemsCollectionChanged;
            }

            InitItems(newValue as IList);
        }

        private void ItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            InitItems(Items);
        }

        private static void SelectedItemChangedHandler(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            (obj as SideNavigation).SelectedItemChangedHandler(args.NewValue as INavigationItem);
        }

        private void SelectedItemChangedHandler(INavigationItem navigationItem)
        {
            if (m_navigationItemsControl != null && m_navigationItemsControl.ItemsSource != null)
            {
                foreach (INavigationItem item in m_navigationItemsControl.ItemsSource)
                {
                    item.IsSelected = item == navigationItem;
                }
            }

            NavigationItemSelectedEventArgs eventArgs = new NavigationItemSelectedEventArgs(NavigationItemSelectedEvent, this, navigationItem);
            RaiseEvent(eventArgs);

            if (NavigationItemSelectedCommand != null && NavigationItemSelectedCommand.CanExecute(navigationItem))
            {
                NavigationItemSelectedCommand.Execute(navigationItem);
            }
        }

        private void SelectNavigationItemCommandHandler(object sender, ExecutedRoutedEventArgs args)
        {
            SelectedItem = args.Parameter as INavigationItem;
        }

        private void InitItems(IList values)
        {
            IList<INavigationItem> navigationItems = new List<INavigationItem>();

            if (values != null)
            {
                foreach (object item in values)
                {
                    if (item is INavigationItem navigationItem)
                    {
                        navigationItems.Add(navigationItem);
                    }
                }
            }

            m_navigationItemsControl.ItemsSource = navigationItems;

            if (SelectedItem != null && !navigationItems.Contains(SelectedItem))
            {
                SelectedItem = null;
            }
        }
    }

    /// <summary>
    /// The arguments for the <see cref="SideNavigation.NavigationItemSelected" /> event.
    /// </summary>
    public class NavigationItemSelectedEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// The selected item.
        /// </summary>
        public INavigationItem NavigationItem { get; private set; }

        /// <summary>
        /// Creates a new <see cref="NavigationItemSelectedEventArgs" />.
        /// </summary>
        /// <param name="routedEvent"></param>
        /// <param name="source">The source object</param>
        /// <param name="navigationItem">The selected item</param>
        public NavigationItemSelectedEventArgs(RoutedEvent routedEvent, object source, INavigationItem navigationItem)
            : base(routedEvent, source)
        {
            NavigationItem = navigationItem;
        }
    }

    /// <summary>
    /// The delegate for handling the <see cref="SideNavigation.NavigationItemSelected" /> event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    public delegate void NavigationItemSelectedEventHandler(object sender, NavigationItemSelectedEventArgs args);
}
