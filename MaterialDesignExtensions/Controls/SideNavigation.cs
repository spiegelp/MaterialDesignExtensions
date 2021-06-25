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

using MaterialDesignExtensions.Commands.Internal;
using MaterialDesignExtensions.Model;

namespace MaterialDesignExtensions.Controls
{
    /// <summary>
    /// A navigation control to be used as side navigation or inside a navigation drawer.
    /// </summary>
    public class SideNavigation : Control
    {
        private const string NavigationItemsControlName = "navigationItemsControl";

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
        /// An event raised before the item of the user selection will be assigned as <see cref="SelectedItem" />.
        /// </summary>
        public static readonly RoutedEvent WillSelectNavigationItemEvent = EventManager.RegisterRoutedEvent(
            nameof(WillSelectNavigationItem), RoutingStrategy.Bubble, typeof(WillSelectNavigationItemEventHandler), typeof(SideNavigation));

        /// <summary>
        /// An event raised before the item of the user selection will be assigned as <see cref="SelectedItem" />.
        /// </summary>
        public event WillSelectNavigationItemEventHandler WillSelectNavigationItem
        {
            add
            {
                AddHandler(WillSelectNavigationItemEvent, value);
            }

            remove
            {
                RemoveHandler(WillSelectNavigationItemEvent, value);
            }
        }

        /// <summary>
        /// The foreground color of the icon of not selected items.
        /// </summary>
        public static readonly DependencyProperty IconForegroundProperty = DependencyProperty.Register(
            nameof(IconForeground), typeof(Brush), typeof(SideNavigation), new PropertyMetadata(null, null));

        /// <summary>
        /// The foreground color of the icon of not selected items.
        /// </summary>
        public Brush IconForeground
        {
            get
            {
                return (Brush)GetValue(IconForegroundProperty);
            }

            set
            {
                SetValue(IconForegroundProperty, value);
            }
        }

        /// <summary>
        /// The font size of the item labels.
        /// </summary>
        public static readonly DependencyProperty LabelFontSizeProperty = DependencyProperty.Register(
            nameof(LabelFontSize), typeof(double), typeof(SideNavigation), new PropertyMetadata(14.0, null));

        /// <summary>
        /// The font size of the item labels.
        /// </summary>
        public double LabelFontSize
        {
            get
            {
                return (double)GetValue(LabelFontSizeProperty);
            }

            set
            {
                SetValue(LabelFontSizeProperty, value);
            }
        }

        /// <summary>
        /// The foreground color of the label of not selected items.
        /// </summary>
        public static readonly DependencyProperty LabelForegroundProperty = DependencyProperty.Register(
            nameof(LabelForeground), typeof(Brush), typeof(SideNavigation), new PropertyMetadata(null, null));

