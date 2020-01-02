using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

namespace MaterialDesignExtensions.Controls
{
    /// <summary>
    /// A custom control implementing the concept of the app bar (https://material.io/design/components/app-bars-top.html#usage).
    /// It provides an optional toggle button for a navigation drawer, an icon, a title and a content area (to add toolbar buttons for example).
    /// </summary>
    [ContentProperty(nameof(Children))]
    public class AppBar : Control
    {
        private const string BackButtonName = "backButton";

        /// <summary>
        /// The height of a default app bar.
        /// </summary>
        public const double DefaultHeight = 64;

        /// <summary>
        /// The height of a prominent app bar.
        /// </summary>
        public const double ProminentHeight = 128;

        /// <summary>
        /// The height of an extra prominent app bar.
        /// </summary>
        public const double ExtraProminentHeight = 136;

        /// <summary>
        /// The height of a dense app bar.
        /// </summary>
        public const double DenseHeight = 48;

        /// <summary>
        /// The height of a dense prominent app bar.
        /// </summary>
        public const double DenseProminentHeight = 96;

        /// <summary>
        /// The height of a dense extra prominent app bar.
        /// </summary>
        public const double DenseExtraProminentHeight = 120;

        /// <summary>
        /// The height of a medium app bar.
        /// </summary>
        public const double MediumHeight = 56;

        /// <summary>
        /// The height of a medium prominent app bar.
        /// </summary>
        public const double MediumProminentHeight = 102;

        /// <summary>
        /// The height of a dense medium prominent app bar.
        /// </summary>
        public const double MediumExtraProminentHeight = 128;

        /// <summary>
        /// An event raised by clicking the back button.
        /// </summary>
        public static readonly RoutedEvent BackEvent = EventManager.RegisterRoutedEvent(
            nameof(Back), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(AppBar));

        /// <summary>
        /// An event raised by clicking the back button.
        /// </summary>
        public event RoutedEventHandler Back
        {
            add
            {
                AddHandler(BackEvent, value);
            }

            remove
            {
                RemoveHandler(BackEvent, value);
            }
        }

        /// <summary>
        /// The icon to show in the upper left corner.
        /// </summary>
        public static readonly DependencyProperty AppIconProperty = DependencyProperty.Register(
            nameof(AppIcon), typeof(object), typeof(AppBar), new PropertyMetadata(null, null));

        /// <summary>
        /// The icon to show in the upper left corner.
        /// </summary>
        public object AppIcon
        {
            get
            {
                return GetValue(AppIconProperty);
            }

            set
            {
                SetValue(AppIconProperty, value);
            }
        }

        /// <summary>
        /// An command called by clicking the back button.
        /// </summary>
        public static readonly DependencyProperty BackCommandProperty = DependencyProperty.Register(
            nameof(BackCommand), typeof(ICommand), typeof(AppBar), new PropertyMetadata(null, null));

        /// <summary>
        /// An command called by clicking the back button.
        /// </summary>
        public ICommand BackCommand
        {
            get
            {
                return (ICommand)GetValue(BackCommandProperty);
            }

            set
            {
                SetValue(BackCommandProperty, value);
            }
        }

        /// <summary>
        /// The items for the content area (toolbar buttons for example).
        /// </summary>
        public static readonly DependencyPropertyKey ChildrenProperty = DependencyProperty.RegisterReadOnly(
            nameof(Children), typeof(IList), typeof(AppBar), new UIPropertyMetadata(null, null));

        /// <summary>
        /// The items for the content area (toolbar buttons for example).
        /// </summary>
        public IList Children
        {
            get
            {
                return (IList)GetValue(ChildrenProperty.DependencyProperty);
            }

            set
            {
                SetValue(ChildrenProperty, value);
            }
        }

        /// <summary>
        /// The state of the navigation drawer button: true = drawer open, false = drawer closed.
        /// </summary>
        public static readonly DependencyProperty IsNavigationDrawerOpenProperty = DependencyProperty.Register(
            nameof(IsNavigationDrawerOpen), typeof(bool), typeof(AppBar), new PropertyMetadata(false));

        /// <summary>
        /// The state of the navigation drawer button: true = drawer open, false = drawer closed.
        /// </summary>
        public bool IsNavigationDrawerOpen
        {
            get
            {
                return (bool)GetValue(IsNavigationDrawerOpenProperty);
            }

            set
            {
                SetValue(IsNavigationDrawerOpenProperty, value);
            }
        }

        /// <summary>
        /// The opaque mode of the <see cref="AppBar" />: opaque or transparent.
        /// </summary>
        public static readonly DependencyProperty OpaqueModeProperty = DependencyProperty.Register(
            nameof(OpaqueMode), typeof(AppBarOpaqueMode), typeof(AppBar), new PropertyMetadata(AppBarOpaqueMode.Opaque));

        /// <summary>
        /// The opaque mode of the <see cref="AppBar" />: opaque or transparent.
        /// </summary>
        public AppBarOpaqueMode OpaqueMode
        {
            get
            {
                return (AppBarOpaqueMode)GetValue(OpaqueModeProperty);
            }

            set
            {
                SetValue(OpaqueModeProperty, value);
            }
        }

        /// <summary>
        /// Shows or hides the back button on the upper left corner.
        /// </summary>
        public static readonly DependencyProperty ShowBackButtonProperty = DependencyProperty.Register(
            nameof(ShowBackButton), typeof(bool), typeof(AppBar), new PropertyMetadata(false));

        /// <summary>
        /// Shows or hides the back button on the upper left corner.
        /// </summary>
        public bool ShowBackButton
        {
            get
            {
                return (bool)GetValue(ShowBackButtonProperty);
            }

            set
            {
                SetValue(ShowBackButtonProperty, value);
            }
        }

