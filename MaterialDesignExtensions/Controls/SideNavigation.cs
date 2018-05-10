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

using MaterialDesignExtensions.Model;

namespace MaterialDesignExtensions.Controls
{
    public class SideNavigation : Control
    {
        private const string NavigationItemsControlName = "navigationItemsControl";

        /// <summary>
        /// Internal command used by the XAML template (public to be available in the XAML template). Not intended for external usage.
        /// </summary>
        public static readonly RoutedCommand SelectNavigationItemCommand = new RoutedCommand();

        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register(
            nameof(Items),
            typeof(IList),
            typeof(SideNavigation),
            new PropertyMetadata(null, ItemsChangedHandler));
        
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

        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(
            nameof(SelectedItem),
            typeof(INavigationItem),
            typeof(SideNavigation),
            new PropertyMetadata(null, SelectedItemChangedHandler));

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
            if (args.NewValue is INavigationItem navigationItem) {
                navigationItem.NavigationItemSelectedCallback?.Invoke(navigationItem);
            }
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
        }

        private void SelectNavigationItemCommandHandler(object sender, ExecutedRoutedEventArgs args)
        {
            SelectedItem = args.Parameter as INavigationItem;
        }
    }
}