        /// <summary>
        /// The foreground color of the label of not selected items.
        /// </summary>
        public Brush LabelForeground
        {
            get
            {
                return (Brush)GetValue(LabelForegroundProperty);
            }

            set
            {
                SetValue(LabelForegroundProperty, value);
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
        /// The background color of the selected item.
        /// </summary>
        public static readonly DependencyProperty SelectionBackgroundProperty = DependencyProperty.Register(
            nameof(SelectionBackground), typeof(Brush), typeof(SideNavigation), new PropertyMetadata(null, null));

        /// <summary>
        /// The background color of the selected item.
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
        /// The background color opacity of the selected item.
        /// </summary>
        public static readonly DependencyProperty SelectionBackgroundOpacityProperty = DependencyProperty.Register(
            nameof(SelectionBackgroundOpacity), typeof(double), typeof(SideNavigation), new PropertyMetadata(0.12, null));

        /// <summary>
        /// The background color opacity of the selected item.
        /// </summary>
        public double SelectionBackgroundOpacity
        {
            get
            {
                return (double)GetValue(SelectionBackgroundOpacityProperty);
            }

            set
            {
                SetValue(SelectionBackgroundOpacityProperty, value);
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

        /// <summary>
        /// A callback called after the <see cref="WillSelectNavigationItem" /> event, because events cannot await async code.
        /// </summary>
        public WillSelectNavigationItemCallbackAsync WillSelectNavigationItemCallbackAsync { get; set; }

        /// <summary>
        /// A command called before the item of the user selection will be assigned as <see cref="SelectedItem" />.
        /// </summary>
        public static readonly DependencyProperty WillSelectNavigationItemCommandProperty = DependencyProperty.Register(
            nameof(WillSelectNavigationItemCommand), typeof(ICommand), typeof(SideNavigation), new PropertyMetadata(null, null));

        /// <summary>
        /// A command called before the item of the user selection will be assigned as <see cref="SelectedItem" />.
        /// </summary>
        public ICommand WillSelectNavigationItemCommand
        {
            get
            {
                return (ICommand)GetValue(WillSelectNavigationItemCommandProperty);
            }

            set
            {
                SetValue(WillSelectNavigationItemCommandProperty, value);
            }
        }

        protected ItemsControl m_navigationItemsControl;

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

            CommandBindings.Add(new CommandBinding(SideNavigationCommands.SelectNavigationItemCommand, SelectNavigationItemCommandHandlerAsync));
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

        private async void SelectNavigationItemCommandHandlerAsync(object sender, ExecutedRoutedEventArgs args)
        {
            INavigationItem navigationItemToSelect = args.Parameter as INavigationItem;

            WillSelectNavigationItemEventArgs eventArgs = new WillSelectNavigationItemEventArgs(WillSelectNavigationItemEvent, this, SelectedItem, navigationItemToSelect);
            RaiseEvent(eventArgs);

            if (WillSelectNavigationItemCommand != null && WillSelectNavigationItemCommand.CanExecute(eventArgs))
            {
                WillSelectNavigationItemCommand.Execute(eventArgs);
            }

            if (!eventArgs.Cancel && (WillSelectNavigationItemCallbackAsync == null || !(await WillSelectNavigationItemCallbackAsync(SelectedItem, navigationItemToSelect))))
            {
                SelectedItem = navigationItemToSelect;
            }
        }

        protected virtual void InitItems(IList values)
        {
            if (m_navigationItemsControl != null)
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

                INavigationItem selectedItem = navigationItems.FirstOrDefault(item => item.IsSelected);

                if (selectedItem != null)
                {
                    if (SelectedItem != selectedItem)
                    {
                        SelectedItem = selectedItem;
                    }
                }
                else
                {
                    SelectedItem = null;
                }
            }
        }
    }

    /// <summary>
    /// The arguments for the <see cref="SideNavigation.WillSelectNavigationItem" /> event.
    /// </summary>
    public class WillSelectNavigationItemEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// Set this property to <code>true</code> for cancelling the selection of <see cref="NavigationItemToSelect" />.
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// The current item before the selection.
        /// </summary>
        public INavigationItem CurrentNavigationItem { get; private set; }

        /// <summary>
        /// The item to be selected.
        /// </summary>
        public INavigationItem NavigationItemToSelect { get; private set; }

        /// <summary>
        /// Creates a new <see cref="WillSelectNavigationItemEventArgs" />.
        /// </summary>
        /// <param name="routedEvent"></param>
        /// <param name="source">The source object</param>
        /// <param name="currentNavigationItem">The current item before the selection</param>
        /// <param name="navigationToSelect">The item to be selected</param>
        public WillSelectNavigationItemEventArgs(RoutedEvent routedEvent, object source, INavigationItem currentNavigationItem, INavigationItem navigationToSelect)
            : base(routedEvent, source)
        {
            CurrentNavigationItem = currentNavigationItem;
            NavigationItemToSelect = navigationToSelect;

            Cancel = false;
        }
    }

    /// <summary>
    /// The delegate for handling the <see cref="SideNavigation.WillSelectNavigationItem" /> event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    public delegate void WillSelectNavigationItemEventHandler(object sender, WillSelectNavigationItemEventArgs args);

    /// <summary>
    /// A delegate for a kind of Javascript style event handling, because events cannot await async code.
    /// It will be called after the <see cref="SideNavigation.WillSelectNavigationItem" /> event.
    /// </summary>
    /// <param name="currentNavigationItem"></param>
    /// <param name="navigationToSelect"></param>
    /// <returns></returns>
    public delegate Task<bool> WillSelectNavigationItemCallbackAsync(INavigationItem currentNavigationItem, INavigationItem navigationToSelect);

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
