using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MaterialDesignExtensions.Controls
{
    /// <summary>
    /// Custom window class for a Material Design like styled window.
    /// </summary>
    public class MaterialWindow : Window
    {
        private const string MinimizeButtonName = "minimizeButton";
        private const string MaximizeRestoreButtonName = "maximizeRestoreButton";
        private const string CloseButtonName = "closeButton";

        /// <summary>
        /// The color for the border and caption area background of the window.
        /// </summary>
        public static readonly DependencyProperty BorderBackgroundBrushProperty = DependencyProperty.Register(
            nameof(BorderBackgroundBrush), typeof(Brush), typeof(MaterialWindow), new FrameworkPropertyMetadata(null, null));

        /// <summary>
        /// The color for the border and caption area background of the window.
        /// </summary>
        public Brush BorderBackgroundBrush
        {
            get
            {
                return (Brush)GetValue(BorderBackgroundBrushProperty);
            }

            set
            {
                SetValue(BorderBackgroundBrushProperty, value);
            }
        }

        /// <summary>
        /// The forground color for the caption area of the window.
        /// </summary>
        public static readonly DependencyProperty BorderForegroundBrushProperty = DependencyProperty.Register(
            nameof(BorderForegroundBrush), typeof(Brush), typeof(MaterialWindow), new FrameworkPropertyMetadata(null, null));

        /// <summary>
        /// The forground color for the caption area of the window.
        /// </summary>
        public Brush BorderForegroundBrush
        {
            get
            {
                return (Brush)GetValue(BorderForegroundBrushProperty);
            }

            set
            {
                SetValue(BorderForegroundBrushProperty, value);
            }
        }

        /// <summary>
        /// Lets the content of the window fade out if the window is inactive.
        /// The default is true (enabled).
        /// </summary>
        public static readonly DependencyProperty FadeContentIfInactiveProperty = DependencyProperty.Register(
                nameof(FadeContentIfInactive), typeof(bool), typeof(MaterialWindow), new FrameworkPropertyMetadata(true));

        /// <summary>
        /// Lets the content of the window fade out if the window is inactive.
        /// The default is true (enabled).
        /// </summary>
        public bool FadeContentIfInactive
        {
            get
            {
                return (bool)GetValue(FadeContentIfInactiveProperty);
            }

            set
            {
                SetValue(FadeContentIfInactiveProperty, value);
            }
        }

        /// <summary>
        /// The icon inside the window's title bar.
        /// </summary>
        public static readonly DependencyProperty TitleBarIconProperty = DependencyProperty.Register(
            nameof(TitleBarIcon), typeof(ImageSource), typeof(MaterialWindow), new FrameworkPropertyMetadata(null, null));

        /// <summary>
        /// The icon inside the window's title bar.
        /// </summary>
        public ImageSource TitleBarIcon
        {
            get
            {
                return (ImageSource)GetValue(TitleBarIconProperty);
            }

            set
            {
                SetValue(TitleBarIconProperty, value);
            }
        }

        private Button m_minimizeButton;
        private Button m_maximizeRestoreButton;
        private Button m_closeButton;

        static MaterialWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MaterialWindow), new FrameworkPropertyMetadata(typeof(MaterialWindow)));
        }

        /// <summary>
        /// Creates a new <see cref="MaterialWindow" />.
        /// </summary>
        public MaterialWindow() : base() { }

        public override void OnApplyTemplate()
        {
            if (m_minimizeButton != null)
            {
                m_minimizeButton.Click -= MinimizeButtonClickHandler;
            }

            m_minimizeButton = GetTemplateChild(MinimizeButtonName) as Button;

            if (m_minimizeButton != null)
            {
                m_minimizeButton.Click += MinimizeButtonClickHandler;
            }

            if (m_maximizeRestoreButton != null)
            {
                m_maximizeRestoreButton.Click -= MaximizeRestoreButtonClickHandler;
            }

            m_maximizeRestoreButton = GetTemplateChild(MaximizeRestoreButtonName) as Button;

            if (m_maximizeRestoreButton != null)
            {
                m_maximizeRestoreButton.Click += MaximizeRestoreButtonClickHandler;
            }

            if (m_closeButton != null)
            {
                m_closeButton.Click -= CloseButtonClickHandler;
            }

            m_closeButton = GetTemplateChild(CloseButtonName) as Button;

            if (m_closeButton != null)
            {
                m_closeButton.Click += CloseButtonClickHandler;
            }

            base.OnApplyTemplate();
        }

        private void CloseButtonClickHandler(object sender, RoutedEventArgs args)
        {
            Close();
        }

        private void MaximizeRestoreButtonClickHandler(object sender, RoutedEventArgs args)
        {
            WindowState = (WindowState == WindowState.Normal) ? WindowState.Maximized : WindowState.Normal;
        }

        private void MinimizeButtonClickHandler(object sender, RoutedEventArgs args)
        {
            WindowState = WindowState.Minimized;
        }
    }
}