        /// <summary>
        /// Shows or hides the toggle button to open the navigation drawer.
        /// </summary>
        public static readonly DependencyProperty ShowNavigationDrawerButtonProperty = DependencyProperty.Register(
            nameof(ShowNavigationDrawerButton), typeof(bool), typeof(AppBar), new PropertyMetadata(false));

        /// <summary>
        /// Shows or hides the toggle button to open the navigation drawer.
        /// </summary>
        public bool ShowNavigationDrawerButton
        {
            get
            {
                return (bool)GetValue(ShowNavigationDrawerButtonProperty);
            }

            set
            {
                SetValue(ShowNavigationDrawerButtonProperty, value);
            }
        }

        /// <summary>
        /// Shows or hides the shadow of the <see cref="AppBar" />.
        /// </summary>
        public static readonly DependencyProperty ShowShadowProperty = DependencyProperty.Register(
            nameof(ShowShadow), typeof(bool), typeof(AppBar), new PropertyMetadata(true));

        /// <summary>
        /// Shows or hides the shadow of the <see cref="AppBar" />.
        /// </summary>
        public bool ShowShadow
        {
            get
            {
                return (bool)GetValue(ShowShadowProperty);
            }

            set
            {
                SetValue(ShowShadowProperty, value);
            }
        }

        /// <summary>
        /// Moves the title into the prominent area (second row).
        /// </summary>
        public static readonly DependencyProperty ShowTitleInProminentAreaProperty = DependencyProperty.Register(
            nameof(ShowTitleInProminentArea), typeof(bool), typeof(AppBar), new PropertyMetadata(false));

        /// <summary>
        ///  If true, moves the title into the prominent area (second row).
        /// </summary>
        public bool ShowTitleInProminentArea
        {
            get
            {
                return (bool)GetValue(ShowTitleInProminentAreaProperty);
            }

            set
            {
                SetValue(ShowTitleInProminentAreaProperty, value);
            }
        }

        /// <summary>
        /// The content to show in the title area.
        /// </summary>
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            nameof(Title), typeof(string), typeof(AppBar), new PropertyMetadata(null));

        /// <summary>
        /// The content to show in the title area.
        /// </summary>
        public object Title
        {
            get
            {
                return (string)GetValue(TitleProperty);
            }

            set
            {
                SetValue(TitleProperty, value);
            }
        }

        /// <summary>
        /// The type of the <see cref="AppBar" />: default, prominent, dense and dense prominent.
        /// </summary>
        public static readonly DependencyProperty TypeProperty = DependencyProperty.Register(
            nameof(Type), typeof(AppBarType), typeof(AppBar), new PropertyMetadata(AppBarType.Default));

        /// <summary>
        /// The type of the <see cref="AppBar" />: default, prominent, dense and dense prominent.
        /// </summary>
        public AppBarType Type
        {
            get
            {
                return (AppBarType)GetValue(TypeProperty);
            }

            set
            {
                SetValue(TypeProperty, value);
            }
        }

        private Button m_backButton;

        static AppBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AppBar), new FrameworkPropertyMetadata(typeof(AppBar)));
        }

        /// <summary>
        /// Creates a new <see cref="AppBar" />.
        /// </summary>
        public AppBar()
            : base()
        {
            m_backButton = null;

            Children = new ObservableCollection<object>();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (m_backButton != null)
            {
                m_backButton.Click -= BackButtonClickHandler;
            }

            m_backButton = Template.FindName(BackButtonName, this) as Button;
            m_backButton.Click += BackButtonClickHandler;
        }

        private void BackButtonClickHandler(object sender, RoutedEventArgs args)
        {
            RoutedEventArgs backArgs = new RoutedEventArgs(BackEvent, this);
            RaiseEvent(backArgs);

            if (BackCommand != null && BackCommand.CanExecute(null))
            {
                BackCommand.Execute(null);
            }
        }
    }

    /// <summary>
    /// The type of the <see cref="AppBar" />.
    /// </summary>
    public enum AppBarType : byte
    {
        /// <summary>
        /// <see cref="AppBar" /> with default height.
        /// </summary>
        Default,

        /// <summary>
        /// <see cref="AppBar" /> with prominent height.
        /// </summary>
        Prominent,

        /// <summary>
        /// <see cref="AppBar" /> with extra prominent height.
        /// </summary>
        ExtraProminent,

        /// <summary>
        /// <see cref="AppBar" /> with dense height.
        /// </summary>
        Dense,

        /// <summary>
        /// <see cref="AppBar" /> with dense prominent height.
        /// </summary>
        DenseProminent,

        /// <summary>
        /// <see cref="AppBar" /> with dense extra prominent area.
        /// </summary>
        DenseExtraProminent,

        /// <summary>
        /// <see cref="AppBar" /> with medium height.
        /// </summary>
        Medium,

        /// <summary>
        /// <see cref="AppBar" /> with medium prominent height.
        /// </summary>
        MediumProminent,

        /// <summary>
        /// <see cref="AppBar" /> with medium extra prominent height.
        /// </summary>
        MediumExtraProminent
    }

    /// <summary>
    /// The opaque mode of the <see cref="AppBar" /> to toggle between opaque and transparent background.
    /// </summary>
    public enum AppBarOpaqueMode : byte
    {
        /// <summary>
        /// An opaque <see cref="AppBar" />.
        /// </summary>
        Opaque,

        /// <summary>
        /// A transparent <see cref="AppBar" />
        /// </summary>
        Transparent
    }
}
